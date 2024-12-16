using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGraph {
    public MapGraphNode root;
    public List<MapGraphNode> allNodes;

    public MapGraph() {
        root = new MapGraphNode("Start");
        root.depth = 1;

        allNodes = new List<MapGraphNode>();
        allNodes.Add(root);
    }

    public MapGraphNode FindNode(string nodeID) {
        foreach (var node in allNodes) { 
            if(node.id == nodeID)
                return node;
        }

        return null;
    }

    public void AddChild(MapGraphNode node, MapGraphNode parent) {
        allNodes.Add(node);
        parent.AddChild(node);
    }

    public void RemoveNode(string nodeID) {
        foreach (var node in allNodes) {
            if (node.id == nodeID) {
                allNodes.Remove(node);
                return;
            }
        }
    }

    public void RemoveNodes(MapGraphNode node, MapGraphNode parent = null) {
        while (node.child.Count > 0) {
            MapGraphNode next = FindNode(node.child[^1]);
            RemoveNodes(next, node);
        }

        node.child.Clear();

        if (parent != null) {
            parent.RemoveChild(node.id);
            RemoveNode(node.id);
        }
    }
}
