using UnityEngine;

public class CptWayne : EnemyBoss {
    public override void Dead()
    {
    }

    public override void Pattern()
    {
    }

    public override void Wait() {
        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
