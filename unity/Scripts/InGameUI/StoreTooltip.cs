using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StoreTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip; // ���� UI ������Ʈ
    public TextMeshProUGUI tooltipText; // ���� �ؽ�Ʈ

    // ���콺�� ����ǥ ������ ���� �÷��� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    // ���콺�� ����ǥ �����ܿ��� ������ �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    // ���� �����ֱ�
    private void ShowTooltip()
    {
        tooltip.SetActive(true); // ���� Ȱ��ȭ        
    }

    // ���� �����
    private void HideTooltip()
    {
        tooltip.SetActive(false); // ���� ��Ȱ��ȭ
    }
}
