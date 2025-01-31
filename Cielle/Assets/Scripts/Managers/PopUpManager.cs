using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PopUpTypes {
    LOCALMAP,
    BOSS_NAME,
    DEAD
}

public class PopUpManager : Singleton<PopUpManager> {
    [SerializeField] Dictionary<PopUpTypes, GameObject> popUps = new Dictionary<PopUpTypes, GameObject>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.buildIndex == 0) {
            foreach(GameObject popUp in popUps.Values) {
                popUp.SetActive(false);
            }
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public GameObject ShowPopUp(PopUpTypes type) {
        GameObject popUp;

        if (popUps.TryGetValue(type, out popUp))
            popUp.SetActive(true);
        else {
            popUp = Instantiate(Resources.Load<GameObject>("PopUps/" + type.ToString()));
            popUp.transform.SetParent(transform);
            popUp.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

            popUps.Add(type, popUp);
        }

        return popUp;
    }

    public void ClosePopUp(PopUpTypes type) {
        GameObject popUp;

        if (popUps.TryGetValue(type, out popUp))
            popUp.SetActive(false);
    }
}
