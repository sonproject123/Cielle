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
    float atk;
    Guns guns;
    bool isShootable = true;

    public void Shoot(Transform Pmuzzle) {
        muzzle = Pmuzzle;
        atk = Stats.Instance.Atk;
        guns = Stats.Instance.GunCategory;
        mouse = GeneralStats.Instance.MouseLocation;

        switch (guns) {
            case Guns.PISTOL:
                PistolFire();
                break;
            case Guns.SHOTGUN:
                ShotgunFire();
                break;
            case Guns.RIFLE:
                RifleFire();
                break;
        }
    }

    private void CreateNormalBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.PLAYERBULLET);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletNormal bulletPlayer = bullet.GetComponent<BulletNormal>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.Speed = 50;
            bulletPlayer.Guns = guns;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(0.5f, 0.5f);
            bulletPlayer.Target = mouse + randomRange;
        }
    }

    private void CreateShotgunBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(ObjectList.PLAYERSHOTGUNBULLET);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletShotgun bulletPlayer = bullet.GetComponent<BulletShotgun>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.Speed = 70;
            bulletPlayer.Guns = guns;

            Vector3 randomRange = MathCalculator.Instance.RandomTarget(0.5f, 0.5f);
            bulletPlayer.Target = mouse + randomRange;
        }
    }

    private void PistolFire() {
        if (isShootable) {
            CreateNormalBullets();
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
            StartCoroutine(GunCooltime(0.15f));
        }
    }

    IEnumerator GunCooltime(float cooltime) {
        isShootable = false;
        yield return CoroutineCache.WaitForSecond(cooltime);
        isShootable = true;
    }
}
