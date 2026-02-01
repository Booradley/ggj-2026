using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActivityTexture
{
    public ActivityType activityType;
    public Texture2D texture;
    public Color32 tint;
}

public class Main : MonoBehaviour
{
    [SerializeField]
    public PlantData[] plantDatas;

    [SerializeField]
    public ActivityTexture[] activityTextures;

    private GameController _gameController = new GameController();
    public GameController GameController { get => _gameController; }

    void Start()
    {
        Dictionary<ActivityType, ActivityTexture> activityTextureLookup = new();
        foreach (ActivityTexture activityTexture in activityTextures)
        {
            activityTextureLookup.Add(activityTexture.activityType, activityTexture);
        }

        _gameController.Initialize(plantDatas, activityTextureLookup);
    }

    void Update()
    {
        _gameController.Update();
    }
}