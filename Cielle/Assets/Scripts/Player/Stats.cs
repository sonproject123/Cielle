using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Singleton<Stats>, IHitable {
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float shield;
    [SerializeField] private float maxShield;
    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float speed;

    private void Start() {
        maxHp = 100;
        hp = maxHp;
        maxShield = 0;
        shield = maxShield;
        speed = 10;
        attack = 1;
        defense = 0;
    }

    public float Hp {
        get {  return hp; }
        set { hp = value; }
    }

    public float MaxHp {
        get { return MaxHp; }
        set { hp = value; }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public void Hit(float damage) {
        hp -= Mathf.Max(1, defense - damage);
        if (hp <= 0.0) {
            Debug.Log("You Died");
        }
    }
}
