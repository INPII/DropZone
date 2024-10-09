using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextOnClick : MonoBehaviour, IPointerClickHandler
{

    public TextMeshProUGUI TextMeshProUGUI;
    public List<GameObject> closeGameObjects = new List<GameObject>(); // ��Ȱ��ȭ�� ���� ������Ʈ ����Ʈ
    public List<GameObject> openGameObjects = new List<GameObject>();  // Ȱ��ȭ�� ���� ������Ʈ ����Ʈ

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
