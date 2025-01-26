using UnityEngine;

public class EnemyState_InDead<T> : GeneralFSM<T> where T : EnemyBoss {
    public EnemyState_InDead(T enemy) : base(enemy) { }
    public override void OnStateEnter() {
        npc.Dead();
    }

    public override void OnStateStay() {
        npc.OnDead();
    }

    public override void OnStateExit() {
        return;
    }
}
