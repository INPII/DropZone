using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Glue")]
public class Glue : ItemData
{
    [Header("���Ӻ�")]
    [Range(0, 100f)]
    public float slowAmount;
    [Header("���� ����")]
    [Range(0, 10f)]
    public float spreadRange;
}
