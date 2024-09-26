using TMPro;
using UnityEngine;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float defaultSpeed; // �⺻ �̵� �ӵ�
    public float moveSpeed;
    public float dashSpeedMultiplier = 2.0f; // �뽬�� �� �ӵ� ����
    public float dashDuration = 0.3f; // �뽬 ���� �ð�
    public float dashCooldown; // �뽬 ��Ÿ��

    public bool canMove = true; // �̵� ���� ����
    public bool canDash = true; // �뽬 ���� ����
    public bool isKnockbackImmune = false; // �˹� �鿪 ���� ����

    private float hAxis;
    private float vAxis;
    public bool isDash; // �뽬 ���� Ȯ��
    private bool isAttack; // ���� ���� Ȯ��
    private bool isRangedAttack; // ���Ÿ� ���� ���� Ȯ��

    public float lastDashTime = -100f;  // ������ �뽬 �ð�
    private Vector3 moveVec;
    private Vector3 rayVec;

    // ���� ��� ���� PlayerStatus ����
    private PlayerStatus playerStatus;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // �� �մ� ���� ó�� ����
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        anim = GetComponent<Animator>();
        moveSpeed = defaultSpeed; // �⺻ �̵� �ӵ��� �ʱ�ȭ

        playerStatus = GetComponent<PlayerStatus>(); // PlayerStatus ������Ʈ ��������
                                                     // UIManager ��ũ��Ʈ�� ã�� ����



        // PhotonView�� �ʿ�: PhotonView�� �߰��Ͽ� �÷��̾��� ��ġ �� �̵� ������ ��Ʈ��ũ �� ����ȭ�ؾ� ��
    }

    void Update()
    {
        GetInput(); // �Է� �ޱ�
        Turn(); // ȸ�� ó��


        // �� ������ Ray ����
        RaycastHit hit;
        float rayDistance = 5.0f; // ĳ���� �տ� Ray�� �� �Ÿ�

        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);

        // ����׿� Ray �׸���
        Debug.DrawRay(rayOrigin, rayVec * rayDistance, Color.red);

        // �� �κп� PhotonView.IsMine üũ�� �ʿ�: ���� �÷��̾ �̵��� ó���ϵ��� �ؾ� ��
    }

    void FixedUpdate()
    {
        Move(); // �̵� ó��       
    }

    public void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // �뽬�� ���� �߿��� �Է��� �ٸ� Ŭ���̾�Ʈ���� ���ĵǾ�� �ϹǷ�, RPC�� �뽬 �Է��� �����ؾ� ��
        // PhotonView�� ���� Dash ���� �Է��� RPC�� ������ �ʿ䰡 ����
        if (Input.GetButtonDown("Fire3") && (playerStatus.currentStatus == PlayerStatus.StatusEffect.None || playerStatus.currentStatus == PlayerStatus.StatusEffect.SuperArmor) && !isAttack)
        {
            StartDash();
            //Dash();
            // StartDash�� ���� ���۵� �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� ��
        }
    }

    public void Move()
    {
        if (!canMove) return;

        if (isRangedAttack) { return; } // ���Ÿ� ���� ���� ���� �̵� �Ұ�

        // ���� ���� �� �̵� �ӵ��� ������ ����
        float adjustedMoveSpeed = isAttack ? moveSpeed / 2 : moveSpeed;

        // ���¿� ���� �̵� �Ұ� ó�� (�ӹ� ������ ���)
        if (playerStatus.currentStatus == PlayerStatus.StatusEffect.Immobilized)
        {
            adjustedMoveSpeed = 0;
        }

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // �� ������ Ray ����
        RaycastHit hit;
        float rayDistance = 1.0f; // ĳ���� �տ� Ray�� �� �Ÿ�

        // ĳ���� �ٴں��� ������ ray�� ��� ���� ��ġ ����
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        // ����׿� Ray �׸���
        Debug.DrawRay(rayOrigin, moveVec * rayDistance, Color.red);

      

        // �̵� �������� Ray�� ��
        if (Physics.Raycast(rayOrigin, moveVec, out hit, rayDistance))
        {
            // ���� �����Ǹ� �̵� ���� (���� "Wall" �±׸� ���� ���)
            if (hit.collider.CompareTag("Untagged"))
            {
                return; // �̵� ����
            }
        }

        //transform.position += moveVec * adjustedMoveSpeed * Time.deltaTime; // ���� ��� ����
        rb.MovePosition(transform.position + moveVec * adjustedMoveSpeed * Time.deltaTime); // Rigidbody�� ���� �̵� ó��

        anim.SetBool("isRun", moveVec != Vector3.zero); // �̵� �ִϸ��̼� ����
    }

    void Dash()
    {
        Vector3 dashPower = transform.forward * 3.0f;
        rb.AddForce(dashPower, ForceMode.VelocityChange);
    }

    private void StartDash()
    {
        if (Time.time >= lastDashTime + dashCooldown && moveVec != Vector3.zero && !isAttack)
        {
            isDash = true;
            moveSpeed = moveSpeed * dashSpeedMultiplier; // �뽬 �� �ӵ� ����
            lastDashTime = Time.time;

            anim.SetTrigger("doDash"); // �뽬 �ִϸ��̼� ����            

            // �뽬 ���۵� ��Ʈ��ũ �� ����ȭ�ؾ� �ϹǷ� PhotonView�� ���� RPC ȣ�� �ʿ�
            Invoke("EndDash", dashDuration); // �뽬 ���� �ð��� ������ ����
        }
    }

    private void EndDash()
    {
        isDash = false;
        moveSpeed = moveSpeed / dashSpeedMultiplier; // �뽬�� ������ ���� �ӵ��� ����
        // �뽬 ���ᵵ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�� �ʿ��ϹǷ� PhotonView�� RPC�� ���
    }

    public void Turn()
    {
        // ����, �뽬 ���� ���� ȸ������ ����
        if (isAttack || isDash) return;

        // �̵� ���� ���� �̵� �������� ȸ��
        if (moveVec != Vector3.zero && !isAttack)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // ȸ�� ���µ� ����ȭ�� �ʿ��� ��� RPC�� ȸ�� ���� ������ �� ����
    }

    // ������ �� ���콺 �������� ȸ��
    public void TurnTowardsMouse()
    {
        // ���콺 �������� ��� ȸ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // ĳ������ ����(y)�� ����

            // ��� ȸ��
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
        }

        // ���콺 �������� ȸ���ϴ� ���۵� ����ȭ�� �ʿ��� �� ����
    }

    // ���� ���¸� ������Ʈ�ϴ� �޼��� (�ܺο��� ȣ�� ����)
    public void SetAttackState(bool isAttacking)
    {
        isAttack = isAttacking;
    }

    // ���Ÿ� ���� ���¸� ������Ʈ�ϴ� �޼��� �߰�
    public void SetRangedAttackState(bool isRangedAttacking)
    {
        isRangedAttack = isRangedAttacking;
    }

}
