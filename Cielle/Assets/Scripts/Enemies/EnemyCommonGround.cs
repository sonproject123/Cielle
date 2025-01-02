using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCommonGround : Enemy {
    protected override GeneralFSM<Enemy> InitialState() {
        return new EnemyState_InPatrol<Enemy>(this);
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
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(Attacking());
        }
    }

    protected abstract IEnumerator Attacking();
}
