using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour {
    [SerializeField] Transform spawnPoint;
    public void WeaponChange(int id) {
        Stats.Instance.MainWeaponId = id;
        Stats.Instance.GunInit();
        UIManager.OnWeaponChange?.Invoke();
    }

    public void EnemySpawn(int id) {
        GameObject enemy = EnemyManager.OnUseEnemy?.Invoke(id);
        float positionX = Random.Range(10.0f, 20.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }

    public void EnemyTransformSpawn(int id) {
        GameObject enemy = EnemyManager.OnUseEnemy?.Invoke(id);
        float positionX = Random.Range(-10.0f, 10.0f);
        enemy.transform.position = new Vector3(spawnPoint.position.x + positionX, spawnPoint.position.y, 0);
    }

    public void WeaponObject(int id) {
        GameObject obj = ObjectManager.Instance.UseObject("ITEM_GUN");
        obj.transform.position = spawnPoint.position;
        GameObject objChild = obj.transform.Find("Interact").gameObject;

        ItemObject io = objChild.GetComponent<ItemObject>();
        if (io != null)
            io.Initialize(id, true);
    }

    public void AcObject(int id, Accessories_Gun_Type type) {
        GameObject obj = ObjectManager.Instance.UseObject("ITEM_ACCESSORY_GUN");
        obj.transform.position = spawnPoint.position;
        GameObject objChild = obj.transform.Find("Interact").gameObject;

        Item_Accessory_GunObject io = objChild.GetComponent<Item_Accessory_GunObject>();
        if (io != null)
            io.Initialize(id, true, type);
    }

    public void MuzzleObject(int id) {
        AcObject(id, Accessories_Gun_Type.MUZZLE);
    }
    public void MagazineObject(int id) {
        AcObject(id, Accessories_Gun_Type.MAGAZINE);
    }
    public void ScopeObject(int id) {
        AcObject(id, Accessories_Gun_Type.SCOPE);
    }
    public void BoostObject(int id) {
        AcObject(id, Accessories_Gun_Type.BOOST);
    }
    public void BulletObject(int id) {
        AcObject(id, Accessories_Gun_Type.BULLET);
    }

    public void LetterBoxTest() {
        LetterBoxManager.Instance.LetterBox(true);
    }
}
