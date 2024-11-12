using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField] protected Transform player;

    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float attack;
    [SerializeField] protected float defense;
    [SerializeField] protected float speed;
    [SerializeField] protected float cooltime;

    protected virtual void Start() {
        maxHp = 100;
        hp = maxHp;
        speed = 10;
        attack = 1;
        defense = 0;
        cooltime = 1;

        player = GameObject.Find("Player").transform;
    }

    public float Hp {
        get { return hp; }
        set { hp = value; }
    }

    public float MaxHp {
        get { return maxHp; }
        set { hp = value; }
    }

    public float Atk {
        get { return attack; }
        set { hp = value; }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public float Cooltime {
        get { return cooltime; }
        set { cooltime = value; }
    }
}
