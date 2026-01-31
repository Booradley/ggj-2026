using UnityEngine;

public class PlantPot : MonoBehaviour, IInteractable
{
    private GameController _gameController;

    public InteractableType InteractableType => InteractableType.PlantPot;

    public Transform Transform => this.transform;

    void Awake()
    {
        _gameController = GameObject.FindFirstObjectByType<Main>().GameController;
    }

    void Start()
    {
        _gameController.RegisterPlantPot(this);
    }

    public void OnInteract()
    {
        _gameController.OnPlantPotInteraction(this);
    }

    public void PlantSeed(PlantData plantData)
    {
        
    }

    public void UpdatePlant(int growthStage)
    {
        
    }

    public void UpdateActivityGoal(int growthStage, int activityGoalIndex)
    {
        
    }

    public void HarvestPlant()
    {
        
    }
}