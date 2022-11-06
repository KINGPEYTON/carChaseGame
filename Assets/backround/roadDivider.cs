using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadDivider : MonoBehaviour
{
    public GameObject controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime/5 * controller.GetComponent<main>().mph, 0, 0); //moves divider across the screen
        //Debug.Log("Hello: ");
        if (transform.position.x <= -12) //checks if its offscreen
        { 
            Destroy(gameObject);
        }
    }
}
