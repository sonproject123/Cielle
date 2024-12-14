using System.Collections.Generic;
using UnityEngine;

public class MapGraphNode {
    public string type;
    public int depth;
    public MapGraphNode parent;
    public List<MapGraphNode> child;
    public Rect size;

    public MapGraphNode(string type, MapGraphNode parent) {
        this.type = type;
        this.parent = parent;
        if (parent != null)
            this.depth = parent.depth + 1;
        else
            this.depth = 1;
        child = new List<MapGraphNode>();
        size = new Rect(100, 100, 125, 50);
    }
}
