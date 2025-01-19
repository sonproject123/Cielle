using UnityEditor.SceneManagement;
using UnityEngine;

public class MapGenerator_Test : MapGenerator_Generic {
    protected override void Awake() {
        stage = "Test";
        bossID = 80;

        base.Awake();
    }
}
