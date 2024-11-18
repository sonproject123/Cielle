using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShotgun : BulletPlayer {
    private void OnEnable() {
        StartCoroutine(BulletMove());
    }

    IEnumerator BulletMove() {
        float bulletTime = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (bulletTime < 0.1f) {
            transform.position += bulletRotation.forward * speed * Time.deltaTime;

            bulletTime += Time.fixedDeltaTime;
            yield return wffu;
        }

        ObjectManager.Instance.ReturnObject(gameObject, ObjectList.PLAYERSHOTGUNBULLET);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack);
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.PLAYERSHOTGUNBULLET);
        }
        else if (other.CompareTag("Wall"))
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.PLAYERSHOTGUNBULLET);
    }
}
