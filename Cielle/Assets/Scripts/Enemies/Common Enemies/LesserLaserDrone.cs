using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserLaserDrone : EnemyStats {
    protected override GeneralFSM<EnemyStats> InitialState() {
        return new EnemyState_InPatrol<EnemyStats>(this);
    }

    public override void Attack() {
    }

    public override void Chase() {
    }

    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        CommonHit(damage, damageShield, stoppingPower, stoppingTime, hitPosition);
    }

    public override void Patrol() {
    }
}
