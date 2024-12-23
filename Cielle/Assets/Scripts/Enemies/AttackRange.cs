using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {
    [SerializeField] GameObject master;
    private EnemyStats es;

    private void Start() {
        es = master.GetComponent<EnemyStats>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            es.AttackRange(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            es.AttackRange(false);
    }
}
