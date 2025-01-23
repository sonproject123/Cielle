using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    public override void Pattern() {
        if (patternCooltimes.TryGetValue(1, out bool isOn) && isOn)
            patternID = 1;
        else
            patternID = 0;

        base.Pattern();
    }

    private void Pattern1() {
        float patternTime = 2;
        float cooltime = patternTime + 23;
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
        float patternTime = 2.5f;
        float cooltime = patternTime + 1;

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
        float cooltime = patternTime + 3;

        StartCoroutine(PatternOngoing(patternTime));

        StartCoroutine(Pattern3Attack());

        StartCoroutine(PatternCooltime(3, cooltime));
    }

    IEnumerator Pattern3Attack() {
        float offset = -15;
        float time = 0;
        float cooltime = 0.1f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < Time.fixedDeltaTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        int angle = 0;
        int plus = 1;
        while (angle >= 0) {
            time = 0;
            muzzleRotation.localRotation = Quaternion.Euler(muzzleRotation.localRotation.x, muzzleRotation.localRotation.y, offset * angle);
            muzzle.localRotation = muzzleRotation.rotation;
            LinearBulletSpawn(forwardTarget.position, 90);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }

            angle += plus;
            if (angle >= 5)
                plus = -1;
        }
    }

    private void Pattern4() {
        float patternTime = 2;
        float cooltime = patternTime + 3;

        StartCoroutine(PatternOngoing(patternTime));

        StartCoroutine(Pattern4Attack());

        StartCoroutine(PatternCooltime(4, cooltime));
    }

    IEnumerator Pattern4Attack() {
        float time = 0;
        float cooltime = 0.15f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < Time.fixedDeltaTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        for (int i = 0; i < 10; i++) {
            time = 0;
            muzzleRotation.localRotation = Quaternion.Euler(0,0,0);
            muzzle.localRotation = muzzleRotation.rotation;
            LinearBulletSpawn(forwardTarget.position, 90);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }

            if (i == 4) {
                while (time < cooltime * 5) {
                    time += Time.deltaTime;
                    yield return wffu;
                }
            }
        }

        muzzleRotation.rotation = Quaternion.Euler(0, 0, 0);
        muzzle.localRotation = muzzleRotation.rotation;
    }

    public override void Dead() {
    }
}
