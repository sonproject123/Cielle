using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData_Gun {
    public int id;
    public string type;
    public string code;
}

[System.Serializable]
public class ItemDataList {
    public List<ItemData_Gun> itemsData = new List<ItemData_Gun>();
}