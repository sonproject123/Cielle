using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Move : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] LayerMask ground;

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
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Wall")) {
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
            Shoot();
        }
    }

    private void Shoot() {
        GameObject bullet = ResourcesManager.Instance.Instantiate("PlayerBullet");
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        float angle = MathCalculator.Instance.Angle(transform.position, Stats.Instance.MouseLocation);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        BulletPlayer bulletPlayer = bullet.GetComponent<BulletPlayer>();
        if (bulletPlayer != null) {
            bulletPlayer.Atk = Stats.Instance.Atk;
            bulletPlayer.Speed = 20;
            bulletPlayer.Target = Stats.Instance.MouseLocation;
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