using UnityEngine;

public class EnemyState_InChase<T> : GeneralFSM<T> where T : Enemy {
    public EnemyState_InChase(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        return;
    }

    public override void OnStateStay() {
        npc.Chase();

        if (npc.isInAttackRange)
            npc.ChangeState(new EnemyState_InAttack<Enemy>(npc));
        else if(!npc.isInChaseRange)
            npc.ChangeState(new EnemyState_InPatrol<Enemy>(npc));
    }

    public override void OnStateExit() {
        // show question mark
        return;
    }
}
