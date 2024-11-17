using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Move : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform playerCenter;
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

        rigidBody.mass = Stats.Instance.Mass;

        gunCategory = Guns.PISTOL;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) 
            isOnGround = true;
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = false;
    }

    private void OnEnable() {
        InputManager.Instance.action += OnkeyUpdate;
    }

    private void OnkeyUpdate() {
        if (GeneralStats.Instance.Pause == true)
            return;

        // Move
        if (Input.GetKey(KeyCode.A) && isMovable) {
            if (isOnFlying) 
                FlyMove(Vector3.left);
            else 
                GroundMove(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D) && isMovable) {
            if (isOnFlying) 
                FlyMove(Vector3.right);
            else 
                GroundMove(Vector3.right);
        }
        if (Input.GetKey(KeyCode.W) && isMovable && isOnFlying) 
            FlyMove(Vector3.up);
        if (Input.GetKey(KeyCode.S) && isMovable && isOnFlying) 
            FlyMove(Vector3.down);
        

        // Jump & Fly
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetKey(KeyCode.W) && !isOnFlying) {
                StartCoroutine(FlyStart());
            }
            else if (isOnFlying) {
                StartCoroutine(FlyEnd());
            }
            else if (isOnGround && !isOnFlying)
                Jump();
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashable) {
            if(Input.GetKey(KeyCode.A) && transform.position.x - GeneralStats.Instance.MouseLocation.x > 0)
                StartCoroutine(Dash(false));
            else if (Input.GetKey(KeyCode.D) && transform.position.x - GeneralStats.Instance.MouseLocation.x < 0)
                StartCoroutine(Dash(false));

            else if(Input.GetKey(KeyCode.A) && transform.position.x - GeneralStats.Instance.MouseLocation.x < 0)
                StartCoroutine(Dash(true));
            else if (Input.GetKey(KeyCode.D) && transform.position.x - GeneralStats.Instance.MouseLocation.x > 0)
                StartCoroutine(Dash(true));

            else
                StartCoroutine(Dash(false));
        }

        // Shoot
        if (Input.GetMouseButtonDown(0)) {
            GunFire.Instance.Shoot(gunCategory, transform);
        }
        if (Input.GetMouseButton(0)) {
            if(gunCategory == Guns.RIFLE)
                GunFire.Instance.Shoot(gunCategory, transform);
        }
    }

    private void GroundMove(Vector3 dir) {
        animator.SetBool("Run", true);
        transform.position += dir * Stats.Instance.Speed * Time.deltaTime;

    }

    private void FlyMove(Vector3 dir) {
        //Ray ray = new Ray(rigidBody.position, dir);
        //RaycastHit hit;
        //
        //if(!Physics.Raycast(ray, out hit, dir.magnitude))
            rigidBody.MovePosition(transform.position + dir * Stats.Instance.FlySpeed * Time.deltaTime);
    }

    private void Jump() {
        rigidBody.AddForce(Vector3.up * Stats.Instance.JumpHeight, ForceMode.Impulse);
    }

    IEnumerator FlyStart() {
        isDashable = false;
        isMovable = false;
        rigidBody.useGravity = false;

        float flytime = 0;
        float flyDashSpeed = Stats.Instance.Speed;
        float speed = Stats.Instance.Speed;
        float flySpeed = Stats.Instance.FlySpeed;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (flytime < Stats.Instance.FlyTime) {
            Vector3 direction = (GeneralStats.Instance.MouseLocation - playerCenter.position).normalized;
            playerCenter.rotation = Quaternion.LookRotation(direction);
            transform.position += playerCenter.forward * flyDashSpeed * Time.fixedDeltaTime;
            flyDashSpeed += Mathf.Max(flySpeed - speed, 0) * Time.fixedDeltaTime * 5;

            flytime += Time.fixedDeltaTime;
            yield return wffu;
        }

        isOnFlying = true;
        isMovable = true;
        isDashable = true;
    }

    IEnumerator FlyEnd() {
        isOnFlying = false;
        rigidBody.useGravity = true;
        isMovable = false;
        isDashable = false;

        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float mass = Stats.Instance.Mass;
        rigidBody.mass = mass * 10;

        while (!isOnGround) {
            rigidBody.mass += mass * 10 * Time.fixedDeltaTime;
            yield return wffu;
        }

        isMovable = true;
        isDashable = true;
        rigidBody.mass = mass;
    }

    IEnumerator Dash(bool back) {
        isDashable = false;
        isMovable = false;

        float dashTime = 0;
        Vector3 dashDirection = transform.forward;
        if (back)
            dashDirection *= -1;

        float dashSpeed;
        if (isOnFlying)
           dashSpeed = Stats.Instance.FlyDashSpeed;
        else
           dashSpeed = Stats.Instance.DashSpeed;

        while (dashTime < Stats.Instance.DashTime) {
            transform.position += dashDirection * Time.deltaTime * dashSpeed;
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