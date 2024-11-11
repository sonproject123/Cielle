using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    public void GameStart() {
        StartCoroutine(SceneryManager.Instance.AsyncLoad(1));
    }

    public void Continue() {
        Debug.Log("Continue");
    }

    public void GameExit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}