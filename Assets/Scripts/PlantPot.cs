using UnityEngine;

public class PlantPot : MonoBehaviour, IInteractable
{
    public InteractableType InteractableType => InteractableType.PlantPot;

    public void OnInteract()
    {

    }
}