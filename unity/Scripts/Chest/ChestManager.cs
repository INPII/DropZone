using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestManager : MonoBehaviour
{
    public TextMeshProUGUI chestText;
    public Transform chestTopPos;
    public ParticleSystem openParticle;
    public GameObject chestBoxUI;
    public bool isChestOpened = false; // �ڽ��� ���� ����
    private bool isPlayerNear; // �÷��̾ ������ �ִ��� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            // �÷��̾�� ����� �� �ؽ�Ʈ ��Ÿ������
            chestText.gameObject.SetActive(true);

            // �÷��̾ ������ ����
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            // Ʈ���� ������ ������ �ؽ�Ʈ �ٽ� ��Ȱ��ȭ
            chestText.gameObject.SetActive(false);

            // �÷��̾ ������ �������� ����
            isPlayerNear = false;
        }
    }

    // ���� ���� �޼���
    public void OpenChest()
    {
        chestTopPos.rotation = Quaternion.Euler(0, 180, 0); // ���� ����
        chestText.text = "to Close";
        chestBoxUI.SetActive(true);
        isChestOpened = !isChestOpened; // ���� ���� ó��
    }

    // ���� �ݱ� �޼���
    public void CloseChest()
    {
        chestTopPos.rotation = Quaternion.Euler(45, 180, 0); // ���� �ݱ�
        chestText.text = "to Open";
        chestBoxUI.SetActive(false);
        isChestOpened = !isChestOpened; // ���� ���� ó��
    }

    private void Update()
    {
        // �÷��̾ ���� �ȿ� ���԰�, ���ڰ� ������ ���� ���¿��� F Ű�� �����ٸ�
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            // ���ڰ� ���� ��������, ���� ���������� ���� �ٸ��� ����
            if (!isChestOpened)
            {
                OpenChest();
            } else
            {
                CloseChest();
            }

            openParticle.Play(); // ��ƼŬ ���
        }
    }
}
