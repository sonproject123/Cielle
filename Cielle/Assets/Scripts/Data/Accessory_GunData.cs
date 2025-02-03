using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Accessory_GunData {
    public int id;
    public string name;
    public string description;
    public float value;
    public string type;
    public string code;
    public string exclusive;
    public string rarity;
}

[System.Serializable]
public class Accessory_GunDataList {
    public List<Accessory_GunData> accessory_GunsData = new List<Accessory_GunData>();
}
