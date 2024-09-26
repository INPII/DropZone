using System.Collections;
using UnityEngine;
using static PlayerStatus;

public class PlayerAttack_Boxer : MonoBehaviour, IAttack
{
    private Animator anim;
    public WeaponManager weaponManager;
    private PlayerMovement playerMovement; // PlayerMovement ����
    public GameObject attackCollider; // ���� �ݶ��̴� (Hit Box)

    public int damage; // ������ �� �� ������(���⿡�� �����ͼ� ����)        
    public float fireDelay;
    private bool isAttack;
    private bool isFireReady;

    public float skillCoolDown = 7.0f; // ��ų ��ٿ�
    private float lastSkillTime = -100f; // ������ ��ų ��� �ð��� ���    

    public PlayerStatus playerStatus; // �÷��̾� ���� ���� (���۾Ƹ� ������ ����)
    public GameObject superArmorEffect; // ���۾Ƹ� ����Ʈ�� ���� GameObject

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement ������Ʈ ����
        playerStatus = GetComponent<PlayerStatus>(); // PlayerStatus ������Ʈ ����
        attackCollider.SetActive(false); // ó������ ��Ȱ��ȭ

        // PhotonView�� �ʿ�: ���� �� ��ų ���� ���� �߿��� ������ ��Ʈ��ũ �� ����ȭ�ؾ� ��
    }

    void Update()
    {
        fireDelay += Time.deltaTime;
        isFireReady = 1.5f < fireDelay;

        // ��ų�� ��Ʈ��ũ ����ȭ�� �ʿ��ϹǷ� PhotonView�� RPC�� ����Ͽ� ��ų ��� �� �̸� �ٸ� Ŭ���̾�Ʈ�� �����ؾ� ��
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCoolDown) // ��Ŭ�� �Է�
        {
            ActivateSuperArmor(); // ���۾Ƹ� ����
            // ���۾Ƹ� ���뵵 �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� RPC ��� �ʿ�
        }
    
    }

    public void GetAttackInput(bool fDown)
    {
        if (fDown && isFireReady)
        {
            StartAttack();
            // ���� ������ �ٸ� Ŭ���̾�Ʈ���� ���޵Ǿ�� �ϹǷ� PhotonView�� ����� ����ȭ�ؾ� ��
        }
    }

    public void StartAttack()
    {
        isAttack = true;

        // ���� ������ ������ ������ ��������
        damage = weaponManager.GetCurrentWeaponDamage();

        playerMovement.SetAttackState(true); // ���� ���� ����
        playerMovement.TurnTowardsMouse(); // ���� ���� �� ���콺 �������� ȸ��        
        anim.SetTrigger("doAttack");
        fireDelay = 0;

        StartCoroutine(PerformMultiAttack());
        Invoke("EndAttack", 1.0f); // 1�� �� ���� ����
    }

    private IEnumerator PerformMultiAttack()
    {
        for (int i = 0; i < 3; i++) // 3 �޺� ����
        {
            ActivateColliderAndParticle();
            yield return new WaitForSeconds(0.33f); // ���� ����
        }
    }

    private void ActivateColliderAndParticle()
    {
        attackCollider.SetActive(true);
        Invoke("DeactivateCollider", 0.33f); // 0.3�� �� �ݶ��̴� ��Ȱ��ȭ
    }

    private void DeactivateCollider()
    {
        attackCollider.SetActive(false);
    }

    private void EndAttack()
    {
        isAttack = false;
        playerMovement.SetAttackState(false); // ���� ���� ����

        // ���� ���ᵵ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� PhotonView�� RPC ��� �ʿ�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus enemyStatus = other.GetComponent<PlayerStatus>();
            if (enemyStatus != null)
            {
                enemyStatus.TakeDamage(damage); // ��� �÷��̾�� �������� ��

                // �����̻� ���� (ex. ice �Ӽ��� ��� ���ο� ���¸� �ο�)                                
                ApplyStatusEffectOnHit(enemyStatus);

                // ���ݿ� ���� ���� ��ȭ ���� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�� �ʿ��ϹǷ� PhotonView�� RPC ��� ���

                // ���� ������ �Ӽ��� Blood�� ��� ü�� ȸ��
                if (weaponManager.GetCurrentWeaponAttribute() == WeaponAttribute.Blood)
                {
                    int healthToRestore = Mathf.CeilToInt(damage * 0.2f); // �������� 20%�� �ݿø��Ͽ� ���
                    playerStatus.RestoreHealth(healthToRestore); // ���ϴ� ��ŭ�� ȸ���� ����
                    Debug.Log($"ü���� {healthToRestore} ȸ���Ǿ����ϴ�.");
                    // ü�� ȸ���� ��Ʈ��ũ ����ȭ�� �ʿ��� �� ����
                }

            }
        }
    }

    // �����̻��� �����ϴ� �Լ�
    private void ApplyStatusEffectOnHit(PlayerStatus enemyStatus)
    {
        // ���� ������ ������ �Ӽ� ��������
        WeaponAttribute currentWeaponAttribute = weaponManager.GetCurrentWeaponAttribute();

        // Ice �Ӽ��� ��� ���濡�� Slow2 ���¸� �ο�
        if (currentWeaponAttribute == WeaponAttribute.Ice)
        {
            enemyStatus.ApplyStatusEffect(StatusEffect.Slow2);
            Debug.Log("Applied Slow2 effect to the enemy.");

            // ���濡�� �����̻��� �ο��ϴ� ���� ���� ��Ʈ��ũ ����ȭ�� �ʿ��� �� ����
        }
    }

    // ���۾ƸӸ� Ȱ��ȭ�ϴ� �Լ� (5�ʰ� ����)
    private void ActivateSuperArmor()
    {
        if (playerStatus.currentStatus != StatusEffect.SuperArmor)
        {
            playerStatus.ApplyStatusEffect(StatusEffect.SuperArmor); // ���۾Ƹ� ���� ����
            Debug.Log("SuperArmor activated for 5 seconds");

            // ���۾Ƹ� ���� ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� RPC�� ó�� �ʿ�

            // ���۾Ƹ� ����Ʈ�� Ȱ��ȭ
            if (superArmorEffect != null)
            {
                superArmorEffect.SetActive(true); // ����Ʈ Ȱ��ȭ
            }

            StartCoroutine(DeactivateSuperArmorAfterDelay(5.0f)); // 5�� �� ���۾Ƹ� ����

            // ��ų ��� �ð��� ����� ��ٿ��� ����
            lastSkillTime = Time.time; // ��ų ��� �ð��� ���
        }
    }


    // 5�� �� ���۾ƸӸ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator DeactivateSuperArmorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerStatus.RemoveStatusEffect(); // �����̻� ���� (���۾Ƹ� ����)

        // ���۾Ƹ� ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� RPC�� ó�� �ʿ�

        // ���۾Ƹ� ����Ʈ�� ��Ȱ��ȭ
        if (superArmorEffect != null)
        {
            superArmorEffect.SetActive(false); // ����Ʈ ��Ȱ��ȭ
        }

        Debug.Log("SuperArmor deactivated after 5 seconds");
    }

    public void ExecuteEvent()
    {
        // ���� �ִϸ��̼ǿ��� Ư�� Ÿ�ֿ̹� �̺�Ʈ�� �����ϰ� ���� ��
        Debug.Log("Attack animation event triggered!");
        // �ʿ��� ���� �߰� (��: ������ ���� ��)
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
}
