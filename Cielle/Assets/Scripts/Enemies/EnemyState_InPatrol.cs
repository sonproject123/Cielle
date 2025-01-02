using UnityEngine;

public class EnemyState_InPatrol<T> : GeneralFSM<T> where T : Enemy {
    public EnemyState_InPatrol(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        return;
    }

    public override void OnStateStay() {
        npc.Patrol();
        
        if (npc.isInAttackRange)
            npc.ChangeState(new EnemyState_InAttack<Enemy>(npc));
    }

    public override void OnStateExit() {
        return;
    }
}
