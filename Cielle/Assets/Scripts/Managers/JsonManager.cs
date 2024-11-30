using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : Singleton<JsonManager> {
    [SerializeField] Dictionary<int, GunData> gunDict = new Dictionary<int, GunData>();
    [SerializeField] Dictionary<int, ExplosionData> explosionDict = new Dictionary<int, ExplosionData>();
    [SerializeField] Dictionary<string, ObjectData> objectDict = new Dictionary<string, ObjectData>();
    [SerializeField] Dictionary<int, EnemyData> enemyDict = new Dictionary<int, EnemyData>();

    private new void Awake() {
        base.Awake();
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

        jsonData = Resources.Load<TextAsset>("JsonDatas/Object");
        ObjectDataList objectData = JsonUtility.FromJson<ObjectDataList>(jsonData.text);
        foreach (var obj in objectData.objectsData)
            objectDict.Add(obj.name, obj);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Enemy");
        EnemyDataList enemyData = JsonUtility.FromJson<EnemyDataList>(jsonData.text);
        foreach (var obj in enemyData.enemysData)
            enemyDict.Add(obj.id, obj);
    }

    public Dictionary<int, GunData> GunDict {
        get { return gunDict; }
    }

    public Dictionary<int, ExplosionData> ExplosionDict {
        get { return explosionDict; }
    }

    public Dictionary<string, ObjectData> ObjectDict {
        get { return objectDict; }
    }

    public Dictionary<int, EnemyData> EnemyDict {
        get { return enemyDict; }
    }
}