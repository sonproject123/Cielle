using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    [SerializeField] List<string> enemyList = new List<string>();
    [SerializeField] GameObject enemy;
    [SerializeField] float positionX;

    private void Start() {
        for (int i = 0; i < 3; i++)
            Spawn();
    }

    public void Spawn() {
        string enemyName = enemyList[Random.Range(0, enemyList.Capacity)];
        enemy = ResourcesManager.Instance.Instantiate(enemyName);
        positionX = Random.Range(-10.0f, 10.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }
}
