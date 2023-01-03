using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBillboard : MonoBehaviour
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
        transform.position = transform.position - new Vector3(Time.deltaTime / 12 * controller.GetComponent<main>().mph, 0, 0); //moves building across the screen
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }

    public void click()
    {
        controller.GetComponent<main>().StartGame();
        myButton.interactable = false;
    }

}
