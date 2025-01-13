using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoxManager : Singleton<LetterBoxManager> {
    [SerializeField] Image top;
    [SerializeField] Image bottom;
    [SerializeField] Color originalColor;
    [SerializeField] Color initColor;
    [SerializeField] Color onColor;
    [SerializeField] bool on;
    [SerializeField] bool isOnToggle;

    private void Start() {
        originalColor = top.color;
        top.gameObject.SetActive(false);
        bottom.gameObject.SetActive(false);

        initColor = originalColor;
        initColor.a = 0;

        on = false;
        isOnToggle = false;
    }

    public void LetterBox() {
        if (!isOnToggle)
            StartCoroutine(LetterBoxToggle());
    }

    IEnumerator LetterBoxToggle() {
        float speed = 7;
        isOnToggle = true;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        if (on) {
            top.color = originalColor;
            bottom.color = originalColor;
            onColor = top.color;

            while (top.color.a > 0) {
                onColor.a -= Time.deltaTime * speed;
                top.color = onColor;
                bottom.color = onColor;
                yield return wffu;
            }

            on = false;
            top.gameObject.SetActive(false);
            bottom.gameObject.SetActive(false);
        }
        else {
            top.color = initColor;
            bottom.color = initColor;
            top.gameObject.SetActive(true);
            bottom.gameObject.SetActive(true);
            onColor = initColor;

            while (top.color.a < 1) {
                onColor.a += Time.deltaTime * speed;
                top.color = onColor;
                bottom.color = onColor;
                yield return wffu;
            }

            on = true;
        }

        isOnToggle = false;
    }
}
