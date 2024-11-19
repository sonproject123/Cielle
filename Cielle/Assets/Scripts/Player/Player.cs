using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable {
    [SerializeField] bool isInvincible = false;

    public void Hit(float damage) {
        Stats.Instance.Hp -= Mathf.Max(1, damage - Stats.Instance.Def);
        UIManager.OnUpdateHpBar();

        if (Stats.Instance.Hp <= 0.0) {
            Debug.Log("You Died");
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

    private void OnDestroy() {
    }
}
