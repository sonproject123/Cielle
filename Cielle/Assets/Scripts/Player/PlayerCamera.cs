using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour {
    [SerializeField] Transform player;

    private void Update() {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(player.position.x, player.position.y + 3, transform.position.z),
            120 * Time.fixedDeltaTime
        );
    }
}
