using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    [SerializeField] protected float attack;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 target;
    [SerializeField] Vector3 direction;

    private void Start() {
        Vector3 direction = (target - transform.position).normalized;
    }

    public float Atk {
        get { return attack; }
        set { attack = value; }
    }

    public float Speed {
        get { return speed; } 
        set { speed = value; } 
    }

    public Vector3 Target {
        get { return target; }
        set { target = value; }
    }

    private void FixedUpdate() {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            IHitable hitable = other.GetComponent<IHitable>();
            hitable.Hit(attack);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
