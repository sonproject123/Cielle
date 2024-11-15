using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Guns {
    PISTOL,
    RIFLE,
    SHOTGUN,
    SNIPER,
    LASER,
    SATELLITE,
}

public class GunFire : Singleton<GunFire> {
    Transform muzzle;
    Guns guns;

    public void Shoot(Guns Pguns, Transform Pmuzzle) {
        muzzle = Pmuzzle;
        guns = Pguns;

        switch (guns) {
            case Guns.PISTOL:
                PistolFire();
                break;
            case Guns.SHOTGUN:
                break;
            case Guns.RIFLE:
                PistolFire();
                break;
        }
    }

    private void CreateNormalBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.PLAYERBULLET);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);
        float angle = MathCalculator.Instance.Angle(bullet.transform.position, Stats.Instance.MouseLocation);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = Stats.Instance.Atk;
            bulletPlayer.Speed = 20;
            bulletPlayer.Target = Stats.Instance.MouseLocation;
            bulletPlayer.Guns = guns;
        }
    }

    private void PistolFire() {
        CreateNormalBullets();
    }
}
