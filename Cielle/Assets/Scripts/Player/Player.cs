using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable {
    [SerializeField] Transform playerCenter;
    [SerializeField] bool isInvincible = false;

    public void Hit(float damage, Vector3 hitPosition) {
        if (!isInvincible) {
            Stats.Instance.Hp -= Mathf.Max(1, damage - Stats.Instance.Def);
            UIManager.OnUpdateHpBar();

            if (Stats.Instance.Hp <= 0.0) {
                Debug.Log("You Died");
            }
            else
                StartCoroutine(Invincible());
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

    IEnumerator Invincible() {
        isInvincible = true;
        yield return CoroutineCache.WaitForSecond(Stats.Instance.Invincible);
        isInvincible = false;
    }
}
