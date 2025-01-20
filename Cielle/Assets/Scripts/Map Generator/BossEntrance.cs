using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] Transform bossPosition;
    [SerializeField] Move playerMove;
    [SerializeField] RoomTemplateStats goalRTS;
    [SerializeField] MapGenerator_Generic generator;
    [SerializeField] EnemyBoss boss;

    [SerializeField] bool isEndable;

    private void Start() {
        isEndable = false;
        goalRTS = transform.parent.gameObject.GetComponent<RoomTemplateStats>();
        bossPosition = GameObject.Find("Boss Point").transform;
        generator = (MapGenerator_Generic)FindFirstObjectByType(typeof(MapGenerator_Generic));
        boss = generator.Boss;
    }

    private void Update() {
        if (isEndable && Input.anyKeyDown)
            EndDirection();
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
        Stats.Instance.IsInvincible = true;
        LetterBoxManager.Instance.LetterBox();

        if (transform.position.x < player.transform.position.x)
            playerMove.OnForcedMove(5, Vector3.left);
        else
            playerMove.OnForcedMove(5, Vector3.right);

        foreach (var wall in goalRTS.bossWalls) {
            BossWall bw = wall.GetComponent<BossWall>();
            bw.OnDoorMove(true);
        }

        UIManager.OnUIAlpha(0);
        PlayerCamera.OnCameraMove?.Invoke(bossPosition.position, 0.2f);

        GameObject popUp = PopUpManager.Instance.ShowPopUp(PopUpTypes.BOSS_NAME);
        BossName bn = popUp.GetComponent<BossName>();
        bn.OnNameInput?.Invoke(boss.Subtitle, boss.Name);

        StartCoroutine(WaitSomeSecond());
    }

    IEnumerator WaitSomeSecond() {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float time = 0;

        while (time < 1) {
            time += Time.deltaTime;

            yield return wffu;
        }

        isEndable = true;
    }

    private void EndDirection() {
        GeneralStats.Instance.Pause = false;
        Stats.Instance.IsInvincible = false;
        LetterBoxManager.Instance.LetterBox();

        UIManager.OnUIAlpha(1);
        PlayerCamera.OnIsCameraFollow?.Invoke(true);
        PopUpManager.Instance.ClosePopUp(PopUpTypes.BOSS_NAME);
        boss.ChangeState(new EnemyState_InPattern<EnemyBoss>(boss));

        gameObject.SetActive(false);
    }
}
