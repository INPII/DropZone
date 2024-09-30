using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    public GameObject loginPanel;    // �α��� �г�
    public GameObject registerPanel; // ȸ������ �г�

    // �ʱ� �������� �α��� �гθ� Ȱ��ȭ
    void Start()
    {
        ShowLogin();
    }

    // �α��� �г��� ���̰� �ϰ� ȸ������ �г��� ����ϴ�.
    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    // ȸ������ �г��� ���̰� �ϰ� �α��� �г��� ����ϴ�.
    public void ShowRegister()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
}
