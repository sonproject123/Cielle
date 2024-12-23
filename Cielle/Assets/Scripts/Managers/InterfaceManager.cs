using UnityEngine;

public interface IHitable {
    public void Hit(float damage, float shieldDamage, float stoppingPower, float stoppingTime, Vector3 hitPosition);
}