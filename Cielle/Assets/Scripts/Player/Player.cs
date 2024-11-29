using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable {
    [SerializeField] Transform playerCenter;
    [SerializeField] bool isInvincible;

    [SerializeField] float shieldRegenTime;
    [SerializeField] float shieldBreakRegenTime;
    [SerializeField] bool isShieldBreak;
    [SerializeField] bool isShieldRegen;

    private void Start() {
        shieldRegenTime = 0;
        shieldBreakRegenTime = 0;

        isInvincible = false;
        isShieldBreak = false;
        isShieldRegen = false;
    }

    public void Hit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        if (!isInvincible) {
            if (Stats.Instance.IsShieldOn) {
                Stats.Instance.Shield -= Mathf.Max(1, damage - Stats.Instance.ShieldDef);
                UIManager.OnUpdateShieldBar?.Invoke();

                if (Stats.Instance.Shield <= 0.0) {
                    StartCoroutine(ShieldBreak());
                    StartCoroutine(Invincible(Stats.Instance.Invincible));

                    Stats.Instance.IsShieldOn = false;
                    UIManager.OnShieldOnOff?.Invoke(false);

                    if (isShieldBreak == false) {
                        isShieldBreak = true;
                        StartCoroutine(ShieldBreakRegen());
                    }
                }
                else {
                    shieldRegenTime = 0;
                    StartCoroutine(Invincible(Stats.Instance.ShieldInvincible));

                    if(isShieldRegen == false) {
                        isShieldRegen = true;
                        StartCoroutine(ShieldRegen());
                    }
                }
            }
            else {
                Stats.Instance.Hp -= Mathf.Max(1, damage - Stats.Instance.Def);
                UIManager.OnUpdateHpBar?.Invoke();
                shieldBreakRegenTime = 0;

                if (Stats.Instance.Hp <= 0.0) {
                    Debug.Log("You Died");
                }
                else
                    StartCoroutine(Invincible(Stats.Instance.Invincible));
            }
        }
    }

    private void Update() {
        LookAtCursor();
    }

    private void LookAtCursor() {
        if (GeneralStats.Instance.MouseLocation.x > transform.position.x) {
            Stats.Instance.IsLeft = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else {
            Stats.Instance.IsLeft = true;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    IEnumerator Invincible(float invincibleTime) {
        isInvincible = true;
        yield return CoroutineCache.WaitForSecond(invincibleTime);
        isInvincible = false;
    }

    private void Stun(float power, Vector3 hitPosition) {
        Stats.Instance.IsMovable = false;
        if(transform.position.x < hitPosition.x) {

        }
    }

    IEnumerator ShieldRegenerator() {
        float shieldRegenValue = Stats.Instance.MaxShield - Stats.Instance.Shield;
        float shieldRegenAmount = Stats.Instance.ShieldRegen / 10.0f;
        float sec = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (shieldRegenValue > 0.0f) {
            sec += Time.deltaTime;
            if(Stats.Instance.IsShieldOn == false) {
                shieldRegenValue = 0;
                yield break;
            }

            if(sec >= 0.1f) {
                sec = 0;
                shieldRegenValue -= shieldRegenAmount;
                Stats.Instance.Shield += shieldRegenAmount;
                if (Stats.Instance.Shield > Stats.Instance.MaxShield)
                    Stats.Instance.Shield = Stats.Instance.MaxShield;
                UIManager.OnUpdateShieldBar?.Invoke();
            }

            yield return wffu;
        }
    }

    IEnumerator ShieldRegen() {
        float shieldRegenTimeTotal = Stats.Instance.ShieldCooltime;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (shieldRegenTime < shieldRegenTimeTotal) {
            if (isShieldBreak) {
                isShieldRegen = false;
                shieldRegenTime = 0;
                yield break;
            }

            shieldRegenTime += Time.deltaTime;
            yield return wffu;
        }

        isShieldRegen = false;
        shieldRegenTime = 0;
        StartCoroutine(ShieldRegenerator());
    }

    IEnumerator ShieldBreakRegen() {
        float shieldBreakRegenTimeTotal = Stats.Instance.ShieldBreakCooltime;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while(shieldBreakRegenTime < shieldBreakRegenTimeTotal) {
            shieldBreakRegenTime += Time.deltaTime;
            yield return wffu;
        }

        Stats.Instance.IsShieldOn = true;
        UIManager.OnShieldOnOff?.Invoke(true);
        isShieldBreak = false;
        shieldBreakRegenTime = 0;
        StartCoroutine(ShieldRegenerator());
    }

    IEnumerator ShieldBreak() {
        float time = 0;
        float slowTime = 0.1f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        GeneralStats.Instance.SlowTime(slowTime);
        PlayerCamera.OnCameraZoomIn?.Invoke(true);

        while(time < 1.0f * slowTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        GeneralStats.Instance.SlowTime();
        PlayerCamera.OnCameraZoomIn?.Invoke(false);
    }
}
