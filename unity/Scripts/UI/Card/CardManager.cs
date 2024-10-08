using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> cardList = new List<GameObject>(); // ī�� ����Ʈ
    public CharacterManager characterManager; // CharacterManager ����
    public Button selectButton; // ���� ��ư (Inspector���� �巡���ؼ� ����)

    private GameObject currentlySelectedCard; // ���� ���õ� ī��
    private int selectedCharacterIndex = -1; // ���õ� ĳ������ �ε���

    void Start()
    {
        // ��� ī�忡 Ŭ�� �̺�Ʈ�� �����ϰ�, Outline �ʱ�ȭ
        foreach (var card in cardList)
        {
            CardSelector selector = card.AddComponent<CardSelector>(); // �� ī�忡 CardSelector �߰�
            selector.Initialize(this); // CardSelector�� CardManager ����

            Outline outline = card.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false; // �ʱ⿡�� Outline ��Ȱ��ȭ
            }
        }

        // ���� �� �⺻ ĳ����(ù ��° ĳ����)�� Ȱ��ȭ
        characterManager.SetSelectedCharacter(0);
    }

    // ī�� ���� �� ȣ��
    public void SelectCard(GameObject newCard)
    {
        // ������ ���õ� ī�尡 �ִٸ� Outline ��Ȱ��ȭ
        if (currentlySelectedCard != null)
        {
            Outline previousOutline = currentlySelectedCard.GetComponent<Outline>();
            if (previousOutline != null)
            {
                previousOutline.enabled = false;
            }
        }

        // ���� ���õ� ī���� Outline Ȱ��ȭ
        Outline newOutline = newCard.GetComponent<Outline>();
        if (newOutline != null)
        {
            newOutline.enabled = true;
            currentlySelectedCard = newCard; // ���� ���õ� ī�� ������Ʈ

            // ���õ� ĳ���� �ε����� ������Ʈ (ī���� �ε����� ĳ���� �ε����� ��ġ�Ѵٰ� ����)
            selectedCharacterIndex = cardList.IndexOf(newCard);
            Debug.Log(selectedCharacterIndex);
        }
    }

    // IPointerClickHandler ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress == selectButton.gameObject) // ���� ��ư�� Ŭ���Ǿ��� ��
        {
            if (selectedCharacterIndex != -1)
            {
                characterManager.SetSelectedCharacter(selectedCharacterIndex); // ���õ� ĳ���͸� �κ� �ݿ�
                Debug.Log("Character Index Sent: " + selectedCharacterIndex);
            }
        }
    }
}
