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
    Animator animator;

    GunData gun;
    float atk;
    Guns gunCode;
    ObjectList objType;
    bool isShootable = true;

    public void Shoot(Transform Pmuzzle, Animator Panimator) {
        if (Stats.Instance.BulletRemain <= 0) {
            // Sound tic tic
            return;
        }

        muzzle = Pmuzzle;
        mouse = GeneralStats.Instance.MouseLocation;
        animator = Panimator;

        gun = Stats.Instance.MainGunData;
        atk = Stats.Instance.Atk + gun.atk;
        gunCode = Stats.Instance.MainGunCode;

        if (isShootable) {
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

    private void CommonFire() {
        Stats.Instance.BulletRemain--;
        UIManager.OnBulletUse?.Invoke();
        StartCoroutine(GunCooltime(gun.cooltime));
    }

    private void NormalFire() {
        animator.SetTrigger("GunFire");
        // Sound baam

        CreateBullets();
        CommonFire();
    }

    private void ShotgunFire() {
        animator.SetTrigger("GunFire");
        // Sound baam

        for (int i = 0; i < 8; i++)
            CreateBullets();
        CommonFire();
    }

    IEnumerator GunCooltime(float cooltime) {
        isShootable = false;
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < cooltime) {
            time += Time.deltaTime;
            if (cooltime >= 0.09f)
                UIManager.OnWeaponCooltime?.Invoke(time, cooltime);
            yield return wffu;
        }

        isShootable = true;
        UIManager.OnWeaponCooltime?.Invoke(0, 0);
    }

    public bool IsShootable { 
        get { return isShootable; }
    }
}
