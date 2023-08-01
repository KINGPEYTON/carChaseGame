using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manhole : MonoBehaviour
{
    public GameObject controller;
    public float speed;
    public ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        speed = 5;

        smoke.emissionRate = Random.Range(20, 80);
        smoke.startLifetime = Random.Range(0.25f, 0.75f);
        smoke.startSpeed = Random.Range(3, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
