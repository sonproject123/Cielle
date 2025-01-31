using UnityEngine;

public class DeadPopUp : MonoBehaviour {
    public void DeadButton() {
        StartCoroutine(SceneryManager.Instance.AsyncLoad(0));
    }
}
