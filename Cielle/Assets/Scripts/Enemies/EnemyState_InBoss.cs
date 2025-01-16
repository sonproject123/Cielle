using UnityEngine;

public class EnemyState_InBoss<T> : GeneralFSM<T> where T : Enemy {
    public EnemyState_InBoss(T enemy) : base(enemy) { }

    public override void OnStateEnter() {
        return;
    }

    public override void OnStateStay() {
        return;
    }

    public override void OnStateExit() {
        return;
    }
}
