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

    public void Shoot(Transform Pmuzzle) {
        muzzle = Pmuzzle;
        guns = Stats.Instance.GunCategory;

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
            bulletPlayer.Guns = guns;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(0.5f, 0.5f);
            bulletPlayer.Target = GeneralStats.Instance.MouseLocation + randomRange;
        }
    }

    private void PistolFire() {
        if (isShootable) {
            CreateNormalBullets();
        }
    }

    private void RifleFire() {
        if (isShootable) {
            CreateNormalBullets();
            StartCoroutine(GunCooltime());
        }
    }

    IEnumerator GunCooltime() {
        isShootable = false;
        yield return CoroutineCache.WaitForSecond(0.15f);
        isShootable = true;
    }
}
