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
    }

    private void Start() {
        isMovable = true;
        isOnGround = true;
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
            else if (isOnFlying)
                StartCoroutine(FlyEnd());
            else if (isOnGround && !isOnFlying)
                Jump();
            else if (Input.GetKey(KeyCode.S) && !isOnFlying && !isOnGround)
                StartCoroutine(FlyEnd());
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
            if (GunFire.Instance.IsShootable == false)
                return;
            if (Stats.Instance.SubGunData.id == 0)
                return;

            isWeaponChangeable = false;
            float cooltime = Mathf.Max(Stats.Instance.MainGunData.reload * 2, 1.0f);

            (Stats.Instance.MainWeaponId, Stats.Instance.SubWeaponId) = (Stats.Instance.SubWeaponId, Stats.Instance.MainWeaponId);
            Stats.Instance.GunInit();
            UIManager.OnWeaponChange?.Invoke();

            StartCoroutine(WeaponChangeCooltime(cooltime));
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
            RigidMove(playerCenter.forward, flyDashSpeed, 0.7f);
            flyDashSpeed += Mathf.Max(flySpeed - speed, 0) * Time.fixedDeltaTime * 10;

            flytime += Time.fixedDeltaTime;
            yield return wffu;
        }

        isMovable = true;
        isDashable = true;
    }

    IEnumerator FlyEnd() {
        isOnFlying = false;
        rigidBody.useGravity = true;
        isMovable = false;
        isDashable = false;
        rigidBody.linearVelocity = Vector3.zero;

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

        //yield return CoroutineCache.WaitForSecond(Stats.Instance.DashCooltime);
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
        PlayerUI.OnReloading?.Invoke(true);
        
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
            PlayerUI.OnReloadingTime.Invoke(time, reloadTime);
        }

        Stats.Instance.BulletRemain = Stats.Instance.BulletMax;
        isFireable = true;
        isWeaponChangeable = true;
        isReloading = false;
        PlayerUI.OnReloading.Invoke(false);
        UIManager.OnBulletUse.Invoke();
    }

    private void OnDisable() {
        InputManager.Instance.action -= OnkeyUpdate;
    }
}