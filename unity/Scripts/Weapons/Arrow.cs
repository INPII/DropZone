using UnityEngine;
using static Bullet;

public class Arrow : MonoBehaviour
{
    public int damage;  // �⺻ ������
    public float lifeTime = 1f;  // ȭ���� �ı��Ǳ������ �ð�
    public GameObject hitEffectPrefab;  // �浹 �� ��Ÿ�� ����Ʈ ������
    public float effectOffset = -1f; // ����Ʈ ��ġ ������ ���� ������
    
    public enum ArrowType
    {
        Normal,
        Ice,        // ���ο�
        Gunpowder,   // ����ź
        Gear
    }

    public ArrowType arrowType;

    private void Start()
    {
        // ���� �ð��� ������ ȭ���� �ı�
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    // Ʈ���� �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        

        // �浹�� ������Ʈ�� �÷��̾��� �� �������� ó��
        var player = other.GetComponent<PlayerStatus>();
        if (player != null)
        {
            player.TakeDamage(damage); // �÷��̾��� ü�¿��� �������� ����
        }

        // arrow�Ӽ��� ���� ȿ�� �����ϱ�
        if (arrowType == ArrowType.Ice && other.gameObject.CompareTag("Player"))
        {
            // ���ο� ȿ�� ����
            PlayerStatus targetStatus = other.gameObject.GetComponent<PlayerStatus>();
            if (targetStatus != null)
            {
                targetStatus.ApplyStatusEffect(PlayerStatus.StatusEffect.Slow2);
            }
        }

        else if (arrowType == ArrowType.Gunpowder && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gunpowder");
        }





        // �浹 ����Ʈ�� ����
        if (hitEffectPrefab != null)
        {
            Vector3 effectPosition = transform.position + transform.forward * effectOffset;
            GameObject collisionEffect = Instantiate(hitEffectPrefab, effectPosition, Quaternion.identity);  // ������ ��ġ�� ����Ʈ ����

            Destroy(collisionEffect, lifeTime);
            
        }

        // ȭ���� �÷��̾ ��, ���� ������ �ı�
        Destroy(gameObject);
    }
}
