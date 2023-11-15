using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideBar : MonoBehaviour
{
    public GameObject controller;
    public float speed;
    public bool movingOut;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingOut && transform.position.x > 0)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        } else if (movingOut)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
            if (transform.position.x <= -33) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
    }
}
