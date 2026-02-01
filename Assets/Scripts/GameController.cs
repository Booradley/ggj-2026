using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public class GameController
{
    private Main _main;
    private List<PlantPotState> _plantStates = new();
    private List<ActivityVolume> _activityVolumes = new();
    private PlantPot _heldPlantPot;

    public void Initialize(Main main)
    {
        _main = main;

        ActivityVolume[] activityVolumes = GameObject.FindObjectsByType<ActivityVolume>(FindObjectsSortMode.None);
        foreach (ActivityVolume activityVolume in activityVolumes)
        {
            _activityVolumes.Add(activityVolume);
        }
    }

    public void Update()
    {
        foreach (PlantPotState plantState in _plantStates)
        {
            plantState.Update();
        }
    }

    public PlantPotState RegisterPlantPot(PlantPot plantPot)
    {
        PlantPotState plantState = new PlantPotState(this, plantPot);
        _plantStates.Add(plantState);
        return plantState;
    }

    public void OnInteract()
    {
        AudioManager.Instance.PlayInteractSound();

        // Drop pot if holding one
        if (_heldPlantPot)
        {
            DropHeldPlant();
        }
    }

    private void DropHeldPlant()
    {
        if (!_heldPlantPot) return;

        _heldPlantPot.gameObject.transform.localPosition = new Vector3(0, 0, 0.75f);
        _heldPlantPot.gameObject.transform.SetParent(null);
        _heldPlantPot = null;
    }

    public void OnPlantPotInteraction(PlantPot plantPot)
    {
        AudioManager.Instance.PlayInteractSound();

        foreach (PlantPotState plantState in _plantStates)
        {
            if (plantState.PlantPot == plantPot)
            {
                if (_heldPlantPot)
                {
                    DropHeldPlant();
                }
                else
                {
                    if (plantState.CanPlant)
                    {
                        plantState.PlantSeed(_main.plantDatas[0]);
                    }
                    else if (plantState.CanHarvest)
                    {
                        MaskWearer maskWearer = GameObject.FindFirstObjectByType<MaskWearer>();
                        maskWearer.WearMask(plantState);

                        plantState.HarvestPlant();
                    }
                    else
                    {
                        Interactor interactor = GameObject.FindFirstObjectByType<Interactor>();
                        plantPot.gameObject.transform.SetParent(interactor.gameObject.transform);
                        plantPot.gameObject.transform.localPosition = new Vector3(0, 0.6f, 0.75f);
                        _heldPlantPot = plantPot;
                    }
                }

                return;
            }
        }
    }

    public Texture2D GetActivityGoalTexture(ActivityType activityType)
    {
        return _main.GetActivityGoalTexture(activityType);
    }

    public Color GetActivityGoalTint(ActivityType activityType)
    {
        return _main.GetActivityGoalTint(activityType);
    }

    public Texture2D[] GetTextures(int growthStage)
    {
        return _main.GetTextures(growthStage);
    }

    private List<ActivityType> _activityTypeResult = new();

    public List<ActivityType> GetActivityTypesFor(PlantPotState plantPotState)
    {
        _activityTypeResult.Clear();

        foreach (ActivityVolume activityVolume in _activityVolumes)
        {
            if (activityVolume.IsAffecting(plantPotState.PlantPot.gameObject))
            {
                _activityTypeResult.Add(activityVolume.activityType);
            }
        }

        return _activityTypeResult;
    }
}

public class MaskState
{
    private int[] _features = new int[]{0, 0, 0, 0};
    public int[] Features { get => _features; }

    private GameController _gameController;
    private PlantData _plantData;

    public MaskState(GameController gameController, PlantData plantData)
    {
        _gameController = gameController;
        _plantData = plantData;
    }

    public int GetIndex(int growthStage)
    {
        return _features[growthStage];
    }

    public void SetIndex(int growthStage, int index)
    {
        _features[growthStage] = index;
    }

    public void PlayMaskSound()
    {
        int totalScore = 0;
        int totalPotentialScore = 0;

        for (int i = 0; i < _features.Length; i++)
        {
            totalScore += _features[i];
            totalPotentialScore += _gameController.GetTextures(i).Length - 1;
        }

        totalScore /= _features.Length;

        AudioManager.Instance.PlayMaskSound(totalScore / totalPotentialScore);
    }
}

