using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuBillboard : MonoBehaviour
{
    public main controller;
    public Button myButton;

    public GameObject statics;
    public float staticTimer;
    public Image backround;
    public bool colorDir;
    public float colorVar;

    public AudioClip staticSound;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        myButton = GetComponent<Button>();

        staticTimer = 1f;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0,0,-10), controller.masterVol  * controller.sfxVol);
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
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol  * controller.sfxVol);
        controller.newGame();
    }
}
