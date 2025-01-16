using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator_Sewer : MapGenerator_Generic {
    protected override void Awake() {
        stage = "Sewer";

        enemies.Add(1);
        enemies.Add(2);
        bossID = 80;

        base.Awake();
    }
}
