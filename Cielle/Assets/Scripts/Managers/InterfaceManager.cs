using UnityEngine;

public interface IInRange {
    public void InRange(bool value);
}

public interface IHitable {
    public void Hit(float damage, float shieldDamage, Vector3 hitPosition);
}