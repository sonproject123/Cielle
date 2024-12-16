using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map_Generator_Sewer : Map_Generator_Generic {
    protected override void Awake() {
        stage = "Sewer";
        base.Awake();
        GenerateMap();
    }

    protected override void GenerateMap() {
        base.GenerateMap();

        Transform[] doorTransforms = new Transform[4]; 
        bool [] doors = new bool[4];
        for (int i = 0; i < 4; i++) {
            if (start.direction[i]) {
                doors[i] = true;
                doorTransforms[i] = start.doors[i];
            }
            else
                doors[i] = false;
        }

        foreach (var nodeID in graph.root.child) {
            MapGraphNode node = graph.FindNode(nodeID);
            string type = node.type;
            List<RoomTemplate> rooms;
            if (!roomTemplates.TryGetValue(type, out rooms)) {
                Debug.LogError("유효하지 않은 type명");
                return;
            }

            System.Random random = new System.Random();
            int firstIndex = random.Next(0, rooms.Count);
            int index = firstIndex;
            do {

                index = (index + 1) % rooms.Count;
            } while (index != firstIndex);
        }
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
