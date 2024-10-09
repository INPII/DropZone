using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Image�� �ٷ�� ���� �ʿ�

public class ImagePointerObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image imageComponent; // Image ������Ʈ
    public GameObject targetObject; // ���콺�� �÷��� �� Ȱ��ȭ�� ������Ʈ

    void Start()
    {
        // Image ������Ʈ ��������
        imageComponent = GetComponent<Image>();

        // ó���� targetObject ��Ȱ��ȭ
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // ���콺�� ������Ʈ ���� �÷��� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            // ���콺�� �÷��� �� targetObject Ȱ��ȭ
            targetObject.SetActive(true);
        }
    }

    // ���콺�� ������Ʈ���� ������ �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            // ���콺�� ����� �� targetObject ��Ȱ��ȭ
            targetObject.SetActive(false);
        }
    }
}
