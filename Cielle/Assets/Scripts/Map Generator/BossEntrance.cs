using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] Transform bossPosition;
    [SerializeField] Move playerMove;
    [SerializeField] RoomTemplateStats goalRTS;

    private void Start() {
        goalRTS = transform.parent.gameObject.GetComponent<RoomTemplateStats>();
        bossPosition = GameObject.Find("Boss Point").transform;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player = other.gameObject;
            playerMove = player.GetComponent<Move>();
            Entrance();
        }
    }

    private void Entrance() {
        GeneralStats.Instance.Pause = true;
        LetterBoxManager.Instance.LetterBox();
        if (transform.position.x < player.transform.position.x)
            playerMove.OnForcedMove(5, Vector3.left);
        else
            playerMove.OnForcedMove(5, Vector3.right);

        foreach (var wall in goalRTS.bossWalls) {
            BossWall bw = wall.GetComponent<BossWall>();
            bw.OnDoorMove(true);
        }

        PlayerCamera.OnCameraMove?.Invoke(true, bossPosition.position, 1f);
    }
}
