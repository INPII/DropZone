using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Trap")]
public class Trap : ItemData
{
    [Header("�ӹ� �ð�")]
    public int rootTime;
    [Header("�����")]
    public int damage;
}
