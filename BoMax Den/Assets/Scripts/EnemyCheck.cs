using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float obstacleDetectionDistance = 1.5f;
    public float obstacleAvoidanceDistance = 2f;

    void Update()
    {
        // ���߼�⣬�ж�ǰ���Ƿ����ϰ���
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, obstacleDetectionDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // �����⵽�ϰ��ѡ�����ҷ����ƹ�
                Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * transform.forward;
                Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * transform.forward;

                // ������ҷ���ѡ��û���ϰ���ķ���
                bool canMoveLeft = !Physics.Raycast(transform.position, leftDirection, obstacleAvoidanceDistance);
                bool canMoveRight = !Physics.Raycast(transform.position, rightDirection, obstacleAvoidanceDistance);

                // �������ѡ���ƶ�����
                if (canMoveLeft && !canMoveRight)
                {
                    MoveInDirection(leftDirection);
                }
                else if (canMoveRight && !canMoveLeft)
                {
                    MoveInDirection(rightDirection);
                }
                // ������Ҷ����ϰ��ֹͣ�ƶ�����ѡ�������߼�
                else
                {
                    StopMoving();
                }
            }
        }
        else
        {
            // ���û���ϰ��������ǰ�ƶ�
            MoveForward();
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        // �ڸ����ķ������ƶ�
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void MoveForward()
    {
        // ��ǰ�ƶ�
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    void StopMoving()
    {
        // ֹͣ�ƶ�����ִ�������߼�
    }
}

