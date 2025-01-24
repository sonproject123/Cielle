using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPatternData {
    public int id;
    public float[] patternTime;
    public float[] cooltime;
}

[System.Serializable]
public class BossPatternDataList {
    public List<BossPatternData> BossPatternsData = new List<BossPatternData>();
}
