using UnityEngine;

public class EnemyState_InHit<T> : GeneralFSM<T> where T : Enemy {
    float damage;
    float damageShield;
    float stoppingPower;
    float stoppingTime;
    Vector3 hitPosition;

    public EnemyState_InHit(T enemy, float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) : base(enemy) {
        this.damage = damage;
        this.damageShield = damageShield;
        this.stoppingPower = stoppingPower;
        this.stoppingTime = stoppingTime;
        this.hitPosition = hitPosition;
    }

    public override void OnStateEnter() {
        npc.OnHit(damage, damageShield, stoppingPower, stoppingTime, hitPosition);

        if (npc.isInAttackRange)
            npc.ChangeState(new EnemyState_InAttack<Enemy>(npc));
        else if (npc.isInChaseRange)
            npc.ChangeState(new EnemyState_InChase<Enemy>(npc));
        else
            npc.ChangeState(new EnemyState_InPatrol<Enemy>(npc));
    }
    public override void OnStateStay() {
        return;
    }

    public override void OnStateExit() {
        return;
    }

}
