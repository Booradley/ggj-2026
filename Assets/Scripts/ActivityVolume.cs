using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityVolume : MonoBehaviour
{
    public ActivityType activityType;

    private List<GameObject> _affecting = new();

    public bool IsAffecting(GameObject gameObject)
    {
        return _affecting.Contains(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        _affecting.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        _affecting.Remove(other.gameObject);
    }
}