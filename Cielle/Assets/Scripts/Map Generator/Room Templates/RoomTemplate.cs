using UnityEngine;

[CreateAssetMenu(fileName ="NewRoomTemplate",menuName ="Create Room Template")]
public class RoomTemplate : ScriptableObject {
    public GameObject room;
    public GameObject sizeObject;
    public Vector2 size;
    public string type;
    public bool [] direction = new bool[4];

    public void Initialize() {
        if (room != null) {
            sizeObject = room.transform.Find("Element_Size").gameObject;
            RoomSize();
            RoomType();
            RoomDirection();
        }
    }

    private void RoomSize() {
        if (sizeObject != null) {
            Collider2D sizeCollider = sizeObject.GetComponent<Collider2D>();
            Vector2 originalSize = sizeCollider.bounds.size;
            Transform sizeTransform = sizeObject.transform;
            size = new Vector2(originalSize.x * sizeTransform.localScale.x, originalSize.y * sizeTransform.localScale.y);
        }
    }

    private void RoomType() {
        string roomName = room.name;
        int index = roomName.IndexOf("_");
        if (index > 0)
            type = roomName.Substring(0, index);
    }

    private void RoomDirection() {
        for (int i = 0; i < 4; i++)
            direction[i] = false;

        string roomDir = room.name;
        int index = roomDir.IndexOf("_");
        roomDir = roomDir.Substring(index + 1);

        if (index > 0 && roomDir.Length > 0) {
            foreach (char letter in roomDir) {
                if (letter == 'U')
                    direction[0] = true;
                else if (letter == 'R')
                    direction[1] = true;
                else if (letter == 'D')
                    direction[2] = true;
                else if (letter == 'L')
                    direction[3] = true;
                else
                    break;
            }
        }
    }
}
