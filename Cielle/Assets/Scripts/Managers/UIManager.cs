using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
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
    public static Action<float, float> OnWeaponCooltime;
    public static Action OnWeaponChange;
    public static Action<float, float> OnWeaponChangeCooltime;

    private void Awake() {
        OnUpdateHpBar = () => { HpBar(); };
        OnBulletUse = () => { BulletUse(); };
        OnBulletChange = () => { BulletChange(); };
        OnMetalChange = () => { MetalChange(); };
        OnWeaponCooltime = (float time, float maxTime) => { WeaponCooltime(time, maxTime); };
        OnWeaponChange = () => { WeaponChange(); };
        OnWeaponChangeCooltime = (float time, float maxTime) => { WeaponChangeCooltime(time, maxTime); };
    }

    private void Start() {
        hpBar.maxValue = Stats.Instance.MaxHp;
        hpBar.value = Stats.Instance.Hp;
        mainWeaponCooltime.maxValue = 0;
        mainWeaponCooltime.value = 0;
        subWeaponCooltime.maxValue = 0;
        subWeaponCooltime.value = 0;

        WeaponChange();
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

    public void WeaponCooltime(float time, float maxTime) {
        mainWeaponCooltime.maxValue = maxTime;
        mainWeaponCooltime.value = time;
    }

    public void WeaponChange() {
        string mainWeaponIconPath = "Icons/" + Stats.Instance.MainGunData.code;
        string subWeaponIconPath = "Icons/" + Stats.Instance.SubGunData.code;
        mainWeaponIcon.sprite = Resources.Load<Sprite>(mainWeaponIconPath);
        subWeaponIcon.sprite = Resources.Load<Sprite>(subWeaponIconPath);
    }

    public void WeaponChangeCooltime(float time, float maxTime) {
        subWeaponCooltime.maxValue = maxTime;
        subWeaponCooltime.value = time;
    }
}
