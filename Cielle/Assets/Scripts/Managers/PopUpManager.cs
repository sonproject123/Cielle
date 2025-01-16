using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpTypes {
    LOCALMAP,
    BOSS_NAME
}

public class PopUpManager : Singleton<PopUpManager> {
    [SerializeField] Dictionary<PopUpTypes, GameObject> popUps = new Dictionary<PopUpTypes, GameObject>();

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
