using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Move : MonoBehaviour {
    [SerializeField] Dictionary<KeyCode, Action> actions = new Dictionary<KeyCode, Action>();
    // if문 도배를 dictionary로 관리하기

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform playerCenter;
    [SerializeField] Transform playerFoot;
    [SerializeField] Transform playerHead;
    [SerializeField] Canvas playerCanvas;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] LayerMask ground;

    [SerializeField] GameObject gun;

    [SerializeField] bool isMovable;
    [SerializeField] bool isOnGround;
    [SerializeField] bool isOnThinGround;
    [SerializeField] bool isOnFlying;
    [SerializeField] bool isDashable;
    [SerializeField] bool isFireable;
    [SerializeField] bool isReloading;
    [SerializeField] bool isWeaponChangeable;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] float horizontalDash;
    [SerializeField] float verticalDash;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        playerUI = playerCanvas.GetComponent<PlayerUI>();
    }

    private void Start() {
        isMovable = true;
        isOnGround = false;
        isOnThinGround = false;
        isOnFlying = false;
        isDashable = true;
        isFireable = true;
        isReloading = false;
        isWeaponChangeable = true;

        rigidBody.mass = Stats.Instance.Mass;

        horizontalInput = 0;
        verticalInput = 0;
        horizontalDash = 0;
        verticalDash = 0;
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Thin Ground")) {
            isOnGround = true;
            isOnThinGround = true;
            if(collision.transform.position.y < playerFoot.transform.position.y)
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
            else
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"));
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            isOnGround = true;
            isOnThinGround = false;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Thin Ground") || collision.gameObject.CompareTag("Wall")) {
            isOnGround = false;
            isOnThinGround = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
        }
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
        if (Stats.Instance.IsMovable == false)
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
            else if (isOnFlying && !isOnThinGround)
                StartCoroutine(Dive());
            else if (Input.GetKey(KeyCode.S) && !isOnFlying && !isOnGround)
                StartCoroutine(Dive());
            else if(Input.GetKey(KeyCode.S) && !isOnFlying && isOnThinGround)
                StartCoroutine(JumpDown());
            else if (isOnGround && !isOnFlying)
                Jump();
        }

        // Dash
        horizontalDash = 0;
        verticalDash = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashable) {
            if (isOnFlying) {
                if (Input.GetKey(KeyCode.A))
                    verticalDash = -1;
                else if (Input.GetKey(KeyCode.D))
                    verticalDash = 1;

                if (Input.GetKey(KeyCode.W))
                    horizontalDash = 1;
                else if (Input.GetKey(KeyCode.S))
                    horizontalDash = -1;

                if(verticalDash == 0 && horizontalDash == 0) {
                    if (Stats.Instance.IsLeft)
                        verticalDash = -1;
                    else
                        verticalDash = 1;
                }
            }
            else {
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
            }
            StartCoroutine(Dash());
        }

        // Shoot
        if (isFireable) {
            if (Input.GetMouseButtonDown(0) && Stats.Instance.MainGunFireType == GunFireType.SINGLE)
                GunFire.Instance.Shoot(transform, animator);
            if (Input.GetMouseButton(0) && Stats.Instance.MainGunFireType == GunFireType.REPEAT)
                GunFire.Instance.Shoot(transform, animator);
        }

        // Reload
        if(Input.GetKeyDown(KeyCode.R)) {
            if (Stats.Instance.BulletRemain == Stats.Instance.BulletMax)
                return;

            Reload();
        }

        // Weapon Change
        if(Input.GetAxis("Mouse ScrollWheel") != 0 && isWeaponChangeable) {
            if (GunFire.Instance.IsShootable == false || Stats.Instance.SubGunData.id == 0)
                return;

            isWeaponChangeable = false;
            float cooltime = Mathf.Max(Stats.Instance.MainGunData.reload * 2, 1.0f);

            (Stats.Instance.MainWeaponId, Stats.Instance.SubWeaponId) = (Stats.Instance.SubWeaponId, Stats.Instance.MainWeaponId);
            Stats.Instance.GunInit();
            UIManager.OnWeaponChange?.Invoke();

            StartCoroutine(WeaponChangeCooltime(cooltime));
        }
    }

    private bool RaycastCheck(Vector3 dir, float wallSensor) {
        bool head = Physics.Raycast(playerHead.position, dir, wallSensor, LayerMask.GetMask("Wall"));
        bool center = Physics.Raycast(playerCenter.position, dir, wallSensor, LayerMask.GetMask("Wall"));
        bool foot = Physics.Raycast(playerFoot.position, dir, wallSensor, LayerMask.GetMask("Wall"));

        return !head && !center && !foot;
    }

    private void RigidMove(Vector3 dir, float speed) {
        rigidBody.MovePosition(rigidBody.position + dir * speed * Time.deltaTime);
    }

    private void GroundMove() {
        animator.SetBool("Run", true);

        Vector3 dir = new Vector3(verticalInput, 0, 0);
        if (RaycastCheck(dir, 0.5f))
            RigidMove(dir, Stats.Instance.Speed);
    }

    private void FlyMove() {
        if(!RaycastCheck(new Vector3(verticalInput, 0, 0), 0.5f))
            verticalInput = 0;
        if (!RaycastCheck(new Vector3(0, horizontalInput, 0), 0.5f))
            horizontalInput = 0;

        Vector3 dir = new Vector3(verticalInput, horizontalInput, 0).normalized;
        RigidMove(dir, Stats.Instance.FlySpeed);
    }

    private void Jump() {
        rigidBody.AddForce(Vector3.up * Stats.Instance.JumpHeight, ForceMode.Impulse);
    }

    IEnumerator JumpDown() {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        rigidBody.AddForce(Vector3.down * 50, ForceMode.Impulse);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"));

        while(time < 0.2f) {
            time += Time.deltaTime;
            yield return wffu;
        }

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
    }

    IEnumerator FlyStart() {
        isDashable = false;
        isMovable = false;
        isOnGround = false;
        isOnThinGround = false;
        isOnFlying = true;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.useGravity = false;

        float flytime = 0;
        float flyDashSpeed = Stats.Instance.Speed;
        float speed = Stats.Instance.Speed;
        float flySpeed = Stats.Instance.FlySpeed;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (flytime < Stats.Instance.FlyTime) {
            Vector3 direction = (GeneralStats.Instance.MouseLocation - playerCenter.position).normalized;
            playerCenter.rotation = Quaternion.LookRotation(direction);
            if (RaycastCheck(playerCenter.forward, 1.0f))
                RigidMove(playerCenter.forward, flyDashSpeed);
            flyDashSpeed += Mathf.Max(flySpeed - speed, 0) * Time.fixedDeltaTime * 10;

            flytime += Time.deltaTime;
            yield return wffu;
        }

        isMovable = true;
        isDashable = true;
    }

    IEnumerator Dive() {
        float startY = rigidBody.position.y;
        isOnFlying = false;
        rigidBody.useGravity = true;
        isMovable = false;
        isDashable = false;
        rigidBody.linearVelocity = Vector3.zero;

        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        RaycastHit floor;
        if (!Physics.Raycast(rigidBody.position, Vector3.down, out floor, 1.5f, LayerMask.GetMask("Wall")))
            rigidBody.AddForce(Vector3.down * 200, ForceMode.Impulse);

        while (!Physics.Raycast(rigidBody.position, Vector3.down, out floor, 1.5f, LayerMask.GetMask("Wall")) && !isOnGround) {
            if (isOnFlying) {
                rigidBody.linearVelocity = Vector3.zero;
                yield break;
            }
            rigidBody.MovePosition(rigidBody.position + Vector3.down * 10 * Time.deltaTime);
            yield return wffu;
        }

        if (floor.collider != null) {
            float power = Mathf.Max(startY - floor.collider.transform.position.y, 0);
            PlayerCamera.OnDive(Mathf.Min(0.75f, power));
        }
        
        rigidBody.linearVelocity = Vector3.zero;

        isMovable = true;
        isDashable = true;
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
            if (RaycastCheck(dashDirection, 1.0f))
                RigidMove(dashDirection, dashSpeed);
            dashTime += Time.fixedDeltaTime;
            yield return wffu;
        }

        isMovable = true;
        isDashable = true;
    }

    IEnumerator WeaponChangeCooltime(float cooltime) {
        float time = 0;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while (time < cooltime) {
            yield return wffu;
            time += Time.deltaTime;
            UIManager.OnWeaponChangeCooltime?.Invoke(time, cooltime);
        }

        isWeaponChangeable = true;
        UIManager.OnWeaponChangeCooltime?.Invoke(0, 0);
    }

    private void Reload() {
        if (isReloading)
            return;

        isReloading = true;
        isFireable = false;
        isWeaponChangeable = false;
        playerUI.Reload(true);
        
        // Sound reloading
        StartCoroutine(WeaponReload());
    }

    IEnumerator WeaponReload() {
        float time = 0;
        float reloadTime = Stats.Instance.MainGunData.reload;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        while(time < reloadTime) {
            yield return wffu;
            time += Time.deltaTime;
            playerUI.Reloading(time, reloadTime);
        }

        Stats.Instance.BulletRemain = Stats.Instance.BulletMax;
        isFireable = true;
        isWeaponChangeable = true;
        isReloading = false;
        playerUI.Reload(false);
        UIManager.OnBulletUse?.Invoke();
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}