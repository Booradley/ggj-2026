using UnityEngine;

public enum InteractableType
{
    PlantPot,
}

public interface IInteractable
{
    public InteractableType InteractableType { get; }

    public Transform Transform { get; }

    public Vector3 InteractionPromptOffset { get; }

    public bool CanInteract { get; }

    public void OnInteract();
}