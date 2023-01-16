using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guard : MonoBehaviour
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
        transform.position = transform.position - new Vector3(Time.deltaTime / 5 * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        //Debug.Log("Hello: ");
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
