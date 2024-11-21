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
        atk = Stats.Instance.Atk + Stats.Instance.MainGunData.atk;
        gunCode = Stats.Instance.MainGunData.code;

        switch (gunCode) {
            case Guns.PISTOL:
                objType = ObjectList.PLAYERBULLET;
                PistolFire();
                break;
            case Guns.SHOTGUN:
                objType = ObjectList.PLAYERSHOTGUNBULLET;
                ShotgunFire();
                break;
            case Guns.RIFLE:
                objType = ObjectList.PLAYERBULLET;
                RifleFire();
                break;
        }
    }

    private void CreateNormalBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(objType);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletNormal bulletPlayer = bullet.GetComponent<BulletNormal>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.Speed = Random.Range(50, 50);
            bulletPlayer.Guns = gunCode;
            bulletPlayer.ObjType = objType;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(0.5f, 0.5f);
            bulletPlayer.Target = mouse + randomRange;
        }
    }

    private void CreateShotgunBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(objType);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletShotgun bulletPlayer = bullet.GetComponent<BulletShotgun>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.Speed = Random.Range(50, 70);
            bulletPlayer.Guns = gunCode;
            bulletPlayer.ObjType = objType;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(1f, 1f);
            bulletPlayer.Target = mouse + randomRange;
        }
    }

    private void PistolFire() {
        if (isShootable) {
            CreateNormalBullets();
            StartCoroutine(GunCooltime(0.01f));
        }
    }

    private void ShotgunFire() {
        if (isShootable) {
            for (int i = 0; i < 8; i++)
                CreateShotgunBullets();
            StartCoroutine(GunCooltime(1.0f));
        }
    }
    private void RifleFire() {
        if (isShootable) {
            CreateNormalBullets();
            StartCoroutine(GunCooltime(0.1f));
        }
    }

    IEnumerator GunCooltime(float cooltime) {
        isShootable = false;
        yield return CoroutineCache.WaitForSecond(cooltime);
        isShootable = true;
    }
}
