using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private List<IInteractable> _validInteractables = new List<IInteractable>();

    void Update()
    {
        foreach (IInteractable interactable in _validInteractables)
        {
            if (interactable.InteractableType == InteractableType.PlantPot)
            {

            }
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
}
