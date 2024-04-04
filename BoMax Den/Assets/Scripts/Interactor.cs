using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public interface IInteractable
    {
        void Interact();
    }

    public Transform InteractorSource;
    public float InteractRange;
    public float InteractionCooldown = 1f; // Cooldown time in seconds
    public InteractUI interactUI;

    private bool interactableFound;
    private float lastInteractionTime;

    void Update()
    {
        CheckForInteractableObjects();

        // Show or hide UI based on interactableFound
        if (interactableFound)
        {
            interactUI.ShowUI();
        }
        else
        {
            interactUI.HideUI();
        }

        if (InputManager.isInteractInput && interactableFound && CanInteract())
        {
            // Handle interact input if an interactable object is found and cooldown is over
            InteractWithClosestObject();
        }
    }

    void CheckForInteractableObjects()
    {
        RaycastHit[] hits = Physics.SphereCastAll(InteractorSource.position, InteractRange, InteractorSource.forward, 0f);
        interactableFound = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactableFound = true;
                break;
            }
        }
    }

    bool CanInteract()
    {
        return Time.time - lastInteractionTime >= InteractionCooldown;
    }

    void InteractWithClosestObject()
    {
        RaycastHit[] hits = Physics.SphereCastAll(InteractorSource.position, InteractRange, InteractorSource.forward, 0f);
        float closestDistance = Mathf.Infinity;
        IInteractable closestInteractable = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                float distance = Vector3.Distance(InteractorSource.position, hit.collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactObj;
                }
            }
        }

        if (closestInteractable != null)
        {
            closestInteractable.Interact();
            lastInteractionTime = Time.time;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InteractorSource.position, InteractRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(InteractorSource.position, InteractorSource.position + InteractorSource.forward * InteractRange);
    }
}