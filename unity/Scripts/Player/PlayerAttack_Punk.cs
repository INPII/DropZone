using System.Collections;
using UnityEngine;
using static PlayerStatus;

public class PlayerAttack_Punk : MonoBehaviour, IAttack
{
    private Animator anim;
    public WeaponManager weaponManager;
    private PlayerMovement playerMovement;
    public GameObject attackCollider; // ���� �ݶ��̴� (��Ʈ�ڽ�)
    public GameObject skillCollider; // ��ų �ݶ��̴� (��Ʈ�ڽ�)

    public int damage; // ������ �� �� ������(���⿡�� �����ͼ� ����) 
    public float fireDelay;
    private bool isAttack;
    private bool isFireReady;

    public float skillCoolDown = 5.0f; // ��ų ��ٿ�
    private float lastSkillTime = -100f; // ������ ��ų ��� �ð� ���

    public float knockbackForce = 10.0f; // �˹��� ��

    public PlayerStatus playerStatus;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        attackCollider.SetActive(false);
        skillCollider.SetActive(false);

        // PhotonView�� �ʿ�: ���ݰ� ��ų ��� ���¸� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
    }

    void Update()
    {
        fireDelay += Time.deltaTime;
        isFireReady = 1.5f < fireDelay;

        // ��ų ����� ��Ʈ��ũ ����ȭ�� �ʿ��ϹǷ� PhotonView�� RPC ����� �ʿ���
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCoolDown)
        {
            UseSkill(); // ��ų ���
        }
    }

    public void GetAttackInput(bool fDown)
    {
        if (fDown && isFireReady)
        {
            StartAttack();
            // ���� ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� RPC ��� �ʿ�
        }
    }

    public void StartAttack()
    {
        // ���� ������ ������ ������ ��������
        damage = weaponManager.GetCurrentWeaponDamage();

        isAttack = true;
        playerMovement.SetAttackState(true);
        playerMovement.TurnTowardsMouse();
        anim.SetTrigger("doAttack");
        fireDelay = 0;

        StartCoroutine(PerformMultiAttack());
        Invoke("EndAttack", 1.0f);

        // ���� ���µ� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� RPC �ʿ�
    }

    private IEnumerator PerformMultiAttack()
    {
        for (int i = 0; i < 2; i++) // 2 �޺� ����
        {
            ActivateAttackCollider();
            yield return new WaitForSeconds(0.5f); // ���� ����
        }
    }

    private void ActivateAttackCollider()
    {
        attackCollider.SetActive(true);
        Invoke("DeactivateAttackCollider", 0.5f); // ���� �ð� ���� �ݶ��̴� ��Ȱ��ȭ
    }

    private void DeactivateAttackCollider()
    {
        attackCollider.SetActive(false);
    }

    private void EndAttack()
    {
        isAttack = false;
        playerMovement.SetAttackState(false);

        // ���� ���ᵵ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
    }

    // ���� �� ��ų ��� ó���ϴ� OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        // ���� �ݶ��̴��� ��ų �ݶ��̴��� ����
        if (other.CompareTag("Player"))
        {
            // ���� �ݶ��̴��� �¾��� ��
            if (attackCollider.activeSelf)
            {
                HandleAttackCollision(other);
            }

            // ��ų �ݶ��̴��� �¾��� ��
            if (skillCollider.activeSelf)
            {
                HandleSkillCollision(other);
            }

            // Ʈ���� �浹 �� ȿ���� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�Ǿ�� �ϹǷ� PhotonView�� RPC ��� ���
        }
    }

    private void HandleAttackCollision(Collider other)
    {
        PlayerStatus enemyStatus = other.GetComponent<PlayerStatus>();
        if (enemyStatus != null)
        {
            // ������ ����
            enemyStatus.TakeDamage(damage);

            // �����̻� ����
            ApplyStatusEffectOnHit(enemyStatus);
        }
    }

    private void HandleSkillCollision(Collider other)
    {
        Rigidbody enemyRb = other.GetComponent<Rigidbody>();
        PlayerStatus enemyStatus = other.GetComponent<PlayerStatus>();

        if (enemyRb != null && enemyStatus != null)
        {
            // �˹� ���� ���
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            knockbackDirection.y = 0f; // y���� �����Ͽ� ��/�Ʒ� �������� ����

            // ���� �о (�˹� ȿ��)
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // �����̻��� Knockback���� ����
            enemyStatus.ApplyStatusEffect(StatusEffect.Knockback);

            // ������ ���� (��ų������ = ������ * 1.5)
            enemyStatus.TakeDamage(Mathf.RoundToInt(damage * 1.5f));
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
        }
    }

    // ��ų ���
    private void UseSkill()
    {
        // ���� ������ ������ ������ ��������
        damage = weaponManager.GetCurrentWeaponDamage();

        lastSkillTime = Time.time;
        playerMovement.SetAttackState(true);
        playerMovement.TurnTowardsMouse();
        anim.SetTrigger("doSkill");


        // ��ų �ݶ��̴� Ȱ��ȭ
        Invoke("ActivateSkillCollider", 0.6f);
        // ��ų ��� �� ��Ȱ��ȭ
        Invoke("DeactivateSkillCollider", 0.8f);

        Invoke("EndAttack", 0.8f);
    }

    private void ActivateSkillCollider()
    {
        skillCollider.SetActive(true);
    }

    private void DeactivateSkillCollider()
    {
        skillCollider.SetActive(false);
    }

    public void ExecuteEvent()
    {
        Debug.Log("Attack animation event triggered!");
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
