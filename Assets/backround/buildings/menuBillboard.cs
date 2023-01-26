using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuBillboard : MonoBehaviour
{
    public GameObject controller;
    public Button myButton;

    public GameObject statics;
    public float staticTimer;
    public Image backround;
    public bool colorDir;
    public float colorVar;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        myButton = GetComponent<Button>();

        staticTimer = 1f;
        colorVar = 100;
    }

    // Update is called once per frame
    void Update()
    {
        staticTimer -= Time.deltaTime;

        if(staticTimer <= 0)
        {
            statics.SetActive(false);
        }

        backround.color = new Color32(255, (byte)colorVar, (byte)colorVar, 255);

        if (colorDir)
        {
            colorVar += Time.deltaTime * 60;
            if (colorVar > 130)
            {
                colorDir = false;
            }
        }
        else
        {
            colorVar -= Time.deltaTime * 60;
            if (colorVar < 20)
            {
                colorDir = true;
            }
        }
    }

    public void click()
    {
        controller.GetComponent<main>().newGame();
    }
}
