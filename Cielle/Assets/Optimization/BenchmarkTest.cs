using UnityEngine;

public class BenchmarkTest : MonoBehaviour {
    public void PerformTest() {
        GameObject bullet = ObjectManager.Instance.UseObject("ENEMYBULLET");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

        BulletEnemy bulletEnemy = bullet.GetComponent<BulletEnemy>();
        bulletEnemy.BulletInit(0, 0, 8, 0, 0, 90, Vector3.up);
    }
}
