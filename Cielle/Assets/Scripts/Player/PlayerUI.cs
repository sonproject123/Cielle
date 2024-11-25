using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] Slider reloadBar;

    public static Action OnReloading;

    private void Awake() {
        OnReloading = () => { Reload(); };
        reloadBar.enabled = false;
    }

    private void Reload() {
        reloadBar.enabled = true;
        
    }
}
