using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneralStats : Singleton<GeneralStats> {
    [SerializeField] bool pause = false;

    public bool Pause {
        get { return pause; }
        set { pause = value; }
    }
}
