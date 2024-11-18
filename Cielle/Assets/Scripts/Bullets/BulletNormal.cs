using UnityEngine;

public class BulletNormal : BulletPlayer {
    private void FixedUpdate() {
        transform.position += bulletRotation.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack);
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.PLAYERBULLET);
        }
        else if (other.CompareTag("Wall"))
            ObjectManager.Instance.ReturnObject(gameObject, ObjectList.PLAYERBULLET);
    }
}
