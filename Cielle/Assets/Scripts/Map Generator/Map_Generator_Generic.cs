using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Map_Generator_Generic : MonoBehaviour {
    [SerializeField] protected string stage;
    [SerializeField] RoomTemplate start = null;
    [SerializeField] List<MapGraphSO> graphs = new List<MapGraphSO>();
    [SerializeField] Dictionary<string, List<RoomTemplate>> roomTemplates = new Dictionary<string, List<RoomTemplate>>();

    protected virtual void Awake() {
        LoadMapGraphs();
        LoadRoomTemplates();
        ShowGraph();
    }

    private void LoadMapGraphs() {
        string folderPath = "Assets/Resources/Rooms/" + stage;
        folderPath = Path.Combine(folderPath, "MapGraphs");
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            Debug.LogError($"Map_Generator에서 폴더 경로를 찾지 못함: {folderPath}");
            return;
        }

        string[] templateGUIDs = AssetDatabase.FindAssets("t:MapGraphSO", new[] { folderPath });

        foreach (string guid in templateGUIDs) {
            string templatePath = AssetDatabase.GUIDToAssetPath(guid);
            MapGraphSO mapGraph = AssetDatabase.LoadAssetAtPath<MapGraphSO>(templatePath);

            if (mapGraph != null)
                graphs.Add(mapGraph);
        }
    }

    private void LoadRoomTemplates() {
        string folderPath = "Assets/Resources/Rooms/" + stage;
        folderPath = Path.Combine(folderPath, "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            Debug.LogError($"Map_Generator에서 폴더 경로를 찾지 못함: {folderPath}");
            return;
        }

        string[] templateGUIDs = AssetDatabase.FindAssets("t:RoomTemplate", new[] { folderPath });

        foreach (string guid in templateGUIDs) {
            string templatePath = AssetDatabase.GUIDToAssetPath(guid);
            RoomTemplate roomTemplate = AssetDatabase.LoadAssetAtPath<RoomTemplate>(templatePath);

            if (roomTemplate != null) {
                if(roomTemplate.type == "Start") {
                    start = roomTemplate;
                    continue;
                }

                List<RoomTemplate> temp;
                if(roomTemplates.TryGetValue(roomTemplate.type, out temp)) {
                    temp.Add(roomTemplate);
                }
                else {
                    temp = new List<RoomTemplate>();
                    temp.Add(roomTemplate);
                    roomTemplates.Add(roomTemplate.type, temp);
                }
            }
        }
    }

    private void ShowGraph() {
        MapGraph temp = graphs[0].graph;
        MapGraphNode root = temp.root;

        Debug.Log("root" + root.depth + root.type);
        foreach(var node in root.child)
        {
            ShowChildGraph(root, node);
        }
    }

    private void ShowChildGraph(MapGraphNode parent, MapGraphNode node) {
        Debug.Log(parent.type + " of " + node.depth + node.type);
        foreach (var child in node.child)
        {
            ShowChildGraph(node, child);
        }
    }
}
