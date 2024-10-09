using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Glue")]
public class Glue : ItemData
{
    [Header("���Ӻ�")]
    [Range(0, 100f)]
    public float slowAmount; // ���� �̵� �ӵ��� ���ҽ�Ű�� ����

    [Header("���� ����")]
    [Range(0, 10f)]
    public float spreadRange; // �����̰� ������ ����

    public GameObject stickyPrefab; // �ٴڿ� ���� ������ ����Ʈ
}