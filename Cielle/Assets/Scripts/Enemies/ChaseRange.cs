using UnityEngine;

public class ChaseRange : MonoBehaviour {
    [SerializeField] GameObject master;
    private EnemyStats es;

    private void Start() {
        master = transform.parent.gameObject;
        es = master.GetComponent<EnemyStats>();
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
