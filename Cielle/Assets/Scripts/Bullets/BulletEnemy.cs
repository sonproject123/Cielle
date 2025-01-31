using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float attackShield;
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingPower;
    [SerializeField] protected float stoppingTime;
    [SerializeField] protected float angle;
    [SerializeField] protected Vector3 target;

    [SerializeField] Transform bulletRotation;
    [SerializeField] Vector3 direction;

    public void BulletInit(float attack, float attackShield, float speed, float stoppingPower, float stoppingTime, float angle, Vector3 target) {
        this.attack = attack;
        this.attackShield = attackShield;
        this.speed = speed;
        this.stoppingPower = stoppingPower;
        this.stoppingTime = stoppingTime;
        this.angle = angle;
        this.target = target;

        Vector3 direction = MathCalculator.Instance.Normalizer(target, transform.position);
        bulletRotation.rotation = Quaternion.LookRotation(direction);
    }

    private void FixedUpdate() {
        transform.position += bulletRotation.forward * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack, attackShield, stoppingPower, stoppingTime, transform.position);
            ObjectManager.Instance.ReturnObject(gameObject, "ENEMYBULLET");
        }
        else if (other.CompareTag("Wall"))
            ObjectManager.Instance.ReturnObject(gameObject, "ENEMYBULLET");
    }
}
