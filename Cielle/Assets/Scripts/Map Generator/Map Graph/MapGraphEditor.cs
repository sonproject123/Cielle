using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class MapGraphEditor : EditorWindow {
    private MapGraph graph;
    private MapGraphNode currentNode;
    private Vector2 offset;
    private Vector2 dragOffset;
    private Vector2 dragPosition;
    private bool isOnDrag;
    private bool isOnWhellDrag;
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

        ZoomDrag();

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

    private void ZoomDrag() {
        Event ev = Event.current;
        if (ev.type == EventType.MouseDown && ev.button == 2) {
            isOnWhellDrag = true;
            dragPosition = ev.mousePosition;
        }

        if (isOnWhellDrag) {
            if (ev.type == EventType.MouseDrag) {
                Vector2 delta = ev.mousePosition - dragPosition;
                dragOffset += delta;
                dragPosition = ev.mousePosition;
                Repaint();
            }

            if (ev.type == EventType.MouseUp && ev.button == 2)
                isOnWhellDrag = false;
        }
    }

    private void CreateChildNode() {
        MapGraphNode newNode = new MapGraphNode("Room");
        newNode.depth = currentNode.depth + 1;
        newNode.size.position = new Vector2(currentNode.size.x + 10, currentNode.size.y + 60);
        graph.AddChild(newNode, currentNode);
    }

    private void DeleteNode() {
        graph.RemoveNodes(currentNode, FindParent(graph.root, currentNode));
        currentNode = null;
        Repaint();
        Event.current.Use();
    }

    private MapGraphNode FindParent(MapGraphNode prevNode, MapGraphNode child) {
        if (child == graph.root)
            return null;

        MapGraphNode temp = null;
        foreach(var nodeID in prevNode.child) {
            if (nodeID == child.id)
                return prevNode;
            else {
                temp = FindParent(graph.FindNode(nodeID), child);
                if (temp != null)
                    return temp;
            }
        }

        return null;
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

        if (ev.type == EventType.MouseUp && ev.button == 0) {
            isOnDrag = false;
            foreach (var node in graph.allNodes)
                node.size.position = new Vector2(node.size.x + dragOffset.x, node.size.y + dragOffset.y);
            dragOffset = Vector2.zero;
        }
    }

    private void CurrentNodeChildSet(MapGraphNode parent) {
        foreach (var nodeID in parent.child) {
            MapGraphNode node = graph.FindNode(nodeID);
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
        foreach (var nodeID in parent.child) {
            MapGraphNode node = graph.FindNode(nodeID);
            DrawNode(node);
            DrawChildNodes(node);
        }
    }

    private void DrawNode(MapGraphNode node) {
        Color origianlColor = GUI.color;
        if (node == currentNode)
            GUI.color = Color.blue;

        
        GUI.Box(new Rect(node.size.position.x + dragOffset.x, node.size.position.y + dragOffset.y, node.size.width, node.size.height), node.type);

        GUI.color = origianlColor;

        if (Event.current.type == EventType.MouseDown && node.size.Contains(Event.current.mousePosition))
            currentNode = node;
    }

    private void DrawConnections() {
        if (graph.root == null)
            return;

        foreach(var nodeID in graph.root.child) {
            MapGraphNode node = graph.FindNode(nodeID);
            DrawNodeLine(graph.root.size, node.size);
            DrawChildConnections(node);
        }
    }

    private void DrawChildConnections(MapGraphNode parent) {
        foreach(var nodeID in parent.child) {
            MapGraphNode node = graph.FindNode(nodeID);
            DrawNodeLine(parent.size, node.size);
            DrawChildConnections(node);
        }
    }

    private void DrawNodeLine(Rect from, Rect to) {
        Vector3 start = new Vector3(from.x + from.width / 2 + dragOffset.x, from.y + from.height + dragOffset.y, 0);
        Vector3 end = new Vector3(to.x + to.width / 2 + dragOffset.x, to.y + dragOffset.y, 0);
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
