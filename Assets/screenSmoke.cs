using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenSmoke : MonoBehaviour
{
    public Image distortIMG;
    public main controller;
    public float lastScreenDistort;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        distortIMG = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lastScreenDistort != controller.screenDistort)
        {
            lastScreenDistort = controller.screenDistort;
            byte screenByte = (byte)(lastScreenDistort * 255);
            distortIMG.color = new Color32(150, 150, 150, screenByte);
        }
    }
}
