using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IHitable, IInRange {
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform muzzleRotation;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected GameObject ui;
    [SerializeField] protected EnemyUI enemyUI;
    [SerializeField] protected bool inRange;

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

    private void Awake() {
        enemyUI = ui.GetComponent<EnemyUI>();

        player = Stats.Instance.PlayerCenter;
    }

    protected virtual void Start() {
        maxHp = 100;
        hp = maxHp;
        speed = 10;
        attack = 10;
        attackShield = 0;
        defense = 0;
        cooltime = 1;
        bulletSpeed = 5;
        stoppingPower = 0;
        stoppingTime = 0;
        stoppingResistance = 0;
        price = 1;

        inRange = false;
        isDead = false;
        isAttack = false;
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

        Vector3 dir = Vector3.right;
        if (transform.position.x < hitPosition.x)
            dir *= -1;

        StartCoroutine(Stopping(dir, stoppingPower - stoppingResistance, stoppingTime));

        if (hp <= 0.0 && !isDead) {
            isDead = true;

            for (int i = 0; i < 10; i++)
                BreakObject(hitPosition);

            for (int i = 0; i < 10; i++)
                MetalObject();

            Destroy(gameObject);
        }
    }

    private void TransformMove(Vector3 dir, float speed, float wallSensor) {
        if (!Physics.Raycast(transform.position, dir, wallSensor, LayerMask.GetMask("Wall")))
            transform.position += dir * speed * Time.deltaTime;
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

    public void InRange(bool value) {
        inRange = value;
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

    protected void LinearBulletSpawn() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.ENEMYBULLET);
        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = muzzle.transform.rotation;

        BulletStats(bullet);
    }

    protected void BreakObject(Vector3 hitPosition) {
        GameObject obj = ObjectManager.Instance.UseObject(ObjectList.BREAKOBJECT);
        obj.transform.position = transform.position;

        Vector3 direction = (transform.position - hitPosition).normalized;

        BreakObject bo = obj.GetComponent<BreakObject>();
        if (bo != null) {
            bo.Speed = UnityEngine.Random.Range(120, 150);

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(0.3f, 0.3f);
            bo.Direction = direction + randomRange;
            bo.OnDead();
        }
    }

    protected void MetalObject() {
        GameObject obj = ObjectManager.Instance.UseObject(ObjectList.METALOBJECT);
        obj.transform.position = transform.position;

        MetalObject mo = obj.GetComponent<MetalObject>();
        if (mo != null) {
            mo.Price = price;
            mo.OnDrop();
        }
    }
}
