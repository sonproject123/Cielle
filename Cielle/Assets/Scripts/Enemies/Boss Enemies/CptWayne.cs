using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CptWayne : EnemyBoss {
    [SerializeField] GameObject patternPoint1;
    [SerializeField] Transform forwardTarget;
    [SerializeField] List<Enemy> pattern1Enemies = new List<Enemy>();

    private void Start() {
        PatternAdd();
    }

    private new void PatternAdd() {
        patternActions = new Action[6];

        int index = 1;
        patternActions[index++] = Pattern1; // 드론 소환
        patternActions[index++] = Pattern2; // 점프
        patternActions[index++] = Pattern3; // 원형 사격
        patternActions[index++] = Pattern4; // 지향 점사 사격
        patternActions[index++] = Pattern5; // 순보
        //patternActions[index++] = Pattern6; // 수류탄 투척

        base.PatternAdd();
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
        if (isInChaseRange && patternCooltimes.TryGetValue(5, out bool is5On) && is5On) {
            patternID = 5;
            StartCoroutine(PatternCooltime(2, patternData.cooltime[2]));
        }
        else if (patternCooltimes.TryGetValue(1, out bool is1On) && is1On)
            patternID = 1;
        else {
            do
                patternID = random.Next(1, patterns.Count + 1);
            while (patternID == 5);
        }

        base.Pattern();
    }

    private void Pattern1() {
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
    }

    private void Pattern2() {
        float offsetX = 60;
        float offsetY = 90;

        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            offsetX *= -1;

        rigidBody.AddForce(Vector3.right * offsetX, ForceMode.Impulse);
        rigidBody.AddForce(Vector3.up * offsetY, ForceMode.Impulse);
    }

    private void Pattern3() {
        StartCoroutine(Pattern3Attack());
    }

    IEnumerator Pattern3Attack() {
        float offset = -10;
        float time = 0;
        float cooltime = 0.075f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < Time.fixedDeltaTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        int angle = 0;
        int plus = 1;
        while (angle >= 0) {
            if (isDead)
                yield break;

            time = 0;
            muzzleRotation.localRotation = Quaternion.Euler(muzzleRotation.localRotation.x, muzzleRotation.localRotation.y, offset * angle);
            LinearBulletSpawn(forwardTarget.position, 90);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }

            angle += plus;
            if (angle >= 8)
                plus = -1;
        }
    }

    private void Pattern4() {
        StartCoroutine(Pattern4Attack());
    }

    IEnumerator Pattern4Attack() {
        float time = 0;
        float cooltime = 0.15f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < Time.fixedDeltaTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        Vector3 playerPosition = player.position;
        for (int i = 1; i <= 3 * 3; i++) {
            if (isDead)
                yield break;

            time = 0;

            float angle = MathCalculator.Instance.Angle(playerPosition, muzzle.position);
            if (playerPosition.x < transform.position.x)
                muzzleRotation.localRotation = Quaternion.Euler(0, 0, angle + 180);
            else
                muzzleRotation.localRotation = Quaternion.Euler(0, 0, angle * -1);

            LinearBulletSpawn(forwardTarget.position, 90);

            while (time < cooltime) {
                time += Time.deltaTime;
                yield return wffu;
            }

            if (i % 3 == 0) {
                while (time < cooltime * 2) {
                    time += Time.deltaTime;
                    yield return wffu;
                }
                LookAtPlayer();
                playerPosition = player.position;
            }
        }

        muzzleRotation.localRotation = Quaternion.Euler(0, 0, 0);
        muzzle.localRotation = muzzleRotation.rotation;
    }

    private void Pattern5() {
        RaycastHit leftHit;
        Physics.Raycast(transform.position, Vector2.left, out leftHit, 9999, LayerMask.GetMask("Wall"));
        RaycastHit rightHit;
        Physics.Raycast(transform.position, Vector2.right, out rightHit, 9999, LayerMask.GetMask("Wall"));

        StartCoroutine(Pattern5Move(leftHit.distance, rightHit.distance));
    }

    IEnumerator Pattern5Move(float left, float right) {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while(time < 0.5f) {
            if (isDead)
                yield break;

            if (left > right)
                rigidBody.MovePosition(rigidBody.position + Vector3.left * speed * Time.deltaTime);
            else
                rigidBody.MovePosition(rigidBody.position + Vector3.right * speed * Time.deltaTime);
            time += Time.deltaTime;
            yield return wffu;
        }
    }

    private void Pattern6() {

    }

    public override void Dead() {
        foreach (var enemy in pattern1Enemies)
            enemy.ForcedDie();
        pattern1Enemies.Clear();

        base.Dead();
    }
}
