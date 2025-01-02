using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserLaserDrone : EnemyStats {
    protected override GeneralFSM<EnemyStats> InitialState() {
        return new EnemyState_InPatrol<EnemyStats>(this);
    }
    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        CommonHit(damage, damageShield, stoppingPower, stoppingTime, hitPosition);
    }

    public override void Patrol() {
        CommonPatrol();
    }

    public override void Chase() {
        CommonChase();
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

        while (time < 0.5) {
            if (BackEdgeCheck())
                break;

            rigidBody.MovePosition(rigidBody.position + moveDirection * speed * -3f * Time.deltaTime);

            time += Time.deltaTime;
            yield return wffu;
        }

        Vector3 rayDir;
        if (isThisLeft)
            rayDir = Vector3.left;
        else
            rayDir = Vector3.right;

        RaycastHit rayHit;
        if (Physics.Raycast(muzzle.position, rayDir, out rayHit, 9999, LayerMask.GetMask("Player"))) {
            IHitable hitable = rayHit.collider.gameObject.GetComponent<IHitable>();
            hitable.Hit(attack, attackShield, stoppingPower, stoppingTime, transform.position);
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
