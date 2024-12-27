using UnityEngine;

public class EnemyState_InAttack<T> : GeneralFSM<T> where T : EnemyStats {
    public EnemyState_InAttack(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        // show !
        return;
    }

    public override void OnStateStay() {
        npc.Attack();

        if(!npc.isInAttackRange)
            npc.ChangeState(new EnemyState_InChase<EnemyStats>(npc));
    }

    public override void OnStateExit() {
        return;
    }
}
