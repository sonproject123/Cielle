using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Guns {
    PISTOL,
    AKIMBO,
    RIFLE,
    SHOTGUN,
    SNIPER,
    BOMBER,
    ROCKET,
    GATLING,
    FLAMETHROWER,
    LASER,
    SATELLITE,
}

public class GunFire : Singleton<GunFire> {
    Transform muzzle;
    Guns guns;
    bool isShootable = true;

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
                RifleFire();
                break;
        }
    }

    private void CreateNormalBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.PLAYERBULLET);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);
        float angle = MathCalculator.Instance.Angle(bullet.transform.position, GeneralStats.Instance.MouseLocation);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = Stats.Instance.Atk;
            bulletPlayer.Speed = 20;
            bulletPlayer.Target = GeneralStats.Instance.MouseLocation;
            bulletPlayer.Guns = guns;
        }
    }

    private void PistolFire() {
        if (isShootable) {
            CreateNormalBullets();
        }
    }

    private void RifleFire() {
        Debug.Log("hi");
        if (isShootable) {
            CreateNormalBullets();
            StartCoroutine(GunCooltime());
        }
    }

    IEnumerator GunCooltime() {
        isShootable = false;
        yield return CoroutineCache.WaitForSecond(0.2f);
        isShootable = true;
    }
}
