using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance;
    public static T Instance {
        get { return instance; }
    }

    protected virtual void Awake() {
        if (instance == null)
            instance = (T)FindObjectOfType(typeof(T));
        else  {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}