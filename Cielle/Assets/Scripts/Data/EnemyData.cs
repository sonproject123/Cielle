using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {
    public int id;
    public string name;
    public float hp;
    public float shield;
    public float attack;
    public float attackShield;
    public float defense;
    public float speed;
    public float stoppingPower;
    public float stoppingTime;
    public float stoppingResistance;
    public float cooltime;
    public float bulletSpeed;
    public int price;
    public string code;
}

[System.Serializable]
public class EnemyDataList {
    public List<EnemyData> enemysData = new List<EnemyData>();
}