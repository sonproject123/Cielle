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
    [SerializeField] protected float attack;
    [SerializeField] protected float defense;
    [SerializeField] protected float speed;
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
        attack = 1;
        defense = 0;
        cooltime = 1;
        bulletSpeed = 5;
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

    public void Hit(float damage, Vector3 hitPosition) {
        hp -= Mathf.Max(1, damage - defense);
        enemyUI.HpBar();

        if (hp <= 0.0 && !isDead) {
            isDead = true;

            for (int i = 0; i < 10; i++)
                BreakObject(hitPosition);

            for (int i = 0; i < 10; i++)
                MetalObject();

            Destroy(gameObject);
        }
    }

    public void InRange(bool value) {
        inRange = value;
    }

    protected void LinearBulletSpawn() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.ENEMYBULLET);
        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = muzzle.transform.rotation;

        BulletEnemy bulletEnemy = bullet.GetComponent<BulletEnemy>();
        if (bulletEnemy != null) {
            bulletEnemy.Atk = attack + UnityEngine.Random.Range(1, 10);
            bulletEnemy.Speed = bulletSpeed;
            bulletEnemy.Target = player.position;
        }
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
