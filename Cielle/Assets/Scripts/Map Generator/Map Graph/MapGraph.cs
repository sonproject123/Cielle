using System.Collections.Generic;
using UnityEngine;

public class MapGraph {
    public MapGraphNode root;

    public MapGraph() {
        root = new MapGraphNode("Start", null);
    }

    public void AddChild(MapGraphNode parent, MapGraphNode child) {
        parent.child.Add(child);
    }

    public void RemoveNodes(MapGraphNode node) {
        while (node.child.Count > 0)
            RemoveNodes(node.child[^1]);

        node.child.Clear();
        if (node.parent != null)
            node.parent.child.Remove(node);
    }
}
