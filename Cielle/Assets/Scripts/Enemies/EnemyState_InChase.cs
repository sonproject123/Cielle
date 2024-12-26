using UnityEngine;

public class EnemyState_InChase<T> : GeneralFSM<T> where T : EnemyStats {
    public EnemyState_InChase(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        return;
    }

    public override void OnStateStay() {
        npc.Chase();

        if (npc.isInAttackRange)
            npc.ChangeState(new EnemyState_InAttack<EnemyStats>(npc));
        else if(!npc.isInChaseRange)
            npc.ChangeState(new EnemyState_InPatrol<EnemyStats>(npc));
    }

    public override void OnStateExit() {
        return;
    }
}
