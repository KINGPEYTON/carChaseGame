using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuBillboard : MonoBehaviour
{
    public GameObject controller;
    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        myButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        controller.GetComponent<main>().newGame();
    }
}
