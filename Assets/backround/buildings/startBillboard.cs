using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBillboard : MonoBehaviour
{

    public GameObject controller;
    public Button myButton;
    public Button shopButton;
    public Button settingsButton;

    public GameObject statics;
    public float staticTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        myButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / 8 * controller.GetComponent<main>().mph, 0, 0); //moves building across the screen
        if (transform.position.x <= -14) //checks if its offscreen
        {
            Destroy(gameObject);
        }

        staticTimer -= Time.deltaTime;

        if (staticTimer <= 0)
        {
            statics.SetActive(false);
        }
    }

    public void click()
    {
        controller.GetComponent<main>().StartGame();
        myButton.interactable = false;
        shopButton.interactable = false;
        settingsButton.interactable = false;
        statics.SetActive(true);
        staticTimer = 1f;
    }

}
