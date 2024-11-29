using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplosionData {
    public int id;
    public float atk;
    public float atkShield;
    public float stopping;
    public float stoppingTime;
    public string code;
}

[System.Serializable]
public class ExplosionDataList {
    public List<ExplosionData> explosionsData = new List<ExplosionData>();
}