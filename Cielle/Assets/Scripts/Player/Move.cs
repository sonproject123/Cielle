using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Move : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        InputManager.Instance.action += OnkeyUpdate;
    }

    private void OnkeyUpdate() {
        if (GeneralStats.Instance.Pause == true)
            return;

        if (Input.GetKey(KeyCode.A)) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            animator.SetBool("Run", true);
            transform.position += Vector3.left * Stats.Instance.Speed * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Run", true);
            transform.position += Vector3.right * Stats.Instance.Speed * Time.deltaTime;
        }
        else {
            animator.SetBool("Run", false);
        }
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}