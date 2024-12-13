using System.Collections.Generic;
using UnityEngine;

public class MapGraphNode {
    public string type;
    public MapGraphNode parent;
    public List<MapGraphNode> child;
    public Rect size;

    public MapGraphNode(string type, MapGraphNode parent) {
        this.type = type;
        this.parent = parent;
        child = new List<MapGraphNode>();
        size = new Rect(100, 100, 125, 50);
    }
}
