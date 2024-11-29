using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPlayer : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
    [SerializeField] protected float stoppingPower;
    [SerializeField] protected float stoppingTime;

    [SerializeField] protected ObjectList objType;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack, attackShield, stoppingPower, stoppingTime, transform.position);
            ObjectManager.Instance.ReturnObject(gameObject, objType);
        }
    }

    public float Atk {
        get { return attack; }
        set { attack = value; }
    }

    public float AtkShield {
        get { return attackShield; }
        set { attackShield = value; }
    }

    public float StoppingPower {
        get { return stoppingPower; }
        set { stoppingPower = value; }
    }

    public float StoppingTime {
        get { return stoppingTime; }
        set { stoppingTime = value; }
    }

    public ObjectList ObjType {
        get { return objType; }
        set { objType = value; }
    }
}
