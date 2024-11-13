using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats {
    protected override void Start() {
        base.Start();
        maxHp = Random.Range(100.0f, 110.0f);
        hp = maxHp;
        speed = 0;
        attack = 10;
        defense = 0;
        cooltime = 3;
        bulletSpeed = 5;

        StartCoroutine(Attack());
    }

    private void Update() {
        Muzzle();
    }

    IEnumerator Attack() {
        while (true) {
            yield return CoroutineCache.WaitForSecond(cooltime);

            if(inRange)
                LinearBulletSpawn();
        }
    }

    private void Muzzle() {
        float angle = MathCalculator.Instance.Angle(player.position, muzzleRotation.position);
        muzzleRotation.rotation = Quaternion.Euler(0, 0, angle);
    }
}
