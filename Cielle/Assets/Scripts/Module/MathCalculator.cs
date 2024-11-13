using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCalculator : Singleton<MathCalculator> {
    public float Angle(Vector3 a, Vector3 b) {
        Vector3 r = (a - b).normalized;
        float angle = Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;
        return angle;
    }
}
