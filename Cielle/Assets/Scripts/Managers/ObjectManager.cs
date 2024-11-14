using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public enum ObjectList {
    PLAYERBULLET,
    ENEMYBULLET
}

public class ObjectManager : Singleton<ObjectManager>{
    [SerializeField] Queue<GameObject> playerBullets = new Queue<GameObject>();
    [SerializeField] Queue<GameObject> enemyBullets = new Queue<GameObject>();

    [SerializeField] int bulletCreateCount;

    private void Start() {
        bulletCreateCount = 500;
        CreateObjects();
    }

    private void CreateObjects() {
        for (int i = 0; i < bulletCreateCount; i++) {
            CreateObject(playerBullets, "PlayerBullet");
            CreateObject(enemyBullets, "EnemyBullet");
        }
    }

    private void CreateObject(Queue<GameObject> queue, string name) {
        GameObject obj = ResourcesManager.Instance.Instantiate(name, transform);
        obj.SetActive(false);
        queue.Enqueue(obj);
    }

    private GameObject CreateRObject(Queue<GameObject> queue, string name) {
        GameObject obj = ResourcesManager.Instance.Instantiate(name, transform);
        obj.SetActive(false);
        queue.Enqueue(obj);
        return obj;
    }

    public GameObject UseObject(ObjectList oList) {
        GameObject obj = null;

        switch (oList) {
            case ObjectList.PLAYERBULLET:
                if(playerBullets.Count > 0)
                    obj = playerBullets.Dequeue();
                else
                    obj = CreateRObject(playerBullets, "PlayerBullet");
                break;
            case ObjectList.ENEMYBULLET:
                if (enemyBullets.Count > 0)
                    obj = enemyBullets.Dequeue();
                else
                    obj = CreateRObject(enemyBullets, "EnemyBullet");
                break;
        }

        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void ReturnObject(GameObject obj, ObjectList oList) {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        switch (oList) { 
            case ObjectList.PLAYERBULLET:
                playerBullets.Enqueue(obj);
                break;
            case ObjectList.ENEMYBULLET:
                enemyBullets.Enqueue(obj);
                break;
        }
    }
}
