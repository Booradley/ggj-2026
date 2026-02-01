using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public struct buttonData
{
    public PlantData plantData;
    public GameObject button;
}

public class Hotbar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posX = selectionIndicator.transform.position.x;
        posY = selectionIndicator.transform.position.y;
        posZ = selectionIndicator.transform.position.z;
        updateSelectionIndicator();
    }

    private int index = 0;

    private InputAction _leftAction;

    private InputAction _rightAction;

    private GameController _gameController;

    public InputActionAsset actions;

    private void Awake()
    {
        _gameController = GameObject.FindFirstObjectByType<Main>().GameController;

        _leftAction = actions.FindActionMap("Player").FindAction("Left");
        _rightAction = actions.FindActionMap("Player").FindAction("Right");
        _leftAction.performed += moveCursorLeft;
        _rightAction.performed += moveCursorRight;

    }

    public buttonData[] buttons;

    private UnityEngine.Vector3 invisible = new UnityEngine.Vector3(0f, 0f, 0f);
    private UnityEngine.Vector3 visible = new UnityEngine.Vector3(3f,3f,1f);
    private bool IsVisible = true;

    public GameObject selectionIndicator;

    private float posX;
    float posY;
    float posZ;

    public void setHotbarVisible()
    {
        if (!IsVisible)
        {
            IsVisible = true;
            this.transform.localScale = visible;
        }
    }

    public void setHotbarInvisible()
    {
        if (IsVisible)
        {
            IsVisible = false;
            this.transform.localScale = invisible;
        }
    }

    private void resetCursor()
    {
        selectionIndicator.transform.position = new UnityEngine.Vector3(0f,posY,posZ);
    }

    // dir is either -1 or 1, with Left being -1 and Right being 1
    private void moveCursorLeft(InputAction.CallbackContext context)
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = buttons.Length - 1;
        }
        _gameController.SetPlantIndex(index);
        updateSelectionIndicator();
    }

    private void moveCursorRight(InputAction.CallbackContext context)
    {
        if (index < buttons.Length - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        _gameController.SetPlantIndex(index);
        updateSelectionIndicator();
    }

    private void updateSelectionIndicator()
    {
        selectionIndicator.transform.position = new UnityEngine.Vector3(buttons[index].button.transform.position.x, posY, posZ);
    }
}
