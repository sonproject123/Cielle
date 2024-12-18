using UnityEngine;

public class RoomTemplateStats : MonoBehaviour {
    public string id;
    public GameObject room;
    public GameObject sizeObject;
    public Transform[] doors = new Transform[4];

    public void Initialize(RoomTemplate template) {
        room = gameObject;
        sizeObject = room.transform.Find("Element_Size").gameObject;
        RoomID();
        Doors(template);
    }

    private void RoomID() {
        id = System.Guid.NewGuid().ToString();
        MapSizeObject mso = sizeObject.GetComponent<MapSizeObject>();
        mso.id = id;
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
