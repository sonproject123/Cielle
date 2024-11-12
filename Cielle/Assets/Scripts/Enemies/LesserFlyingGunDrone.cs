using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats, IInRange {
    [SerializeField] Transform muzzleRotation;
    [SerializeField] Transform muzzle;
    [SerializeField] bool inRange = false;

    protected override void Start() {
        base.Start();
        maxHp = Random.Range(100.0f, 110.0f);
        hp = maxHp;
        speed = 0;
        attack = 10;
        defense = 0;
        cooltime = 5;

        StartCoroutine(Attack());
    }

    private void Update() {
        Muzzle();
    }

    public void InRange(bool value) {
        inRange = value;
    }

    IEnumerator Attack() {
        while (true) {
            yield return CoroutineCache.WaitForSecond(cooltime);

            if(inRange)
                BulletSpawn();
        }
    }

    private void Muzzle() {
        float angle = MathCalculator.Instance.Angle(player.position, muzzleRotation.position);
        muzzleRotation.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void BulletSpawn() {
        GameObject bullet = ResourcesManager.Instance.Instantiate("EnemyBullet");
        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = muzzle.transform.rotation;

        BulletEnemy bulletEnemy = bullet.GetComponent<BulletEnemy>();
        if(bulletEnemy != null) {
            bulletEnemy.Atk = attack + Random.Range(1, 10);
            bulletEnemy.Speed = 3;
        }
    }
}
