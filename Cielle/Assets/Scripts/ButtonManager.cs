using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : Singleton<ButtonManager> {
    public void Pistol() {
        Stats.Instance.MainWeaponId = 1;
        Stats.Instance.GunChange();
    }

    public void Rifle() {
        Stats.Instance.MainWeaponId = 2;
        Stats.Instance.GunChange();
    }

    public void Shotgun() {
        Stats.Instance.MainWeaponId = 3;
        Stats.Instance.GunChange();
    }


    public void LesserDrone() {
        GameObject enemy = ResourcesManager.Instance.Instantiate("Enemies/Lesser_Gun_Drone");
        float positionX = Random.Range(10.0f, 20.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }
}
