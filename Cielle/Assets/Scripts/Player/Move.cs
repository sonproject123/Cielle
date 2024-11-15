using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Move : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] LayerMask ground;

    [SerializeField] Guns gunCategory;
    [SerializeField] GameObject gun;

    [SerializeField] bool isMovable;

    [SerializeField] bool isOnGround;
    [SerializeField] bool isOnFlying;

    [SerializeField] protected bool isDashable;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        isMovable = true;
        isOnGround = true;
        isOnFlying = false;
        isDashable = true;

        gunCategory = Guns.PISTOL;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    private void OnEnable() {
        InputManager.Instance.action += OnkeyUpdate;
    }

    private void OnkeyUpdate() {
        if (GeneralStats.Instance.Pause == true)
            return;

        if (Input.GetKey(KeyCode.A) && isMovable) {
            animator.SetBool("Run", true);
            transform.position += Vector3.left * Stats.Instance.Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && isMovable) {
            animator.SetBool("Run", true);
            transform.position += Vector3.right * Stats.Instance.Speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetKey(KeyCode.W)) {

            }

            if (isOnGround == true)
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashable) {
            if(Input.GetKey(KeyCode.A) && transform.position.x - Stats.Instance.MouseLocation.x > 0)
                StartCoroutine(Dash(false));
            else if (Input.GetKey(KeyCode.D) && transform.position.x - Stats.Instance.MouseLocation.x < 0)
                StartCoroutine(Dash(false));

            else if(Input.GetKey(KeyCode.A) && transform.position.x - Stats.Instance.MouseLocation.x < 0)
                StartCoroutine(Dash(true));
            else if (Input.GetKey(KeyCode.D) && transform.position.x - Stats.Instance.MouseLocation.x > 0)
                StartCoroutine(Dash(true));

            else
                StartCoroutine(Dash(false));
        }

        if (Input.GetMouseButtonDown(0)) {
            GunFire.Instance.Shoot(gunCategory, transform);
        }
        if (Input.GetMouseButton(0)) {
            if(gunCategory == Guns.RIFLE)
                GunFire.Instance.Shoot(gunCategory, transform);
        }
    }

    private void Jump() {
        isOnGround = false;
        rigidBody.AddForce(Vector3.up * Stats.Instance.JumpHeight, ForceMode.Impulse);
    }

    IEnumerator Dash(bool back) {
        isDashable = false;
        isMovable = false;

        float dashTime = 0;
        Vector3 dashDirection = transform.forward;
        if (back)
            dashDirection *= -1;

        while (dashTime < Stats.Instance.DashTime) {
            transform.position += dashDirection * Time.deltaTime * Stats.Instance.DashSpeed;
            dashTime += Time.deltaTime;
            yield return null;
        }

        isMovable = true;

        yield return CoroutineCache.WaitForSecond(Stats.Instance.DashCooltime);
        isDashable = true;
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}