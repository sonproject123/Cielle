using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public enum ObjectList {
    PLAYERBULLET,
    PLAYERSHOTGUNBULLET,

    PLAYERDIVEEXPLOSION,

    ENEMYBULLET,

    BREAKOBJECT,
    METALOBJECT
}

public class ObjectManager : Singleton<ObjectManager>{
    [SerializeField] Queue<GameObject> playerBullets = new Queue<GameObject>();
    [SerializeField] Queue<GameObject> enemyBullets = new Queue<GameObject>();
    [SerializeField] int bulletCreateCount;

    [SerializeField] Queue<GameObject> playerShotgunBullets = new Queue<GameObject>();
    [SerializeField] int shotgunBulletCreateCount;

    [SerializeField] Queue<GameObject> diveExplosions = new Queue<GameObject>();
    [SerializeField] int diveExplosionCreateCount;

    [SerializeField] Queue<GameObject> breakObject = new Queue<GameObject>();
    [SerializeField] Queue<GameObject> metalObject = new Queue<GameObject>();
    [SerializeField] int deadObjectCreateCount;



    private void Start() {
        bulletCreateCount = 200;
        shotgunBulletCreateCount = 50;
        diveExplosionCreateCount = 3;

        deadObjectCreateCount = 500;

        CreateObjects();
    }

    private void CreateObjects() {
        for (int i = 0; i < bulletCreateCount; i++) {
            CreateObject(playerBullets, "Bullets/Player_Normal");
            CreateObject(enemyBullets, "Bullets/Enemy_Normal");
        }

        for (int i = 0; i < shotgunBulletCreateCount; i++)
            CreateObject(playerShotgunBullets, "Bullets/Player_Shotgun");

        for (int i = 0; i < diveExplosionCreateCount; i++)
            CreateObject(diveExplosions, "Bullets/Explosion_Dive_Attack");

        for (int i = 0; i < deadObjectCreateCount; i++) {
            CreateObject(breakObject, "Objects/Break_Object");
            CreateObject(metalObject, "Objects/Metal");
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
                    obj = CreateRObject(playerBullets, "Bullets/Player_Normal");
                break;
            case ObjectList.PLAYERSHOTGUNBULLET:
                if (playerShotgunBullets.Count > 0)
                    obj = playerShotgunBullets.Dequeue();
                else
                    obj = CreateRObject(playerShotgunBullets, "Bullets/Player_Shotgun");
                break;
            case ObjectList.PLAYERDIVEEXPLOSION:
                if (diveExplosions.Count > 0)
                    obj = diveExplosions.Dequeue();
                else
                    obj = CreateRObject(diveExplosions, "Bullets/Explosion_Dive_Attack");
                break;

            case ObjectList.ENEMYBULLET:
                if (enemyBullets.Count > 0)
                    obj = enemyBullets.Dequeue();
                else
                    obj = CreateRObject(enemyBullets, "Bullets/Enemy_Normal");
                break;

            case ObjectList.BREAKOBJECT:
                if (breakObject.Count > 0)
                    obj = breakObject.Dequeue();
                else
                    obj = CreateRObject(breakObject, "Objects/Break_Object");
                break;
            case ObjectList.METALOBJECT:
                if (metalObject.Count > 0)
                    obj = metalObject.Dequeue();
                else
                    obj = CreateRObject(metalObject, "Objects/Metal");
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
            case ObjectList.PLAYERSHOTGUNBULLET:
                playerShotgunBullets.Enqueue(obj);
                break;

            case ObjectList.PLAYERDIVEEXPLOSION:
                diveExplosions.Enqueue(obj);
                break;

            case ObjectList.ENEMYBULLET:
                enemyBullets.Enqueue(obj);
                break;

            case ObjectList.BREAKOBJECT:
                breakObject.Enqueue(obj);
                break;
            case ObjectList.METALOBJECT:
                metalObject.Enqueue(obj);
                break;
        }
    }
}
