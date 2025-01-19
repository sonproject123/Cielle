using UnityEngine;

public class EnemyState_InWait<T> : GeneralFSM<T> where T : Enemy {
    public EnemyState_InWait(T enemy) : base(enemy) { }

    public override void OnStateEnter() { 
           return;
    }

    public override void OnStateStay() {
//npc.Wait();
    }

    public override void OnStateExit() {
        return;
    }
}