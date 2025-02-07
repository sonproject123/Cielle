using UnityEngine;

public class Item_Accessory_GunObject : ItemObject {
    [SerializeField] public Accessories_Gun_Type acType;

    protected override void Awake() { 
        type = ItemObjectType.ITEM_ACCESSORY_GUN;
        base.Awake();
    }

    protected override void InitializeChild() {
        Item_Accessories_Gun data;
        ItemManager.Instance.AcGunDict.TryGetValue(id, out data);
        acType = data.type;

        iconPath = "Icons/" + data.code;
    }
}
