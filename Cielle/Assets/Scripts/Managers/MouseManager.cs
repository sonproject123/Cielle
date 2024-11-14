using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseManager : Singleton<MouseManager> {
    [SerializeField] Texture2D cursorNormal;
    [SerializeField] Texture2D cursorCross;
    [SerializeField] Vector2 center;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        cursorNormal = ResourcesManager.Instance.Load<Texture2D>("CursorNormal");
        cursorCross = ResourcesManager.Instance.Load<Texture2D>("CursorCross");
    }

    public void State(int state) {
        switch (state) {
            case 0:
                Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case 1:
                center = new Vector2(cursorCross.width / 2, cursorCross.height / 2);
                Cursor.SetCursor(cursorCross, center, CursorMode.ForceSoftware);
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        State(scene.buildIndex);
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}