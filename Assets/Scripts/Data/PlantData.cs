using System;
using UnityEngine;

[Serializable]
public enum ActivityType
{
    None,
    Water,
    Dry,
    Light,
    Dark,
    Loud,
    Quiet,
    Hot,
    Cold,
    Bug,
}

[Serializable]
public struct GrowthStage
{
    public ActivityType[] activityGoals;

    public float yOffset;
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
}