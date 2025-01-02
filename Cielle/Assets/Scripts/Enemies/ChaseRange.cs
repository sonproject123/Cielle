using UnityEngine;

public class ChaseRange : MonoBehaviour {
    [SerializeField] GameObject master;
    private Enemy es;

    private void Start() {
        master = transform.parent.gameObject;
        es = master.GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            es.ChaseRange(true);
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
            es.ChaseRange(false);
    }
}
