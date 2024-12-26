using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats {
    protected override GeneralFSM<EnemyStats> InitialState() {
        return new EnemyState_InPatrol<EnemyStats>(this);
    }

    public override void Patrol() {

    }

    public override void Chase() {
    }

    public override void Attack() {
        if (!isAttack) {
            isAttack = true;
            StartCoroutine(AttackCoolTime());
        }
    }

    IEnumerator AttackCoolTime() {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while(time < cooltime) {
            Muzzle();
            yield return wffu;

            time += Time.deltaTime;
        }

        LinearBulletSpawn();
        isAttack = false;
    }

    private void Muzzle() {
        float angle = MathCalculator.Instance.Angle(player.position, muzzleRotation.position);
        muzzleRotation.rotation = Quaternion.Euler(0, 0, angle);
    }
}
