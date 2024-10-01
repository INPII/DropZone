using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Normal,
        Ice,        // ���ο�
        Gunpowder   // ����ź
    }

    public bool isPiercing; // ����ź ����
    public BulletType bulletType; // �߻�ü�� �Ӽ�
    public float explosionRadius = 3f; // ����ź�� �ݰ�
    public int damage; // �Ѿ��� ������ ��
        
    public float lifeTime; // �Ѿ��� ���� �ð�
    public GameObject shooter; // �߻�ü�� �߻��� ��ü (�ڱ� �ڽ��� �ĺ��ϱ� ���� ����)

    public GameObject explosionEffectPrefab; // ���� ����Ʈ ������
    public float effectOffset = 1.5f; // ����Ʈ ��ġ ������ ���� ������

    public Color normalColor = Color.yellow;
    public Color iceColor = Color.blue;
    public Color gunpowderColor = Color.red;

    private Renderer bulletRenderer;

    void Awake()
    {
        bulletRenderer = GetComponent<Renderer>(); // �߻�ü�� �������� ������
    }

    void Start()
    {
        // �߻�ü�� �Ӽ��� ���� �ʱ� ����
        ApplyBulletProperties();       

        // ���� �ð��� ������ �Ѿ� �ı�
        Destroy(gameObject, lifeTime);

    }

    private void ApplyBulletProperties()
    {
        // �߻�ü�� �Ӽ��� ���� ���� ����
        switch (bulletType)
        {
            case BulletType.Normal:
                bulletRenderer.material.color = normalColor;
                break;
            case BulletType.Ice:
                bulletRenderer.material.color = iceColor;
                break;
            case BulletType.Gunpowder:
                bulletRenderer.material.color = gunpowderColor;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ��뿡�� �������� ����
        PlayerStatus enemyStatus = other.gameObject.GetComponent<PlayerStatus>();

        if (enemyStatus != null && other.gameObject != shooter) // �ڱ� �ڽ��� ����
        {
            // ������ ����
            enemyStatus.TakeDamage(damage);
            Debug.Log($"�Ѿ� ������ {damage} �����");
        }

        // Ư�� ȿ�� ����
        if (bulletType == BulletType.Ice && other.gameObject.CompareTag("Player"))
        {
            // ���ο� ȿ�� ����
            PlayerStatus targetStatus = other.gameObject.GetComponent<PlayerStatus>();
            if (targetStatus != null)
            {
                targetStatus.ApplyStatusEffect(PlayerStatus.StatusEffect.Slow2);
            }
        }
        else if (bulletType == BulletType.Gunpowder)
        {
            // ����ź ȿ�� ����
            Explode();
        }
                
        // ����ź�� �ƴ� ��쿡�� �ı�
        if (isPiercing)
        {
            return;
        }

        if (other.gameObject.CompareTag("Wall") || bulletType != BulletType.Gunpowder)
        {
            Destroy(gameObject);
        }

    }

    private void Explode()
    {
        Vector3 effectPosition = transform.position + transform.forward * effectOffset;

        // ���� ����Ʈ ����
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, effectPosition, Quaternion.identity);

            // ����Ʈ�� ���� �ð� �Ŀ� �ı��ǵ��� ���� 
            Destroy(explosionEffect, 1f); // ���� ����Ʈ�� 1�� �� �ı��ǵ��� ����
        }

        // ����ź�� ȿ���� �����ϴ� ����
        Collider[] hitColliders = Physics.OverlapSphere(effectPosition, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            PlayerStatus targetStatus = hitCollider.GetComponent<PlayerStatus>();
            if (targetStatus != null && hitCollider.gameObject != shooter) // �ڱ� �ڽ��� ����
            {
                targetStatus.TakeDamage(10); // ����ź�� ������
            }
        }

        // ���� �� �߻�ü �ı�
        if (!isPiercing)
        {
            Destroy(gameObject);
        }
    }
        
}