using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideBar : MonoBehaviour
{
    public GameObject controller;
    public float speed;
    public bool movingOut;

    public Transform lastGuard;

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
            try
            {
                if (lastGuard.position.x < 12)
                {
                    transform.position = new Vector3(lastGuard.position.x - 13.75f, transform.position.y, 0); //moves guard across the screen
                }
            }
            catch
            {
                transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
            }
            if (transform.position.x <= -33) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
    }
}
