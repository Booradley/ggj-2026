using System;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    public PlantData plantData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(plantData.test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
