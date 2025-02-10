using UnityEngine;

public class ItemStats : Singleton<ItemStats> {
    [SerializeField] float maxHp;
    [SerializeField] float maxHpMult;

    [SerializeField] float maxShield;
    [SerializeField] float maxShieldMult;
    [SerializeField] float shieldDefense;

    [SerializeField] float shieldRegen;
    [SerializeField] float shieldCooltime;
    [SerializeField] float shieldBreakCooltime;
    [SerializeField] float shieldInvincible;

    [SerializeField] float attack;
    [SerializeField] float attackShield;

    [SerializeField] float defense;
    [SerializeField] float invincible;

    [SerializeField] float gunRecoil;
    [SerializeField] float reloadTime;
    [SerializeField] float bulletLife;

    [SerializeField] float maxBullet;

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        maxHp = 0;
        maxHpMult = 1;

        maxShield = 0;
        maxShieldMult = 1;
        shieldDefense = 1;

        shieldRegen = 1;
        shieldCooltime = 1;
        shieldBreakCooltime = 1;
        shieldInvincible = 1;

        attack = 1;
        attackShield = 1;

        defense = 1;
        invincible = 1;

        gunRecoil = 1;
        reloadTime = 1;
        bulletLife = 1;
        maxBullet = 1;
    }

    public float MaxHP { get { return maxHp; } set { maxHp = value; } }
    public float MaxHPMult { get { return maxHpMult; } set { maxHpMult = value; } }
    public float MaxShield { get { return maxShield; } set { maxShield = value; } }
    public float MaxShieldMult { get { return maxShieldMult; } set { maxShieldMult = value; } }
    public float ShieldDefense { get { return shieldDefense; } set { shieldDefense = value; } }
    public float ShieldRegen { get { return shieldRegen; } set { shieldRegen = value; } }
    public float ShieldCooltime { get { return shieldCooltime; } set { shieldCooltime = value; } }
    public float ShieldBreakCooltime { get { return shieldBreakCooltime; } set { shieldBreakCooltime = value; } }
    public float ShieldInvincible { get { return shieldInvincible; } set { shieldInvincible = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public float AttackShield { get { return attackShield; } set { attackShield = value; } }
    public float Defense { get { return defense; } set { defense = value; } }
    public float Invincible { get { return invincible; } set { invincible = value; } }
    public float GunRecoil { get { return gunRecoil; } set { gunRecoil = value; } }
    public float ReloadTime { get { return reloadTime; } set { reloadTime = value; } }
    public float BulletLife { get { return bulletLife; } set { bulletLife = value; } }
    public float MaxBullet { get { return maxBullet; } set { maxBullet = value; } }
}
