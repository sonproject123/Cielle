using System.Collections.Generic;
using UnityEngine;

public class Item_Accessory_GunObject : ItemObject {
    [SerializeField] Accessories_Gun_Type acType;

    protected override void Awake() { 
        type = ItemObjectType.ITEM_ACCESSORY_GUN;
        base.Awake();
    }

    public void Initialize(int id, bool isFree, Accessories_Gun_Type acType) {
        this.acType = acType;
        Initialize(id, isFree);
    }

    protected override void InitializeChild() {
        Dictionary<int, Item_Accessories_Gun> dataDict = ItemManager.Instance.AcGunDict(acType);
        Item_Accessories_Gun data;
        dataDict.TryGetValue(id, out data);

        iconPath = "Icons/" + data.code;
    }

    public Accessories_Gun_Type AcType { get { return acType; } }
}
