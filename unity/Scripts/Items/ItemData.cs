using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class ItemData : ScriptableObject
{
    public enum ItemType // ������ ����
    {
        Used,
        Heal,
        Money,
    }

    [Header("������ Id")]
    public int id;
    [Header("������ ����")]
    public ItemType itemType;
    [Header("��Ÿ��")]
    public int coolDownTime;
    [Header("������ �̸�")]
    public string itemName;
    [Header("������ ����")]
    [Multiline]
    public string toolTip;
    [Header("������ �̹���")]
    public Sprite itemImage;
    [Header("������ ������")]
    public GameObject itemPrefab;
}
