using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : Singleton<JsonManager> {
    [SerializeField] Dictionary<int, GunData> gunDict = new Dictionary<int, GunData>();

    private void Start() {
        LoadData();
    }

    private void LoadData() {
        TextAsset jsonData = Resources.Load<TextAsset>("JsonDatas/Gun");
        GunDataList gunData = JsonUtility.FromJson<GunDataList>(jsonData.text);
        foreach (var gun in gunData.gunsData)
            gunDict.Add(gun.id, gun);
        
    }

    public Dictionary<int, GunData> GunDict {
        get { return gunDict; }
    }
}