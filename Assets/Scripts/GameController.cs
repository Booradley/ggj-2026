using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController
{
    private PlantData[] _plantDatas;
    private List<PlantState> _plantStates = new List<PlantState>();

    public void Initialize(PlantData[] plantDatas)
    {
        _plantDatas = plantDatas;
    }

    public void Update()
    {
        foreach (PlantState plantState in _plantStates)
        {
            plantState.Update();
        }
    }

    public PlantState RegisterPlantPot(PlantPot plantPot)
    {
        PlantState plantState = new PlantState(plantPot);
        _plantStates.Add(plantState);
        return plantState;
    }

    public void OnPlantPotInteraction(PlantPot plantPot)
    {
        foreach (PlantState plantState in _plantStates)
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
                    
                }

                return;
            }
        }
    }
}

public class PlantState
    {
        private PlantPot _plantPot;
        public PlantPot PlantPot { get => _plantPot; }

        private PlantData _plantData;
        public PlantData PlantData { get => _plantData; }

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
                    && _growthStage == _plantData.growthStages.Length 
                    && _growthStageCycle == _plantData.growthStages[_plantData.growthStages.Length - 1].activityGoals.Length
                    && _cycleTimeRemaining <= 0;
            }
        }

        private float _cycleTimeRemaining = 0;
        private float _score = 0;

        public PlantState(PlantPot plantPot)
        {
            _plantPot = plantPot;
            
            ResetState();
        }

        private void ResetState()
        {
            _plantData = null;
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
                if (_cycleTimeRemaining <= 0)
                {
                    if (_growthStageCycle < _plantData.growthStages[_growthStage].activityGoals.Length)
                    {
                        // TODO: Update running score

                        // Go to next Activity Goal
                        _growthStageCycle++;
                        _cycleTimeRemaining = _plantData.growSecondsPerStage;

                        _plantPot.UpdateActivityGoal(_growthStage, _growthStageCycle);
                    }
                    else
                    {
                        // TODO: Update running score
                        // TODO: Use score to determine texture to show on mask

                        if (_growthStage < _plantData.growthStages.Length)
                        {
                            // Go to next Growth Stage
                            _growthStage++;
                            _growthStageCycle = 0;
                            _cycleTimeRemaining = _plantData.growSecondsPerStage;

                            _plantPot.UpdatePlant(_growthStage);
                            _plantPot.UpdateActivityGoal(_growthStage, _growthStageCycle);
                        }
                        else
                        {
                            // Fully Grown!
                            _cycleTimeRemaining = 0;

                            _plantPot.UpdatePlant(_growthStage + 1);
                            return;
                        }
                    }
                }
            }
        }

        public void PlantSeed(PlantData plantData)
        {
            _plantData = plantData;
            _plantPot.PlantSeed(plantData);
        }

        public void HarvestPlant()
        {
            _plantPot.HarvestPlant();
            
            ResetState();
        }
    }