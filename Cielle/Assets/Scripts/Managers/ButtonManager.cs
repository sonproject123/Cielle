using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : Singleton<ButtonManager> {
    public void Pistol() {
        Stats.Instance.GunCategory = Guns.PISTOL;
    }

    public void Rifle() {
        Stats.Instance.GunCategory = Guns.RIFLE;
    }
}
