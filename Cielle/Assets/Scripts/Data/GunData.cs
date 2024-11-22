using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData {
    public int id;
    public string name;
    public string descriptions;
    public float atk;
    public float minSpeed;
    public float maxSpeed;
    public float cooltime;
    public float reload;
    public float recoil;
    public float playerRecoil;
    public int bullet;
    public string type;
    public string code;
}

[System.Serializable]
public class GunDataList {
    public List<GunData> gunsData = new List<GunData>();
}