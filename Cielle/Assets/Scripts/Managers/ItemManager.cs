using System.Collections;
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
        acGunDict.Add(Accessories_Gun_Type.MUZZLE, new Dictionary<int, Item_Accessories_Gun>());
        acGunDict.Add(Accessories_Gun_Type.MAGAZINE, new Dictionary<int, Item_Accessories_Gun>());
        acGunDict.Add(Accessories_Gun_Type.SCOPE, new Dictionary<int, Item_Accessories_Gun>());
        acGunDict.Add(Accessories_Gun_Type.BOOST, new Dictionary<int, Item_Accessories_Gun>());
        acGunDict.Add(Accessories_Gun_Type.BULLET, new Dictionary<int, Item_Accessories_Gun>());

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
        acGunDict.TryGetValue(acObj.AcType, out acDict);

        if (mainWeaponAcGunSlot[acObj.AcType] != 0) {
            GameObject obj = ObjectManager.Instance.UseObject("ITEM_ACCESSORY_GUN");
            obj.transform.position = nearObject.gameObject.transform.position;
            GameObject objChild = obj.transform.Find("Interact").gameObject;

            Item_Accessory_GunObject io = objChild.GetComponent<Item_Accessory_GunObject>();
            if (io != null)
                io.Initialize(mainWeaponAcGunSlot[acObj.AcType], true, acObj.AcType);
        }

        mainWeaponAcGunSlot[acObj.AcType] = acObj.ID;
        EquipmentUpdate();
        acObj.GetItem();
    }

    private void EquipmentUpdate() {
        float currentHp = Stats.Instance.Hp;
        float currentShield = Stats.Instance.Shield;
        int currentBullet = Stats.Instance.BulletRemain;
        ItemStats.Instance.Initialize();
        Stats.Instance.InitializedStats();

        foreach(var slot in mainWeaponAcGunSlot) {
            if (slot.Value == 0)
                continue;
            Dictionary<int, Item_Accessories_Gun> dataDict;
            acGunDict.TryGetValue(slot.Key, out dataDict);
            Item_Accessories_Gun data;
            dataDict.TryGetValue(slot.Value, out data);

            ItemStats.Instance.MaxHP += data.maxHp;
            ItemStats.Instance.MaxHPMult *= data.maxHpMult;

            ItemStats.Instance.MaxShield += data.maxShield;
            ItemStats.Instance.MaxShieldMult *= data.maxShieldMult;
            ItemStats.Instance.ShieldDefense *= data.shieldDefense;

            ItemStats.Instance.ShieldRegen *= data.shieldRegen;
            ItemStats.Instance.ShieldCooltime *= data.shieldCooltime;
            ItemStats.Instance.ShieldBreakCooltime *= data.shieldBreakCooltime;
            ItemStats.Instance.ShieldInvincible *= data.shieldInvincible;

            ItemStats.Instance.Attack *= data.attack;
            ItemStats.Instance.AttackShield *= data.attackShield;

            ItemStats.Instance.Defense *= data.defense;
            ItemStats.Instance.Invincible *= data.invincible;

            ItemStats.Instance.GunRecoil *= data.gunRecoil;
            ItemStats.Instance.ReloadTime *= data.reloadTime;
            ItemStats.Instance.BulletLife *= data.bulletLife;
            ItemStats.Instance.MaxBullet *= data.maxBullet;
        }

        Stats.Instance.MaxHp += ItemStats.Instance.MaxHP;
        Stats.Instance.MaxHp *= ItemStats.Instance.MaxHPMult;

        Stats.Instance.MaxShield += ItemStats.Instance.MaxShield;
        Stats.Instance.MaxShield *= ItemStats.Instance.MaxShieldMult;
        Stats.Instance.ShieldDef *= ItemStats.Instance.ShieldDefense;

        Stats.Instance.ShieldRegen *= ItemStats.Instance.ShieldRegen;
        Stats.Instance.ShieldCooltime *= ItemStats.Instance.ShieldCooltime;
        Stats.Instance.ShieldBreakCooltime *= ItemStats.Instance.ShieldBreakCooltime;
        Stats.Instance.ShieldInvincible *= ItemStats.Instance.ShieldInvincible;

        Stats.Instance.Atk *= ItemStats.Instance.Attack;
        Stats.Instance.AtkShield *= ItemStats.Instance.AttackShield;

        Stats.Instance.Def *= ItemStats.Instance.Defense;
        Stats.Instance.Invincible *= ItemStats.Instance.Invincible;

        Stats.Instance.Hp = Mathf.Min(currentHp, Stats.Instance.MaxHp);
        Stats.Instance.Shield = Mathf.Min(currentShield, Stats.Instance.Shield);
        Stats.Instance.GunInit();
        Stats.Instance.BulletRemain = Mathf.Min(currentBullet, Stats.Instance.BulletMax);
        UIManager.OnBulletChange?.Invoke();
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

    public Dictionary<int, Item_Accessories_Gun> AcGunDict(Accessories_Gun_Type type) {
        Dictionary<int, Item_Accessories_Gun> dict;
        acGunDict.TryGetValue(type, out dict);

        return dict;
    }
}
