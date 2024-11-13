using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Move : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Vector3 mousePosition;

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

        if (Input.GetMouseButtonDown(0)) {
            MousePosition();
            Shoot();
        }
    }

    private void MousePosition() {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y, 
            -Camera.main.transform.position.z));
    }

    private void Shoot() {
        GameObject bullet = ResourcesManager.Instance.Instantiate("PlayerBullet");
        bullet.transform.position = new Vector3(0,0,0);
        bullet.transform.rotation = Quaternion.Euler(0, 0, MathCalculator.Instance.Angle(transform.position, mousePosition));

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = Stats.Instance.Atk + Random.Range(1, 10);
            bulletPlayer.Speed = 3;
            bulletPlayer.Target = mousePosition;
        }
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}