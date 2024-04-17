using UnityEngine;

public class coldbullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.FreezePlayer(1.0f);  
            }
            Destroy(gameObject);  // 命中后销毁特殊子弹
        }
    }
}
