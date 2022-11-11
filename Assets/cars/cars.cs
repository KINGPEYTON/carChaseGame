using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour
{
    public main controller;

    public Sprite[] skins; //array of car skins

    public float speed; //speed of thr car

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(12, 18);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0);//move towards in game
        } else
        {
            transform.position = transform.position + new Vector3(Time.deltaTime * (speed/2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)

            if(controller.bannedLanes.Contains(transform.position.y) && transform.position.x < -10)
            {
                newCarLane();
            }
        }
        //Debug.Log("Hello: ");
        if (transform.position.x <= -12|| transform.position.x >= 22) // checks if the car is on screen
        {
            Destroy(gameObject); // destroys it otherwise
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            speed = collision.GetComponent<cars>().speed; //changes the speed so cars won't go through eachother
        }
    }

    public void newCarLane()
    {
        if (controller.playing)
        {
            transform.position = new Vector3(12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0);  //spawn new car in a random lane before going on screen;
        }
        else
        {
            transform.position = new Vector3(-12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0);  //spawn new car in a random lane before going on screen;
        }
    }
}
