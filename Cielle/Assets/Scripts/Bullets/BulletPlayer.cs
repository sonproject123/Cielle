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

    private void Start() {
        Vector3 direction = (target - transform.position).normalized;
        bulletRotation.rotation = Quaternion.LookRotation(direction);
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
}
