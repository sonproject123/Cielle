using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour {
    [SerializeField] Collider metalCollider;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform target;
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;

    [SerializeField] int price;
    [SerializeField] bool isOnGround;

    public Action OnDrop;
    public Action OnPlayerAceessed;

    private void Awake() {
        metalCollider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();
        OnPlayerAceessed = () => { GoToPlayer(); };
        OnDrop = () => { Drop(); };

        speed = 30;
    }

    public void Drop() {
        rigidBody.AddForce(direction * 100, ForceMode.Impulse);
    }

    public void GoToPlayer() {
        if (!isOnGround)
            return;

        metalCollider.isTrigger = true;
        rigidBody.useGravity = false;

        Vector3 dir = (target.position - transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Stats.Instance.Metals += price;
            UIManager.OnMetalChange();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.METALOBJECT);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground"))
            isOnGround = true;
    }

    public int Price {
        get { return price; }
        set { price = value; }
    }

    public Vector3 Direction {
        set { direction = value; }
    }

    public Transform Target {
        set { target = value; }
    }
}
