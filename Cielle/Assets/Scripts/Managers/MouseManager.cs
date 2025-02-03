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
        cursorCross = ResourcesManager.Instance.Load<Texture2D>("GameOverall/CursorCross");
    }

    public void State(int state) {
        if (state == 0)
            Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
        else
            CrossCursor();
    }

    private void CrossCursor() {
        center = new Vector2(cursorCross.width / 2, cursorCross.height / 2);
        Cursor.SetCursor(cursorCross, center, CursorMode.ForceSoftware);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        State(scene.buildIndex);
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}