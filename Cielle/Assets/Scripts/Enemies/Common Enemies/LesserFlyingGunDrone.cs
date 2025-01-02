using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : Enemy {
    protected override GeneralFSM<Enemy> InitialState() {
        return new EnemyState_InPatrol<Enemy>(this);
    }
    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        CommonHit(damage, damageShield, stoppingPower, stoppingTime, hitPosition);
    }

    public override void Patrol() {
        return;
    }

    public override void Chase() {
        return;
    }

    public override void Attack() {
        if (!isAttack) {
            isAttack = true;
            StartCoroutine(Attacking());
        }
    }


    IEnumerator Attacking() {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        Vector3 playerPosition = player.position;

        StartCoroutine(Muzzle(playerPosition));
        while(time < cooltime) {
            yield return wffu;

            time += Time.deltaTime;
        }

        int count = 0;
        while (count < 3) {
            Vector3 originalPosition = transform.position;
            Vector3 originalMuzzlePosition = muzzle.localPosition;
            Vector3 direction = MathCalculator.Instance.Normalizer(playerPosition, muzzle.position);
            
            LinearBulletSpawn(playerPosition);
            muzzle.localPosition += direction * -10f * Time.deltaTime;
            transform.position += direction * -10f * Time.deltaTime;
            yield return CoroutineCache.WaitForSecond(0.1f);

            muzzle.localPosition = originalMuzzlePosition;
            transform.position = originalPosition;
            yield return CoroutineCache.WaitForSecond(0.1f);
            count++;
        }

        isAttack = false;
        yield break;
    }

    IEnumerator Muzzle(Vector3 playerPosition) {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float nowAngle = muzzleRotation.eulerAngles.z;
        float angle = MathCalculator.Instance.Angle(playerPosition, muzzleRotation.position);
        float dif = 15f;

        while (angle != nowAngle) {
            if (Mathf.Abs(angle - nowAngle) < dif) {
                muzzleRotation.rotation = Quaternion.Euler(0, 0, angle);
                yield break;
            }
            else if (angle > nowAngle)
                muzzleRotation.rotation = Quaternion.Euler(0, 0, nowAngle + dif);
            else
                muzzleRotation.rotation = Quaternion.Euler(0, 0, nowAngle - dif);

            nowAngle = muzzleRotation.eulerAngles.z;
            yield return wffu;
        }
    }
}
