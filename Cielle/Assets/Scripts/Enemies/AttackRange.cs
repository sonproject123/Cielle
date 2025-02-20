using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {
    [SerializeField] GameObject master;
    private Enemy es;

    private void Start() {
        master = transform.parent.gameObject;
        es = master.GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (es != null && other.CompareTag("Player"))
            es.AttackRange(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            es.AttackRange(false);
    }
}
