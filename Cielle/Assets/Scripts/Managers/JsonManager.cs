using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : Singleton<JsonManager> {
    [SerializeField] Dictionary<int, GunData> gunDict = new Dictionary<int, GunData>();
    [SerializeField] Dictionary<string, ObjectData> objectDict = new Dictionary<string, ObjectData>();
    [SerializeField] Dictionary<int, ItemData_Gun> itemDict = new Dictionary<int, ItemData_Gun>();
    [SerializeField] Dictionary<int, EnemyData> enemyDict = new Dictionary<int, EnemyData>();
    [SerializeField] Dictionary<int, BossPatternData> bossPatternDict = new Dictionary<int, BossPatternData>();

    private new void Awake() {
        base.Awake();
        LoadData();
    }

    private void LoadData() {
        TextAsset jsonData = Resources.Load<TextAsset>("JsonDatas/Gun");
        GunDataList gunData = JsonUtility.FromJson<GunDataList>(jsonData.text);
        foreach (var data in gunData.gunsData)
            gunDict.Add(data.id, data);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Object");
        ObjectDataList objectData = JsonUtility.FromJson<ObjectDataList>(jsonData.text);
        foreach (var data in objectData.objectsData)
            objectDict.Add(data.name, data);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Item");
        ItemDataList itemData = JsonUtility.FromJson<ItemDataList>(jsonData.text);
        foreach (var data in itemData.itemsData)
            itemDict.Add(data.id, data);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Enemy");
        EnemyDataList enemyData = JsonUtility.FromJson<EnemyDataList>(jsonData.text);
        foreach (var data in enemyData.enemysData)
            enemyDict.Add(data.id, data);

        jsonData = Resources.Load<TextAsset>("JsonDatas/Boss Pattern");
        BossPatternDataList bossPatternData = JsonUtility.FromJson<BossPatternDataList>(jsonData.text);
        foreach (var data in bossPatternData.BossPatternsData)
            bossPatternDict.Add(data.id, data);
    }

    public Dictionary<int, GunData> GunDict {
        get { return gunDict; }
    }

    public Dictionary<string, ObjectData> ObjectDict {
        get { return objectDict; }
    }

    public Dictionary<int, ItemData_Gun> ItemDict {
        get { return itemDict; }
    }

    public Dictionary<int, EnemyData> EnemyDict {
        get { return enemyDict; }
    }

    public Dictionary<int, BossPatternData> BossPatternDict {
        get { return bossPatternDict; }
    }
}