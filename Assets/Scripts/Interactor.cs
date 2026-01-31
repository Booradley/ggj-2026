using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private InputAction _interactAction;

    public InputActionAsset actions;

    private List<IInteractable> _validInteractables = new List<IInteractable>();

    private void Awake()
    {
        _interactAction = actions.FindActionMap("Player").FindAction("Interact");
        _interactAction.performed += OnInteractPerformed;
    }

    private void OnEnable()
    {
        _interactAction.Enable();
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteractPerformed;
        _interactAction.Disable();
    }

    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        IInteractable closestInteractable = GetClosestInteractable();
        closestInteractable?.OnInteract();
    }

    void Update()
    {
        IInteractable closestInteractable = GetClosestInteractable();
        if (closestInteractable != null)
        {
            
        } 
        else
        {
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (!_validInteractables.Contains(interactable))
            {
                _validInteractables.Add(interactable);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _validInteractables.Remove(interactable);
        }
    }

    private IInteractable GetClosestInteractable()
    {
        IInteractable closestInteractable = null;
        float closestDistance = math.INFINITY;

        foreach (IInteractable interactable in _validInteractables)
        {
            if (interactable.InteractableType == InteractableType.PlantPot)
            {
                float distance = (this.transform.position - interactable.Transform.position).magnitude;
                if (closestInteractable == null || distance < closestDistance)
                {
                    closestInteractable = interactable;
                    closestDistance = distance;
                }
            }
        }

        return closestInteractable;
    }
}
