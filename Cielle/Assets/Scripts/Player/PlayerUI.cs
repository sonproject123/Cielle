using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] Quaternion fixRotation;

    [SerializeField] Slider reloadBar;

    public static Action<bool> OnReloading;
    public static Action<float, float> OnReloadingTime;

    private void Awake() {
        fixRotation = transform.rotation;

        OnReloading = (bool state) => { Reload(state); };
        OnReloadingTime = (float time, float maxTime) => { Reloading(time, maxTime); };

        reloadBar.gameObject.SetActive(false);
    }

    private void LateUpdate() {
        transform.rotation = fixRotation;
    }

    private void Reload(bool state) {
        reloadBar.gameObject.SetActive(state);
        reloadBar.maxValue = 1;
        reloadBar.value = 0;
    }

    private void Reloading(float time, float maxTime) {
        reloadBar.maxValue = maxTime;
        reloadBar.value = time;
    }
}
