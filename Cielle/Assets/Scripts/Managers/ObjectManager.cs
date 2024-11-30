using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>{
    [SerializeField] Dictionary<string, Queue<GameObject>> objectList = new Dictionary<string, Queue<GameObject>>();

    private void Start() {
        foreach(var dict in JsonManager.Instance.ObjectDict) {
            objectList.Add(dict.Key, new Queue<GameObject>());

            Queue<GameObject> queue = null;
            if (objectList.TryGetValue(dict.Key, out queue)) {
                for (int i = 0; i < dict.Value.count; i++) {
                    GameObject temp = CreateObject(queue, dict.Value.path);
                }
            }
        }
    }

    private GameObject CreateObject(Queue<GameObject> queue, string name) {
        GameObject obj = ResourcesManager.Instance.Instantiate(name, transform);
        obj.SetActive(false);
        queue.Enqueue(obj);
        return obj;
    }

    public GameObject UseObject(string name) {
        GameObject obj = null;
        Queue<GameObject> queue = null;
        ObjectData data = null;

        if (objectList.TryGetValue(name, out queue)) {
            if (queue.Count > 0)
                obj = queue.Dequeue();
            else if (JsonManager.Instance.ObjectDict.TryGetValue(name, out data))
                obj = CreateObject(queue, data.path);
        }

        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void ReturnObject(GameObject obj, string name) {
        Queue<GameObject> queue = null;

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        if (objectList.TryGetValue(name, out queue))
            queue.Enqueue(obj);
    }
}
