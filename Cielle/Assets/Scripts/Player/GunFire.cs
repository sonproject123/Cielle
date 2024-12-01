using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SNIPER,
//BOMBER,
//ROCKET,
//GATLING,
//FLAMETHROWER,
//LASER,
//SATELLITE,

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
    float atkShield;
    float minSpeed;
    float maxSpeed;
    float stoppingPower;
    float stoppingTime;
    string code;
    string bulletName;
    bool isShootable = true;

    public void Initialize(Transform Pmuzzle, Animator Panimator) {
        muzzle = Pmuzzle;
        mouse = GeneralStats.Instance.MouseLocation;
        animator = Panimator;

        gun = Stats.Instance.MainGunData;
        atk = Stats.Instance.Atk * gun.atk;
        atkShield = Stats.Instance.AtkShield;
        minSpeed = gun.minSpeed;
        maxSpeed = gun.maxSpeed;
        stoppingPower = gun.stopping;
        stoppingTime = gun.stoppingTime;
        code = gun.code;
        bulletName = gun.bulletCode;
    }

    public void Shoot(Transform Pmuzzle, Animator Panimator) {
        if (Stats.Instance.BulletRemain <= 0) {
            // Sound tic tic
            return;
        }

        Initialize(Pmuzzle, Panimator);

        if (isShootable) {
            switch (code) {
                case "PISTOL":
                    NormalFire();
                    break;
                case "SHOTGUN":
                    ShotgunFire();
                    break;
                case "RIFLE":
                    NormalFire();
                    break;
                case "AKIMBO":
                    StartCoroutine(AkimboFire());
                    break;
                case "ROCKET":
                    NormalFire();
                    break;
            }
        }
    }

    private void CreateBullets() {
        GameObject bullet = ObjectManager.Instance.UseObject(bulletName);
        bullet.transform.position = new Vector3(muzzle.position.x, muzzle.position.y + 1, 0);

        float angle = MathCalculator.Instance.Angle(bullet.transform.position, mouse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = atk;
            bulletPlayer.AtkShield = atkShield;
            bulletPlayer.Speed = Random.Range(minSpeed, maxSpeed);
            bulletPlayer.StoppingPower = stoppingPower;
            bulletPlayer.StoppingTime = stoppingTime;
            bulletPlayer.BulletName = bulletName;
            bulletPlayer.MuzzlePosition = muzzle.position;

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

    IEnumerator AkimboFire() {
        float cooltime = 0.1f;

        animator.SetTrigger("GunFire");
        // Sound baam
        CreateBullets();

        yield return CoroutineCache.WaitForSecond(cooltime);

        // Sound baam
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
