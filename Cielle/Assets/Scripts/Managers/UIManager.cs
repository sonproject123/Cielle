using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] Slider hpBar;

    public static Action OnUpdateHpBar;

    private void Awake() {
        OnUpdateHpBar = () => { HpBar(); };
    }

    private void Start() {
        hpBar.maxValue = Stats.Instance.MaxHp;
        hpBar.value = Stats.Instance.Hp;
    }

    public void HpBar() {
        hpBar.value = Stats.Instance.Hp;
    }
}
