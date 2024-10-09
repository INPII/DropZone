using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Grenade")]
public class Grenade : ItemData
{
    [Header("���� ����")]
    [Range(0, 10)]
    public float explosionRange;
    [Header("�����")]
    public int damage;

    [Header("���� ����Ʈ")]
    public GameObject explosionEffect; // ���� ����Ʈ ������
}
