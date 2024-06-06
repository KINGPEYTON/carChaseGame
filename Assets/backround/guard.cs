using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard : MonoBehaviour
{
    public main controller;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
