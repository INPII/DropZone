using UnityEngine;
using System.Collections.Generic;

public class GameObjectManager : MonoBehaviour
{
    // ���콺 ���� ȿ���� ������ ������Ʈ���� ����Ʈ
    public List<GameObject> objectsToScale = new List<GameObject>();

    // �⺻ ũ�� ���� ���� (1�� ���� ũ��, 1.2�� 20% ����)
    public float hoverScaleMultiplier = 1.2f;

    // �� ������Ʈ�� ���� ũ�⸦ ������ ��ųʸ�
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        // ����Ʈ�� �ִ� ��� ������Ʈ�� ���� ũ�⸦ ����
        foreach (var obj in objectsToScale)
        {
            if (obj != null)
            {
                originalScales[obj] = obj.transform.localScale;
            }
        }
    }

    void Update()
    {
        // ���콺�� ���� ��� ������Ʈ ���� �ִ��� Ȯ��
        foreach (var obj in objectsToScale)
        {
            if (obj != null)
            {
                if (IsMouseOver(obj))
                {
                    // ���콺�� ������Ʈ ���� ���� �� ũ�� Ȯ��
                    obj.transform.localScale = originalScales[obj] * hoverScaleMultiplier;
                }
                else
                {
                    // ���콺�� ������Ʈ�� ����� ���� ũ��� ����
                    obj.transform.localScale = originalScales[obj];
                }
            }
        }
    }

    // ���콺�� Ư�� ������Ʈ ���� �ִ��� Ȯ���ϴ� �Լ�
    bool IsMouseOver(GameObject obj)
    {
        // ���콺 ��ġ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast�� ����Ͽ� ���콺�� ������Ʈ ���� �ִ��� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == obj)
            {
                return true;
            }
        }

        return false;
    }
}
