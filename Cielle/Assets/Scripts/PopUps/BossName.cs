using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossName : MonoBehaviour {
    [SerializeField] Text subtitle;
    [SerializeField] Text bossName;

    public Action<string, string> OnNameInput;

    private void Awake() {
        OnNameInput = (string sub, string name) => { NameInput(sub, name); };
    }

    private void OnEnable() {
        StartCoroutine(NameViewing());
    }

    IEnumerator NameViewing() {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        float time = 0;
        float duration = 3;
        float textSpeed = 1f;
        float textDistance = 20;

        Transform subTransform = subtitle.transform;
        Vector2 subPosition = subtitle.transform.position;
        Color subColor = subtitle.color;
        Color subAlpha = subColor;
        subAlpha.a = 0;
        subtitle.color = subAlpha;

        Transform nameTransform = bossName.transform;
        Vector2 namePosition = bossName.transform.position;
        Color nameColor = bossName.color;
        Color nameAlpha = nameColor;
        nameAlpha.a = 0;
        bossName.color = nameAlpha;

        while (time < duration) {
            time += Time.deltaTime;

            subTransform.position = Vector2.Lerp(
            subTransform.position,
            new Vector2(subPosition.x + textDistance, subPosition.y),
            textSpeed * Time.fixedDeltaTime
            );

            float t = Mathf.Clamp01(time / (duration / 2));
            subColor.a = Mathf.Lerp(subAlpha.a, 1, t);
            subtitle.color = subColor;

            if (time > (duration / 2)) {
                nameTransform.position = Vector2.Lerp(
                nameTransform.position,
                new Vector2(namePosition.x + textDistance, namePosition.y),
                textSpeed * Time.fixedDeltaTime
                );

                float t2 = Mathf.Clamp01(time / duration);
                nameColor.a = Mathf.Lerp(nameAlpha.a, 1, t2);
                bossName.color = nameColor;
            }
            yield return wffu; 
        }
    }

    public void NameInput(string sub, string name) {
        subtitle.text = sub;
        bossName.text = name;
    }
}
