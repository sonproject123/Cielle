using UnityEngine;

public class MapGraphSO : ScriptableObject {
    public MapGraph graph;

    public void CreateNewGraph() {
        graph = new MapGraph();
    }
}
