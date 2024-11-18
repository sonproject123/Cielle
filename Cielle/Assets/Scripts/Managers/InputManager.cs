using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InputManager : Singleton<InputManager> {
    public Action action;

    void Update() {
        if (Input.anyKey == false) {
            Stats.Instance.IsMove = false;
            return;
        }

        if (action != null)
            action.Invoke();
    }
}