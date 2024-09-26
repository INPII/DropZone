using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestSpawner : MonoBehaviour
{
    // ���� ��������
    public List<Transform> spawnSpots;
    // �ڿ�
    public static Resources resources;

    // ����
    public GameObject chest;
    public static ChestSpawner instance = null;

    private void Awake()
    {
       if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static ChestSpawner Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Start()
    {
        Chest _chest = chest.GetComponent<Chest>();
        // �������� ����
        for (int i = spawnSpots.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            // ����Ʈ�� i��° ��ҿ� j��° ��Ҹ� ��ȯ

            Transform temp = spawnSpots[i];
            spawnSpots[i] = spawnSpots[j];
            spawnSpots[j] = temp;
        }

        for (int i = 0; i < 20; i++)
        {
            Instantiate(chest, spawnSpots[i]);
        }
    }
}
