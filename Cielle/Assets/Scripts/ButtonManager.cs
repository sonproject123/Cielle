using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : Singleton<ButtonManager> {
    public void Pistol() {
        Stats.Instance.GunCategory = Guns.PISTOL;
        Stats.Instance.GunFireType = GunFireType.SINGLE;
    }

    public void Rifle() {
        Stats.Instance.GunCategory = Guns.RIFLE;
        Stats.Instance.GunFireType = GunFireType.REPEAT;
    }

    public void Shotgun() {
        Stats.Instance.GunCategory = Guns.SHOTGUN;
        Stats.Instance.GunFireType = GunFireType.SINGLE;
    }


    public void LesserDrone() {
        GameObject enemy = ResourcesManager.Instance.Instantiate("Enemy_LesserFlyingGunDrone");
        float positionX = Random.Range(10.0f, 20.0f);
        enemy.transform.localPosition = new Vector3(positionX, 0, 0);
    }
}
