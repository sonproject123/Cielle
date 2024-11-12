using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {
    [SerializeField] GameObject master;
    private IInRange iMaster;

    private void Start() {
        iMaster = master.GetComponent<IInRange>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            iMaster.InRange(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            iMaster.InRange(false);
    }
}
