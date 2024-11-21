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

    [SerializeField] GameObject gun;

    [SerializeField] bool isMovable;
    [SerializeField] bool isOnGround;
    [SerializeField] bool isOnFlying;
    [SerializeField] bool isDashable;
    [SerializeField] bool isWeaponChangeable;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] float horizontalDash;
    [SerializeField] float verticalDash;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        isMovable = true;
        isOnGround = true;
        isOnFlying = false;
        isDashable = true;
        isWeaponChangeable = true;

        rigidBody.mass = Stats.Instance.Mass;

        horizontalInput = 0;
        verticalInput = 0;
        horizontalDash = 0;
        verticalDash = 0;
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

    private void Update() {
        if (!Stats.Instance.IsMove) {
            if (isOnFlying)
                return;
            else
                animator.SetBool("Run", false);
        }
    }

    private void OnkeyUpdate() {
        if (GeneralStats.Instance.Pause == true)
            return;
        Stats.Instance.IsMove = true;

        // Move
        horizontalInput = 0;
        verticalInput = 0;

        if (isMovable && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) {
            if (Input.GetKey(KeyCode.A))
                verticalInput = -1;
            else if (Input.GetKey(KeyCode.D))
                verticalInput = 1;

            if (Input.GetKey(KeyCode.W))
                horizontalInput = 1;
            else if (Input.GetKey(KeyCode.S))
                horizontalInput = -1;

            if (isOnFlying)
                FlyMove();
            else if (verticalInput != 0)
                GroundMove();
        }

        // Jump & Fly
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetKey(KeyCode.W) && !isOnFlying) 
                StartCoroutine(FlyStart());
            else if (isOnFlying) 
                StartCoroutine(FlyEnd());
            else if (isOnGround && !isOnFlying)
                Jump();
        }

        // Dash
        horizontalDash = 0;
        verticalDash = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashable) {
            if (Input.GetKey(KeyCode.A))
                verticalDash = -1;
            else if (Input.GetKey(KeyCode.D)) 
                verticalDash = 1;
            else {
                if (Stats.Instance.IsLeft)
                    verticalDash = -1;
                else
                    verticalDash = 1;
            }

            if (Input.GetKey(KeyCode.W) && isOnFlying)
                horizontalDash = 1;
            else if (Input.GetKey(KeyCode.S) && isOnFlying)
                horizontalDash = -1;

            StartCoroutine(Dash());
        }

        // Shoot
        if (Input.GetMouseButtonDown(0) && Stats.Instance.MainGunFireType == GunFireType.SINGLE) {
            animator.SetTrigger("GunFire");
            GunFire.Instance.Shoot(transform);

        }
        if (Input.GetMouseButton(0) && Stats.Instance.MainGunFireType == GunFireType.REPEAT) {
            animator.SetTrigger("GunFire");
            GunFire.Instance.Shoot(transform);
        }

        // Weapon Change
        if(Input.mouseScrollDelta.y != 0 && isWeaponChangeable) {
            isWeaponChangeable = false;

            (Stats.Instance.MainWeaponId, Stats.Instance.SubWeaponId) = (Stats.Instance.SubWeaponId, Stats.Instance.MainWeaponId);
            Stats.Instance.GunChange();

            StartCoroutine(WeaponChangeCooltime());
        }
    }

    private void RigidMove(Vector3 dir, float speed, float wallSensor) {
        if(!Physics.Raycast(rigidBody.position, dir, wallSensor, LayerMask.GetMask("Wall")))
            rigidBody.MovePosition(rigidBody.position + dir * speed * Time.deltaTime);
    }

    private void GroundMove() {
        animator.SetBool("Run", true);

        Vector3 dir = new Vector3(verticalInput, 0, 0);
        RigidMove(dir, Stats.Instance.Speed, 0.5f);
    }

    private void FlyMove() {
        Vector3 dir = new Vector3(verticalInput, horizontalInput, 0).normalized;
        RigidMove(dir, Stats.Instance.FlySpeed, 0.5f);
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
            RigidMove(playerCenter.forward, flyDashSpeed, 0.7f);
            flyDashSpeed += Mathf.Max(flySpeed - speed, 0) * Time.fixedDeltaTime * 10;

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
        rigidBody.AddForce(Vector3.down * 200, ForceMode.Impulse);

        while (!isOnGround && !Physics.Raycast(rigidBody.position, Vector3.down, 2.0f, LayerMask.GetMask("Wall"))) {
            rigidBody.MovePosition(rigidBody.position + Vector3.down * 50 * Time.deltaTime);
            yield return wffu;
        }

        isMovable = true;
        isDashable = true;
        rigidBody.mass = mass;
    }

    IEnumerator Dash() {
        isDashable = false;
        isMovable = false;

        float dashTime = 0;
        float dashEndTime = Stats.Instance.DashTime;
        Vector3 dashDirection = new Vector3(verticalDash, horizontalDash, 0).normalized;

        float dashSpeed;
        if (isOnFlying)
           dashSpeed = Stats.Instance.FlyDashSpeed;
        else
           dashSpeed = Stats.Instance.DashSpeed;

        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (dashTime < dashEndTime) {
            RigidMove(dashDirection, dashSpeed, 0.8f);
            dashTime += Time.fixedDeltaTime;
            yield return wffu;
        }
        isMovable = true;

        yield return CoroutineCache.WaitForSecond(Stats.Instance.DashCooltime);
        isDashable = true;
    }

    IEnumerator WeaponChangeCooltime() {
        yield return CoroutineCache.WaitForSecond(0.2f);
        isWeaponChangeable = true;
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}