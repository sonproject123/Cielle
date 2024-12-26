using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralFSM<T> {
    protected T npc;

    public GeneralFSM(T npc) {
        this.npc = npc;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateStay();
    public abstract void OnStateExit();
}
