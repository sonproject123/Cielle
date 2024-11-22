using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] Slider hpBar;
    [SerializeField] Slider bulletBar;
    [SerializeField] Text bulletRemainText;
    [SerializeField] Text bulletMaxText;

    public static Action OnUpdateHpBar;
    public static Action OnBulletUse;
    public static Action OnBulletChange;

    private void Awake() {
        OnUpdateHpBar = () => { HpBar(); };
        OnBulletUse = () => { BulletUse(); };
        OnBulletChange = () => { BulletChange(); };
    }

    private void Start() {
        hpBar.maxValue = Stats.Instance.MaxHp;
        hpBar.value = Stats.Instance.Hp;

        BulletChange();
    }

    public void HpBar() {
        hpBar.value = Stats.Instance.Hp;
    }

    public void BulletUse() {
        if (Stats.Instance.BulletRemain > 9999) {
            Stats.Instance.BulletRemain = Stats.Instance.BulletMax;
            return;
        }

        bulletBar.value = Stats.Instance.BulletRemain;
        bulletRemainText.text = Stats.Instance.BulletRemain.ToString();
    }

    public void BulletChange() {
        bulletBar.maxValue = Stats.Instance.BulletMax;
        bulletBar.value = Stats.Instance.BulletRemain;

        if (Stats.Instance.BulletRemain > 9999) {
            bulletMaxText.text = "INF";
            bulletRemainText.text = "";
        }
        else {
            bulletMaxText.text = Stats.Instance.BulletMax.ToString();
            bulletRemainText.text = Stats.Instance.BulletRemain.ToString();
        }
    }
}
