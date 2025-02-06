using UnityEngine;

public class RoomTemplate : ScriptableObject {
    public GameObject room;
    public Vector2 size;
    public string type;
    public bool [] direction = new bool[4];

    public void Initialize() {
        if (room != null) {
            RoomSize();
            RoomType();
            RoomDirection();
        }
    }

    private void RoomSize() {
        Collider2D collider2D = room.GetComponent<Collider2D>();
        if (collider2D is BoxCollider2D box)
            size = new Vector2(box.size.x, box.size.y);
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
