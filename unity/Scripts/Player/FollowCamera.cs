using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform; // Player�� ã�� ī�޶� ����
    }

    // Update is called once per frame
    void Update()
    {
        // ��ǥ ��ġ�� ���: ĳ������ ��ġ + ������(��ܰ� �������� ����)
        Vector3 desiredPosition = target.position + offset;

        // ī�޶��� ���� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶��� ��ġ�� ������Ʈ
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
