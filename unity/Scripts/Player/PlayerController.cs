using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerStatus playerStatus; // PlayerStatus ����
    public IAttack playerAttack; // ���� �������̽��� ���� ��ũ��Ʈ ����
    public WeaponManager weaponManager; // WeaponManager ����    

    void Awake()
    {
        playerAttack = GetComponent<IAttack>(); // �������� ���� ��ũ��Ʈ�� �Ҵ�
        // PhotonView�� �ʿ�: PhotonView�� �Ҵ��Ͽ� ��Ʈ��ũ �󿡼� �� �÷��̾ �� ������ Ȯ���ϴ� �뵵�� ���
    }

    void Update()
    {
        //�÷��̾ ���� ������ ���� �ƹ� ���۵� ���� ����
        if (playerStatus.currentStatus == PlayerStatus.StatusEffect.Dead)
        {
            return;
        }

        // �� �κп� PhotonView.IsMine üũ�� �ʿ���: ���� �÷��̾ �ƴ� ��� �̵��� ���� ���� �Է��� ���� �ʵ��� ó��
        playerMovement.GetInput(); // �̵� �Է� �ޱ�
        playerMovement.Move(); // �̵�        

        // ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� RPC�� ����
        // �� �κп� PhotonView�� ���� RPC ȣ���� �ʿ���
        playerAttack.GetAttackInput(Input.GetButton("Fire1")); // ���� �Է�

        // ���� ���浵 �ٸ� Ŭ���̾�Ʈ�� ����ȭ�ؾ� �ϹǷ� RPC�� ����
        // ���� ���濡 ���� ó���� PhotonView�� ���� ����ȭ�� �ʿ���
        weaponManager.HandleWeaponSwitch(); // ���� ���� �� ����Ʈ ���� ó��       
    }
}
