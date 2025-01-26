using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyBoss : Enemy, IHitable {
    [SerializeField] protected Dictionary<int, Action> patterns = new Dictionary<int, Action>();
    [SerializeField] protected Dictionary<int, bool> patternCooltimes = new Dictionary<int, bool>();
    [SerializeField] protected new GeneralFSM<EnemyBoss> currentState;
    [SerializeField] protected RoomTemplateStats goalRTS;

    [SerializeField] protected BossPatternData patternData;
    [SerializeField] protected Action[] patternActions;
    [SerializeField] protected int patternID;
    [SerializeField] protected bool isPatternOnGoing;
    [SerializeField] protected bool isSkipEndable;

    private new void Awake() {
        isBoss = true;
        isPatternOnGoing = false;
        isSkipEndable = false;
        currentState = InitialBossState();
        currentState.OnStateEnter();
        ui.SetActive(false);
        
        base.Awake();
    }

    private new void OnEnable() {
        base.OnEnable();
        if (id != 0)
            JsonManager.Instance.BossPatternDict.TryGetValue(id, out patternData);
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
        BreakObject(hitPosition);

        if (hp <= 0.0 && !isDead) {
            isDead = true;
            ChangeState(new EnemyState_InDead<EnemyBoss>(this));
        }
    }

    public virtual void Wait() {
        LookAtPlayer();
    }

    public abstract void PatternInit();

    protected virtual void PatternAdd() {
        for (int i = 1; i < patternActions.Length; i++) {
            patterns.Add(i, patternActions[i]);
            patternCooltimes.Add(i, true);
        }
    }

    public virtual void Pattern() {
        if (isPatternOnGoing)
            return;

        if (patternID == 0)
            patternID = random.Next(1, patterns.Count + 1);

        if (patternCooltimes.TryGetValue(patternID, out bool isOn) && isOn) {
            isPatternOnGoing = true;
            patterns.TryGetValue(patternID, out Action pattern);
            LookAtPlayer();

            StartCoroutine(PatternOngoing(patternData.patternTime[patternID]));
            StartCoroutine(PatternCooltime(patternID, patternData.cooltime[patternID]));
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

        while (time < coolTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        patternCooltimes[id] = true;
    }

    public virtual void Dead() {
        GeneralStats.Instance.Pause = true;
        Stats.Instance.IsInvincible = true;
        LetterBoxManager.Instance.LetterBox();

        foreach (var wall in goalRTS.bossWalls) {
            BossWall bw = wall.GetComponent<BossWall>();
            bw.OnDoorMove(false);
        }

        UIManager.OnUIAlpha(0, true);
        PlayerCamera.OnCameraMove?.Invoke(transform.position, 0.2f);
        StartCoroutine(WaitSomeSecond(1));
    }

    public virtual void OnDead() {
        if (isSkipEndable && Input.anyKeyDown)
            EndDirection();
    }

    private void EndDirection() {
        GeneralStats.Instance.Pause = false;
        Stats.Instance.IsInvincible = false;
        LetterBoxManager.Instance.LetterBox();

        UIManager.OnUIAlpha(1, false);
        PlayerCamera.OnIsCameraFollow?.Invoke(true);
        ChangeState(new EnemyState_InPattern<EnemyBoss>(this));

        for (int i = 0; i < 30; i++)
            BreakObject(new Vector3(transform.localPosition.x, transform.localPosition.y - 3, transform.localPosition.z));

        for (int i = 0; i < price; i++)
            MetalObject();

        EnemyManager.OnReturnEnemy?.Invoke(gameObject, id);
    }

    IEnumerator WaitSomeSecond(float waitTime) {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float time = 0;

        while (time < waitTime) {
            time += Time.deltaTime;
            yield return wffu;
        }

        isSkipEndable = true;
    }

    public override void Patrol() { return; }
    public override void Chase() { return; }
    public override void Attack() { return; }
    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) { return; }

    public RoomTemplateStats GoalRTS {
        set { goalRTS = value; }
    }
}
