using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PasswordCheck : MonoBehaviour
{
    // ��й�ȣ�� ��й�ȣ Ȯ�� InputField
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;

    // ��й�ȣ�� ��ġ�� �� ������ ������ �� �ؽ�Ʈ
    
    public TextMeshProUGUI matchText; // �´� ��� ������ �ؽ�Ʈ

    // ��й�ȣ�� ��ġ���� ���� �� ������ �޽���
    public TextMeshProUGUI errorText;

    void Start()
    {
        // ó���� �����ܰ� �޽����� ����
        
        matchText.gameObject.SetActive(false);
        errorText.gameObject.SetActive(false);

        // InputField �� ������ �����ϴ� ������ �߰�
        passwordField.onValueChanged.AddListener(delegate { CheckPasswordMatch(); });
        confirmPasswordField.onValueChanged.AddListener(delegate { CheckPasswordMatch(); });
    }

    // ��й�ȣ�� ��й�ȣ Ȯ�� �ʵ��� ��ġ ���θ� Ȯ���ϴ� �Լ�
    void CheckPasswordMatch()
    {
        if (!string.IsNullOrEmpty(passwordField.text) && passwordField.text == confirmPasswordField.text)
        {
            // ��й�ȣ�� ��ġ�� ���
            
            matchText.gameObject.SetActive(true);
            errorText.gameObject.SetActive(false);
            matchText.text = "��й�ȣ�� ��ġ�մϴ�!"; // ���ϴ� �޽����� ���� ����
        }
        else
        {
            // ��й�ȣ�� ��ġ���� ���� ���
            
            matchText.gameObject.SetActive(false);
            errorText.gameObject.SetActive(true);
            errorText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�!"; // ���ϴ� ���� �޽����� ���� ����
        }
    }
}
