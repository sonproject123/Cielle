using UnityEngine;

public class ItemStats : Singleton<ItemStats> {
    private float maxHp;
    private float maxHpMult;

    private float maxShield;
    private float maxShieldMult;

    private float shieldDefense;
    private float shieldDefenseMult;

    private float shieldRegen;
    private float shieldCooltime;
    private float shieldBreakCooltime;
    private float shieldInvincible;

    private float attack;
    private float attackShield;

    private float defense;
    private float invincible;

    private float gunRecoil;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        maxHp = 0;
        maxHpMult = 1;

        maxShield = 0;
        maxShieldMult = 1;

        shieldDefense = 0;
        shieldDefenseMult = 1;

        shieldRegen = 1;
        shieldCooltime = 1;
        shieldBreakCooltime = 1;
        shieldInvincible = 1;

        attack = 1;
        attackShield = 1;

        defense = 1;
        invincible = 1;

        gunRecoil = 1;
    }

    public float MaxHP { get { return maxHp; } set { maxHp = value; } }
    public float MaxHPMult { get { return maxHpMult; } set { maxHpMult = value; } }
    public float MaxShield { get { return maxShield; } set { maxShield = value; } }
    public float MaxShieldMult { get { return maxShieldMult; } set { maxShieldMult = value; } }
    public float ShieldDefense { get { return shieldDefense; } set { shieldDefense = value; } }
    public float ShieldDefenseMult { get { return shieldDefenseMult; } set { shieldDefenseMult = value; } }
    public float ShieldRegen { get { return shieldRegen; } set { shieldRegen = value; } }
    public float ShieldCooltime { get { return shieldCooltime; } set { shieldCooltime = value; } }
    public float ShieldBreakCooltime { get { return shieldBreakCooltime; } set { shieldBreakCooltime = value; } }
    public float ShieldInvincible { get { return shieldInvincible; } set { shieldInvincible = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public float AttackShield { get { return attackShield; } set { attackShield = value; } }
    public float Defense { get { return defense; } set { defense = value; } }
    public float Invincible { get { return invincible; } set { invincible = value; } }
    public float GunRecoil { get { return gunRecoil; } set { gunRecoil = value; } }
}
