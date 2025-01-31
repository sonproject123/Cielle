using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Quaternion fixRotation;

    [SerializeField] Slider reloadBar;
    [SerializeField] Image deadBackground;
    [SerializeField] Color DBColor;

    private void Awake() {
        fixRotation = transform.rotation;

        reloadBar.gameObject.SetActive(false);
        DBColor = deadBackground.color;
        DBColor.a = 0;
        deadBackground.gameObject.SetActive(false);
        deadBackground.color = DBColor;
    }

    private void LateUpdate() {
        transform.rotation = fixRotation;
    }

    public void Reload(bool state) {
        reloadBar.gameObject.SetActive(state);
        reloadBar.maxValue = 1;
        reloadBar.value = 0;
    }

    public void Reloading(float time, float maxTime) {
        reloadBar.maxValue = maxTime;
        reloadBar.value = time;
    }

    public void DeadBackground() {
        StartCoroutine(Deading());
    }

    IEnumerator Deading() {
        float time = 0;
        float duration = 1;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        deadBackground.gameObject.SetActive(true);
        deadBackground.transform.position = new Vector3(player.position.x, player.position.y, player.position.z + 1);
        while (time < duration) {
            time += Time.deltaTime;
            DBColor.a = Mathf.Lerp(0, 1, time / duration);
            deadBackground.color = DBColor;

            yield return wffu;
        }

        DBColor.a = 1;
        deadBackground.color = DBColor;
    }
}
