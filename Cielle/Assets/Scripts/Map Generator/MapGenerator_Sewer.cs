using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator_Sewer : MapGenerator_Generic {
    protected override void Awake() {
        stage = "Sewer";
        base.Awake();
        GenerateMap();
    }

    protected override void GenerateMap() {
        base.GenerateMap();

        
    }

    private bool GenerateChildMap(MapGraphNode parent) {
        if (parent.child.Count <= 0)
            return true;


        foreach (var nodeID in parent.child) {
            MapGraphNode node = graph.FindNode(nodeID);


        }

        return false;
    }
}
