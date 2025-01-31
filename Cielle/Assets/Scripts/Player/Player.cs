using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour, IHitable {
    [SerializeField] Transform playerCenter;
    [SerializeField] Volume hitVolume;
    [SerializeField] Animator animator;
    [SerializeField] Canvas playerCanvas;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] Rigidbody rigidBody;

    [SerializeField] float shieldRegenTime;
    [SerializeField] float shieldBreakRegenTime;
    [SerializeField] bool isShieldBreak;
    [SerializeField] bool isShieldRegen;
    [SerializeField] bool isDead;

    [SerializeField] int aniDeath = Animator.StringToHash("Death");

    private void Awake() {
        animator = GetComponent<Animator>();
        playerUI = playerCanvas.GetComponent<PlayerUI>();
        rigidBody = GetComponent<Rigidbody>();
        hitVolume = GameObject.Find("Hit Volume").GetComponent<Volume>();
    }

    private void Start() {
        shieldRegenTime = 0;
        shieldBreakRegenTime = 0;

        isShieldBreak = false;
        isShieldRegen = false;
        isDead = false;
    }

    public void Hit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        if (!Stats.Instance.IsInvincible) {
            if (Stats.Instance.IsShieldOn) {
                Stats.Instance.Shield -= Mathf.Max(1, damage - Stats.Instance.ShieldDef);
                UIManager.OnUpdateShieldBar?.Invoke();

                if (Stats.Instance.Shield <= 0.0) {
                    StartCoroutine(SlowZoomIn(1));
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

                if (Stats.Instance.Hp <= 0.0 && !isDead) {
                    isDead = true;
                    StartCoroutine(Dead(hitPosition));
                } 
                else
                    StartCoroutine(Invincible(Stats.Instance.Invincible));
            }
        }
    }

    IEnumerator Dead(Vector3 hitPosition) {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        if (hitPosition.x > playerCenter.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, -1);

        transform.position = new Vector3(transform.position.x, transform.position.y, -3f);
        rigidBody.useGravity = true;

        animator.SetTrigger(aniDeath);
        GeneralStats.Instance.Pause = true;
        Stats.Instance.IsInvincible = true;
        LetterBoxManager.Instance.LetterBox(true);
        UIManager.OnUIAlpha(0, true);
        playerUI.DeadBackground();
        StartCoroutine(SlowZoomIn(3));

        while (time < 3) {
            time += Time.deltaTime;
            yield return wffu;
        }

        PopUpManager.Instance.ShowPopUp(PopUpTypes.DEAD);
    }

    private void Update() {
        if (!GeneralStats.Instance.Pause)
            LookAtCursor();
    }

    private void LookAtCursor() {
        if (GeneralStats.Instance.MouseLocation.x > transform.position.x) {
            Stats.Instance.IsLeft = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            Stats.Instance.IsLeft = true;
            transform.localScale = new Vector3(1, 1, -1);
        }
    }

    IEnumerator Invincible(float invincibleTime) {
        Stats.Instance.IsInvincible = true;
        yield return CoroutineCache.WaitForSecond(invincibleTime);
        Stats.Instance.IsInvincible = false;
    }

    private void Stun(float power, Vector3 hitPosition) {
        Stats.Instance.IsStuned = true;
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

    IEnumerator SlowZoomIn(float duration) {
        float time = 0;
        float slowTime = 0.1f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        GeneralStats.Instance.SlowTime(slowTime);
        PlayerCamera.OnCameraZoomIn?.Invoke(true);

        while(time < duration * slowTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        GeneralStats.Instance.SlowTime();
        PlayerCamera.OnCameraZoomIn?.Invoke(false);
    }
}
