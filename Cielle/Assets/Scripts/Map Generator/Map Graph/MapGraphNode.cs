using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGraphNode {
    public string id;
    public string type;
    public int depth;
    public List<string> child;
    public Rect size;

    public MapGraphNode(string type) {
        this.id = System.Guid.NewGuid().ToString();
        this.type = type;
        child = new List<string>();
        size = new Rect(100, 100, 125, 50);
    }

    public void AddChild(MapGraphNode childNode) {
        child.Add(childNode.id);
    }

    public void RemoveChild(string childNodeID) {
        if (child.Contains(childNodeID)) 
            child.Remove(childNodeID);
    }
}
