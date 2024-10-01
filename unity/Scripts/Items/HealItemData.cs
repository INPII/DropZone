using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Heal item")]
public class HealItemData : ItemData
{
    [Header("����ϴµ� �ҿ�Ǵ� �ð�")]
    public int usingTime; 
    [Header("����")]
    public int healAmount;
}
