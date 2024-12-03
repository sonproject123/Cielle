using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour {
    [SerializeField] Rigidbody rigidBody;

    public Action<float, Vector3> OnDead;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        OnDead = (float speed, Vector3 direction) => { ObjectForce(speed, direction); };
    }

    private void ObjectForce(float speed, Vector3 direction) {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall") || other.CompareTag("Ground") || other.CompareTag("Thin Ground")) {
            ObjectManager.Instance.ReturnObject(gameObject, "BREAKOBJECT");
        }
    }
}
