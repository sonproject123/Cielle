using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBoss : Enemy, IHitable {
    [SerializeField] protected Dictionary<int, Action> patterns = new Dictionary<int, Action>();
    [SerializeField] protected Dictionary<int, bool> patternCooltimes = new Dictionary<int, bool>();
    [SerializeField] protected new GeneralFSM<EnemyBoss> currentState;

    [SerializeField] protected bool isPatternOnGoing;

    private new void Awake() {
        isBoss = true;
        isPatternOnGoing = false;
        currentState = InitialBossState();
        currentState.OnStateEnter();
        ui.SetActive(false);
        base.Awake();
    }

    protected new void FixedUpdate() {
        currentState.OnStateStay();
    }

    public void ChangeState(GeneralFSM<EnemyBoss> newState) {
        currentState.OnStateExit();
        currentState = newState;
        currentState.OnStateEnter();
    }

    public void UIOn() {
        ui.SetActive(true);
    }

    protected GeneralFSM<EnemyBoss> InitialBossState() {
        return new EnemyState_InWait<EnemyBoss>(this);
    }

    public new void Hit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
        hp -= Mathf.Max(1, damage - defense);
        enemyUI.HpBar();

        if (hp <= 0.0 && !isDead) {
            isDead = true;

            for (int i = 0; i < 10; i++)
                BreakObject(hitPosition);

            if (!isSummoned) {
                for (int i = 0; i < price; i++)
                    MetalObject();
            }

            //EnemyManager.OnReturnEnemy?.Invoke(gameObject, id);
        }
    }

    public virtual void Wait() {
        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public abstract void PatternInit();

    public virtual void Pattern() {
        if (isPatternOnGoing)
            return;

        System.Random random = new System.Random();
        int id = random.Next(1, patterns.Count + 1);

        if (patternCooltimes.TryGetValue(id, out bool isOn) && isOn) {
            isPatternOnGoing = true;
            patterns.TryGetValue(id, out Action pattern);
            pattern.Invoke();
        }
    }

    protected IEnumerator PatternOngoing(float patternTime) {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < patternTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        isPatternOnGoing = false;
    }

    protected IEnumerator PatternCooltime(int id, float coolTime) {
        patternCooltimes[id] = false;

        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < coolTime)
        {
            time += Time.deltaTime;
            yield return wffu;
        }

        patternCooltimes[id] = true;
    }

    public abstract void Dead();

    public override void Patrol() { return; }
    public override void Chase() { return; }
    public override void Attack() { return; }
    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) { return; }
}
