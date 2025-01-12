using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : Singleton<ButtonManager> {
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

    public void BossDoor(bool isClosing) {
        
    }
}
