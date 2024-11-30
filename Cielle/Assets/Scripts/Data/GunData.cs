using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData {
    public int id;
    public string name;
    public string descriptions;
    public float atk;
    public float atkShield;
    public float minSpeed;     // �Ѿ� �ӵ�
    public float maxSpeed;
    public float cooltime;     // �߻� ����
    public float reload;       // ������ �ð�
    public float recoil;       // ������ �ݵ�
    public float playerRecoil; // ���� �߻� �� �ݵ�
    public float stopping;     // �� �ݵ�
    public float stoppingTime; // �� �ݵ� �ð�
    public int bullet;         // źâ �ϳ��� �Ѿ� ��
    public string type;        // �ܹ�, ����
    public string code;
    public string bulletCode;
}

[System.Serializable]
public class GunDataList {
    public List<GunData> gunsData = new List<GunData>();
}