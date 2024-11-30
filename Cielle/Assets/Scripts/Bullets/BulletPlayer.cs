using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingPower;
    [SerializeField] protected float stoppingTime;
    [SerializeField] protected Vector3 target;
    [SerializeField] protected Vector3 muzzlePosition;

    [SerializeField] protected Transform bulletRotation;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected Guns guns;
    [SerializeField] protected string bulletName;

    private void Start() {
        Vector3 direction = (target - transform.position).normalized;
        bulletRotation.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack, attackShield, stoppingPower, stoppingTime, muzzlePosition);
            ObjectManager.Instance.ReturnObject(gameObject, bulletName);
        }
        else if (other.CompareTag("Wall"))
            ObjectManager.Instance.ReturnObject(gameObject, bulletName);
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

    public float StoppingPower {
        get { return stoppingPower; }
        set { stoppingPower = value; }
    }

    public float StoppingTime {
        get { return stoppingTime; }
        set { stoppingTime = value; }
    }

    public Vector3 Target {
        get { return target; }
        set { target = value; }
    }
    public Vector3 MuzzlePosition {
        get { return muzzlePosition; }
        set { muzzlePosition = value; }
    }

    public Guns Guns {
        get { return guns; }
        set { guns = value; }
    }

    public string BulletName {
        get { return bulletName; }
        set { bulletName = value; }
    }
}
