using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Generator_Sewer : Map_Generator_Generic {
    protected override void Awake() {
        stage = "Sewer";
        base.Awake();
    }
}
