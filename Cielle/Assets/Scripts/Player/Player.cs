using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable {
    public void Hit(float damage) {
        Stats.Instance.Hp -= Mathf.Max(1, Stats.Instance.Def - damage);

        if (Stats.Instance.Hp <= 0.0) {
            Debug.Log("You Died");
        }
    }
}
