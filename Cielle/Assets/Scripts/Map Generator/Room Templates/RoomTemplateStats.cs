using System.Collections.Generic;
using UnityEngine;

public class RoomTemplateStats : MonoBehaviour {
    public string id;
    public GameObject room;
    public Transform[] doors = new Transform[4];
    public List<GameObject> childRooms = new List<GameObject>();

    public Transform pointsParent;
    public List<Transform> spawnPoints = new List<Transform>();
    public Transform bossWallsParent;
    public List<GameObject> bossWalls = new List<GameObject>();

    public void Initialize(RoomTemplate template) {
        room = gameObject;
        RoomID();
        Doors(template);
        SpawnPoints();
        BossWalls();
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

    private void SpawnPoints() {
        pointsParent = transform.Find("Spawn Points");
        if (pointsParent != null) {
            foreach (Transform point in pointsParent)
                spawnPoints.Add(point);
        }
    }

    private void BossWalls() {
        bossWallsParent = transform.Find("Boss Walls");
        if (bossWallsParent != null) {
            foreach (Transform wall in bossWallsParent) 
                bossWalls.Add(wall.gameObject);
        }
    }
}
