using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData {
    public int id;
    public string name;
    public string descriptions;
    public float atk;
    public float atkShield;
    public float minSpeed;     // 총알 속도
    public float maxSpeed;
    public float cooltime;     // 발사 간격
    public float reload;       // 재장전 시간
    public float recoil;       // 조준점 반동
    public float playerRecoil; // 공중 발사 시 반동
    public float stopping;     // 적 반동
    public float stoppingTime; // 적 반동 시간
    public int bullet;         // 탄창 하나당 총알 수
    public string type;        // 단발, 연발
    public string code;
    public string bulletCode;
}

[System.Serializable]
public class GunDataList {
    public List<GunData> gunsData = new List<GunData>();
}