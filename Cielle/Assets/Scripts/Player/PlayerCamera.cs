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

    [SerializeField] bool isCameraMovable;

    public static Action<bool> OnIsCameraMovable;
    public static Action<bool> OnCameraZoomIn;

    private void Start() {
        OnIsCameraMovable = (bool state) => { IsCameraMovable(state); };
        OnCameraZoomIn = (bool state) => { CameraZoomIn(state); };

        cameraOriginalSpeed = 120;
        cameraSpeed = cameraOriginalSpeed;
        cameraOriginalY = 5;
        cameraY = cameraOriginalY;
        cameraOriginalZ = -15;
        cameraZ = cameraOriginalZ;

        isCameraMovable = true;
    }

    private void Update() {
        if(isCameraMovable)
            PlayerFollow();
    }

    private void PlayerFollow() {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(player.position.x, player.position.y + cameraY, cameraZ),
            cameraSpeed * Time.fixedDeltaTime
        );
    }

    private void IsCameraMovable(bool state) {
        isCameraMovable = state;
    }

    private void CameraZoomIn(bool state) {
        if (state) {
            cameraSpeed = 20;
            cameraY -= 3;
            cameraZ += 10;
        }
        else {
            cameraSpeed = cameraOriginalSpeed;
            cameraY = cameraOriginalY;
            cameraZ = cameraOriginalZ;
        }
    }
}
