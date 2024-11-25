using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {
    [SerializeField] Transform mainObject;
    [SerializeField] EnemyStats stats;
    [SerializeField] Slider hpBar;

    private void Awake() {
        stats = mainObject.GetComponent<EnemyStats>();
    }

    private void Start() {
        hpBar.maxValue = stats.MaxHp;
        hpBar.value = stats.Hp;
    }

    public void HpBar() {
        hpBar.value = stats.Hp;
    }
}
