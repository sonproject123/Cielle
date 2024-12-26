using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStats : MonoBehaviour, IHitable {
    [SerializeField] protected int id = 0;
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform muzzleRotation;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject ui;
    [SerializeField] protected EnemyUI enemyUI;

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

    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isAttack;

    [SerializeField] protected GeneralFSM<EnemyStats> currentState;
    [SerializeField] public bool isInAttackRange;
    [SerializeField] public bool isInChaseRange;

    protected abstract GeneralFSM<EnemyStats> InitialState();
    public abstract void Patrol();
    public abstract void Chase();
    public abstract void Attack();

    private void Awake() {
        enemyUI = ui.GetComponent<EnemyUI>();
        currentState = InitialState();
        currentState.OnStateEnter();
    }

    private void OnEnable() {
        if (id != 0) {
            JsonManager.Instance.EnemyDict.TryGetValue(id, out enemyData);
            Initialize();
        }
    }

    private void Update() {
        currentState.OnStateStay();
    }

    public void ChangeState(GeneralFSM<EnemyStats> newState) {
        currentState.OnStateExit();
        currentState = newState;
        currentState.OnStateEnter();
    }

    private void Initialize() {
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
        isDead = false;
        isAttack = false;
        player = Stats.Instance.PlayerCenter;
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

    public void Hit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        hp -= Mathf.Max(1, damage - defense);
        enemyUI.HpBar();

        if (hp <= 0.0 && !isDead) {
            isDead = true;

            for (int i = 0; i < 10; i++)
                BreakObject(hitPosition);

            for (int i = 0; i < 10; i++)
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
    private void TransformMove(Vector3 dir, float speed, float wallSensor) {
        if (!Physics.Raycast(transform.position, dir, wallSensor, LayerMask.GetMask("Wall")))
            transform.position += dir * speed * Time.deltaTime;
    }

    public void AttackRange(bool value) {
        isInAttackRange = value;
    }

    public void ChaseRange(bool value) {
        isInChaseRange = value;
    }

    protected void RigidMove() {

    }

    protected void LinearBulletSpawn() {
        GameObject bullet = ObjectManager.Instance.UseObject("ENEMYBULLET");
        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = muzzle.transform.rotation;

        BulletStats(bullet);
    }
    protected void BulletStats(GameObject bullet) {
        BulletEnemy bulletEnemy = bullet.GetComponent<BulletEnemy>();
        if (bulletEnemy != null) {
            bulletEnemy.Atk = attack;
            bulletEnemy.AtkShield = attackShield;
            bulletEnemy.Speed = bulletSpeed;
            bulletEnemy.StoppingPower = stoppingPower;
            bulletEnemy.StoppingTime = stoppingTime;
            bulletEnemy.Target = player.position;
        }
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
            mo.OnDrop(price);
    }
}
