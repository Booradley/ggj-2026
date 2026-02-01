using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hotbarLength = 100;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public GameObject Hotbar;

    public GameObject Cursor;

    public float hotbarLength;

    // dir is either -1 or 1, with Left being -1 and Right being 1
    // dependency: assumes Hotbar size is 100 x 20
    void changeCursorPos(int dir)
    {
        float pos = Cursor.transform.position.x;
        // called when player presses <- or -> while the Hotbar is visible
        // Checks if cursor is already at max pos in that dir, return if so; else:
        // Moves the cursor 20 px in the corresponding direction
        if (dir < 0 && pos < 40)
        {
            pos = pos + hotbarLength/5;
        }
        else if (dir > 0 && pos > -40)
        {
            pos = pos - hotbarLength/5;
        }
        else
        {
            return;
        }
    }
}
