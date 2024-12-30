using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {
    [SerializeField] Quaternion fixRotation;

    [SerializeField] GameObject mainObject;
    [SerializeField] EnemyStats stats;
    [SerializeField] Slider hpBar;

    private void Awake() {
        fixRotation = transform.rotation;

        stats = mainObject.GetComponent<EnemyStats>();
    }

    private void OnEnable() {
        hpBar.maxValue = stats.MaxHp;
        hpBar.value = stats.Hp;
    }

    private void LateUpdate() {
        transform.rotation = fixRotation;
    }

    public void HpBar() {
        hpBar.value = stats.Hp;
    }
}
