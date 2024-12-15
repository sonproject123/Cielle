using System.Collections.Generic;
using UnityEngine;

public class MapGraphSO : ScriptableObject {
    public MapGraph graph;
    public int MaxDepth;

    public void Initialize() {
        graph = null;
        graph = new MapGraph();
    }

    public void CopyGraph(MapGraph editGraph) {
        Initialize();

        CopyNode(editGraph.root, graph.root);
    }

    private void CopyNode(MapGraphNode editNode, MapGraphNode thisNode) {
        thisNode.type = editNode.type;
        thisNode.depth = editNode.depth;
        thisNode.size = editNode.size;
        thisNode.child = new List<MapGraphNode>();

        CopyChild(editNode, thisNode);
    }

    private void CopyChild(MapGraphNode editNode, MapGraphNode thisNode) {
        foreach (var node in editNode.child) {
            MapGraphNode newNode = new MapGraphNode(node.type, thisNode);
            CopyNode(node, newNode);
            thisNode.child.Add(newNode);
        }
    }

}
