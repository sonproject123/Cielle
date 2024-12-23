using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats {
    private void Update() {
        Muzzle();

        if (isInAttackRange && !isAttack) {
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
