using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>{
    [SerializeField] Dictionary<string, Queue<GameObject>> objectList = new Dictionary<string, Queue<GameObject>>();
    [SerializeField] List<(string, GameObject)> activedObjects = new List<(string, GameObject)>();

    private void Start() {
        foreach(var dict in JsonManager.Instance.ObjectDict) {
            objectList.Add(dict.Key, new Queue<GameObject>());

            Queue<GameObject> queue;
            if (objectList.TryGetValue(dict.Key, out queue)) {
                for (int i = 0; i < dict.Value.count; i++)
                    CreateObject(queue, dict.Value.path);
            }
        }
    }

    public void AllReturn() {
        for (int i = activedObjects.Count - 1; i >= 0; i--) {
            Queue<GameObject> queue;

            activedObjects[i].Item2.transform.SetParent(transform);
            objectList.TryGetValue(activedObjects[i].Item1, out queue);
            queue.Enqueue(activedObjects[i].Item2);

            activedObjects[i].Item2.SetActive(false);
            activedObjects.RemoveAt(i);
        }
    }

    private void CreateObject(Queue<GameObject> queue, string name) {
        GameObject obj = ResourcesManager.Instance.Instantiate(name, transform);
        obj.SetActive(false);
        queue.Enqueue(obj);
    }

    public GameObject UseObject(string name) {
        GameObject obj = null;
        Queue<GameObject> queue;
        ObjectData data;

        objectList.TryGetValue(name, out queue);
        if (queue.Count <= 0) {
            JsonManager.Instance.ObjectDict.TryGetValue(name, out data);
            CreateObject(queue, data.path);
        }
        else {
            while (queue.Count > 0) {
                obj = queue.Dequeue();
                if (obj.activeInHierarchy)
                    continue;
                else
                    break;
            }
        }

        if (obj == null) {
            JsonManager.Instance.ObjectDict.TryGetValue(name, out data);
            CreateObject(queue, data.path);
            obj = queue.Dequeue();
        }

        activedObjects.Add((name, obj));
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void ReturnObject(GameObject obj, string name) {
        Queue<GameObject> queue;

        obj.transform.SetParent(transform);
        objectList.TryGetValue(name, out queue);
        queue.Enqueue(obj);

        activedObjects.Remove((name, obj));
        obj.gameObject.SetActive(false);
    }
}
