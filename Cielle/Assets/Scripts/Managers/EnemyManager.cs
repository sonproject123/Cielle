using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] List<int> EnemyIDList = new List<int>();
    [SerializeField] Dictionary<int, Queue<GameObject>> enemyList = new Dictionary<int, Queue<GameObject>>();

    public static Func<int, GameObject> OnUseEnemy; 
    public static Action<GameObject, int> OnReturnEnemy; 

    private void Start() {
        OnUseEnemy = UseEnemy;
        OnReturnEnemy = (GameObject obj, int id) => { ReturnEnemy(obj, id); };

        foreach (var id in EnemyIDList) {
            EnemyData enemyData;
            if (!JsonManager.Instance.EnemyDict.TryGetValue(id, out enemyData))
                continue;

            enemyList.Add(id, new Queue<GameObject>());

            Queue<GameObject> queue = null;
            if (enemyList.TryGetValue(id, out queue)) {
                for (int i = 0; i < 20; i++) {
                    GameObject temp = CreateEnemy(queue, id, "Enemies/" + enemyData.code);
                }
            }
        }
    }

    private GameObject CreateEnemy(Queue<GameObject> queue, int id, string path) {
        GameObject obj = ResourcesManager.Instance.Instantiate(path, transform);
        Enemy enemyStats = obj.GetComponent<Enemy>();
        enemyStats.Id = id;

        obj.SetActive(false);
        queue.Enqueue(obj);
        return obj;
    }

    public GameObject UseEnemy(int id) {
        GameObject obj = null;
        Queue<GameObject> queue = null;
        EnemyData data = null;

        if (enemyList.TryGetValue(id, out queue)) {
            if (queue.Count > 0)
                obj = queue.Dequeue();
            else if (JsonManager.Instance.EnemyDict.TryGetValue(id, out data))
                obj = CreateEnemy(queue, id, "Enemies/" + data.code);
        }

        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void ReturnEnemy(GameObject obj, int id) {
        Queue<GameObject> queue = null;

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        if (enemyList.TryGetValue(id, out queue))
            queue.Enqueue(obj);
    }
}
