using UnityEngine;

public class BulletNormal : BulletPlayer {
    private void FixedUpdate() {
        transform.position += bulletRotation.forward * speed * Time.deltaTime;
    }
}
