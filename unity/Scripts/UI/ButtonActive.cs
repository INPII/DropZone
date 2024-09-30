using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button; // �α��� ��ư
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);  // ��ư�� �⺻ ũ��
    public Vector3 enlargedScale = new Vector3(1.2f, 1.2f, 1.2f);  // ��ư�� Ȯ��� ũ��

    public Color normalColor = Color.white; // �⺻ ����
    public Color hoverColor = Color.green; // ���콺 ���� �� ����

    // ���콺�� ��ư ���� �ö��� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ũ�� Ȯ��
        button.transform.localScale = enlargedScale;
        // ���� ����
        button.image.color = hoverColor;
    }

    // ���콺�� ��ư���� ����� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        // ũ�� �������
        button.transform.localScale = normalScale;
        // ���� �������
        button.image.color = normalColor;
    }
}
