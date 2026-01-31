using UnityEngine;

public enum InteractableType
{
    PlantPot,
}

public interface IInteractable
{
    public InteractableType InteractableType { get; }

    public Transform Transform { get; }

    public void OnInteract();
}