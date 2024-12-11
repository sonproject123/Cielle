using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Map_Generator_Generic : MonoBehaviour {
    [SerializeField] protected string stage;
    [SerializeField] RoomTemplate start = null;
    [SerializeField] List<RoomTemplate> rooms = new List<RoomTemplate>();
    [SerializeField] List<RoomTemplate> monsters = new List<RoomTemplate>();
    [SerializeField] List<RoomTemplate> corridors = new List<RoomTemplate>();
    [SerializeField] List<RoomTemplate> goals = new List<RoomTemplate>();
    [SerializeField] List<RoomTemplate> deadends = new List<RoomTemplate>();

    protected virtual void Awake() {
        string folderPath = "Assets/Resources/Rooms/" + stage;
        folderPath = Path.Combine(folderPath, "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            Debug.LogError($"Map_Generator에서 폴더 경로를 찾지 못함: {folderPath}");
            return;
        }

        string[] templateGUIDs = AssetDatabase.FindAssets("t:RoomTemplate", new[] { folderPath });

        foreach(string guid in templateGUIDs) {
            string templatePath = AssetDatabase.GUIDToAssetPath(guid);
            RoomTemplate roomTemplate = AssetDatabase.LoadAssetAtPath<RoomTemplate>(templatePath);

            if (roomTemplate != null) {
                switch (roomTemplate.type) {
                    case "Start":
                        start = roomTemplate;
                        break;
                    case "Room":
                        rooms.Add(roomTemplate);
                        break;
                    case "Corridor":
                        corridors.Add(roomTemplate);
                        break;
                    case "Monster":
                        monsters.Add(roomTemplate);
                        break;
                    case "DeadEnd":
                        deadends.Add(roomTemplate);
                        break;
                    case "Goal":
                        rooms.Add(roomTemplate);
                        break;
                }
            }
        }
    }
}
