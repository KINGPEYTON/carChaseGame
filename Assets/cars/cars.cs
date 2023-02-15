using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour
{
    public main controller;

    public Sprite[] skins; //array of car skins

    public float speed; //speed of thr car
    public int lane; //lane of car

    public bool switchUp;
    public bool switchDown;
    public GameObject turnUp;
    public GameObject turnDown;
    public float switchTimer;

    public float turningTimer;
    public float turnTime; // the time it shoul take the player car to switch lanes
    public int blinkTime; 
    public float targPos;
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(12, 18);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        setLane();


        turnTime = 3.5f;
        blinkTime = 45;
        switchTimer = Random.Range(2, blinkTime);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph)
            {
                transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0);//move towards in game
                switchTimer -= Time.deltaTime;

                if (switchTimer < 0)
                {
                    switchTimer = 100;
                    if(Random.Range(0,2) == 0 && lane > 0)
                    {
                        switchUp = true;
                        targPos += 1.25f;
                    }
                    else if(lane < 4)
                    {
                        switchDown = true;
                        targPos -= 1.25f;
                    }
                    disMove = (targPos - transform.position.y) * turnTime; //calculates the speed the player car needs to go to switch lanes
                    overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
                }
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

        if (transform.position.x <= -15 || transform.position.x >= 15) // checks if the car is on screen
        {
            Destroy(gameObject); // destroys it otherwise
        }

        if (turnDown != null && turnDown != null)
        {
            turnDown.SetActive(switchDown && turningTimer % 1 < 0.5f);
            turnUp.SetActive(switchUp && turningTimer % 1 < 0.5f);
            if (switchDown || switchUp)
            {
                turningTimer += Time.deltaTime;
            }

            if (turningTimer > 1.5f)
            {
                if (transform.position.y != targPos)
                {
                    transform.position += new Vector3(0, 4 * (Time.deltaTime / disMove), 0); //moves the player towards where they need to be

                    overshoot -= Mathf.Abs(4 * Time.deltaTime / disMove); //calculate the distance it moved since getting its new current
                    if (overshoot < 0) //checks if its past if target
                    {
                        transform.position = new Vector3(transform.position.x, targPos, 0); //places player car where it should be
                        overshoot = 0; //resets overshoot
                        turningTimer = 0;
                        switchDown = false;
                        switchUp = false;
                        setLane();
                        switchTimer = Random.Range(blinkTime/2, blinkTime);
                        //switchTimer = Random.Range(100, 100);
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            if(transform.position.y > collision.transform.position.y)
            {
                disMove *= -1;
                targPos += 1.25f;
                switchUp = false;
                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
            }
            else if(transform.position.y < collision.transform.position.y)
            {
                disMove *= -1;
                targPos -= 1.25f;
                switchDown = false;
                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                speed = collision.GetComponent<cars>().speed - 1; //changes the speed so cars won't go through eachother
            }
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
        targPos = transform.position.y;

        if (turnDown != null && turnDown != null)
        {
            turnUp.GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
            turnDown.GetComponent<SpriteRenderer>().sortingOrder = 4 + lane;
        }
    }

    void switchLane()
    {

    }
}
