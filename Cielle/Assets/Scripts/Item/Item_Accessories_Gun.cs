using UnityEngine;

[CreateAssetMenu(fileName = "New_Accessory_Gun", menuName = "Create Accessory_Gun_SO")]
public class Item_Accessories_Gun : ScriptableObject {
    public int id;
    public string itemName;
    public string descriptions;

    public float maxHp = 0;
    public float maxHpMult = 1;

    public float maxShield = 0;
    public float maxShieldMult = 1;
    public float shieldDefense = 1;

    public float shieldRegen = 1;
    public float shieldCooltime = 1;
    public float shieldBreakCooltime = 1;
    public float shieldInvincible = 1;

    public float attack = 1;
    public float attackShield = 1;

    public float defense = 1;
    public float invincible = 1;

    public float gunRecoil = 1;

    public float maxBullet = 1;

    public Accessories_Gun_Type type;
    public string code;
    public int[] exclusive; // count 0 = toAll
    public Item_Rarity rarity;
    public int price;
}
