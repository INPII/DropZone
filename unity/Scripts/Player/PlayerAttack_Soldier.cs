using System.Collections;
using UnityEngine;

public class PlayerAttack_Soldier : MonoBehaviour, IAttack
{
    private Animator anim;
    public GameObject bulletPrefab; // �߻�ü ������
    public GameObject piercingBulletPrefab; // ����ź ������
    public Transform bulletPos; // �߻� ��ġ
    public float bulletSpeed = 50f; // �߻�ü �ӵ�
    public float piercingBulletSpeed = 70f; // ����ź �߻�ü �ӵ�
    public float fireDelay = 1.5f; // ���� ����
    private bool isFireReady = true;
    private bool isAttacking = false; // ���� ������ �����ϴ� �÷���

    public int maxAmmo = 20; // �ִ� ź�� ��
    public int currentAmmo; // ���� ź�� ��
    public float reloadTime = 2.0f; // ������ �ð�
    private bool isReloading = false; // ������ ����

    public WeaponManager weaponManager; // weaponManager ����
    public PlayerMovement playerMovement; // PlayerMovement ����
    public float skillCoolDown = 5.0f; // ��ų ��Ÿ��
    private float lastSkillTime = -100f; // ������ ��ų ��� �ð��� ���

    public GameObject fireEffect; // �߻� ����Ʈ (Ȱ��ȭ/��Ȱ��ȭ �� ����Ʈ)

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement ����
        currentAmmo = maxAmmo; // ź�� �ʱ�ȭ

        // �߻� ����Ʈ�� ��Ȱ��ȭ�� ���·� ����
        if (fireEffect != null)
        {
            fireEffect.SetActive(false); // �߻� ����Ʈ ��Ȱ��ȭ
        }

