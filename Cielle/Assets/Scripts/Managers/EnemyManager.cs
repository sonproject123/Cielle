using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager> {
    [SerializeField] Dictionary<int, Queue<GameObject>> enemyList = new Dictionary<int, Queue<GameObject>>();

    private void Start() {
        foreach (var dict in JsonManager.Instance.EnemyDict) {
            enemyList.Add(dict.Key, new Queue<GameObject>());

            Queue<GameObject> queue = null;
            if (enemyList.TryGetValue(dict.Key, out queue)) {
                for (int i = 0; i < 30; i++) {
                    GameObject temp = CreateEnemy(queue, dict.Key, "Enemies/" + dict.Value.code);
                }
            }
        }
    }

    private GameObject CreateEnemy(Queue<GameObject> queue, int id, string path) {
        GameObject obj = ResourcesManager.Instance.Instantiate(path, transform);
        EnemyStats enemyStats = obj.GetComponent<EnemyStats>();
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
