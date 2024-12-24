using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmenyStatus {
    PATROL,
    CHASE,
    ATTACK,
}

public abstract class EnemyFSM {
    public abstract void OnStateEnter();
    public abstract void OnStateStay();
    public abstract void OnStateExit();
}
