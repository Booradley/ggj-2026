public enum InteractableType
{
    PlantPot,
}

public interface IInteractable
{
    public InteractableType InteractableType { get; }

    public void OnInteract();
}