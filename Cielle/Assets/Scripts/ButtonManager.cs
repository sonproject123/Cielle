using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
