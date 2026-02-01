using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameController
{
    private PlantData[] _plantDatas;
    private Dictionary<ActivityType, ActivityTexture> _activityGoalTextures;
    private List<PlantPotState> _plantStates = new List<PlantPotState>();
    private PlantPot _heldPlantPot;

    public void Initialize(PlantData[] plantDatas, Dictionary<ActivityType, ActivityTexture> activityGoalTextures)
    {
        _plantDatas = plantDatas;
        _activityGoalTextures = activityGoalTextures;
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
        PlantPotState plantState = new PlantPotState(plantPot);
        _plantStates.Add(plantState);
        return plantState;
    }

    public void OnInteract()
    {
        // Drop pot if holding one
        if (_heldPlantPot)
        {
            
        }
    }

    public void OnPlantPotInteraction(PlantPot plantPot)
    {
        foreach (PlantPotState plantState in _plantStates)
        {
            if (plantState.PlantPot == plantPot)
            {
                if (plantState.CanPlant)
                {
                    plantState.PlantSeed(_plantDatas[0]);
                }
                else if (plantState.CanHarvest)
                {
                    plantState.HarvestPlant();
                }
                else
                {
                    // Pick pot up if not holding one
                    if (!_heldPlantPot)
                    {
                        
                    }
                }

                return;
            }
        }
    }

    public Texture2D GetActivityGoalTexture(ActivityType activityType)
    {
        return _activityGoalTextures[activityType].texture;
    }

    public Color GetActivityGoalTint(ActivityType activityType)
    {
        return _activityGoalTextures[activityType].tint;
    }
}

public class MaskState
{
    private int _eyeIndex;
    public int EyeIndex { get; }
    
    private int _noseIndex;
    public int NoseIndex { get; }

    private int _mouthIndex;
    public int MouthIndex { get; }

    private PlantData _plantData;

    public MaskState(PlantData plantData)
    {
        _plantData = plantData;
    }

    public void SetEyeIndex(int index)
    {
        _eyeIndex = index;
    }

    public void SetNoseIndex(int index)
    {
        _noseIndex = index;
    }

    public void SetMouthIndex(int index)
    {
        _mouthIndex = index;
    }
}

public class PlantPotState
    {
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
            get => true;
        }

        private float _cycleTimeRemaining = 0;
        private float _score = 0;

        public PlantPotState(PlantPot plantPot)
        {
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
                _score += Time.deltaTime * (IsActivityGoalMet ? 1 : -1);

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
            _maskState = new MaskState(plantData);
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