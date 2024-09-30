using Polyperfect.Universal;
using System.Collections;
using UnityEngine;

public class PlayerAttack_Archer : MonoBehaviour, IAttack
{
    private Animator anim;

    public GameObject[] ArrowPrefab; // �߻�ü ������
    public GameObject[] skillArrowPrefab; // ��ų �߻�ü ������
    public GameObject[] chargingAttributeEffect; // ��¡ ����Ʈ
    private int currentAttributeIndex = 0; // ���� �Ӽ� �ε���


    public Transform ArrowPos; // �߻� ��ġ
    public float ArrowSpeed = 30f; // �߻�ü �ӵ�

    
    public float maxArrowSpeed = 50f;  // �ִ� ȭ�� �ӵ�
    public float maxChargeTime = 2.0f; // �ִ� ��¡ �ð�
    public float minChargeTime = 0.5f; // �ּ� ��¡ �ð�
    public float fireDelay = 2.5f; // ���� ����
    private bool isFireReady = true;

    public ParticleSystem attackEffect; // ����(�Ҵк�) ����Ʈ

    public WeaponManager weaponManager; // WeaponManager ����
    public PlayerMovement playerMovement; // PlayerMovement ����
    public float skillCoolDown = 5.0f; // ��ų ��Ÿ��
    private float lastSkillTime = -100f; // ������ ��ų ��� �ð��� ���
    private bool isCharging = false; // ��¡ ���� 
    private float currentChargeTime = 0f; // ��¡�ð� 1

    

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement ����
        weaponManager = GetComponent<WeaponManager>();
    }

    void Start()
    {
        // ������ �� ù ��° �Ӽ� ����Ʈ�� Ȱ��ȭ
        SwitchAttributeEffect(0);
    }

    void Update()
    {
        // ���� ��Ÿ�� ó��
        fireDelay += Time.deltaTime;
        isFireReady = fireDelay >= 0.7f;
        HandleAttributeSwitch();

        // ��¡ ���� �� ���콺 �������� ȸ��
        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            playerMovement.TurnTowardsMouse(); // ��¡ �� ���콺 �������� ȸ��
            playerMovement.canDash = false;
            isFireReady = false; // ��¡ �� ���� �Ұ�

        }

        // ��Ŭ��(��ų) �Է� ó��
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCoolDown && !playerMovement.isDash )
        {
            StartCharging();
        }

        if (Input.GetMouseButtonUp(1) && isCharging)
        {
            ReleaseAndShootSkill();
        }
    }

    
    public void GetAttackInput(bool fDown)
    {
        if (fDown && isFireReady && !playerMovement.isDash) 
        {
            StartAttack();
        }
    }

    // �⺻ ���� ����
    public void StartAttack()
    {        
        playerMovement.SetRangedAttackState(true);  // ���Ÿ� ���� �� �̵� �Ұ�
        isFireReady = false; // ���� �� ���� �Ұ�
        playerMovement.SetAttackState(true); // ���� ���� ����
        playerMovement.TurnTowardsMouse(); // ���� ���� �� ���콺 �������� ȸ��  
        anim.SetTrigger("doAttack"); // ���� �ִϸ��̼� ����
        fireDelay = 0; // ��Ÿ�� �ʱ�ȭ

        // �߻�ü ���� �� �߻�
        
        // �Ҵк� ����Ʈ ����
        if (attackEffect != null)
        {
            attackEffect.Play();
        }
        GameObject Arrow = Instantiate(ArrowPrefab[currentAttributeIndex], ArrowPos.position, ArrowPos.rotation);
        Rigidbody rb = Arrow.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = ArrowPos.forward * ArrowSpeed; // �߻�ü �ӵ� ����
        }

        // �÷��̾�� ȭ���� �浹�� ����
        Collider playerCollider = GetComponent<Collider>();  // �÷��̾��� �ݶ��̴� ��������
        Collider arrowCollider = Arrow.GetComponent<Collider>();  // ȭ���� �ݶ��̴� ��������
        if (playerCollider != null && arrowCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, arrowCollider);
        }

        // WeaponManager�� ���� ������ �������� �����ͼ� ����
        int weaponDamage = weaponManager.GetCurrentWeaponDamage();
        Arrow.GetComponent<Arrow>().SetDamage(weaponDamage);

        Invoke("EndAttack", 0.3f); // 0.3�� �� ���� ����
    }

    // ��¡ ����
    public void StartCharging()
    {        
        isCharging = true; // ��¡ ���·� ��ȯ
        anim.SetBool("isCharging", true); // ��¡ �ִϸ��̼� ����
        currentChargeTime = 0f; // 1
        playerMovement.moveSpeed /= 2f;   // ��¡ �� �̵��ӵ� ����                       


        // ��¡ ����Ʈ ����
        if (chargingAttributeEffect[currentAttributeIndex] != null)
        {
            ParticleSystem chargingEffect = chargingAttributeEffect[currentAttributeIndex].GetComponent<ParticleSystem>();
            if (chargingEffect != null)
            {
                chargingEffect.Play();
            }
        }
    }

    // ��¡ �� ��ų �߻�
    public void ReleaseAndShootSkill()
    {
        isCharging = false; // ��¡ ����
        lastSkillTime = Time.time; // ��ų ��� �ð� ���             
        anim.SetBool("isCharging", false); // ��¡ �ִϸ��̼� ����
        playerMovement.moveSpeed *= 2f; // �̵��ӵ� ����

        float chargePercent = Mathf.Clamp01(currentChargeTime / maxChargeTime); // 0�� 1 ���� ��
        float skillArrowSpeed = Mathf.Lerp(ArrowSpeed, maxArrowSpeed, chargePercent); // ȭ�� �ӵ� ����

        // ��¡ ���ʽ� ������ ��� (-5, 0, 5, 10, 15 �ܰ�)
        int chargeBonus = CalculateDamage(chargePercent);

        // WeaponManager�� ���� ������ �������� ������
        int weaponDamage = weaponManager.GetCurrentWeaponDamage();
        int finalDamage = weaponDamage + chargeBonus;

        // ��¡ ����Ʈ ����
        if (chargingAttributeEffect[currentAttributeIndex] != null)
        {
            ParticleSystem chargingEffect = chargingAttributeEffect[currentAttributeIndex].GetComponent<ParticleSystem>();
            if (chargingEffect != null)
            {
                chargingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        // ��ų �߻�ü ���� �� �߻�

        // �Ҵк� ����Ʈ ����
        if (attackEffect != null)
        {
            attackEffect.Play();
        }

        GameObject skillArrow = Instantiate(skillArrowPrefab[currentAttributeIndex], ArrowPos.position, ArrowPos.rotation);
        Rigidbody rb = skillArrow.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = ArrowPos.forward * skillArrowSpeed; // ��ų �߻�ü �ӵ� ����
        }

        // �÷��̾�� ȭ���� �浹�� ����
        Collider playerCollider = GetComponent<Collider>();  // �÷��̾��� �ݶ��̴� ��������
        Collider arrowCollider = skillArrow.GetComponent<Collider>();  // ȭ���� �ݶ��̴� ��������
        if (playerCollider != null && arrowCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, arrowCollider);
        }

        // ���� ������ ����
        skillArrow.GetComponent<Arrow>().SetDamage(finalDamage);

        Invoke("EndAttack", 0.7f); // 0.7�� �� ���� ����
    }
    private int CalculateDamage(float chargePercent)
    {
        if (chargePercent >= 1.0f)
        {
            return 15;
        }
        else if (chargePercent >= 0.75f)
        {
            return 10;
        }
        else if (chargePercent >= 0.5f)
        {
            return 5;
        }
        else if (chargePercent >= 0.25f)
        {
            return 0;
        }
        else
        {
            return -5;
        }
    }

    // ���� ����
    private void EndAttack()
    {    
        playerMovement.SetRangedAttackState(false);  // ���Ÿ� ���� ���� ����
        playerMovement.SetAttackState(false); // ���� ���� ����
        isFireReady = true; // ���� ���� ���·� ��ȯ
    }
    public void HandleAttributeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchAttributeEffect(0); // 1�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchAttributeEffect(1); // 2, 5�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchAttributeEffect(2); // 3, 6�� �Ӽ�
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchAttributeEffect(3); // 4, 7�� �Ӽ�
        }
    }

    private void SwitchAttributeEffect(int attributeIndex)
    {
        // ������ �Ѵ� �ε����� �Էµ��� �ʵ��� ���� ó��
        if (attributeIndex >= chargingAttributeEffect.Length || attributeIndex >= skillArrowPrefab.Length || attributeIndex >= ArrowPrefab.Length)
            return;

        // ��� �Ӽ� ���� ����Ʈ�� ��Ȱ��ȭ�ϰ� ������ �Ӽ� ���� ����Ʈ�� Ȱ��ȭ
        for (int i = 0; i < chargingAttributeEffect.Length; i++)
        {
            chargingAttributeEffect[i].SetActive(i == attributeIndex);
            ArrowPrefab[i].SetActive(i == attributeIndex);
            skillArrowPrefab[i].SetActive(i == attributeIndex);
        }

        // ���� �Ӽ� �ε����� ������Ʈ
        currentAttributeIndex = attributeIndex;
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
