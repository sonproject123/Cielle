using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : Singleton<JsonManager> {
    [SerializeField] Dictionary<int, GunData> gunDict = new Dictionary<int, GunData>();
    [SerializeField] Dictionary<int, ExplosionData> explosionDict = new Dictionary<int, ExplosionData>();

    private void Start() {
        LoadData();
    }

    private void LoadData() {
        TextAsset jsonData = Resources.Load<TextAsset>("JsonDatas/Gun");
        GunDataList gunData = JsonUtility.FromJson<GunDataList>(jsonData.text);
        foreach (var obj in gunData.gunsData)
            gunDict.Add(obj.id, obj);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Explosion");
        ExplosionDataList explosionData = JsonUtility.FromJson<ExplosionDataList>(jsonData.text);
        foreach (var obj in explosionData.explosionsData)
            explosionDict.Add(obj.id, obj);
    }

    public Dictionary<int, GunData> GunDict {
        get { return gunDict; }
    }

    public Dictionary<int, ExplosionData> ExplosionDict {
        get { return explosionDict; }
    }
}