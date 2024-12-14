using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapGraphEditor : EditorWindow {
    private MapGraph graph;
    private MapGraphNode currentNode;
    private Vector2 offset;
    private bool isOnDrag;
    private string roomPath = Application.dataPath + "/Resources/Rooms";
    private List<string> roomTypes = new List<string>();

    [MenuItem("Tools/Map Graph Editor")]
    public static void ShowWindow() {
        GetWindow<MapGraphEditor>("Map Graph Editor");
    }

    private void OnEnable() {
        roomTypes.Add("Room");
        roomTypes.Add("Crossroad");
        roomTypes.Add("Corridor");
        roomTypes.Add("Monster");
        roomTypes.Add("DeadEnd");
        roomTypes.Add("Goal");
    }

    private void OnGUI() {
        if (graph == null) {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create New Graph", GUILayout.Width(150), GUILayout.Height(30)))
                graph = new MapGraph();
            if (GUILayout.Button("Load Graph", GUILayout.Width(150), GUILayout.Height(30)))
                LoadGraph();

            GUILayout.EndHorizontal();
            return;
        }

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Create Child", GUILayout.Width(150), GUILayout.Height(30)) && currentNode != null)
            CreateChildNode();

        foreach (var type in roomTypes) {
            if (GUILayout.Button("Rename To " + type, GUILayout.Width(150), GUILayout.Height(30)) && currentNode != null && currentNode != graph.root)
                currentNode.type = type;
        }

        if (GUILayout.Button("Save Graph", GUILayout.Width(150), GUILayout.Height(30)))
            SaveGraph();
        if (GUILayout.Button("Load Graph", GUILayout.Width(150), GUILayout.Height(30)))
            LoadGraph();

        if (GUILayout.Button("Delete Node", GUILayout.Width(150), GUILayout.Height(30)) && currentNode != null)
            DeleteNode();

        GUILayout.EndVertical();

        DrawNodes();
        DrawConnections();

        if (currentNode != null && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
            DeleteNode();
    }

    private void CreateChildNode() {
        MapGraphNode newNode = new MapGraphNode("Room", currentNode);
        newNode.size.position = new Vector2(currentNode.size.x + 10, currentNode.size.y + 60);
        graph.AddChild(currentNode, newNode);
    }

    private void DeleteNode() {
        graph.RemoveNodes(currentNode);
        currentNode = null;
        Repaint();
        Event.current.Use();
    }

    private void SaveGraph() {
        if (graph == null)
            return;

        string path = EditorUtility.SaveFilePanel("Save Graph", roomPath, "NewMapGraph.asset", "asset");
        if (!string.IsNullOrEmpty(path)) {
            path = FileUtil.GetProjectRelativePath(path);
            MapGraphSO loadedSO = AssetDatabase.LoadAssetAtPath<MapGraphSO>(path);

            if (loadedSO != null) {
                loadedSO.graph = graph;
                EditorUtility.SetDirty(loadedSO);
                Debug.Log("저장됨: " + path);
            }
            else {
                MapGraphSO SO = ScriptableObject.CreateInstance<MapGraphSO>();
                SO.graph = graph;
                AssetDatabase.CreateAsset(SO, path);
                Debug.Log("다음 경로에 ScriptableObject 생성: " + path);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void LoadGraph() {
        AssetDatabase.Refresh();

        string path = EditorUtility.OpenFilePanel("Load Graph", roomPath, "asset");
        if (!string.IsNullOrEmpty(path)) {
            path = FileUtil.GetProjectRelativePath(path);
            MapGraphSO loadedSO = AssetDatabase.LoadAssetAtPath<MapGraphSO>(path);
            if (loadedSO != null) {
                graph = loadedSO.graph;
                Debug.Log("다음 경로에서 ScriptableObject 불러옴: " + path);
            }
        }
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
