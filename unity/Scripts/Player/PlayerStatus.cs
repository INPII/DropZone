using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // UI�� ����ϱ� ���� ���ӽ����̽�

public class PlayerStatus : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    private Animator anim;

    private Collider[] colliders; // �÷��̾ ����� ��� �ݶ��̴��� ����
    private Coroutine statusEffectCoroutine; // ���� ���� ���� �����̻� �ڷ�ƾ

    public GameObject stunEffect; // ���� ����Ʈ
    public GameObject slowEffect; // ���ο� ����Ʈ

    public Image hpBarImage;  // HP �� UI �̹���

    public enum StatusEffect
    {
        None,         // ���� ����
        Dead,         // ��� ����
        Stunned,      // ���� ����
        Immobilized,  // �ӹ� ����
        Slow1,        // ���ο�1 ���� (�̵� �ӵ� ����, �뽬 �Ұ�)
        Slow2,        // ���ο�2 ���� (�̵� �ӵ� ����)
        Knockback,    // �˹� ����
        SuperArmor,   // ���۾Ƹ� ���� (Boxer ��ų)
        blood         // ü�� ȸ�� �Ӽ� ���� ���� ����
    }

    public StatusEffect currentStatus = StatusEffect.None;  // �⺻ ���´� None

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHP = maxHP;  // HP �ʱ�ȭ
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        // �÷��̾� ������Ʈ�� �پ� �ִ� ��� �ݶ��̴��� ������
        colliders = GetComponentsInChildren<Collider>();

        // HP �̹��� �ʱ�ȭ
        if (hpBarImage != null)
        {
            UpdateHPUI();
        }

        // PhotonView�� �ʿ�: ���� ����(��: HP, �����̻� ��)�� ��Ʈ��ũ �󿡼� ����ȭ�ؾ� ��
    }

    void Update()
    {
        HandleStatusEffects();  // �����̻� ó��
    }

    // ������ ó�� �� ��� ó��
    public void TakeDamage(int damage)
    {
        if (currentStatus == StatusEffect.Dead) return;  // �̹� ���� ���¸� ������ ó�� ����

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP�� ������ �������� �ʰ� ����
        Debug.Log("Player HP: " + currentHP);

        // HP�� ����Ǹ� HP �� ������Ʈ
        UpdateHPUI();

        if (currentHP <= 0)
        {
            ApplyStatusEffect(StatusEffect.Dead);

            // �������� ��� ���µ� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� RPC�� �ʿ�
        }
    }

    // �����̻� ����Ʈ�� �����ϴ� �Լ� (�ߺ� �ڵ� ����)
    private void ManageStatusEffect(StatusEffect status, bool isActive)
    {
        switch (status)
        {
            case StatusEffect.Stunned:
                if (stunEffect != null)
                {
                    stunEffect.SetActive(isActive); // ���� ����Ʈ Ȱ��ȭ/��Ȱ��ȭ
                }
                break;

            case StatusEffect.Slow1:
            case StatusEffect.Slow2:
                if (slowEffect != null)
                {
                    slowEffect.SetActive(isActive); // ���ο� ����Ʈ Ȱ��ȭ/��Ȱ��ȭ
                }
                break;
        }

        // �����̻� ����Ʈ�� �ٸ� Ŭ���̾�Ʈ���� ����ȭ�Ǿ�� �ϹǷ�, PhotonView�� RPC�� �ʿ�
    }

    // �����̻� ó��
    void HandleStatusEffects()
    {
        switch (currentStatus)
        {
            case StatusEffect.None:
                // ���� ����: �̵��� ���� ����
                playerMovement.canMove = true;
                playerMovement.canDash = true;
                break;

            case StatusEffect.Dead:
                // ���� ����: �̵��� ���� �Ұ�
                playerMovement.canMove = false;
                playerMovement.canDash = false;
                gameObject.layer = LayerMask.NameToLayer("Dead");
                anim.SetBool("isDying", true);
                break;

            case StatusEffect.Stunned:
                // ���� ����: �̵��� ���� �Ұ�
                playerMovement.canMove = false;
                playerMovement.canDash = false;
                anim.SetTrigger("doStunned");
                ManageStatusEffect(StatusEffect.Stunned, true);  // ���� ����Ʈ Ȱ��ȭ
                break;

            case StatusEffect.Immobilized:
                // �ӹ� ����: �뽬 �Ұ�, �̵� �Ұ�
                playerMovement.canMove = false;
                playerMovement.canDash = false;
                break;

            case StatusEffect.Slow1:
                // ���ο�1 ����: �̵� �ӵ� ����, �뽬 �Ұ�
                playerMovement.moveSpeed = playerMovement.defaultSpeed * 0.5f;
                playerMovement.canDash = false;
                ManageStatusEffect(StatusEffect.Slow1, true);  // ���ο�1 ����Ʈ Ȱ��ȭ
                break;

            case StatusEffect.Slow2:
                // ���ο�2 ����: �̵� �ӵ��� ����
                playerMovement.moveSpeed = playerMovement.defaultSpeed * 0.5f;
                ManageStatusEffect(StatusEffect.Slow2, true);  // ���ο�2 ����Ʈ Ȱ��ȭ
                break;

            case StatusEffect.Knockback:
                // �˹� ó�� ����                
                break;

            case StatusEffect.SuperArmor:
                // ���۾Ƹ� ����: �����̻� �鿪                
                playerMovement.isKnockbackImmune = true;
                break;

                // �����̻� ���� ���� ���׵� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ�, PhotonView�� RPC�� �ʿ�
        }
    }

    // �����̻� �ο� �Լ�
    public void ApplyStatusEffect(StatusEffect newStatus)
    {
        // ���°� None�̰ų�, �˹� ���¿����� �ٸ� �����̻����� ��ȯ�� ��� (�Ͻ��� �鿪)
        if (currentStatus != StatusEffect.None && currentStatus != StatusEffect.Knockback) return;

        // ���۾Ƹ� ������ ���� �����̻� ������� ����
        if (currentStatus == StatusEffect.SuperArmor) return;

        currentStatus = newStatus;
        Debug.Log("New Status: " + newStatus);

        // ���� �����̻� ���� �ڷ�ƾ �ߴ�
        if (statusEffectCoroutine != null)
        {
            StopCoroutine(statusEffectCoroutine);
        }

        // ���º� ���� �ð� ����
        float effectDuration = GetStatusEffectDuration(newStatus);

        if (effectDuration > 0f)
        {
            // ���� �ð��� 0���� ũ�� �ش� �ð� �Ŀ� ���� ����
            statusEffectCoroutine = StartCoroutine(RemoveStatusEffectAfterDelay(effectDuration));
        }

        // ���� ���浵 �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ�, PhotonView�� RPC�� �ʿ�
    }

    // ���º� ���� �ð��� ��ȯ�ϴ� �Լ�
    private float GetStatusEffectDuration(StatusEffect status)
    {
        switch (status)
        {
            case StatusEffect.Slow1:
                return 2.0f;
            case StatusEffect.Slow2:
                return 2.0f;
            case StatusEffect.Knockback:
                return 1.5f; // �˹��� 1.5�� ���� ����
            case StatusEffect.Stunned:
                return 1.0f; // ������ 1�� ���� ����
            default:
                return 0f; // None, Dead ���� ���� �ð��� �����Ƿ� 0 ��ȯ
        }
    }

    // �����̻� ���� �Լ� (�ߺ� �ڵ� ����)
    public void RemoveStatusEffect()
    {
        currentStatus = StatusEffect.None;
        playerMovement.moveSpeed = playerMovement.defaultSpeed; // �̵� �ӵ��� �⺻ �ӵ��� ����

        // ��� �����̻� ����Ʈ�� ��Ȱ��ȭ
        ManageStatusEffect(StatusEffect.Stunned, false);
        ManageStatusEffect(StatusEffect.Slow1, false);
        ManageStatusEffect(StatusEffect.Slow2, false);

        // �����̻� ������ ����ȭ�ؾ� ��: �����̻� ���� ���� ������ PhotonView�� RPC�� ����
    }

    // ���� �ð� �� �����̻��� �����ϴ� �ڷ�ƾ
    private IEnumerator RemoveStatusEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveStatusEffect();
        Debug.Log("Status effect removed after delay");
    }

    // �˹� ���¿��� ���� �浹 �� ���� ���·� ����
    void OnCollisionEnter(Collision collision)
    {
        if (currentStatus == StatusEffect.Knockback && collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("���� �ε�����!");
            ApplyStatusEffect(StatusEffect.Stunned); // �ٷ� ���� ���·� ��ȯ     
            // �浹 ���� �� �����̻� ��ȯ�� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
        }
    }

    // ���۾Ƹ� Ȱ��ȭ �Լ� (Boxer ��ų ��� �� ȣ��)
    public void ActivateSuperArmor()
    {
        currentStatus = StatusEffect.SuperArmor;
        Debug.Log("SuperArmor activated");
        // ���۾Ƹ� ���µ� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
    }

    // ���۾Ƹ� ��Ȱ��ȭ �Լ� (��ų ���ӽð��� ���� �� ȣ��)
    public void DeactivateSuperArmor()
    {
        currentStatus = StatusEffect.None;  // ���۾Ƹ� ���¿��� ���� ���·� ���ư�
        Debug.Log("SuperArmor deactivated");
        // ���۾Ƹ� ��Ȱ��ȭ�� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
    }

    // ���� �� ü�� ȸ�� ���
    public void RestoreHealth(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // ü���� �ִ�ġ�� ���� �ʵ��� ����

        // ü�� ȸ�� �� HP �� ������Ʈ
        UpdateHPUI();

        // ü�� ȸ�� ��ɵ� ��Ʈ��ũ �� ����ȭ�� �ʿ��� �� ����
    }

    // HP UI ������Ʈ �Լ�
    private void UpdateHPUI()
    {
        if (hpBarImage != null)
        {
            // HP ������ ���� fillAmount ������Ʈ
            hpBarImage.fillAmount = (float)currentHP / maxHP;
        }
    }
}
