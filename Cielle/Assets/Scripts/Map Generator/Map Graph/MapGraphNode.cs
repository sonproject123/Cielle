using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGraphNode {
    public string type;
    public List<MapGraphNode> child;
    public Rect size;

    public MapGraphNode(string type) {
        this.type = type;
        child = new List<MapGraphNode>();
        size = new Rect(60, 50, 150, 50);
    }
}
