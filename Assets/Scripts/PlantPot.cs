using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantPot : MonoBehaviour, IInteractable
{
    private GameController _gameController;
    private GameObject _mask;
    private PlantData _plantData;
    private PlantState _plantState;
    private GameObject _activityGoalIndicator;

    public InteractableType InteractableType => InteractableType.PlantPot;

    public Transform Transform => this.transform;

    public Vector3 interactionPromptUIOffset;

    public Vector3 InteractionPromptOffset { get => interactionPromptUIOffset; }

    public GameObject activityGoalIndicatorPrefab;

    public Vector3 activityGoalIndicatorOffset;

    public bool CanInteract { get => true; }

    public Vector3 plantOffset;

    void Awake()
    {
        _gameController = GameObject.FindFirstObjectByType<Main>().GameController;
    }

    void Start()
    {
        _plantState = _gameController.RegisterPlantPot(this);

        _activityGoalIndicator = GameObject.Instantiate(activityGoalIndicatorPrefab);
        _activityGoalIndicator.transform.position = this.transform.position + (Camera.main.transform.rotation * activityGoalIndicatorOffset);
        _activityGoalIndicator.SetActive(false);
    }

    public void OnInteract()
    {
        _gameController.OnPlantPotInteraction(this);
    }

    public void PlantSeed(PlantData plantData)
    {
        _plantData = plantData;

        _mask = GameObject.Instantiate(plantData.plantPrefab);
        _mask.transform.position = this.transform.position + plantOffset;
        _mask.transform.rotation = this.transform.rotation;

        UpdatePlant(0);
        UpdateActivityGoal(0, 0);
    }

    public void UpdatePlant(int growthStage)
    {
        _mask.transform.position = this.transform.position + plantOffset + new Vector3(0, _plantData.growthStages[growthStage].yOffset, 0);
    }

    public void UpdateActivityGoal(int growthStage, int activityGoalIndex)
    {
        _activityGoalIndicator.SetActive(true);

        VisualElement image = _activityGoalIndicator.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Icon");
        image.style.backgroundImage = new StyleBackground(_gameController.GetActivityGoalTexture(_plantData.growthStages[growthStage].activityGoals[activityGoalIndex]));
    }

    public void OnPlantReady()
    {
        _activityGoalIndicator.SetActive(false);
        _mask.transform.position = this.transform.position + plantOffset;
    }

    public void HarvestPlant()
    {
        GameObject.Destroy(_mask);

        _plantData = null;
        _mask = null;
    }
}