using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour {
    [SerializeField] Collider metalCollider;
    [SerializeField] Transform target;
    [SerializeField] int price;

    public Action OnPlayerAceessed;

    private void Awake() {
        metalCollider = GetComponent<Collider>();
        OnPlayerAceessed = () => { GoToPlayer(); };
    }

    public void GoToPlayer() {
        metalCollider.isTrigger = true;
    }

    IEnumerator Move() {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        yield return wffu;
    }

    public int Price {
        get { return price; }
        set { price = value; }
    }
}
