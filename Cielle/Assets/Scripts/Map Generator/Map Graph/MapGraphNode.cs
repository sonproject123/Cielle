using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGraphNode {
    public string id;
    public string type;
    public int depth;
    public List<MapGraphNode> child;
    public Rect size;

    public MapGraphNode(string type, MapGraphNode parent = null) {
        this.type = type;
        if (parent != null)
            this.depth = parent.depth + 1;
        else
            this.depth = 1;
        child = new List<MapGraphNode>();
        size = new Rect(100, 100, 125, 50);
    }
}
