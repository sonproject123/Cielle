using System.Collections.Generic;
using UnityEngine;

public class RoomTemplateStats : MonoBehaviour {
    public string id;
    public GameObject room;
    public Transform[] doors = new Transform[4];
    public List<GameObject> childRooms = new List<GameObject>();

    public void Initialize(RoomTemplate template) {
        room = gameObject;
        RoomID();
        Doors(template);
    }

    private void RoomID() {
        id = System.Guid.NewGuid().ToString();
    }

    private void Doors(RoomTemplate template) {
        for (int i = 0; i < 4; i++) {
            doors[i] = null;

            if (template.direction[i]) {
                switch (i) {
                    case 0: doors[i] = room.transform.Find("Element_Door_U"); break;
                    case 1: doors[i] = room.transform.Find("Element_Door_R"); break;
                    case 2: doors[i] = room.transform.Find("Element_Door_D"); break;
                    case 3: doors[i] = room.transform.Find("Element_Door_L"); break;
                }
            }
        }
    }
}
