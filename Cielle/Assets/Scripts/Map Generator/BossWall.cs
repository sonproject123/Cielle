using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour {
    [SerializeField] float originalPosition;
    [SerializeField] float movedPosition;
    public Action<bool> OnDoorMove;

    private void Start() {
        OnDoorMove = (bool isClosing) => { DoorMove(isClosing); };
        originalPosition = transform.position.y;
        movedPosition = originalPosition - 10;
    }

    public void DoorMove(bool isClosing) {
        StartCoroutine(DoorMoving(isClosing));
    }

    IEnumerator DoorMoving(bool isClosing) {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float speed = 10;

        if (isClosing) {
            speed *= -1;
            while (transform.position.y > movedPosition) {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                yield return wffu;
            }
        }
        else {
            while (transform.position.y < originalPosition) {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                yield return wffu;
            }
        }
    }
}
