using UnityEngine;

public abstract class EnemyBoss : Enemy, IHitable {
    [SerializeField] protected new GeneralFSM<EnemyBoss> currentState;

    private new void Awake() {
        isBoss = true;
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

    public abstract void Wait();
    public abstract void Pattern();
    public abstract void Dead();

    public override void Patrol() { return; }
    public override void Chase() { return; }
    public override void Attack() { return; }
    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) { return; }
}
