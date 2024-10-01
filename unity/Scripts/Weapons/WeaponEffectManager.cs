using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatus;

public class WeaponEffectManager : MonoBehaviour
{
    public GameObject[] attributeEffects;  // �Ӽ� ���� ����Ʈ �迭 (Attack_Point ������ ����Ʈ��)
    public GameObject[] attributeEffects_Left;  // �Ӽ� ���� ����Ʈ �迭 (Weapon_Effect_L ������ ����Ʈ��)
    public GameObject[] attributeEffects_Right;  // �Ӽ� ���� ����Ʈ �迭 (Weapon_Effect_R ������ ����Ʈ��)

    // �� �Ӽ��� �ش��ϴ� �����̻��� ���� (ice -> Slow2���� ����)
    public StatusEffect[] attributeStatusEffects = { StatusEffect.None, StatusEffect.Slow2, StatusEffect.None, StatusEffect.None };

    private int currentAttributeIndex = 0; // ���� �Ӽ� �ε���

    public int healthRestoreAmount = 5; // ���� �� ȸ���� ���� (�⺻ 5)

    void Start()
    {
        // ������ �� ù ��° �Ӽ� ����Ʈ�� Ȱ��ȭ
        SwitchAttributeEffect(0);
    }

    // �Ӽ� ����Ʈ ��ü ����
    public void HandleAttributeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchAttributeEffect(0); // 4�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchAttributeEffect(1); // 5�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchAttributeEffect(2); // 6�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchAttributeEffect(3); // 7�� �Ӽ�
        }
    }

    private void SwitchAttributeEffect(int attributeIndex)
    {
        // ������ �Ѵ� �ε����� �Էµ��� �ʵ��� ���� ó��
        if (attributeIndex >= attributeEffects.Length || attributeIndex >= attributeEffects_Left.Length || attributeIndex >= attributeEffects_Right.Length)
            return;

        // ��� �Ӽ� ���� ����Ʈ�� ��Ȱ��ȭ�ϰ� ������ �Ӽ� ���� ����Ʈ�� Ȱ��ȭ
        for (int i = 0; i < attributeEffects.Length; i++)
        {
            attributeEffects[i].SetActive(i == attributeIndex);
        }

        // ��� �޼� ���� �Ӽ� ����Ʈ�� ��Ȱ��ȭ�ϰ� ������ �޼� ����Ʈ�� Ȱ��ȭ
        for (int i = 0; i < attributeEffects_Left.Length; i++)
        {
            attributeEffects_Left[i].SetActive(i == attributeIndex);
        }

        // ��� ������ ���� �Ӽ� ����Ʈ�� ��Ȱ��ȭ�ϰ� ������ ������ ����Ʈ�� Ȱ��ȭ
        for (int i = 0; i < attributeEffects_Right.Length; i++)
        {
            attributeEffects_Right[i].SetActive(i == attributeIndex);
        }

        // ���� �Ӽ� �ε����� ������Ʈ
        currentAttributeIndex = attributeIndex;
    }

    // ���� �Ӽ��� �´� �����̻��� ��ȯ�ϴ� �Լ�
    public StatusEffect GetCurrentStatusEffect()
    {
        if (currentAttributeIndex < attributeStatusEffects.Length)
        {
            return attributeStatusEffects[currentAttributeIndex];
        }

        return StatusEffect.None; // �⺻������ �����̻��� ���� ��
    }

    // ���� �� ȣ��Ǵ� �Լ�
    public void OnAttackHit()
    {
        if (currentAttributeIndex == 0) // 4�� ���Կ� �ش��� ���
        {
            RestoreHealth();
        }
    }

    // �÷��̾��� ü���� ȸ���ϴ� �Լ�
    private void RestoreHealth()
    {
        PlayerStatus playerStatus = GetComponent<PlayerStatus>(); // �÷��̾� ���� ��������
        if (playerStatus != null)
        {
            playerStatus.RestoreHealth(healthRestoreAmount); // ü�� ȸ��
        }
    }
}
