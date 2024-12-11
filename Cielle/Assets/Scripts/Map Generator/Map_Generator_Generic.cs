using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Map_Generator_Generic : MonoBehaviour {
    [SerializeField] protected string stage;
    [SerializeField] List<RoomTemplate> roomTemplates = new List<RoomTemplate>();

    protected virtual void Awake() {
        string folderPath = "Assets/Resources/Rooms/" + stage;
        folderPath = Path.Combine(folderPath, "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            Debug.LogError($"Map_Generator���� ���� ��θ� ã�� ����: {folderPath}");
            return;
        }

        string[] templateGUIDs = AssetDatabase.FindAssets("t:RT", new[] { folderPath });

        foreach(string guid in templateGUIDs) {
            string templatePath = AssetDatabase.GUIDToAssetPath(guid);
            RoomTemplate roomTemplate = AssetDatabase.LoadAssetAtPath<RoomTemplate>(templatePath);

            if (roomTemplate != null) {
                roomTemplates.Add(roomTemplate);
            }
        }
    }
}
