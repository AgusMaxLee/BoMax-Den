using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuButtonInteraction : MonoBehaviour
{
    private GraphicRaycaster raycaster;

    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            // Create a pointer event data with the current event system
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            // Create a list to store raycast results
            List<RaycastResult> results = new List<RaycastResult>();

            // Perform the raycast
            raycaster.Raycast(eventData, results);

            // Check if any UI elements were hit
            if (results.Count > 0)
            {
                // Loop through the results to find the clicked button
                foreach (RaycastResult result in results)
                {
                    Button clickedButton = result.gameObject.GetComponent<Button>();
                    if (clickedButton != null)
                    {
                        // Check which button was clicked and call the corresponding method
                        if (clickedButton.CompareTag("PlayButton"))
                        {
                            OnPlayButtonClick();
                        }
                        else if (clickedButton.CompareTag("QuitButton"))
                        {
                            OnQuitButtonClick();
                        }
                        break; // Exit loop after handling the click
                    }
                }
            }
        }
    }

    void OnPlayButtonClick()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene("MainTest");
    }

    void OnQuitButtonClick()
    {
        Debug.Log("Quit Button Clicked!");
        Application.Quit();
    }
}
