using UnityEngine;

public class Item_GunObject : ItemObject {

    protected override void Awake() {
        type = ItemObjectType.GUN;
        base.Awake();
    }

    protected override void InitializeChild() {
        GunData gunData;
        JsonManager.Instance.GunDict.TryGetValue(id, out gunData);

        iconPath = "Icons/" + gunData.code;
    }
}