        // PhotonView�� �ʿ�: ���Ÿ� ����(�߻�ü) �� ��ų ������ ��Ʈ��ũ �� ����ȭ�ؾ� ��
    }

    void Update()
    {
        // ���� ��Ÿ�� ó��
        fireDelay += Time.deltaTime;
        isFireReady = fireDelay >= 0.4f;

        // ���� ������ (RŰ �Է� ��)
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // ��ų ����� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�� �ʿ��ϹǷ� PhotonView�� RPC ����� �ʿ���
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCoolDown)
        {
            UseSkill();
        }
    }

    public void GetAttackInput(bool fDown)
    {
        if (fDown && isFireReady)
        {
            StartAttack();
            // ���� ���۵� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� PhotonView�� RPC ��� �ʿ�
        }
    }

    // �Ϲ� ����
    public void StartAttack()
    {
        // ������ ���̶�� ���� �Ұ�
        if (isReloading)
        {
            Debug.Log("������ �߿��� ������ �� �����ϴ�.");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("ź���� �����մϴ�! �������� �ʿ��մϴ�.");
            StartCoroutine(Reload());
            return;
        }

        isFireReady = false; // ���� �� ������ ����
        isAttacking = true; // ���� �� ���� ����
        playerMovement.SetAttackState(true); // ���� ���� ����
        playerMovement.TurnTowardsMouse(); // ���� ���� �� ���콺 �������� ȸ��  
        anim.SetTrigger("doAttack"); // ���� �ִϸ��̼� ����
        fireDelay = 0; // ��Ÿ�� �ʱ�ȭ

        // ���� �� �߻�ü�� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� �߻�ü ���� �� �߻� ������ RPC�� ����

        // �߻� ����Ʈ�� Ȱ��ȭ
        if (fireEffect != null)
        {
            fireEffect.transform.position = bulletPos.position; // �߻� ��ġ�� ����Ʈ ��ġ
            fireEffect.SetActive(true); // ����Ʈ Ȱ��ȭ
            Invoke("DisableFireEffect", 0.2f); // 0.2�� �� ����Ʈ�� ��Ȱ��ȭ
        }

        // �߻�ü ����
        GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation * Quaternion.Euler(0, 180, 0));
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // ������ �������� �߻�ü�� ����
            bulletScript.damage = weaponManager.GetCurrentWeaponDamage();
            bulletScript.shooter = gameObject; // �߻��ڸ� ���� ���� ������Ʈ�� ����

            // ������ �Ӽ��� ���� �߻�ü�� �Ӽ��� ����
            WeaponAttribute currentAttribute = weaponManager.GetCurrentWeaponAttribute();
            switch (currentAttribute)
            {
                case WeaponAttribute.Ice:
                    bulletScript.bulletType = Bullet.BulletType.Ice;
                    break;
                case WeaponAttribute.Gunpowder:
                    bulletScript.bulletType = Bullet.BulletType.Gunpowder;
                    break;
                default:
                    bulletScript.bulletType = Bullet.BulletType.Normal;
                    break;
            }
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletPos.forward * bulletSpeed; // �߻�ü �ӵ� ����
        }

        currentAmmo--; // ź�� ����
        Invoke("TryEndAttack", 1.3f); // 1.3�� �� ���� ���� �õ�

        // �߻�ü�� ���õ� ��� ���۵� ��Ʈ��ũ �� ����ȭ �ʿ�
    }

    // ��ų ��� (��Ŭ������ ���)
    public void UseSkill()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("ź���� �����մϴ�! �������� �ʿ��մϴ�.");
            StartCoroutine(Reload());
            return;
        }

        lastSkillTime = Time.time; // ��ų ��� �ð� ���
        playerMovement.SetAttackState(true); // ���� ���� ����
        playerMovement.TurnTowardsMouse(); // ���� ���� �� ���콺 �������� ȸ��  
        anim.SetTrigger("doSkill"); // ��ų �ִϸ��̼� ����

        // ����ź �߻� ������ �ٸ� Ŭ���̾�Ʈ���� ����ȭ�� �ʿ��ϹǷ� RPC ��� ���

        // �߻� ����Ʈ�� Ȱ��ȭ
        if (fireEffect != null)
        {
            fireEffect.transform.position = bulletPos.position; // �߻� ��ġ�� ����Ʈ ��ġ
            fireEffect.SetActive(true); // ����Ʈ Ȱ��ȭ
            Invoke("DisableFireEffect", 0.2f); // 0.2�� �� ����Ʈ�� ��Ȱ��ȭ
        }

        // ����ź �߻�
        GameObject piercingBullet = Instantiate(piercingBulletPrefab, bulletPos.position, bulletPos.rotation * Quaternion.Euler(0, 180, 0));
        Bullet bulletScript = piercingBullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.damage = weaponManager.GetCurrentWeaponDamage(); // ������ �������� ����ź�� ����
            bulletScript.shooter = gameObject; // �߻��ڸ� ���� ���� ������Ʈ�� ����

            WeaponAttribute currentAttribute = weaponManager.GetCurrentWeaponAttribute();
            switch (currentAttribute)
            {
                case WeaponAttribute.Ice:
                    bulletScript.bulletType = Bullet.BulletType.Ice;
                    break;
                case WeaponAttribute.Gunpowder:
                    bulletScript.bulletType = Bullet.BulletType.Gunpowder;
                    break;
                default:
                    bulletScript.bulletType = Bullet.BulletType.Normal;
                    break;
            }
        }

        Rigidbody rb = piercingBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletPos.forward * piercingBulletSpeed; // ����ź �ӵ� ����
        }

        currentAmmo--; // ź�� ����
        Invoke("TryEndAttack", 1.3f); // 1.3�� �� ���� ���� �õ�
    }

    private void DisableFireEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.SetActive(false); // ����Ʈ ��Ȱ��ȭ
        }
    }

    private void TryEndAttack()
    {
        if (!isFireReady && isAttacking)
        {
            // ���� ������ ������ �ʾ����Ƿ� �������� ����
            Debug.Log("��� ���� ���Դϴ�. �������� �ʽ��ϴ�.");
        }
        else
        {
            // ������ �����ٸ� ���� ����
            EndAttack();
        }
    }

    // ���� ����
    private void EndAttack()
    {
        playerMovement.SetAttackState(false); // ���� ���� ����
        isAttacking = false; // ���� ���� ����
        isFireReady = true; // ���� ���� ���·� ��ȯ
        Debug.Log("���� ����");

        // ���� ���� ���µ� �ٸ� Ŭ���̾�Ʈ�� ����ȭ �ʿ�
    }

    // ������ �޼���
    private IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        anim.SetTrigger("doReload"); // ������ �ִϸ��̼� ���� (�ʿ信 ���� �߰�)

        Debug.Log("������ ��...");
        yield return new WaitForSeconds(reloadTime); // ������ �ð� ���

        currentAmmo = maxAmmo; // ź�� ����
        isReloading = false;
        Debug.Log("������ �Ϸ�!");

        // ������ ���� ���� ��Ʈ��ũ �� ����ȭ �ʿ�
    }

    public void ExecuteEvent()
    {
        // ���� �ִϸ��̼ǿ��� Ư�� Ÿ�ֿ̹� �̺�Ʈ�� �����ϰ� ���� ��        
    }

    // IAttack �������̽� ����
    public float GetSkillCooldown()
    {
        return skillCoolDown;
    }

    public float GetLastSkillTime()
    {
        return lastSkillTime;
    }

    // Soldier ���� ��ź�� �޼���
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
}
