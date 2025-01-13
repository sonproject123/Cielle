using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour {
    [SerializeField] Collider metalCollider;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;

    [SerializeField] int price;

    public Action<int> OnDrop;
    public Action<Transform> OnPlayerAceessed;
    public Action OnPlayerExit;

    private void Awake() {
        metalCollider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();

        OnDrop = (int prc) => { Drop(prc); };
        OnPlayerAceessed = (Transform player) => { GoToPlayer(player); };
        OnPlayerExit = () => { PlayerExit(); };

        speed = 20;
    }

    private void OnEnable() {
        rigidBody.linearVelocity = Vector3.zero;
        PlayerExit();
    }

    public void Drop(int prc) {
        price = prc;
        direction = new Vector3(UnityEngine.Random.Range(-5f, 5f), -1, 0).normalized;
        rigidBody.AddForce(direction * 0.02f, ForceMode.Impulse);
    }

    public void GoToPlayer(Transform player) {
        metalCollider.isTrigger = true;
        rigidBody.useGravity = false;

        Vector3 dir = (player.position - transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + dir * speed * Time.deltaTime);
    }

    public void PlayerExit() {
        metalCollider.isTrigger = false;
        rigidBody.useGravity = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Stats.Instance.Metals += price;
            UIManager.OnMetalChange();
            ObjectManager.Instance.ReturnObject(gameObject, "METALOBJECT");
        }
    }
}
