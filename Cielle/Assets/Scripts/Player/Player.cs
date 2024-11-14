using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable {
    [SerializeField] bool isInvincible = false;

    public void Hit(float damage) {
        Stats.Instance.Hp -= Mathf.Max(1, Stats.Instance.Def - damage);

        if (Stats.Instance.Hp <= 0.0) {
            Debug.Log("You Died");
        }
    }

    private void Update() {
        Stats.Instance.MouseLocation = MathCalculator.Instance.MousePosition();
        LookAtCursor();
    }

    private void LookAtCursor() {
        if(Stats.Instance.MouseLocation.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            transform.rotation = Quaternion.Euler(0, 270, 0);
    }
}
