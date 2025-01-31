using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : Singleton<Stats> {
    [SerializeField] Transform player;

    [SerializeField] float hp;
    [SerializeField] float maxHp;

    [SerializeField] float shield;
    [SerializeField] float maxShield;
    [SerializeField] float shieldDefense;
    [SerializeField] float shieldRegen;
    [SerializeField] float shieldCooltime;
    [SerializeField] float shieldBreakCooltime;
    [SerializeField] float shieldInvincible;
    [SerializeField] bool isShieldOn;
                     
    [SerializeField] float attack;
    [SerializeField] float attackShield;
                     
    [SerializeField] float defense;

    [SerializeField] float speed;

    [SerializeField] float mass;

    [SerializeField] float invincible;

    [SerializeField] float jumpHeight;

    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooltime;

    [SerializeField] float flyTime;
    [SerializeField] float flySpeed;
    [SerializeField] float flyDashSpeed;

    [SerializeField] bool isMove;
    [SerializeField] bool isStuned;
    [SerializeField] bool isLeft;
    [SerializeField] bool isInvincible;
    [SerializeField] bool isDead;

    [SerializeField] int mainWeaponId;
    [SerializeField] int subWeaponId;
    [SerializeField] GunData mainGunData;
    [SerializeField] GunData subGunData;

    [SerializeField] int bulletMax;
    [SerializeField] int bulletRemain;

    [SerializeField] string mainGunCode;
    [SerializeField] string subGunCode;
    [SerializeField] GunFireType mainGunFireType;

    [SerializeField] float gainRange;
    [SerializeField] int metals;
    [SerializeField] int totalMetals;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.buildIndex == 0) {
            GeneralStats.Instance.Pause = false;
            LetterBoxManager.Instance.LetterBox(false);
            Initialize();
        }
        else
            player = GameObject.Find("Player Center").transform;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Initialize() {
        mainWeaponId = 1;
        subWeaponId = 3;
        GunInit();

        maxHp = 100;
        hp = maxHp;

        maxShield = 100;
        shield = maxShield;
        shieldDefense = 1;
        shieldRegen = 10; // per second
        shieldCooltime = 10;
        shieldBreakCooltime = 20;
        shieldInvincible = 0.1f;
        isShieldOn = true;

        attack = 1;
        attackShield = 0;

        defense = 0;

        speed = 15;

        mass = 5;

        invincible = 0.5f;

        jumpHeight = 70;

        dashSpeed = 30;
        dashTime = 0.3f;
        dashCooltime = 0.5f;

        flyTime = 0.5f;
        flySpeed = 20;
        flyDashSpeed = 40;

        isMove = false;
        isStuned = false;
        isLeft = false;
        isInvincible = false;
        isDead = false;

        gainRange = 4;
        metals = 0;
    }

    #region Property
    public Transform PlayerCenter {
        get { return player; }
    }

    public float Hp {
        get {  return hp; }
        set { hp = value; }
    }
    public float MaxHp {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float Shield {
        get { return shield; }
        set { shield = value; }
    }

    public float MaxShield {
        get { return maxShield; }
        set { maxShield = value; }
    }

    public float ShieldDef {
        get { return shieldDefense; }
        set { shieldDefense = value; }
    }

    public float ShieldRegen {
        get { return shieldRegen; }
        set { shieldRegen = value; }
    }

    public float ShieldCooltime {
        get { return shieldCooltime; }
        set { shieldCooltime = value; }
    }

    public float ShieldBreakCooltime {
        get { return shieldBreakCooltime; }
        set { shieldBreakCooltime = value; }
    }

    public float ShieldInvincible {
        get { return shieldInvincible; }
        set { shieldInvincible = value; }
    }

    public bool IsShieldOn {
        get { return isShieldOn; }
        set { isShieldOn = value; }
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

    public bool IsStuned {
        get { return isStuned; }
        set { isStuned = value; }
    }
    public bool IsLeft {
        get { return isLeft; }
        set { isLeft = value; }
    }

    public bool IsInvincible {
        get { return isInvincible; }
        set { isInvincible = value; }
    }

    public bool IsDead {
        get { return isDead; }
        set { isDead = value; }
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

    public string MainGunCode {
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

    public GunData SubGunData {
        get { return subGunData; }
        set { subGunData = value; }
    }

    public float GainRange {
        get { return gainRange; }
        set { gainRange = value; }
    }

    public int Metals {
        get { return metals; }
        set { metals = value; }
    }

    public int TotalMetals {
        get { return totalMetals; }
        set { totalMetals = value; }
    }

    #endregion

    public void GunInit() {
        JsonManager.Instance.GunDict.TryGetValue(mainWeaponId, out mainGunData);
        JsonManager.Instance.GunDict.TryGetValue(subWeaponId, out subGunData);

        mainGunCode = mainGunData.code;
        mainGunFireType = (GunFireType)System.Enum.Parse(typeof(GunFireType), mainGunData.type);
        subGunCode = subGunData.code;
       
        bulletMax = mainGunData.bullet;
        bulletRemain = bulletMax;
        UIManager.OnBulletChange?.Invoke();
    }
}