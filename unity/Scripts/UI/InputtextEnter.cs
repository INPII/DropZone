using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputtextEnter : MonoBehaviour
{
    public TMP_InputField inputField;  // ä�� �Է� �ʵ� (TextMeshPro)
    public Button Button;  // '������' ��ư

    void Start()
    {
        // TextMeshPro InputField�� onSubmit �̺�Ʈ�� ����Ͽ� ����Ű�� ������ �� ��ư Ŭ�� �̺�Ʈ�� Ʈ����
        inputField.onSubmit.AddListener(delegate { Triggerclick(); });
    }

    // '������' ��ư Ŭ���� ���� ȿ���� �ִ� �޼���
    public void Triggerclick()
    {
        // ��ư Ŭ�� ȿ���� �����ϰ� ó��
        Button.onClick.Invoke();
    }
}
