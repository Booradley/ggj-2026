using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantPot : MonoBehaviour, IInteractable
{
    private GameController _gameController;
    private Mask _mask;
    private PlantPotState _plantPotState;
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
        _plantPotState = _gameController.RegisterPlantPot(this);

        _activityGoalIndicator = GameObject.Instantiate(activityGoalIndicatorPrefab);
        _activityGoalIndicator.transform.position = this.transform.position + (Camera.main.transform.rotation * activityGoalIndicatorOffset);
        _activityGoalIndicator.SetActive(false);
    }

    public void OnInteract()
    {
        _gameController.OnPlantPotInteraction(this);
    }

    public void OnPlantSeed()
    {
        _mask = GameObject.Instantiate(_plantPotState.PlantData.plantPrefab).GetComponent<Mask>();
        _mask.transform.position = this.transform.position + plantOffset;
        _mask.transform.rotation = this.transform.rotation;

        UpdatePlant(0);
        UpdateActivityGoal(0, 0);
    }

    public void UpdatePlant(int growthStage)
    {
        _mask.SetIndex(growthStage, _plantPotState.MaskState.GetIndex(growthStage));
        _mask.transform.position = this.transform.position + plantOffset + new Vector3(0, _plantPotState.PlantData.growthStages[growthStage].yOffset, 0);
    }

    public void UpdateActivityGoal(int growthStage, int activityGoalIndex)
    {
        _activityGoalIndicator.SetActive(true);

        ActivityType activityType = _plantPotState.PlantData.growthStages[growthStage].activityGoals[activityGoalIndex];
        VisualElement root = _activityGoalIndicator.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
        VisualElement image = root.Q<VisualElement>("Icon");

        root.style.backgroundColor = new StyleColor(_gameController.GetActivityGoalTint(activityType));
        image.style.backgroundImage = new StyleBackground(_gameController.GetActivityGoalTexture(activityType));
        image.style.color = new StyleColor(_gameController.GetActivityGoalTint(activityType));

        Debug.Log(root.style.backgroundColor);
    }

    public void OnPlantReady()
    {
        _activityGoalIndicator.SetActive(false);
        _mask.transform.position = this.transform.position + plantOffset;
    }

    public void OnHarvestPlant()
    {
        GameObject.Destroy(_mask);
        _mask = null;
    }
}