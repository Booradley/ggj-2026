using System;
using UnityEngine;

[Serializable]
public enum SoilType
{
    Sand,
    Dirt,
    Water,
    Hair,
}

[Serializable]
public enum ActivityType
{
    Water,
    Dry,
    Light,
    Dark,
    Loud,
    Quiet,
    Hot,
    Cold,
}

[Serializable]
public struct GrowthStage
{
    public ActivityType[] activityGoals;

    public GameObject[] parts;
}

[CreateAssetMenu(fileName = "PlantData", menuName = "Scriptable Objects/Plant Data")]
public class PlantData : ScriptableObject
{
    [SerializeField]
    public GameObject plantPrefab;

    [SerializeField, Range(0f, 30f)]
    public float growSecondsPerStage = 30f;

    [SerializeField]
    public GrowthStage[] growthStages;

    [SerializeField]
    public SoilType soilType;
}