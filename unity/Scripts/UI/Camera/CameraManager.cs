using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;       // ���� ī�޶�
    public Camera secondaryCamera;  // �߰� ī�޶� (���� ī�޶�)

    void Start()
    {
        // ���� ī�޶��� ����Ʈ�� ��ü ȭ�� ����
        mainCamera.rect = new Rect(0, 0, 1, 1);

        // ���� ī�޶��� ����Ʈ�� ȭ���� ������ �ϴܿ� 25% ũ��� ��ġ
        secondaryCamera.rect = new Rect(0.75f, 0f, 0.25f, 0.25f);
    }
}
