using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputButtonActive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_InputField inputField1;  // TMP_InputField�� ����
    public TMP_InputField inputField2;  // TMP_InputField�� ����
    public Button button; // �α��� ��ư
    public Color activeColor = Color.black; // Ȱ��ȭ ����
    public Color inactiveColor = new Color(1, 1, 1, 0.5f); // ��Ȱ��ȭ ����

    public Vector3 normalScale = new Vector3(1f, 1f, 1f);  // ��ư�� �⺻ ũ��
    public Color hoverColor = Color.green; // ���콺�� ��ư ���� ���� �� ����

    private Color originalColor; // ���� ������ ������ ����

    void Start()
    {
        UpdateButtonAppearance(); // �ʱ� ���� �� ũ�� ������Ʈ
        if (inputField1 != null)
            inputField1.onValueChanged.AddListener(delegate { UpdateButtonAppearance(); });
        if (inputField2 != null)
            inputField2.onValueChanged.AddListener(delegate { UpdateButtonAppearance(); });
    }

    void UpdateButtonAppearance()
    {
        // Ȱ��ȭ ���� �˻�
        bool isField1Active = inputField1 != null && !string.IsNullOrEmpty(inputField1.text);
        bool isField2Active = inputField2 != null && !string.IsNullOrEmpty(inputField2.text);

        // �� �Է� �ʵ� �� �ϳ��� �����ϴ� ��� �� �ʵ��� ���븸 �˻�
        if (inputField1 == null || inputField2 == null)
        {
            button.interactable = isField1Active || isField2Active;
        }
        // �� �Է� �ʵ� ��� �����ϴ� ���, �� �ʵ� ��� ������ �־�� Ȱ��ȭ
        else
        {
            button.interactable = isField1Active && isField2Active;
        }

        // ��ư�� ���� ����
        button.image.color = button.interactable ? activeColor : inactiveColor;

        // ���콺�� ��ư ���� �ö󰡰ų� �������� �̺�Ʈ�� ���� ũ�� ���� ó��
    }

    // ���콺�� ��ư ���� �ö��� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            button.image.color = hoverColor; // ���콺 ���� �� ���� ����
        }
    }

    // ���콺�� ��ư���� ����� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            button.image.color = button.interactable ? activeColor : inactiveColor; // ũ�� �������
        }
    }
}
