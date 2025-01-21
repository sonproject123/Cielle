using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CptWayne : EnemyBoss {
    [SerializeField] GameObject patternPoint1;
    [SerializeField] List<GameObject> pattern1Enemies = new List<GameObject>();
    [SerializeField] List<Enemy> pattern1EnemiesStats = new List<Enemy>();

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

        patternPoint1 = ResourcesManager.Instance.Instantiate("Enemies/Boss/Cpt_Wayne_PatternPoint1", transform);
        patternPoint1.transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

        for (int i = 0; i < 4; i++) {
            GameObject enemy = EnemyManager.OnUseEnemy?.Invoke(1);
            Enemy enemyStat = enemy.GetComponent<Enemy>();
            enemyStat.MaxHp *= 1.5f;
            enemyStat.IsSummoned = true;
            enemy.SetActive(false);
            pattern1Enemies.Add(enemy);
            pattern1EnemiesStats.Add(enemyStat);
        }
    }

    private void Pattern1() {
        float patternTime = 3;
        float cooltime = 30;
        StartCoroutine(PatternOngoing(patternTime));

        int index = 0;
        foreach (Transform point in patternPoint1.transform) {
            GameObject enemy = pattern1Enemies[index];
            Enemy enemyStat = pattern1EnemiesStats[index++];
            enemyStat.Hit(999999, 0, 0, 0, Vector3.up);
            
            enemy.transform.position = new Vector3(point.position.x, point.position.y, 0);
            enemy.SetActive(true);
        }

        StartCoroutine(PatternCooltime(1, cooltime));
    }

    private void Pattern2() {
        float patternTime = 1;
        float cooltime = 3000;
        StartCoroutine(PatternOngoing(patternTime));



        StartCoroutine(PatternCooltime(2, cooltime));
    }

    private void Pattern3() {
        float patternTime = 1;
        float cooltime = 3000;
        StartCoroutine(PatternOngoing(patternTime));
        StartCoroutine(PatternCooltime(3, cooltime));
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
