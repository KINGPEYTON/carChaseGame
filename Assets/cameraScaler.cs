using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScaler : MonoBehaviour
{
    // Set this to the in-world distance between the left & right edges of your scene.
    private float sceneWidth = 22;

    public float cameraSize;
    public float cameraHight;

    // Start is called before the first frame update
    void Awake()
    {
        setScales();
    }

    // Update is called once per frame
    void setScales()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        if (desiredHalfHeight > 5)
        {
            sceneWidth = 22 - ((desiredHalfHeight - 5) / 1.25f);
            unitsPerPixel = sceneWidth / Screen.width;
            desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        }
        cameraSize = desiredHalfHeight;
        cameraHight = desiredHalfHeight - 5;
    }

    public static float getScale(float sSize)
    {
        float unitsPerPixel = sSize / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        return desiredHalfHeight;
    }
}
