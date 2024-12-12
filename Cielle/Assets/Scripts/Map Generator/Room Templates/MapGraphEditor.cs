using UnityEngine;
using UnityEditor;

public class MapGraphEditor : EditorWindow {
    private MapGraph graph;
    private MapGraphNode currentNode;
    private Vector2 offset;
    private bool isOnDrag;

    [MenuItem("Tools/Map Graph Editor")]
    public static void ShowWindow() {
        GetWindow<MapGraphEditor>("Map Graph Editor");
    }

    private void OnGUI() {
        if (graph == null) {
            if (GUILayout.Button("Create New Graph"))
                graph = new MapGraph();
            return;
        }

        if (GUILayout.Button("Create Child") && currentNode != null) {
            MapGraphNode newNode = new MapGraphNode("New");
            newNode.size.position = new Vector2(currentNode.size.x + 10, currentNode.size.y + 60);
            graph.AddChild(currentNode, newNode);
        }

        DrawNodes();
        DrawConnections();
    }

    private void DrawNodes() {
        Event ev = Event.current;

        if (graph.root != null) {
            DrawNode(graph.root);
            DrawChildNodes(graph.root);
        }

        if (isOnDrag) { 
            currentNode.size.position = ev.mousePosition - offset;
            Repaint();
        }

        if (ev.type == EventType.MouseDown && ev.button == 0) {
            if (graph.root.size.Contains(ev.mousePosition)) 
                CurrentNodeSet(graph.root);
            else
                CurrentNodeChildSet(graph.root);
        }

        if (ev.type == EventType.MouseUp && ev.button == 0)
            isOnDrag = false;
    }

    private void CurrentNodeChildSet(MapGraphNode parent) {
        foreach (var node in parent.child) {
            if (node.size.Contains(Event.current.mousePosition)) {
                CurrentNodeSet(node);
                break;
            }
            else
                CurrentNodeChildSet(node);
        }
    }

    private void CurrentNodeSet(MapGraphNode node) {
        currentNode = node;
        offset = Event.current.mousePosition - currentNode.size.position;
        isOnDrag = true;
    }

    private void DrawChildNodes(MapGraphNode parent) {
        foreach (var node in parent.child) {
            DrawNode(node);
            DrawChildNodes(node);
        }
    }

    private void DrawNode(MapGraphNode node) {
        Color origianlColor = GUI.color;
        if (node == currentNode)
            GUI.color = Color.blue;

        GUI.Box(node.size, node.type);

        if (Event.current.type == EventType.MouseDown && node.size.Contains(Event.current.mousePosition) && Event.current.clickCount == 2) {
            string newName = EditorGUI.TextField(new Rect(node.size.x, node.size.y, node.size.width, 20), node.type);
            node.type = newName;
        }

        GUI.color = origianlColor;

        if (Event.current.type == EventType.MouseDown && node.size.Contains(Event.current.mousePosition))
            currentNode = node;
    }

    private void DrawConnections() {
        if (graph.root == null)
            return;

        foreach(var node in graph.root.child) {
            DrawNodeLine(graph.root.size, node.size);
            DrawChildConnections(node);
        }
    }

    private void DrawChildConnections(MapGraphNode parent) {
        foreach(var child in parent.child) {
            DrawNodeLine(parent.size, child.size);
            DrawChildConnections(child);
        }
    }

    private void DrawNodeLine(Rect from, Rect to) {
        Vector3 start = new Vector3(from.x + from.width / 2, from.y + from.height, 0);
        Vector3 end = new Vector3(to.x + to.width / 2, to.y, 0);
        Handles.DrawLine(start, end);
        DrawArrowHead(start, end);
    }

    private void DrawArrowHead(Vector3 start, Vector3 end) {
        Vector3 direction = (end - start).normalized;
        Vector3 left = direction * 10;
        Vector3 right = direction * -10;
        left = Quaternion.Euler(0, 0, -135) * left;
        right = Quaternion.Euler(0, 0, -45) * right;

        Vector3 arrowLeft = end + left;
        Vector3 arrowRight = end + right;

        Handles.DrawLine(end, arrowLeft);
        Handles.DrawLine(end, arrowRight);
    }
}
