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

public enum GunFireType {
    SINGLE,
    REPEAT
}

public class GunFire : Singleton<GunFire> {
    Transform muzzle;
    Vector3 mouse;

    GunData gun;
    float atk;
    Guns gunCode;
    ObjectList objType;
    bool isShootable = true;

    public void Shoot(Transform Pmuzzle) {
        muzzle = Pmuzzle;
        mouse = GeneralStats.Instance.MouseLocation;

        gun = Stats.Instance.MainGunData;
        atk = Stats.Instance.Atk + gun.atk;
        gunCode = Stats.Instance.MainGunCode;

        switch (gunCode) {
            case Guns.PISTOL:
                objType = ObjectList.PLAYERBULLET;
                NormalFire();
                break;
            case Guns.SHOTGUN:
                objType = ObjectList.PLAYERSHOTGUNBULLET;
                ShotgunFire();
                break;
            case Guns.RIFLE:
                objType = ObjectList.PLAYERBULLET;
                NormalFire();
                break;
        }
    }

    private void CreateBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(objType);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.Speed = Random.Range(gun.minSpeed, gun.maxSpeed);
            bulletPlayer.Guns = gunCode;
            bulletPlayer.ObjType = objType;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(gun.recoil, gun.recoil);
            bulletPlayer.Target = mouse + randomRange;
        }
    }

    private void NormalFire() {
        if (isShootable) {
            CreateBullets();
            StartCoroutine(GunCooltime(gun.cooltime));
        }
    }

    private void ShotgunFire() {
        if (isShootable) {
            for (int i = 0; i < 8; i++)
                CreateBullets();
            StartCoroutine(GunCooltime(1.0f));
        }
    }

    IEnumerator GunCooltime(float cooltime) {
        isShootable = false;
        yield return CoroutineCache.WaitForSecond(cooltime);
        isShootable = true;
    }
}
