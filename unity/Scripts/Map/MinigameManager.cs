using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public Rigidbody player;
    public Transform playerPosition;
    public Vector3 roundOnePos;
    public Vector3 roundTwoPos;
    public Vector3 roundThreePos;
    public Vector3 roundFourPos;
    public Vector3 roundFivePos;
    private int level;
    private int roundOneNum;
    private int roundTwoNum;
    private int roundThreeNum;
    private int roundFourNum;

    void Start()
    {
        level = 0;
        roundOneNum = Random.Range(0, 3);
        roundTwoNum = Random.Range(0, 3);
        roundThreeNum = Random.Range(0, 3);
        roundFourNum = Random.Range(0, 3);

        Debug.Log(roundOneNum);
        Debug.Log(roundTwoNum);
        Debug.Log(roundThreeNum);
        Debug.Log(roundFourNum);
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            int portalNum = int.Parse(other.gameObject.name);

            if (level == 0)
            {
                // ��Ż��ȣ�� ������ȣ ��
                // ���� ��Ż�̶��
                if (portalNum == roundOneNum)
                {
                    level++;
                    Debug.Log(level);
                    playerPosition.position = roundTwoPos;
                }
                // Ʋ�� ��Ż�̶��
                else
                {
                    playerPosition.position = roundOnePos;
                }
            }
            else if (level == 1)
            {
                // ��Ż��ȣ�� ������ȣ ��
                // ���� ��Ż�̶��
                if (portalNum == roundTwoNum)
                {
                    level++;
                    Debug.Log(level);
                    playerPosition.position = roundThreePos;
                }
                // Ʋ�� ��Ż�̶��
                else
                {
                    playerPosition.position = roundTwoPos;
                }
            }
            else if (level == 2)
            {
                // ��Ż��ȣ�� ������ȣ ��
                // ���� ��Ż�̶��
                if (portalNum == roundThreeNum)
                {
                    level++;
                    Debug.Log(level);
                    playerPosition.position = roundFourPos;
                }
                // Ʋ�� ��Ż�̶��
                else
                {
                    playerPosition.position = roundThreePos;
                }
            }
            else
            {
                // ��Ż��ȣ�� ������ȣ ��
                // ���� ��Ż�̶��
                if (portalNum == roundFourNum)
                {
                    level++;
                    Debug.Log(level);
                    playerPosition.position = roundFivePos;
                }
                // Ʋ�� ��Ż�̶��
                else
                {
                    playerPosition.position = roundFourPos;
                }
            }
        }
    }
}
