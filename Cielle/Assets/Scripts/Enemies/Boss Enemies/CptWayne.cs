using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CptWayne : EnemyBoss {
    [SerializeField] GameObject patternPoint1;
    [SerializeField] Transform forwardTarget;
    [SerializeField] List<Enemy> pattern1Enemies = new List<Enemy>();

    Action pattern1;
    Action pattern2;
    Action pattern3;
    Action pattern4;

    private void Start() {
        PatternAdd();
    }

    private void PatternAdd() {
        pattern1 = () => { Pattern1(); };
        patterns.Add(1, pattern1);

        pattern2 = () => { Pattern2(); };
        patterns.Add(2, pattern2);

        pattern3 = () => { Pattern3(); };
        patterns.Add(3, pattern3);

        pattern4 = () => { Pattern4(); };
        patterns.Add(4, pattern4);


        for (int i = 1; i <= patterns.Count; i++)
            patternCooltimes.Add(i, true);
    }

    public override void PatternInit() {
        float offsetX = 15;
        float offsetY = 8;
        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            offsetX *= -1;

        patternPoint1 = ResourcesManager.Instance.Instantiate("Enemies/Boss/Cpt_Wayne_PatternPoint1");
        patternPoint1.transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
    }

    private void Pattern1() {
        float patternTime = 3;
        float cooltime = 30;
        StartCoroutine(PatternOngoing(patternTime));

        foreach(var enemy in pattern1Enemies)
            enemy.ForcedDie();
        pattern1Enemies.Clear();

        foreach (Transform point in patternPoint1.transform) {
            GameObject enemy = EnemyManager.OnUseEnemy?.Invoke(1);
            Enemy enemyStat = enemy.GetComponent<Enemy>();
            enemy.SetActive(true);
            enemyStat.IsSummoned = true;
            enemyStat.Revive();
            
            enemy.transform.position = new Vector3(point.position.x, point.position.y, 0);
            pattern1Enemies.Add(enemyStat);
        }

        StartCoroutine(PatternCooltime(1, cooltime));
    }

    private void Pattern2() {
        float patternTime = 3;
        float cooltime = 2;

        float offsetX = 75;
        float offsetY = 125;
        StartCoroutine(PatternOngoing(patternTime));

        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            offsetX *= -1;

        rigidBody.AddForce(Vector3.right * offsetX, ForceMode.Impulse);
        rigidBody.AddForce(Vector3.up * offsetY, ForceMode.Impulse);

        StartCoroutine(PatternCooltime(2, cooltime));
    }

    private void Pattern3() {
        float patternTime = 2;
        float cooltime = 5;
        StartCoroutine(PatternOngoing(patternTime));

        StartCoroutine(Pattern3Attack());

        StartCoroutine(PatternCooltime(3, cooltime));
    }

    IEnumerator Pattern3Attack() {
        float offset = -15;
        float time = 0;
        float cooltime = 0.15f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        for (int angle = 0; angle <= 5; angle++) {
            muzzleRotation.localRotation = Quaternion.Euler(muzzleRotation.localRotation.x, muzzleRotation.localRotation.y, offset * angle);
            LinearBulletSpawn(forwardTarget.position);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }
            time = 0;
        }

        for (int angle = 4; angle >= 0; angle--) {
            muzzleRotation.localRotation = Quaternion.Euler(muzzleRotation.localRotation.x, muzzleRotation.localRotation.y, offset * angle);
            LinearBulletSpawn(forwardTarget.position);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }
            time = 0;
        }
    }

    private void Pattern4() {
        float patternTime = 1;
        float cooltime = 3000;
        StartCoroutine(PatternOngoing(patternTime));
        StartCoroutine(PatternCooltime(4, cooltime));
    }

    

    public override void Dead() {
    }
}
