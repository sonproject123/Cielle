using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStats : Singleton<GeneralStats> {
    [SerializeField] bool pause = false;
    [SerializeField] float originalFixedTime;
    [SerializeField] Vector3 mousePosition;
    [SerializeField] WaitForFixedUpdate wffu = new WaitForFixedUpdate();

    private void Start() {
        originalFixedTime = Time.fixedDeltaTime;
    }

    public bool Pause {
        get { return pause; }
        set { pause = value; }
    }

    public Vector3 MouseLocation {
        get { return mousePosition; }
        set { mousePosition = value; }
    }

    public WaitForFixedUpdate WFFU {
        get { return wffu; }
    }

    private void Update() {
        MouseLocation = MousePosition();
    }

    public Vector3 MousePosition() {
        return Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z));
    }

    public void SlowTime(float scale = 1.0f) {
        Time.timeScale = scale;
        Time.fixedDeltaTime = originalFixedTime * Time.timeScale;
    }
}
