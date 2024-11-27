using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 target;

    [SerializeField] Transform bulletRotation;
    [SerializeField] Vector3 direction;

    private void Start() {
        Vector3 direction = MathCalculator.Instance.Normalizer(target, transform.position);
        bulletRotation.rotation = Quaternion.LookRotation(direction);
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

    public Vector3 Target {
        get { return target; }
        set { target = value; }
    }

    private void FixedUpdate() {
        transform.position += bulletRotation.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack, attackShield, transform.position);
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.ENEMYBULLET);
        }
        else if (other.CompareTag("Wall")) {
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.ENEMYBULLET);
        }
    }
}
