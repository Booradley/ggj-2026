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

    [SerializeField]
    public Texture2D[] eyeTextures;

    [SerializeField]
    public Texture2D[] noseTextures;

    [SerializeField]
    public Texture2D[] mouthTextures;

    private GameController _gameController = new GameController();
    public GameController GameController { get => _gameController; }

    private Dictionary<ActivityType, ActivityTexture> _activityGoalTextures = new();

    void Start()
    {
        foreach (ActivityTexture activityTexture in activityTextures)
        {
            _activityGoalTextures.Add(activityTexture.activityType, activityTexture);
        }

        _gameController.Initialize(this);
    }

    void Update()
    {
        _gameController.Update();
    }

    public Texture2D GetActivityGoalTexture(ActivityType activityType)
    {
        return _activityGoalTextures[activityType].texture;
    }

    public Color GetActivityGoalTint(ActivityType activityType)
    {
        return _activityGoalTextures[activityType].tint;
    }

    public Texture2D[] GetTextures(int growthStage)
    {
        if (growthStage == 0)
        {
            return eyeTextures;
        }
        else if (growthStage == 1)
        {
            return noseTextures;
        }

        return mouthTextures;
    }
}