using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] Color uiColor;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider bulletBar;
    [SerializeField] Slider mainWeaponCooltime;
    [SerializeField] Slider subWeaponCooltime;

    [SerializeField] Image mainWeaponIcon;
    [SerializeField] Image subWeaponIcon;

    [SerializeField] Text bulletRemainText;
    [SerializeField] Text bulletMaxText;
    [SerializeField] Text MetalText;

    public static Action OnUpdateHpBar;
    public static Action OnBulletUse;
    public static Action OnBulletChange;
    public static Action OnMetalChange;
    public static Action OnWeaponCooltime;

    private void Awake() {
        uiColor = GetComponent<Color>();

        OnUpdateHpBar = () => { HpBar(); };
        OnBulletUse = () => { BulletUse(); };
        OnBulletChange = () => { BulletChange(); };
        OnMetalChange = () => { MetalChange(); };
    }

    private void Start() {
        hpBar.maxValue = Stats.Instance.MaxHp;
        hpBar.value = Stats.Instance.Hp;

        BulletChange();

        MetalText.text = Stats.Instance.Metals.ToString();
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

    public void MetalChange() {
        MetalText.text = Stats.Instance.Metals.ToString();
    }

    public void SlotChange() {
        //mainWeaponIcon

    }
}
