using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSizeCollide : MonoBehaviour {
    [SerializeField] public string id;
    [SerializeField] public List<string> collidingRooms = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Room")) {
            MapSizeCollide otherMSC = collision.gameObject.GetComponent<MapSizeCollide>();
            collidingRooms.Add(otherMSC.id);
            collidingRooms = collidingRooms.Distinct().ToList();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Room")) {
            MapSizeCollide otherMSC = collision.gameObject.GetComponent<MapSizeCollide>();
            collidingRooms.Remove(otherMSC.id);
        }
    }
}
