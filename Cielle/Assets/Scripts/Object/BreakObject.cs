using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour {
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 direction;

    public Action OnDead;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        OnDead = () => { ObjectForce(); };
    }

    private void ObjectForce() {
        rigidBody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.BREAKOBJECT);
        }
    }

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public Vector3 Direction {
        get { return direction; }
        set { direction = value; }
    }
}
