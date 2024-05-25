using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class largeRoadOBJ : MonoBehaviour
{
    public GameObject controller;
    public float speed;
    public float destroyLimit;
        
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -destroyLimit) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
