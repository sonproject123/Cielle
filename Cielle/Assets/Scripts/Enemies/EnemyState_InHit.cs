using UnityEngine;

public class EnemyState_InHit<T> : GeneralFSM<T> where T : EnemyStats {
    public EnemyState_InHit(T enemy) : base(enemy) { }
    public override void OnStateEnter() {
        return;
    }

    public override void OnStateExit() {
    }

    public override void OnStateStay() {
        return;
    }
}
