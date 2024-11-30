using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gain : MonoBehaviour {
    [SerializeField] Transform player;

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Metal")) {
            MetalObject mo = other.GetComponent<MetalObject>();
            if (mo != null) 
                mo.OnPlayerAceessed(player);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Metal")) {
            MetalObject mo = other.GetComponent<MetalObject>();
            if (mo != null) {
                mo.OnPlayerExit();
            }
        }
    }
}
