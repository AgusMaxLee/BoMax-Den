using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float obstacleDetectionDistance = 1.5f;
    public float obstacleAvoidanceDistance = 2f;

    void Update()
    {
        // 射线检测，判断前方是否有障碍物
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, obstacleDetectionDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // 如果检测到障碍物，选择左右方向绕过
                Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * transform.forward;
                Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * transform.forward;

                // 检测左右方向，选择没有障碍物的方向
                bool canMoveLeft = !Physics.Raycast(transform.position, leftDirection, obstacleAvoidanceDistance);
                bool canMoveRight = !Physics.Raycast(transform.position, rightDirection, obstacleAvoidanceDistance);

                // 根据情况选择移动方向
                if (canMoveLeft && !canMoveRight)
                {
                    MoveInDirection(leftDirection);
                }
                else if (canMoveRight && !canMoveLeft)
                {
                    MoveInDirection(rightDirection);
                }
                // 如果左右都有障碍物，停止移动或者选择其他逻辑
                else
                {
                    StopMoving();
                }
            }
        }
        else
        {
            // 如果没有障碍物，继续向前移动
            MoveForward();
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        // 在给定的方向上移动
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void MoveForward()
    {
        // 向前移动
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    void StopMoving()
    {
        // 停止移动或者执行其他逻辑
    }
}

