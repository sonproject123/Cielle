using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFlyingGunDrone : EnemyStats {
    [SerializeField] Transform muzzle;

    private void Awake() {
        muzzle = transform.Find("Rotate Dummy");
    }

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

    IEnumerator Attack() {
        while (true) {
            yield return CoroutineCache.WaitForSecond(cooltime);
            Debug.Log("attack");
        }
    }

    private void Muzzle() {
        float angle = MathCalculator.Instance.Angle(player.position, muzzle.position);
        muzzle.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void BulletSpawn() {
        base.BulletSpawn();
        Debug.Log("spwan");
    }
}
