using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBillboard : MonoBehaviour
{

    public main controller;
    public Button myButton;
    public Button shopButton;
    public Button settingsButton;
    public GameObject bigBillboard;

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
        statics.SetActive(false);
        colorVar = 100;
    }

    // Update is called once per frame
    void Update()
    {
        backround.color = new Color32((byte)colorVar, (byte)colorVar, 255, 255);

        if (colorDir)
        {
            colorVar += Time.deltaTime * 60;
            if(colorVar > 180)
            {
                colorDir = false;
            }
        }
        else
        {
            colorVar -= Time.deltaTime * 60;
            if (colorVar < 70)
            {
                colorDir = true;
            }
        }

        transform.position = transform.position - new Vector3(Time.deltaTime / 8 * controller.GetComponent<main>().mph, 0, 0); //moves building across the screen
        if (transform.position.x <= -14) //checks if its offscreen
        {
            Destroy(gameObject);
        }

        staticTimer -= Time.deltaTime;

        if (staticTimer <= 0 && controller.playing)
        {
            controller.billboards.Remove(gameObject);
            GameObject bboard = Instantiate(bigBillboard, transform.position, Quaternion.identity, GameObject.Find("buildings").transform);
            controller.billboards.Insert(0, bboard);
            Destroy(gameObject);

        }
    }

    public void click()
    {
        controller.StartGame();
        shopButton.interactable = false;
        settingsButton.interactable = false;
        statics.SetActive(true);
        staticTimer = 1f;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0,0,-10), controller.masterVol * controller.sfxVol);
    }

}
