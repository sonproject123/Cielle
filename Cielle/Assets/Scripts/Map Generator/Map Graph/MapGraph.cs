using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGraph {
    public MapGraphNode root;

    public MapGraph() {
        root = new MapGraphNode("Start");
    }

    public void AddChild(MapGraphNode parent, MapGraphNode child) {
        parent.child.Add(child);
    }

    public void RemoveNodes(MapGraphNode node, MapGraphNode parent = null) {
        while (node.child.Count > 0)
            RemoveNodes(node.child[^1], node);

        node.child.Clear();

        if (parent != null)
            parent.child.Remove(node);
    }
}
