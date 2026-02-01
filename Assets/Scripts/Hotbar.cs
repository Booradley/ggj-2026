using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public struct ButtonData
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
        resetCursor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private UnityEngine.Vector3 invisible = new UnityEngine.Vector3(0f, 0f, 0f);
    private UnityEngine.Vector3 visible = new UnityEngine.Vector3(3f,3f,1f);
    private bool IsVisible = true;

    public GameObject selectionIndicator;

    public ButtonData[] buttons;

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
    private void changeCursorPos(int dir)
    {
        float pos = selectionIndicator.transform.position.x;
        // called when player presses <- or -> while the Hotbar is visible
        // Moves the cursor 20 px in the corresponding direction
        // if cursor is past the max Left pos, set cursor to max Right pos & vice versa
        if (dir < 0)
        {
            posX = posX + 114;
            if (pos > 228)
            {
                selectionIndicator.transform.position = new UnityEngine.Vector3(posX, posY,posZ);
            }
        }
        else if (dir > 0)
        {
            pos = pos - 114;
            if (pos < -228)
            {
                selectionIndicator.transform.position = new UnityEngine.Vector3(posX, posY,posZ);
            }
        }
    }
}
