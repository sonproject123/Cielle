using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    [SerializeField] List<int> enemyList = new List<int>();

    private void Start() {
        for (int i = 0; i < 3; i++)
            Spawn();
    }

    public void Spawn() {
        int enemyId = enemyList[Random.Range(0, enemyList.Capacity)];
        GameObject enemy = EnemyManager.OnUseEnemy(enemyId);
        float positionX = Random.Range(10.0f, 20.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }
}
