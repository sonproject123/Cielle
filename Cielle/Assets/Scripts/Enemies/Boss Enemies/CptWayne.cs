using UnityEngine;

public class CptWayne : EnemyBoss {
    public override void Wait() {
        if (Stats.Instance.PlayerCenter.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public override void Pattern()
    {
    }

    public override void Dead()
    {
    }
}
