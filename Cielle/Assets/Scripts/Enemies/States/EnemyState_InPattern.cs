using UnityEngine;

public class EnemyState_InPattern<T> : GeneralFSM<T> where T : EnemyBoss {
    public EnemyState_InPattern(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        npc.PatternInit();
    }

    public override void OnStateStay() {
        npc.Pattern();
    }

    public override void OnStateExit() {
        return;
    }
}
