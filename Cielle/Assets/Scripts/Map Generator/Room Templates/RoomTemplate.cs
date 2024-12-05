using UnityEngine;

[CreateAssetMenu(fileName ="NewRoomTemplate",menuName ="Create Room Template")]
public class RoomTemplate : ScriptableObject {
    public GameObject room;
    public Vector3 size;
    public string type;

    public void RoomSize() {
        if(room != null) {
            Transform sizeObject = room.transform.Find("Element_Empty");
            if (sizeObject != null) 
                size = new Vector3(sizeObject.localScale.x, sizeObject.localScale.y, sizeObject.localScale.z);
        }
    }

    public void RoomType() {
        int index = room.name.IndexOf("_");
        if (index > 0)
            room.name = room.name.Substring(0, index);
    }
}
