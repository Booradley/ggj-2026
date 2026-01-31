using System;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    public PlantData[] plantDatas;

    private GameController _gameController = new GameController();
    public GameController GameController { get => _gameController; }

    void Start()
    {
        _gameController.Initialize(plantDatas);
    }

    void Update()
    {
        _gameController.Update();
    }
}