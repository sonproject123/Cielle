using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Item_Rarity {
    COMMON,
    RARE,
    UNIQUE
}

public enum Accessories_Gun_Type {
    MUZZLE,
    MAGAZINE,
    SCOPE,
    BOOST,
    BULLET
}

public class ItemManager : Singleton<ItemManager> {
    [SerializeField] Dictionary<Accessories_Gun_Type, Dictionary<int, Item_Accessories_Gun>> acGunDict = new Dictionary<Accessories_Gun_Type, Dictionary<int, Item_Accessories_Gun>>();
    [SerializeField] Dictionary<int, Item_Accessories_Gun> acGunMuzzleDict = new Dictionary<int, Item_Accessories_Gun>();
    [SerializeField] Dictionary<int, Item_Accessories_Gun> acGunMagazineDict = new Dictionary<int, Item_Accessories_Gun>();
    [SerializeField] Dictionary<int, Item_Accessories_Gun> acGunScopeDict = new Dictionary<int, Item_Accessories_Gun>();
    [SerializeField] Dictionary<int, Item_Accessories_Gun> acGunBoostDict = new Dictionary<int, Item_Accessories_Gun>();
    [SerializeField] Dictionary<int, Item_Accessories_Gun> acGunBulletDict = new Dictionary<int, Item_Accessories_Gun>();

    [SerializeField] Dictionary<Accessories_Gun_Type, int> mainWeaponAcGunSlot = new Dictionary<Accessories_Gun_Type, int>();
    [SerializeField] Dictionary<Accessories_Gun_Type, int> subWeaponAcGunSlot = new Dictionary<Accessories_Gun_Type, int>();

    private new void Awake() {
        base.Awake();
        InitializeSlot();
        InitializeDict();
    }

    private void InitializeSlot() {
        mainWeaponAcGunSlot.Add(Accessories_Gun_Type.MUZZLE, 0);
        mainWeaponAcGunSlot.Add(Accessories_Gun_Type.MAGAZINE, 0);
        mainWeaponAcGunSlot.Add(Accessories_Gun_Type.SCOPE, 0);
        mainWeaponAcGunSlot.Add(Accessories_Gun_Type.BOOST, 0);
        mainWeaponAcGunSlot.Add(Accessories_Gun_Type.BULLET, 0);

        subWeaponAcGunSlot.Add(Accessories_Gun_Type.MUZZLE, 0);
        subWeaponAcGunSlot.Add(Accessories_Gun_Type.MAGAZINE, 0);
        subWeaponAcGunSlot.Add(Accessories_Gun_Type.SCOPE, 0);
        subWeaponAcGunSlot.Add(Accessories_Gun_Type.BOOST, 0);
        subWeaponAcGunSlot.Add(Accessories_Gun_Type.BULLET, 0);
    }

    private void InitializeDict() {
        acGunDict.Add(Accessories_Gun_Type.MUZZLE, acGunMuzzleDict);
        acGunDict.Add(Accessories_Gun_Type.MAGAZINE, acGunMagazineDict);
        acGunDict.Add(Accessories_Gun_Type.SCOPE, acGunScopeDict);
        acGunDict.Add(Accessories_Gun_Type.BOOST, acGunBoostDict);
        acGunDict.Add(Accessories_Gun_Type.BULLET, acGunBulletDict);

        Item_Accessories_Gun[] acGuns = Resources.LoadAll<Item_Accessories_Gun>("Items/Accessories_Gun");
        foreach(var acGun in acGuns) {
            Dictionary<int, Item_Accessories_Gun> acDict;
            acGunDict.TryGetValue(acGun.type, out acDict);

            acDict.Add(acGun.id, acGun);
        }
    }

    public void EquipAccessoryGun(ItemObject nearObject) {
        Item_Accessory_GunObject acObj = nearObject as Item_Accessory_GunObject;

        Dictionary<int, Item_Accessories_Gun> acDict;
        acGunDict.TryGetValue(acObj.acType, out acDict);

        mainWeaponAcGunSlot[acObj.acType] = acObj.ID;
        EquipmentUpdate();
        acObj.GetItem();
    }

    private void EquipmentUpdate() {
        foreach(var slot in mainWeaponAcGunSlot) {
            //ItemStats.Instance.MaxHP = 
        }
    }

    public void EquipWeapon(ItemObject nearObject, bool isReloading) {
        if (Stats.Instance.SubWeaponId == 0) {
            int currentBullet = Stats.Instance.BulletRemain;
            Stats.Instance.SubWeaponId = nearObject.ID;
            nearObject.GetItem();
            Stats.Instance.GunInit();
            UIManager.OnWeaponChange?.Invoke();
            Stats.Instance.BulletRemain = currentBullet;
            UIManager.OnBulletUse?.Invoke();
        }
        else if (GunFire.Instance.IsShootable && !isReloading) {
            int slotID = Stats.Instance.MainWeaponId;
            Vector3 objectPosition = nearObject.transform.position;
            Stats.Instance.MainWeaponId = nearObject.ID;
            nearObject.GetItem();
            Stats.Instance.GunInit();
            UIManager.OnWeaponChange?.Invoke();
            UIManager.OnBulletUse?.Invoke();

            if (nearObject.Type == ItemObjectType.ITEM_GUN) {
                GameObject obj = ObjectManager.Instance.UseObject("ITEM_GUN");
                obj.transform.position = objectPosition;
                GameObject objChild = obj.transform.Find("Interact").gameObject;

                ItemObject io = objChild.GetComponent<ItemObject>();
                if (io != null)
                    io.Initialize(slotID, true);
            }
        }
    }

    public Dictionary<int, Item_Accessories_Gun> AcGunDict { get { return acGunMuzzleDict; } }
}
