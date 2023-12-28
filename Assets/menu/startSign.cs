using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class startSign : MonoBehaviour
{
    public main controller;
    public Image backround;
    public bool colorDir;
    public float colorVar;

    public float speed;
    public Transform parentOBJ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
    }

    // Update is called once per frame
    void Update()
    {
        parentOBJ.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -16) //checks if its offscreen
        {
            Destroy(gameObject);
        }

        if (!controller.playing)
        {
            if (colorDir)
            {
                colorVar += Time.deltaTime * 60;
                if (colorVar > 190)
                {
                    colorDir = false;
                }
            }
            else
            {
                colorVar -= Time.deltaTime * 60;
                if (colorVar < 60)
                {
                    colorDir = true;
                }
            }
        } else
        {
            if (colorVar > 0)
            {
                colorVar -= Time.deltaTime * 60;
            }
            else
            {
                colorVar = 0;
            }
        }


        backround.color = new Color32((byte)colorVar, (byte)colorVar, 255, 255);
    }
}
