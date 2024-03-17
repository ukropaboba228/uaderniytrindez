using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Texture2D cursorTexture;
    private Rect cursorRect;

    void Start()
    {
        cursorTexture = new Texture2D(12, 12);

        Color[] pixels = cursorTexture.GetPixels();
        for(int i = 0; i < pixels.Length; i++) 
        { 
            pixels[i] = Color.clear;
        }
        cursorTexture.SetPixels(pixels);
        cursorTexture.Apply();

        for (int x = 1; x < 10; x++)
        {
            cursorTexture.SetPixel(x, 5, Color.red);
        }

        for (int y = 0; y < 10; y++)
        {
            cursorTexture.SetPixel(5, y, Color.red);
        }

        cursorTexture.Apply();

        cursorRect = new Rect(Screen.width / 2 - 6, Screen.height / 2 - 6, 12, 12);

        Cursor.lockState = CursorLockMode.Locked;
        Screen.lockCursor = true;
    }

    void OnGUI()
    {
        GUI.DrawTexture(cursorRect, cursorTexture);
    }
}
