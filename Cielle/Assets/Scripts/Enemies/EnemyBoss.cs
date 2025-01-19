using UnityEngine;

public abstract class EnemyBoss : Enemy {
    protected override GeneralFSM<Enemy> InitialState() {
        return new EnemyState_InWait<Enemy>(this);
    }

    public override void OnHit(float damage, float damageShield, float stoppingPower, float stoppingTime, Vector3 hitPosition) {
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

            EnemyManager.OnReturnEnemy?.Invoke(gameObject, id);
        }
    }

    public abstract void Wait();
    public abstract void Pattern();
    public abstract void Dead();

    public override void Patrol() { return; }
    public override void Chase() { return; }
    public override void Attack() { return; }
}