public class PlantPotState
    {
        private GameController _gameController;
        private PlantPot _plantPot;
        public PlantPot PlantPot { get => _plantPot; }

        private PlantData _plantData;
        public PlantData PlantData { get => _plantData; }


        private MaskState _maskState;
        public MaskState MaskState { get => _maskState; }

        private int _growthStage = 0;
        public int GrowthStage { get => _growthStage; }

        private int _growthStageCycle = 0;
        public int GrowthStageCycle { get => _growthStageCycle; }

        public ActivityType CurrentActivityGoal { get => _plantData?.growthStages[_growthStage].activityGoals[_growthStageCycle] ?? ActivityType.None; }

        public bool HasPlant { get => _plantData != null; }

        public bool CanPlant { get => _plantData == null; }

        public bool CanHarvest
        {
            get
            {
                return _plantData != null
                    && _growthStage == _plantData.growthStages.Length - 1
                    && _growthStageCycle == _plantData.growthStages[_plantData.growthStages.Length - 1].activityGoals.Length - 1
                    && _cycleTimeRemaining <= 0;
            }
        }

        public bool IsActivityGoalMet
        {
            get
            {
                List<ActivityType> activityTypes = _gameController.GetActivityTypesFor(this);
                return activityTypes.Contains(CurrentActivityGoal)
                    || (CurrentActivityGoal == ActivityType.Dry && !activityTypes.Contains(ActivityType.Water))
                    || (CurrentActivityGoal == ActivityType.Quiet && !activityTypes.Contains(ActivityType.Loud));
            }
        }

        private float _cycleTimeRemaining = 0;
        private float _score = 0;

        public PlantPotState(GameController gameController, PlantPot plantPot)
        {
            _gameController = gameController;
            _plantPot = plantPot;

            ResetState();
        }

        private void ResetState()
        {
            _plantData = null;
            _maskState = null;
            _growthStage = 0;
            _growthStageCycle = 0;
            _cycleTimeRemaining = 0;
            _score = 0;
        }

        public void Update()
        {
            if (HasPlant && !CanHarvest)
            {
                _cycleTimeRemaining -= Time.deltaTime;
                _score += Time.deltaTime * (IsActivityGoalMet ? 1 : -1);// * (UnityEngine.Random.Range(0f, 1f) > 0.5f ? 1 : -1);

                //Debug.Log($"{_growthStage}, {_growthStageCycle}, {_score}");

                if (_cycleTimeRemaining <= 0)
                {
                    if (_growthStageCycle < _plantData.growthStages[_growthStage].activityGoals.Length - 1)
                    {
                        // Go to next Activity Goal
                        _growthStageCycle++;
                        _cycleTimeRemaining = _plantData.growSecondsPerStage;

                        _plantPot.UpdateActivityGoal(_growthStage, _growthStageCycle);

                        Debug.Log("New Goal... Score: " + _score);
                    }
                    else
                    {
                        // Use score to determine texture to show on mask
                        float maxScore = _plantData.growthStages[_growthStage].activityGoals.Length * _plantData.growSecondsPerStage;
                        float scoreRatio = (maxScore + math.clamp(_score, -maxScore, maxScore)) / (maxScore * 2);

                        AudioManager.Instance.PlayMaskSound(scoreRatio);

                        _maskState.SetIndex(_growthStage, (int)((_gameController.GetTextures(_growthStage).Length - 1) * scoreRatio));
                        _plantPot.UpdateMask(_growthStage);

                        Debug.Log("Growing... Score: " + _score + ", " + scoreRatio);

                        if (_growthStage < _plantData.growthStages.Length - 1)
                        {
                            // Go to next Growth Stage
                            _growthStage++;
                            _growthStageCycle = 0;
                            _cycleTimeRemaining = _plantData.growSecondsPerStage;
                            _score = 0;

                            _plantPot.UpdatePlant(_growthStage);
                            _plantPot.UpdateActivityGoal(_growthStage, _growthStageCycle);
                        }
                        else
                        {
                            // Fully Grown!
                            _cycleTimeRemaining = 0;
                            _score = 0;

                            _plantPot.OnPlantReady();
                        }
                    }
                }
            }
        }

        public void PlantSeed(PlantData plantData)
        {
            _maskState = new MaskState(_gameController, plantData);
            _plantData = plantData;
            _cycleTimeRemaining = _plantData.growSecondsPerStage;

            _plantPot.OnPlantSeed();
        }

        public void HarvestPlant()
        {
            _plantPot.OnHarvestPlant();

            ResetState();
        }
    }