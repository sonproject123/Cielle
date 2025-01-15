using UnityEngine;

public class EnemyBoss : Enemy {
    protected override GeneralFSM<Enemy> InitialState() {
        return new EnemyState_InPatrol<Enemy>(this);
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
        else if (hp > 0.0)
        {
            Vector3 dir = Vector3.right;
            if (transform.position.x < hitPosition.x)
                dir *= -1;
        }
    }

    public override void Patrol() { return; }

    public override void Chase() { return; }

    public override void Attack() { return; }
}
