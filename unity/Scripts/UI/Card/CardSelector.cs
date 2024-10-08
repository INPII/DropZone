using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardManager cardManager;

    // CardManager�� ����
    public void Initialize(CardManager manager)
    {
        cardManager = manager;
    }

    // ī�尡 Ŭ���� �� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        if (cardManager != null)
        {
            cardManager.SelectCard(gameObject); // Ŭ���� ī�� ����

        }
    }

    // �����Ͱ� ī�� ���� ���� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �����Ͱ� ī�� ���� ������ �� �߻��� �߰� ������ ���⿡ �ۼ��� �� �ֽ��ϴ�.
    }

    // �����Ͱ� ī�� ������ ���� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        // �����Ͱ� ī�� ������ ������ �� �߻��� �߰� ������ ���⿡ �ۼ��� �� �ֽ��ϴ�.
    }
}
