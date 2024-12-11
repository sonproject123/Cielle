using UnityEngine;
using UnityEditor;
using System.IO;

public class RoomTemplateAutoCreator : MonoBehaviour {
    static string stageName = "Sewer";

    [MenuItem("Tools/Room Template Creation")]
    private static void CreateRoomTemplates() {
        string folderPath = "Assets/Resources/Rooms/" + stageName;
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            Debug.LogError($"경로가 올바르지 않음: {folderPath}");
            return;
        }

        string[] roomPrefabs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        string outputFolder = Path.Combine(folderPath, "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(outputFolder))
            AssetDatabase.CreateFolder(folderPath, "ScriptableObjects");

        foreach (string guid in roomPrefabs) {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null) {
                string roomName = prefab.name;
                int index = roomName.IndexOf("_");
                roomName = roomName.Substring(0, index);
                if (roomName == "Element")
                    continue;

                RoomTemplate roomTemplate = ScriptableObject.CreateInstance<RoomTemplate>();
                roomTemplate.room = prefab;
                roomTemplate.Initialize();

                string templatePath = Path.Combine(outputFolder, prefab.name + "_RT.asset");
                AssetDatabase.CreateAsset(roomTemplate, templatePath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("룸 템플릿 생성 완료");
    }
}
