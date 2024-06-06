using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manhole : MonoBehaviour
{
    public GameObject controller;
    public float speed;
    public ParticleSystem smoke;
    public Sprite[] skins; //array of car skins

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");

        int manholeVal = Random.Range(0, skins.Length);
        GetComponent<SpriteRenderer>().sprite = skins[manholeVal]; //set the skin to a random one at spawn

        if (manholeVal == 0)
        {
            smoke.emissionRate = Random.Range(20, 60);
            smoke.startLifetime = Random.Range(0.35f, 0.85f);
            smoke.startSpeed = Random.Range(2, 6);
            smoke.gameObject.transform.localPosition += new Vector3(0, 1.4f, 0);
        }
        else
        {
            smoke.emissionRate = Random.Range(50, 100);
            smoke.startLifetime = Random.Range(0.75f, 1.15f);
            smoke.startSpeed = Random.Range(5, 10);
        }
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

    public float getSmokeValue()
    {
        return (smoke.emissionRate * smoke.startSpeed);
    }
}
