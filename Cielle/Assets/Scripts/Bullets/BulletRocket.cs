using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : BulletPlayer {
    private void OnEnable() {
        StartCoroutine(BulletMove());
    }

    IEnumerator BulletMove() {
        float bulletTime = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (bulletTime < 2.0f) {
            transform.position += bulletRotation.forward * speed * Time.deltaTime;

            bulletTime += Time.fixedDeltaTime;
            yield return wffu;
        }

        ObjectManager.Instance.ReturnObject(gameObject, bulletName);
    }
}
