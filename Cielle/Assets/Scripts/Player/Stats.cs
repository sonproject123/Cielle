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

    [SerializeField] protected float mass;

    [SerializeField] protected float invincible;

    [SerializeField] protected float jumpHeight;

    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTime;
    [SerializeField] protected float dashCooltime;

    [SerializeField] protected float flyTime;
    [SerializeField] protected float flySpeed;
    [SerializeField] protected float flyDashSpeed;

    [SerializeField] protected bool isMove;
    [SerializeField] protected bool isLeft;

    [SerializeField] protected int mainWeaponId;
    [SerializeField] protected int subWeaponId;
    [SerializeField] protected GunData mainGunData;

    [SerializeField] protected int bulletMax;
    [SerializeField] protected int bulletRemain;

    [SerializeField] Guns mainGunCode;
    [SerializeField] GunFireType mainGunFireType;

    private void Start() {
        mainWeaponId = 1;
        subWeaponId = 2;
        GunInit();

        maxHp = 100;
        hp = maxHp;

        maxShield = 0;
        shield = maxShield;

        attack = 0;
        attackShield = 0;

        defense = 0;

        speed = 15;

        mass = 5;

        invincible = 1;

        jumpHeight = 60;

        dashSpeed = 30;
        dashTime = 0.3f;
        dashCooltime = 0.5f;

        flyTime = 0.5f;
        flySpeed = 20;
        flyDashSpeed = 40;

        isMove = false;
        isLeft = false;
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

    public float Mass {
        get { return mass; }
        set {  mass = value; }
    }

    public float Invincible {
        get { return invincible; }
        set { invincible = value; }
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

    public float FlyTime {
        get { return flyTime; }
        set { flyTime = value; }
    }
    public float FlySpeed {
        get { return flySpeed; }
        set { flySpeed = value; }
    }
    public float FlyDashSpeed {
        get { return flyDashSpeed; }
        set { flyDashSpeed = value; }
    }

    public bool IsMove {
        get { return isMove; }
        set { isMove = value; }
    }
    public bool IsLeft {
        get { return isLeft; }
        set { isLeft = value; }
    }

    public int MainWeaponId {
        get { return mainWeaponId; }
        set { mainWeaponId = value; }
    }
    public int SubWeaponId {
        get { return subWeaponId; }
        set { subWeaponId = value; }
    }

    public int BulletMax {
        get { return bulletMax; }
        set { bulletMax = value; }
    }

    public int BulletRemain {
        get { return bulletRemain; }
        set { bulletRemain = value; }
    }

    public Guns MainGunCode {
        get { return mainGunCode; }
        set { mainGunCode = value; }
    }
    public GunFireType MainGunFireType {
        get { return mainGunFireType; }
        set { mainGunFireType = value; }
    }
    public GunData MainGunData {
        get { return mainGunData; }
        set { mainGunData = value; }
    }

    public void GunInit() {
        JsonManager.Instance.GunDict.TryGetValue(MainWeaponId, out mainGunData);

        mainGunCode = (Guns)System.Enum.Parse(typeof(Guns), mainGunData.code);
        mainGunFireType = (GunFireType)System.Enum.Parse(typeof(GunFireType), mainGunData.type);
        bulletMax = mainGunData.bullet;
        bulletRemain = bulletMax;
    }

    public void GunChange() {
        GunInit();
        UIManager.OnBulletChange();
    }
}
