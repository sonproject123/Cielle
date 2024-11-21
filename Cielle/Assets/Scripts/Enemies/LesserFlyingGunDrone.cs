using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats {
    

    protected override void Start() {
        base.Start();
        maxHp = 100;
        hp = maxHp;
        speed = 0;
        attack = 10;
        defense = 0;
        cooltime = 3;
        bulletSpeed = 5;
    }

    private void Update() {
        Muzzle();

        if (inRange && !isAttack) {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        yield return CoroutineCache.WaitForSecond(cooltime);
        LinearBulletSpawn();
        isAttack = false;
    }

    private void Muzzle() {
        float angle = MathCalculator.Instance.Angle(player.position, muzzleRotation.position);
        muzzleRotation.rotation = Quaternion.Euler(0, 0, angle);
    }
}
