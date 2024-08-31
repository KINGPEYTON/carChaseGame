using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class startSign : MonoBehaviour
{
    public main controller;
    public Image backround;
    public float colorTimer;
    public float colorVarR;
    public float colorVarG;
    public float colorVarB;

    public float speed;
    public Transform parentOBJ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        colorVarB = 255;
    }

    // Update is called once per frame
    void Update()
    {
        parentOBJ.position -= new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -16) //checks if its offscreen
        {
            Destroy(parentOBJ.gameObject);
        }

        if (!controller.playing)
        {
            colorTimer += Time.deltaTime;
            colorVarR = 55 + getValueScale(Mathf.Abs((colorTimer % 2) - 1), 0, 1, 100);
            colorVarG = 105 + getValueScale(Mathf.Abs((colorTimer % 3) - 1.5f), 0, 1.5f, 100);
            colorVarB = 155 + getValueScale(Mathf.Abs((colorTimer % 4) - 2), 0, 2, 100);
        } else
        {
            if (colorVarR > 0)
            {
                colorVarR -= Time.deltaTime * 75;
                if (colorVarR < 0)
                {
                    colorVarR = 0;
                }
            }
            if (colorVarG > 0)
            {
                colorVarG -= Time.deltaTime * 75;
                if (colorVarG < 0)
                {
                    colorVarG = 0;
                }
            }
            if (colorVarB > 150)
            {
                colorVarB -= Time.deltaTime * 50;
                if (colorVarB < 150)
                {
                    colorVarB = 150;
                }
            }
        }


        backround.color = new Color32((byte)colorVarR, (byte)colorVarG, (byte)colorVarB, 255);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
