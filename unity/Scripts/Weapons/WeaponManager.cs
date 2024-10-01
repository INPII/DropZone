using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] leftWeapons;  // �޼տ� ������ ���� �迭 (Hierarchy���� �̸� ��ġ�� �ִ� ������Ʈ)
    public GameObject[] rightWeapons; // �����տ� ������ ���� �迭 (Hierarchy���� �̸� ��ġ�� �ִ� ������Ʈ)

    public GameObject[] attributeEffects;  // ���� �Ӽ��� ���� ����Ʈ �迭

    private int currentWeaponIndex = 0; // ���� ���õ� ���� �ε���

    private Weapon currentWeapon; // ���� ���õ� ����

    void Start()
    {
        // ������ �� ù ��° ���⸸ Ȱ��ȭ
        SwitchWeapon(0);

        // ���� ���� �� ���´� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� PhotonView�� RPC ��� �ʿ�
    }

    // ���� ��ü ����
    public void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0); // 1�� ����
            // ���� ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� ��
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1); // 2�� ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2); // 3�� ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3); // 4�� ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchWeapon(4); // 5�� ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchWeapon(5); // 6�� ����
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchWeapon(6); // 7�� ����
        }

        // ���� ��ü�� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ�, RPC�� ���� ���� ���¸� ������ �� ����
    }

    private void SwitchWeapon(int weaponIndex)
    {
        // �޼� ���Ⱑ �ִ� ��쿡�� ó��
        if (leftWeapons.Length > 0)
        {
            // ������ �Ѵ� �ε����� �Էµ��� �ʵ��� ���� ó��
            if (weaponIndex < leftWeapons.Length)
            {
                // ��� �޼� ���⸦ ��Ȱ��ȭ�ϰ� ������ ���⸸ Ȱ��ȭ
                for (int i = 0; i < leftWeapons.Length; i++)
                {
                    leftWeapons[i].SetActive(i == weaponIndex);
                }
                // ���� ���õ� �޼� ������ Weapon ������Ʈ�� ������
                currentWeapon = leftWeapons[weaponIndex].GetComponent<Weapon>();
            }
        }

        // ������ ���Ⱑ �ִ� ��쿡�� ó��
        if (rightWeapons.Length > 0)
        {
            // ������ �Ѵ� �ε����� �Էµ��� �ʵ��� ���� ó��
            if (weaponIndex < rightWeapons.Length)
            {
                // ��� ������ ���⸦ ��Ȱ��ȭ�ϰ� ������ ���⸸ Ȱ��ȭ
                for (int i = 0; i < rightWeapons.Length; i++)
                {
                    rightWeapons[i].SetActive(i == weaponIndex);
                }
                // ���� ���õ� ������ ������ Weapon ������Ʈ�� ������
                currentWeapon = rightWeapons[weaponIndex].GetComponent<Weapon>();
            }
        }

        // ���� ���� �ε����� ������Ʈ
        currentWeaponIndex = weaponIndex;

        // ���� �Ӽ��� ���� ���� ����Ʈ Ȱ��ȭ (�迭�� �ִ� ��쿡��, �ٰŸ����� ���)
        if (attributeEffects != null && attributeEffects.Length > 0)
        {
            ActivateEffectBasedOnAttribute();
        }

        // ���� �Ӽ� �� ���� ���� ���µ� ��Ʈ��ũ �� ����ȭ�� �ʿ��� �� ����
    }

    // ���� �Ӽ��� ���� ����Ʈ�� Ȱ��ȭ�ϴ� �Լ�
    private void ActivateEffectBasedOnAttribute()
    {
        // ��� ����Ʈ�� ��Ȱ��ȭ
        foreach (var effect in attributeEffects)
        {
            effect.SetActive(false);
        }

        // ���� ������ �Ӽ��� �´� ����Ʈ�� Ȱ��ȭ
        if (currentWeapon != null)
        {
            switch (currentWeapon.weaponAttribute)
            {
                case WeaponAttribute.None:
                    attributeEffects[0].SetActive(true); // Ice ����Ʈ Ȱ��ȭ
                    break;
                case WeaponAttribute.Blood:
                    attributeEffects[1].SetActive(true); // Blood ����Ʈ Ȱ��ȭ
                    break;
                case WeaponAttribute.Ice:
                    attributeEffects[2].SetActive(true); // Ice ����Ʈ Ȱ��ȭ
                    break;
                case WeaponAttribute.Iron:
                    attributeEffects[3].SetActive(true); // Iron ����Ʈ Ȱ��ȭ
                    break;
                case WeaponAttribute.Gear:
                    attributeEffects[4].SetActive(true); // Gear ����Ʈ Ȱ��ȭ
                    break;
                case WeaponAttribute.Gunpowder:
                    attributeEffects[5].SetActive(true); // Gunpowder ����Ʈ Ȱ��ȭ
                    break;
            }
        }

        // ���� �Ӽ��� ���� ����Ʈ ��ȭ ���� ��Ʈ��ũ �� ����ȭ�� �ʿ��� �� ����
    }

    // ���� ������ ������ ��ȯ
    public int GetCurrentWeaponDamage()
    {
        return currentWeapon != null ? currentWeapon.damage : 0;
    }

    // ���� ������ �Ӽ� ��ȯ
    public WeaponAttribute GetCurrentWeaponAttribute()
    {
        return currentWeapon != null ? currentWeapon.weaponAttribute : WeaponAttribute.None;
    }
}
