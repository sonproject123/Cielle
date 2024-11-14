using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IHitable, IInRange {
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform muzzleRotation;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected bool inRange;

    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float attack;
    [SerializeField] protected float defense;
    [SerializeField] protected float speed;
    [SerializeField] protected float cooltime;
    [SerializeField] protected float bulletSpeed;

    protected virtual void Start() {
        maxHp = 100;
        hp = maxHp;
        speed = 10;
        attack = 1;
        defense = 0;
        cooltime = 1;
        bulletSpeed = 5;
        inRange = false;

        player = GameObject.Find("Player Center").transform;
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

    public void Hit(float damage) {
        hp -= Mathf.Max(1, defense - damage);

        if (hp <= 0.0)
        {
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
            bulletEnemy.Atk = attack + Random.Range(1, 10);
            bulletEnemy.Speed = bulletSpeed;
            bulletEnemy.Target = player.position;
        }
    }
}
