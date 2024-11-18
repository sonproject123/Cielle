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

    public Vector3 RandomTarget(float rangeX, float rangeY) {
        return new Vector3(
            Random.Range(-rangeX, rangeX),
            Random.Range(-rangeY, rangeY),
            0
        );
    }
}
