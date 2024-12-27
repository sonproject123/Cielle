using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGenerator_Generic : MonoBehaviour {
    [SerializeField] protected Transform player;
    [SerializeField] protected string stage;
    [SerializeField] protected RoomTemplate start = null;
    [SerializeField] protected Dictionary<string, List<RoomTemplate>> roomTemplates = new Dictionary<string, List<RoomTemplate>>();
    [SerializeField] protected MapGraph graph = null;
    [SerializeField] List<MapGraphSO> graphs = new List<MapGraphSO>();
    [SerializeField] Collider2D[] colliders;

    [SerializeField] protected List<GameObject> generatedRooms = new List<GameObject>();

    protected virtual void Awake() {
        LoadMapGraphs();
        LoadRoomTemplates();
        SelectGraph();
        GenerateMap();
    }

    public void ReGenerate() {
        DestrotMap();
        SelectGraph();
        GenerateMap();
    }

    protected void DestrotMap() {
        foreach(var room in generatedRooms) {
            if (room != null)
                Destroy(room);
        }

        generatedRooms.Clear();
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
        graph = graphs[random.Next(0, graphs.Count)].graph;
    }

    protected GameObject RoomInstantiate(GameObject room) {
        GameObject clone = Object.Instantiate(room, transform);

        int index = clone.name.IndexOf("(Clone)");
        if (index > 0)
            clone.name = clone.name.Substring(0, index);

        return clone;
    }

    protected async void GenerateMap() {
        bool isGeneratedAll = await GenerateRoom(graph.root, start, null, Vector3.zero, Vector3.zero, null, 1, " ", true);
        while (!isGeneratedAll)
            ReGenerate();
    }

    private async UniTask<bool> GenerateRoom(MapGraphNode node, RoomTemplate genRoomRT, RoomTemplateStats parentRTS, Vector3 parentPosition, Vector3 parentSize, Transform parentDoorPosition, int parentDoorDir, string parentID, bool isStart) {
        GameObject room = RoomInstantiate(genRoomRT.room);
        RoomTemplateStats genRoomRTS = room.GetComponent<RoomTemplateStats>();
        genRoomRTS.Initialize(genRoomRT);
        int roomDoorIndex = (parentDoorDir + 2) % 4;
        Transform roomDoor = genRoomRTS.doors[roomDoorIndex];

        if (isStart)
            room.transform.position = Vector3.zero;
        else
            RoomPosition(room, roomDoor, genRoomRTS, genRoomRT, parentPosition, parentSize, parentDoorPosition, parentDoorDir, parentID);

        bool isNotCollide = await CollideJudge(room, genRoomRT, genRoomRTS, parentID);

        if (!isNotCollide) {
            Destroy(room);
            return false;
        }

        generatedRooms.Add(room);
        if (parentRTS != null)
            parentRTS.childRooms.Add(room);

        bool isChildPlaced = await GenerateNextRoom(node, genRoomRT, genRoomRTS, roomDoorIndex);
        if (!isStart && !isChildPlaced) {
            DestroyChildRooms(genRoomRTS);
            parentRTS.childRooms.Remove(room);
            generatedRooms.Remove(room);
            Destroy(room);
            return false;
        }

        if (isStart) {
            Transform playerSpawnPoint = room.transform.Find("Spawn Point").transform;
            player.position = playerSpawnPoint.position;
        }

        return true;
    }

    private void RoomPosition(GameObject room, Transform roomDoor, RoomTemplateStats genRoomRTS, RoomTemplate genRoomRT, Vector3 parentPosition, Vector3 parentSize, Transform parentDoorPosition, int parentDoorDir, string parentID) {
        float offsetX = 0;
        float offsetY = 0;

        switch (parentDoorDir) {
            case 0:
                offsetX = parentPosition.x + (parentDoorPosition.localPosition.x - roomDoor.localPosition.x);
                offsetY = parentPosition.y + (parentSize.y / 2 + genRoomRT.size.y / 2);
                break;
            case 1:
                offsetY = parentPosition.y + (parentDoorPosition.localPosition.y - roomDoor.localPosition.y);
                offsetX = parentPosition.x + (parentSize.x / 2 + genRoomRT.size.x / 2);
                break;
            case 2:
                offsetX = parentPosition.x + (parentDoorPosition.localPosition.x - roomDoor.localPosition.x);
                offsetY = parentPosition.y - (parentSize.y / 2 + genRoomRT.size.y / 2);
                break;
            case 3:
                offsetY = parentPosition.y + (parentDoorPosition.localPosition.y - roomDoor.localPosition.y);
                offsetX = parentPosition.x - (parentSize.x / 2 + genRoomRT.size.x / 2);
                break;
        }

        room.transform.position = new Vector3(offsetX, offsetY, 0);
    }

    private async UniTask<bool> CollideJudge(GameObject room, RoomTemplate genRoomRT, RoomTemplateStats genRoomRTS, string parentID) {
        await UniTask.Delay((int)(Time.fixedDeltaTime * 1000));
        
        Vector2 point = new Vector2(room.transform.position.x, room.transform.position.y);
        colliders = Physics2D.OverlapBoxAll(point, genRoomRT.size, 0);

        foreach (var collider in colliders) {
            RoomTemplateStats colliderRTS = collider.gameObject.GetComponent<RoomTemplateStats>();
            if (colliderRTS == null || colliderRTS.id == parentID || colliderRTS.id == genRoomRTS.id)
                continue;
            else 
                return false;
        }

        return true;
    }

    private void DestroyChildRooms(RoomTemplateStats parentRTS) {
        for (int i = parentRTS.childRooms.Count - 1; i >= 0; i--) {
            var room = parentRTS.childRooms[i];
            RoomTemplateStats rts = room.gameObject.GetComponent<RoomTemplateStats>();
            DestroyChildRooms(rts);
            generatedRooms.Remove(room);
            parentRTS.childRooms.RemoveAt(i);
            Destroy(room);
        }
    }

    private async UniTask<bool> GenerateNextRoom(MapGraphNode parentNode, RoomTemplate parentRT, RoomTemplateStats parentRTS, int parentDir) {
        Transform[] doorTransforms = new Transform[4];
        bool[] doors = new bool[4];
        int nextDoor = 0;
        for (int i = 0; i < 4; i++) {
            if (i == parentDir) {
                doors[i] = false;
                doorTransforms[i] = null;
            }
            else if (parentRT.direction[i]) {
                doors[i] = true;
                doorTransforms[i] = parentRTS.doors[i];
            }
            else {
                doors[i] = false;
                doorTransforms[i] = null;
            }
        }

        foreach (var nodeID in parentNode.child) {
            MapGraphNode node = graph.FindNode(nodeID);
            string type = node.type;
            List<RoomTemplate> rooms;
            if (!roomTemplates.TryGetValue(type, out rooms)) {
                Debug.LogError("유효하지 않은 type명");
                return false;
            }

            for (int i = 0; i < 4; i++) {
                if (doors[i]) {
                    nextDoor = i;
                    break;
                }
            }

            System.Random random = new System.Random();
            int firstIndex = random.Next(0, rooms.Count);
            int index = firstIndex;
            bool isGenerated = false;
            do {
                if (!rooms[index].direction[(nextDoor + 2) % 4]) {
                    index = (index + 1) % rooms.Count;
                    continue;
                }

                isGenerated = await GenerateRoom(node, rooms[index], parentRTS, parentRTS.room.transform.position, parentRT.size, doorTransforms[nextDoor].transform, nextDoor, parentRTS.id, false);
                if (isGenerated) {
                    doors[nextDoor] = false;
                    break;
                }

                index = (index + 1) % rooms.Count;
            } while (index != firstIndex);

            if (!isGenerated)
                return false;
        }

        return true;
    }
}
