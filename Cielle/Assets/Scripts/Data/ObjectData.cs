using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData {
    public string name;
    public int count;
    public string path;
}

[System.Serializable]
public class ObjectDataList {
    public List<ObjectData> objectsData = new List<ObjectData>();
}