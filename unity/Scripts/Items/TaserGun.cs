using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Taser Gun")]
public class TaserGun : ItemData
{
    [Header("���� �ð�")]
    public int stunTime;

    [Header("�߻� ����Ʈ")]
    public GameObject shootEffect; // �߻� �� ��Ÿ���� ����Ʈ
}
