using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] float cameraOriginalSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraOriginalY;
    [SerializeField] float cameraY;
    [SerializeField] float cameraOriginalZ;
    [SerializeField] float cameraZ;

    [SerializeField] bool isCameraFollowPlayer;

    public static Action<Vector3, float> OnCameraMove;
    public static Action<bool> OnIsCameraMovable;
    public static Action<bool> OnCameraZoomIn;
    public static Action<float> OnDive;

    private void Start() {
        player = Stats.Instance.PlayerCenter;

        OnCameraMove = (Vector3 targetPosition, float speed) => { CameraMove(targetPosition, speed = 30); };
        OnIsCameraMovable = (bool state) => { IsCameraFollowPlayer(state); };
        OnCameraZoomIn = (bool state) => { CameraZoomIn(state); };
        OnDive = (float power) => { Dive(power); };

        cameraOriginalSpeed = 120;
        cameraSpeed = cameraOriginalSpeed;
        cameraOriginalY = 0;
        cameraY = cameraOriginalY;
        cameraOriginalZ = -15;
        cameraZ = cameraOriginalZ;

        isCameraFollowPlayer = true;
    }

    private void Update() {
        if(isCameraFollowPlayer)
            PlayerFollow();
    }

    private void PlayerFollow() {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(player.position.x, player.position.y + cameraY, cameraZ),
            cameraSpeed * Time.fixedDeltaTime
        );
    }

    private void CameraMove(Vector3 targetPosition, float speed = 30) {
        isCameraFollowPlayer = false;
        cameraSpeed = speed;

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(targetPosition.x, targetPosition.y, cameraZ),
            cameraSpeed * Time.fixedDeltaTime
        );

        cameraSpeed = cameraOriginalSpeed;
    }

    private void IsCameraFollowPlayer(bool state) {
        isCameraFollowPlayer = state;
    }

    private void CameraZoomIn(bool state) {
        if (state) {
            cameraSpeed = 20;
            cameraY -= 0;
            cameraZ += 10;
        }
        else {
            cameraSpeed = cameraOriginalSpeed;
            cameraY = cameraOriginalY;
            cameraZ = cameraOriginalZ;
        }
    }

    private void CameraZoomOut(bool state) {
    }

    private void Dive(float power) {
        StartCoroutine(DiveMove(power));
    }

    IEnumerator DiveMove(float power) {
        float time = 0;
        float speed = 0.05f;
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;

        cameraY -= power;
        cameraSpeed *= speed;

        while (time < 0.1f) {
            time += Time.deltaTime;
            yield return wffu;
        }

        cameraY += power;
        cameraSpeed /= speed;
    }
}
