using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Button�� ����ϱ� ���� �ʿ�

public class ButtonClickManager : MonoBehaviour, IPointerClickHandler
{
    public Button button; // ��ư ������Ʈ�� ����
    public List<GameObject> closeGameObjects = new List<GameObject>(); // ��Ȱ��ȭ�� ���� ������Ʈ ����Ʈ
    public List<GameObject> openGameObjects = new List<GameObject>();  // Ȱ��ȭ�� ���� ������Ʈ ����Ʈ

    // Ŭ�� �̺�Ʈ ó��
    public void OnPointerClick(PointerEventData eventData)
    {
        // closeGameObjects ����Ʈ�� �ִ� ������Ʈ�� ��Ȱ��ȭ
        foreach (GameObject obj in closeGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // openGameObjects ����Ʈ�� �ִ� ������Ʈ�� Ȱ��ȭ
        foreach (GameObject obj in openGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
