using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>{
    [SerializeField] Dictionary<string, Queue<GameObject>> objectList = new Dictionary<string, Queue<GameObject>>();

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
        obj = queue.Dequeue();

        obj.SetActive(true);
        Debug.Log(queue.Count);
        obj.transform.parent = null;
        return obj;
    }

    public void ReturnObject(GameObject obj, string name) {
        Queue<GameObject> queue;

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        objectList.TryGetValue(name, out queue);
        queue.Enqueue(obj);
    }
}
