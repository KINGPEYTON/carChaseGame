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

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        myButton = GetComponent<Button>();

        staticTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        staticTimer -= Time.deltaTime;

        if(staticTimer <= 0)
        {
            statics.SetActive(false);
        }
    }

    public void click()
    {
        controller.GetComponent<main>().newGame();
    }
}
