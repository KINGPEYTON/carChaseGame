using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour
{
    public main controller;

    public Sprite[] skins; //array of car skins

    public float speed; //speed of thr car
    public int lane; //lane of car

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(12, 18);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        setLane();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph)
            {
                transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0);//move towards in game
            } else
            {
                transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / speed), 0, 0);
            }
        }
        else
        {
            transform.position = transform.position + new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)

            if (controller.bannedLanes.Contains(lane) && transform.position.x < -10)
            {
                newCarLane();
            }
        }
        //Debug.Log("Hello: ");
        if (transform.position.x <= -15 || transform.position.x >= 15) // checks if the car is on screen
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

    public virtual void newCarLane()
    {
        if (controller.playing)
        {
            transform.position = new Vector3(12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0);  //spawn new car in a random lane before going on screen;
        }
        else
        {
            transform.position = new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0);  //spawn new car in a random lane before going on screen;
        }
        setLane();
    }

    public void setLane()
    {
        lane = Mathf.Abs((int)((transform.position.y / 1.25f) - 0.65f));
        GetComponent<SpriteRenderer>().sortingOrder = 3 + lane;
    }
}
