using System;
using UnityEngine;

public class MaskWearer : MonoBehaviour
{
    public GameObject headBone;

    public Vector3 maskOffset = new Vector3();

    private Mask _mask;

    public void RemoveMask()
    {
        if (!_mask) return;

        GameObject.Destroy(_mask.gameObject);
        _mask = null;
    }

    public void WearMask(PlantPotState plantPotState)
    {
        RemoveMask();

        plantPotState.MaskState.PlayMaskSound();

        _mask = GameObject.Instantiate(plantPotState.PlantData.plantPrefab).GetComponent<Mask>();
        _mask.transform.SetParent(headBone.transform, false);
        _mask.transform.localPosition = maskOffset;

        for (int i = 0; i < plantPotState.MaskState.Features.Length; i++)
        {
            Debug.Log(plantPotState.MaskState.GetIndex(i));
            _mask.SetIndex(i, plantPotState.MaskState.GetIndex(i));
        }
    }
}