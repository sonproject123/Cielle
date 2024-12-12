using System.Collections.Generic;
using UnityEngine;

public class MapGraph {
    public MapGraphNode root;

    public MapGraph() {
        root = new MapGraphNode("Start");
    }

    public void AddChild(MapGraphNode parent, MapGraphNode child) {
        parent.child.Add(child);
    }
}
