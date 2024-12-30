using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] Quaternion fixRotation;

    [SerializeField] Slider reloadBar;

    private void Awake() {
        fixRotation = transform.rotation;

        reloadBar.gameObject.SetActive(false);
    }

    private void LateUpdate() {
        transform.rotation = fixRotation;
    }

    public void Reload(bool state) {
        reloadBar.gameObject.SetActive(state);
        reloadBar.maxValue = 1;
        reloadBar.value = 0;
    }

    public void Reloading(float time, float maxTime) {
        reloadBar.maxValue = maxTime;
        reloadBar.value = time;
    }
}
