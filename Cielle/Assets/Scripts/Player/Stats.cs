using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Singleton<Stats> {
    [SerializeField] Vector3 mousePosition;

    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;

    [SerializeField] protected float shield;
    [SerializeField] protected float maxShield;
                     
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
                     
    [SerializeField] protected float defense;

    [SerializeField] protected float speed;

    [SerializeField] protected float invincible;

    [SerializeField] protected float jumpHeight;

    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTime;
    [SerializeField] protected float dashCooltime;

    private void Start() {
        maxHp = 100;
        hp = maxHp;

        maxShield = 0;
        shield = maxShield;

        attack = 1;
        attackShield = 0;

        defense = 0;

        speed = 15;

        invincible = 1;

        jumpHeight = 50;

        dashSpeed = 30;
        dashTime = 0.3f;
        dashCooltime = 1;
    }

    public Vector3 MouseLocation {
        get { return mousePosition; }
        set { mousePosition = value; }
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

    public float AtkShield {
        get { return attackShield; }
        set { attackShield = value; }
    }

    public float Def {
        get { return defense; }
        set { defense = value; }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public float JumpHeight {
        get { return jumpHeight; }
        set { jumpHeight = value; }
    }

    public float DashSpeed {
        get { return dashSpeed; }
        set { dashSpeed = value; }
    }

    public float DashTime {
        get { return dashTime; }
        set { dashTime = value; }
    }

    public float DashCooltime {
        get { return dashCooltime; }
        set { dashCooltime = value; }
    }
}
