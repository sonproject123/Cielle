using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHitable {
    [SerializeField] protected int id = 0;
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform muzzleRotation;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject ui;
    [SerializeField] protected EnemyUI enemyUI;

    [SerializeField] protected Rigidbody rigidBody;
    [SerializeField] protected Transform centerChecker;
    [SerializeField] protected Transform frontChecker;
    [SerializeField] protected Transform backChecker;
    [SerializeField] protected Transform bottomChecker;
    [SerializeField] protected Transform backBottomChecker;

    [SerializeField] protected Vector3 originalScale;
    [SerializeField] protected Vector3 moveDirection;

    [SerializeField] protected string enemyName;
    [SerializeField] protected string subtitle;
    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float shield;
    [SerializeField] protected float maxShield;
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
    [SerializeField] protected float defense;
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingPower;
    [SerializeField] protected float stoppingTime;
    [SerializeField] protected float stoppingResistance;
    [SerializeField] protected float cooltime;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected int price;

    [SerializeField] protected bool isBoss;
    [SerializeField] protected bool isSummoned;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected bool isChaseCooltime;
    [SerializeField] protected bool isThisLeft;

    [SerializeField] protected GeneralFSM<Enemy> currentState;
    [SerializeField] public bool isInAttackRange;
    [SerializeField] public bool isInChaseRange;

    [SerializeField] protected System.Random random;

    protected virtual GeneralFSM<Enemy> InitialState() {
        return new EnemyState_InPatrol<Enemy>(this);
    }
    public abstract void Patrol();
    public abstract void Chase();
    public abstract void Attack();
    public abstract void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition);

    protected void Awake() {
        random = new System.Random();
        enemyUI = ui.GetComponent<EnemyUI>();
        rigidBody = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        if (!isBoss) {
            currentState = InitialState();
            currentState.OnStateEnter();
        }

        moveDirection = new Vector3(-1, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected virtual void OnEnable() {
        if (id != 0) {
            JsonManager.Instance.EnemyDict.TryGetValue(id, out enemyData);
            Initialize();
        }
    }

    protected void FixedUpdate() {
        currentState.OnStateStay();
    }

    public void ChangeState(GeneralFSM<Enemy> newState) {
        currentState.OnStateExit();
        currentState = newState;
        currentState.OnStateEnter();
    }

    private void Initialize() {
        enemyName = enemyData.name;
        subtitle = enemyData.subtitle;

        maxHp = enemyData.hp;
        hp = maxHp;
        speed = enemyData.speed;
        attack = enemyData.attack;
        attackShield = enemyData.attackShield;
        defense = enemyData.defense;
        cooltime = enemyData.cooltime;
        bulletSpeed = enemyData.bulletSpeed;
        stoppingPower = enemyData.stoppingPower;
        stoppingTime = enemyData.stoppingTime;
        stoppingResistance = enemyData.stoppingResistance;
        price = enemyData.price;

        isInAttackRange = false;
        isInChaseRange = false;
        isChaseCooltime = false;
        isThisLeft = true;
        isDead = false;
        isAttack = false;
        isSummoned = false;
        player = Stats.Instance.PlayerCenter;
    }

    #region Property
    public string Name {
        get { return enemyName; }
    }

    public string Subtitle {
        get { return subtitle; }
    }

    public int Id {
        get { return id; }
        set { id = value; }
    }

    public float Hp {
        get { return hp; }
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
        get { return MaxShield; }
        set { MaxShield = value; }
    }

    public float Atk {
        get { return attack; }
        set { attack = value; }
    }

    public float AtkShield {
        get { return attackShield; }
        set { attackShield = value; }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public float Cooltime {
        get { return cooltime; }
        set { cooltime = value; }
    }

    public bool IsSummoned {
        get { return isSummoned; }
        set { isSummoned = value; }
    }
    #endregion

    public void ForcedDie() {
        CommonHit(999999999, 999999999, 0, 0, Vector3.down);
    }

    public void Revive() {
        isDead = false;
        hp = maxHp;
        enemyUI.HpBar();
    }

    public void Hit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        if (!isBoss) {
            currentState.OnStateExit();
            currentState = new EnemyState_InHit<Enemy>(this, damage, damageShield, stoppingPower, stoppingTime, hitPosition);
            currentState.OnStateEnter();
        }
    }
    IEnumerator Stopping(Vector3 dir, float stoppingPower, float stoppingTime) {
        if (stoppingPower < 0)
            yield break;

        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while(time < stoppingTime) {
            time += Time.deltaTime;
            TransformMove(dir, stoppingPower, 1);
            yield return wffu;
        }
    }
    protected void TransformMove(Vector3 dir, float speed, float wallSensor) {
        if (!Physics.Raycast(transform.position, dir, wallSensor, LayerMask.GetMask("Wall")))
            transform.position += dir * speed * Time.deltaTime;
    }

    protected void LookAtPlayer() {
        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    protected void CommonHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        hp -= Mathf.Max(1, damage - defense);
        enemyUI.HpBar();
        BreakObject(hitPosition);

        if (hp <= 0.0 && !isDead) {
            isDead = true;

            for (int i = 0; i < 10; i++)
                BreakObject(hitPosition);

            if (!isSummoned)
                for (int i = 0; i < price; i++)
                    MetalObject();

            EnemyManager.OnReturnEnemy?.Invoke(gameObject, id);
        }
        else if (hp > 0.0) {
            Vector3 dir = Vector3.right;
            if (transform.position.x < hitPosition.x)
                dir *= -1;
            StartCoroutine(Stopping(dir, stoppingPower - stoppingResistance, stoppingTime));
        }
    }

    protected bool EdgeCheck() {
        Vector3 rayDir = (frontChecker.position - centerChecker.position).normalized;
        float rayDistance = Mathf.Abs(frontChecker.position.x - centerChecker.position.x);
        if (Physics.Raycast(centerChecker.position, rayDir, rayDistance, LayerMask.GetMask("Wall")))
            return true;

        rayDir = (bottomChecker.position - frontChecker.position).normalized;
        rayDistance = Mathf.Abs(bottomChecker.position.y - frontChecker.position.y);
        if (!Physics.Raycast(frontChecker.position, rayDir, rayDistance, LayerMask.GetMask("Wall")))
            return true;
        
        return false;
    }

    protected void CommonPatrol() {
        bool isAtEdge = EdgeCheck();

        if (isAtEdge) {
            transform.rotation = Quaternion.Euler(0, (transform.eulerAngles.y + 180) % 360, 0);
            moveDirection *= -1;
            isThisLeft = !isThisLeft;
        }

        rigidBody.MovePosition(rigidBody.position + moveDirection * speed * Time.deltaTime);
    }

    protected void CommonChase() {
        if (!isChaseCooltime) {
            if (player.position.x < transform.position.x && isThisLeft == false) {
                moveDirection = Vector3.left;
                isThisLeft = true;
                StartCoroutine(ChaseCooltime());
            }
            else if (player.position.x > transform.position.x && isThisLeft == true) {
                moveDirection = Vector3.right;
                isThisLeft = false;
                StartCoroutine(ChaseCooltime());
            }

            LookAtPlayer();
        }

        bool isAtEdge = EdgeCheck();
        if (isAtEdge) {
            //idle Animation
        }
        else
            rigidBody.MovePosition(rigidBody.position + moveDirection * speed * 1.2f * Time.deltaTime);
    }

    IEnumerator ChaseCooltime() {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        isChaseCooltime = true;

        while (time < 0.5) {
            time += Time.deltaTime;
            yield return wffu;
        }

        isChaseCooltime = false;
    }

    public void AttackRange(bool value) {
        isInAttackRange = value;
    }

    public void ChaseRange(bool value) {
        isInChaseRange = value;
    }

    protected void LinearBulletSpawn(Vector3 target, float angle = 0) {
        GameObject bullet = ObjectManager.Instance.UseObject("ENEMYBULLET");
        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = Quaternion.Euler(muzzle.transform.eulerAngles.x, muzzle.transform.eulerAngles.y, muzzle.transform.eulerAngles.z + angle);
        
        BulletStats(bullet, angle, target);
    }

    protected void BulletStats(GameObject bullet, float angle, Vector3 target) {
        BulletEnemy bulletEnemy = bullet.GetComponent<BulletEnemy>();
        bulletEnemy.BulletInit(attack, attackShield, bulletSpeed, stoppingPower, stoppingTime, angle, target);
    }

    protected void BreakObject(Vector3 hitPosition) {
        GameObject obj = ObjectManager.Instance.UseObject("BREAKOBJECT");
        obj.transform.position = transform.position;

        Vector3 direction = (transform.position - hitPosition).normalized;

        BreakObject bo = obj.GetComponent<BreakObject>();
        if (bo != null) {
            float speed = UnityEngine.Random.Range(120, 150);
            Vector3 dir = direction + MathCalculator.Instance.RandomTarget(0.3f, 0.3f);

            bo.OnDead(speed, dir);
        }
    }

    protected void MetalObject() {
        GameObject obj = ObjectManager.Instance.UseObject("METALOBJECT");
        obj.transform.position = transform.position;

        MetalObject mo = obj.GetComponent<MetalObject>();
        if (mo != null) 
            mo.OnDrop(1);
    }
}
