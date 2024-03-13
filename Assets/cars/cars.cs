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
    public bool willSwith;
    public float switchTimer;

    public float turningTimer;
    public float turnTime; // the time it shoul take the player car to switch lanes
    public float targPos;
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    public int blinkTime;
    public float speedMin;
    public float speedMax;

    public float odds;

    public AudioClip horn;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        setLane();


        turnTime = 3.5f;
        switchLane(blinkTime);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0); //move fowards in game

                switchTimer -= Time.deltaTime;
                if (switchTimer < 0) //checks if its time to switch lanes
                {
                    int maxLane = 0;
                    if (controller.topLane)
                    {
                        maxLane = 0;
                    }
                    else
                    {
                        maxLane = 1;
                    }
                    if (Random.Range(0, 2) == 0 && lane > maxLane) //randomly decides which lane to switch to and if its a valid lane
                    {
                        switchUp = true; //initiats the switch up
                        targPos += 1.25f;
                    }
                    else if (lane < 4) //checks if its switching to a valid lane
                    {
                        switchDown = true; //initiats the switch down
                        targPos -= 1.25f;
                    }
                    disMove = (targPos - transform.position.y) * turnTime; //calculates the speed the car needs to go to switch lanes
                    overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
                    switchTimer = 100; //so the car wont try to switch lanes again before its done switching currently
                }
            }
            else
            {
                transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / speed), 0, 0); //car movment in the start up animation
            }

            if (blinkTime > -1) // checks if its a car that can turn
            {
                turnDown.SetActive(switchDown && turningTimer % 1 < 0.5f); //turns the down blinker on if it should
                turnUp.SetActive(switchUp && turningTimer % 1 < 0.5f); //turns the up blinker on if it should
                if (switchDown || switchUp) //checks if one of the blinkers are on
                {
                    turningTimer += Time.deltaTime;
                }

                if (turningTimer > 1.5f) //if its time to turn
                {
                    if (transform.position.y != targPos) //checks to see if its not in its target pos
                    {
                        transform.position += new Vector3(0, 4 * (Time.deltaTime / disMove), 0); //moves the car towards where they need to be

                        overshoot -= Mathf.Abs(4 * Time.deltaTime / disMove); //calculate the distance it moved since getting its new current
                        if (overshoot < 0) //checks if its past if target
                        {
                            transform.position = new Vector3(transform.position.x, targPos, 0); //places car where it should be
                            overshoot = 0; //resets overshoot
                            turningTimer = 0;
                            switchDown = false; //turns down blinker off
                            switchUp = false; //turns up blinker off
                            setLane(); //update so the car knows what lane its in
                            switchLane(blinkTime * 2); //sees if it wants to switch lanes again
                        }
                    }
                }
            }
        }
        else
        {
            if (speed > 0)
            {
                transform.position = transform.position + new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)

                if (controller.bannedLanes.Contains(lane) && transform.position.x < -10)
                {
                    newCarLane();
                }
            }
            else //sets hazards off if it crashes
            {
                if (turnDown != null && turnUp != null)
                {
                    turningTimer += Time.deltaTime;
                    turnDown.SetActive(turningTimer % 1 < 0.5f); //turns the down blinker on if it should
                    turnUp.SetActive(turningTimer % 1 < 0.5f); //turns the up blinker on if it should
                }
            }
        }

        if (transform.position.x <= -15 || transform.position.x >= 15) // checks if the car is on screen
        {
            Destroy(gameObject); // destroys it otherwise
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            if(transform.position.y > collision.transform.position.y)
            {
                if (disMove < 0)
                {
                    targPos += 1.25f;
                    disMove *= -1;
                }
                switchUp = false;
                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
            }
            else if(transform.position.y < collision.transform.position.y)
            {
                if (disMove > 0)
                {
                    targPos -= 1.25f;
                    disMove *= -1;
                }
                switchDown = false;
                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                speed = collision.GetComponent<cars>().speed - 1; //changes the speed so cars won't go through eachother
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            if (transform.position.y == collision.transform.position.y && transform.position.x < collision.transform.position.x)
            {
                speed -= Time.deltaTime * 3;
                collision.GetComponent<cars>().speed += Time.deltaTime * 4; //changes the speed so cars won't go through eachother
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
            float maxLane = 0;
            if (controller.topLane)
            {
                maxLane = 0.65f;
            }
            else
            {
                maxLane = -0.6f;
            }
            transform.position = new Vector3(-14, maxLane, 0);  //spawn new car in a random lane before going on screen;
        }
        setLane();
    }

    public virtual void setLane()
    {
        lane = Mathf.Abs((int)((transform.position.y / 1.25f) - 0.65f));
        GetComponent<SpriteRenderer>().sortingOrder = 3 + lane;
        targPos = transform.position.y;

        if (blinkTime > -1)
        {
            turnUp.GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
            turnDown.GetComponent<SpriteRenderer>().sortingOrder = 4 + lane;
        }
    }

    void switchLane(int blinkOdds)
    {
        int switchOdds = Random.Range(0, blinkOdds);
        if (!controller.inTutorial && switchOdds == 0)
        {
            switchTimer = Random.Range(30 / controller.mph, 120 / controller.mph);
        }
        else
        {
            switchTimer = 1000;
        }
    }
}
