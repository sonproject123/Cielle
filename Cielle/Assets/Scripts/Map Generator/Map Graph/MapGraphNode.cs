using System.Collections.Generic;
using UnityEngine;

public class MapGraphNode {
    public string type;
    public List<MapGraphNode> child;
    public Rect size;

    public MapGraphNode(string type) {
        this.type = type;
        child = new List<MapGraphNode>();
        size = new Rect(60, 50, 125, 50);
    }
}
