using UnityEngine;
using UnityEngine.EventSystems;
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

                // �� ������Ʈ�� MouseHoverHandler ������Ʈ�� �߰�
                MouseHoverHandler hoverHandler = obj.AddComponent<MouseHoverHandler>();
                hoverHandler.Initialize(this, obj);
            }
        }
    }

    // ������Ʈ ũ�⸦ �����ϴ� �Լ�
    public void ScaleObject(GameObject obj, bool isHovered)
    {
        if (isHovered)
        {
            obj.transform.localScale = originalScales[obj] * hoverScaleMultiplier;
        }
        else
        {
            obj.transform.localScale = originalScales[obj];
        }
    }
}

public class MouseHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObjectManager gameObjectManager;
    private GameObject targetObject;

    // �ʱ�ȭ �Լ�
    public void Initialize(GameObjectManager manager, GameObject obj)
    {
        gameObjectManager = manager;
        targetObject = obj;
    }

    // ���콺�� ������Ʈ ���� �� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObjectManager.ScaleObject(targetObject, true);
    }

    // ���콺�� ������Ʈ�� ��� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObjectManager.ScaleObject(targetObject, false);
    }
}
