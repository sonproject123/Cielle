using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSM {
    protected EnemyStats enemy;

    public EnemyFSM(EnemyStats enemy) {
        this.enemy = enemy;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateStay();
    public abstract void OnStateExit();
}
