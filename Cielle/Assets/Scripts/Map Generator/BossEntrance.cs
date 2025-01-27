using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] Transform bossPosition;
    [SerializeField] Move playerMove;
    [SerializeField] RoomTemplateStats goalRTS;
    [SerializeField] MapGenerator_Generic generator;
    [SerializeField] EnemyBoss boss = null;

    [SerializeField] bool isEndable;

    private void Start() {
        isEndable = false;
        goalRTS = transform.parent.gameObject.GetComponent<RoomTemplateStats>();
        bossPosition = GameObject.Find("Boss Point").transform;
        generator = (MapGenerator_Generic)FindFirstObjectByType(typeof(MapGenerator_Generic));
    }

    private void Update() {
        if (boss == null)
            boss = generator.Boss;
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
        boss.gameObject.SetActive(true);

        if (transform.position.x < player.transform.position.x)
            playerMove.OnForcedMove(5, Vector3.left);
        else
            playerMove.OnForcedMove(5, Vector3.right);

        foreach (var wall in goalRTS.bossWalls) {
            BossWall bw = wall.GetComponent<BossWall>();
            bw.OnDoorMove(true);
        }

        UIManager.OnUIAlpha(0, true);
        PlayerCamera.OnCameraMove?.Invoke(bossPosition.position, 0.2f);

        GameObject popUp = PopUpManager.Instance.ShowPopUp(PopUpTypes.BOSS_NAME);
        BossName bn = popUp.GetComponent<BossName>();
        bn.OnNameInput?.Invoke(boss.Subtitle, boss.Name);

        StartCoroutine(WaitSomeSecond(1));
    }

    IEnumerator WaitSomeSecond(float waitTime) {
        WaitForFixedUpdate wffu = GeneralStats.Instance.WFFU;
        float time = 0;

        while (time < waitTime) {
            time += Time.deltaTime;

            yield return wffu;
        }

        isEndable = true;
    }

    private void EndDirection() {
        GeneralStats.Instance.Pause = false;
        Stats.Instance.IsInvincible = false;
        LetterBoxManager.Instance.LetterBox();

        UIManager.OnUIAlpha(1, false);
        PlayerCamera.OnIsCameraFollow?.Invoke(true);
        PopUpManager.Instance.ClosePopUp(PopUpTypes.BOSS_NAME);
        boss.ChangeState(new EnemyState_InPattern<EnemyBoss>(boss));
        boss.IsActive = true;

        gameObject.SetActive(false);
    }
}
