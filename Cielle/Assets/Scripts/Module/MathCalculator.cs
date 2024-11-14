using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCalculator : Singleton<MathCalculator> {
    public Vector3 Normalizer(Vector3 a, Vector3 b) {
        return (a - b).normalized;
    }

    public float Angle(Vector3 a, Vector3 b) {
        Vector3 r = Normalizer(a, b);
        float angle = Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;
        return angle;
    }

    public Vector3 MousePosition() {
        return Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z));
    }
}
