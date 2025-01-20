using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserLaserDrone : EnemyCommonGround {
    public override void Patrol() {
        if (!isAttack)
            base.Patrol();
    }

    public override void Chase() {
        if (!isAttack)
            base.Chase();
    }

    protected override IEnumerator Attacking() {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < 0.5) {
            if (BackEdgeCheck())
                break;

            rigidBody.MovePosition(rigidBody.position + moveDirection * speed * -3f * Time.deltaTime);

            time += Time.deltaTime;
            yield return wffu;
        }

        Vector3 rayDir;
        RaycastHit rayHit;
        if (transform.position.x < muzzle.position.x)
            rayDir = Vector3.left;
        else
            rayDir = Vector3.right;

        LinearBulletSpawn(new Vector3(player.position.x, transform.position.y, 0));
        if (Physics.Raycast(muzzle.position, rayDir, out rayHit, 9999, LayerMask.GetMask("Player"))) {
            //IHitable hitable = rayHit.collider.gameObject.GetComponent<IHitable>();
            //hitable.Hit(attack, attackShield, stoppingPower, stoppingTime, transform.position);
        }

        while (time < cooltime) {
            yield return wffu;
            time += Time.deltaTime;
        }

        isAttack = false;
        yield break;
    }

    private bool BackEdgeCheck() {
        Vector3 rayDir = (backChecker.position - centerChecker.position).normalized;
        float rayDistance = Mathf.Abs(backChecker.position.x - centerChecker.position.x);
        if (Physics.Raycast(centerChecker.position, rayDir, rayDistance, LayerMask.GetMask("Wall")))
            return true;

        rayDir = (backBottomChecker.position - backChecker.position).normalized;
        rayDistance = Mathf.Abs(backBottomChecker.position.y - backChecker.position.y);
        if (!Physics.Raycast(backChecker.position, rayDir, rayDistance, LayerMask.GetMask("Wall")))
            return true;
        
        return false;
    }
}
