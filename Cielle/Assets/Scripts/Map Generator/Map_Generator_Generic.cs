using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Map_Generator_Generic : MonoBehaviour {
    [SerializeField] protected Transform player;
    [SerializeField] protected string stage;
    [SerializeField] protected RoomTemplate start = null;
    [SerializeField] protected Dictionary<string, List<RoomTemplate>> roomTemplates = new Dictionary<string, List<RoomTemplate>>();
    [SerializeField] protected MapGraph graph = null;
    [SerializeField] List<MapGraphSO> graphs = new List<MapGraphSO>();

    protected virtual void Awake() {
        LoadMapGraphs();
        LoadRoomTemplates();
        SelectGraph();
    }

    protected void ReGenerate() {
        SelectGraph();
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

    private void SelectGraph() {
        System.Random random = new System.Random();
        graph =  graphs[random.Next(0, graphs.Count)].graph;
    }

    protected GameObject RoomInstantiate(GameObject room) {
        GameObject clone = Object.Instantiate(room, transform);

        int index = clone.name.IndexOf("(Clone)");
        if (index > 0)
            clone.name = clone.name.Substring(0, index);

        return clone;
    }

    protected virtual void GenerateMap() {
        RoomTemplate startTemplate = start;
        GameObject startRoom = RoomInstantiate(startTemplate.room);
        startRoom.transform.position = Vector3.zero;
        Transform playerSpawnPoint = startRoom.transform.Find("Spawn Point").transform;
        player.position = playerSpawnPoint.position;
    }

    protected void GenerateRoom(RoomTemplate template, Transform parentTransform, int parentDoorDir) {
        GameObject room = RoomInstantiate(template.room);

        switch (parentDoorDir) {
            case 0:

                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
        room.transform.position = Vector3.zero;
    }
}
