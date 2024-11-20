using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 target;

    [SerializeField] protected Transform bulletRotation;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected Guns guns;
    [SerializeField] protected ObjectList objType;

    private void Start() {
        Vector3 direction = (target - transform.position).normalized;
        bulletRotation.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack, transform.position);
            ObjectManager.Instance.ReturnObject(gameObject, objType);
        }
        else if (other.CompareTag("Wall"))
            ObjectManager.Instance.ReturnObject(gameObject, objType);
    }

    public float Atk {
        get { return attack; }
        set { attack = value; }
    }

    public float Speed {
        get { return speed; } 
        set { speed = value; } 
    }

    public Vector3 Target {
        get { return target; }
        set { target = value; }
    }

    public Guns Guns {
        get { return guns; }
        set { guns = value; }
    }

    public ObjectList ObjType {
        get { return objType; }
        set { objType = value; }
    }
}
