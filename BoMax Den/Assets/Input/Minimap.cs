using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private RectTransform rect;
    private Transform player;
    private static Image item;
    private Image playerImage;
    void Start()
    {
        item = Resources.Load<Image>("Image");
        rect = GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
            playerImage = Instantiate(item);
    }


    
    void Update()
    {
        
    }
    private void ShowPlayer()
    {
        playerImage.rectTransform.sizeDelta = new Vector2(20, 20);
        playerImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        playerImage.rectTransform.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
        playerImage.sprite = Resources.Load<Sprite>("Input/Textrue/player");
        playerImage.transform.SetParent(transform, false);
    }

}
