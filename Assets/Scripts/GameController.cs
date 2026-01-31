using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private class PlantState
    {
        private PlantData _plantData;
        public PlantData PlantData { get => _plantData; }

        private int _growthStage = 0;
        public int GrowthStage { get => _growthStage; }

        private int _growthStageCycle = 0;
        public int GrowthStageCycle { get => _growthStageCycle; }

        public PlantState(PlantData plantData)
        {
            _plantData = plantData;
            _growthStage = 0;
            _growthStageCycle = 0;
        }
    }

    private PlantData[] _plantDatas;
    private List<PlantState> _plantStates = new List<PlantState>();

    public void Initialize(PlantData[] plantDatas)
    {
        _plantDatas = plantDatas;
    }

    public void Update()
    {

    }
}
