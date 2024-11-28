using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InputManager : Singleton<InputManager> {
    public Action action;

    void Update() {
        if (Input.anyKey == false && Input.GetAxis("Mouse ScrollWheel") == 0) {
            Stats.Instance.IsMove = false;
            return;
        }

        action?.Invoke();
    }
}