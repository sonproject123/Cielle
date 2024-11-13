using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Singleton<Stats> {
    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;

    [SerializeField] protected float shield;
    [SerializeField] protected float maxShield;
                     
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
                     
    [SerializeField] protected float defense;

    [SerializeField] protected float speed;

    private void Start() {
        maxHp = 100;
        hp = maxHp;
        maxShield = 0;
        shield = maxShield;
        speed = 10;
        attack = 1;
        attackShield = 0;
        defense = 0;
    }

    public float Hp {
        get {  return hp; }
        set { hp = value; }
    }

    public float MaxHp {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float Atk {
        get { return attack; }
        set { attack = value; }
    }

    public float Def {
        get { return defense; }
        set { defense = value; }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }
}
