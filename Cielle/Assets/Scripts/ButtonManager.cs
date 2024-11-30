using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : Singleton<ButtonManager> {
    public void WeaponChange(int id) {
        Stats.Instance.MainWeaponId = id;
        Stats.Instance.GunInit();
        UIManager.OnWeaponChange?.Invoke();
    }

    public void EnemySpawn(int id) {
        GameObject enemy = EnemyManager.Instance.UseEnemy(id);
        float positionX = Random.Range(10.0f, 20.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }
}
