using UnityEngine;

[CreateAssetMenu(fileName = "New_Accessory_Gun", menuName = "Create Accessory_Gun_SO")]
public class Item_Accessories_Gun : ScriptableObject {
    public int id;
    public string itemName;
    public string descriptions;
    public float value1 = 0;
    public float value2 = 0;
    public float value3 = 0;
    public Accessories_Gun_Type type;
    public string code;
    public int exclusive;
    public Item_Rarity rarity;
}
