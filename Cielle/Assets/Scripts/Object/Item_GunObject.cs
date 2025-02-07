using UnityEngine;

public class Item_GunObject : ItemObject {

    protected override void Awake() {
        type = ItemObjectType.ITEM_GUN;
        base.Awake();
    }

    protected override void InitializeChild() {
        GunData data;
        JsonManager.Instance.GunDict.TryGetValue(id, out data);

        iconPath = "Icons/" + data.code;
    }
}
