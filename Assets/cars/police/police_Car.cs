using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class police_Car : cars
{
    public playerCar playerCarOBJ;

    public bool chasingPlayer;
    public bool crashed;
    public AudioClip crash;
    public AudioSource siren;

    public bool chasingVan;
    public maniac_van vanOBJ;

    // Start is called before the first frame update
    void Start()
    {
        if (chasingVan)
        {
            siren.Play();
        }
        speedMin = 14;
        speedMax = 27;

        speed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        playerCarOBJ = GameObject.Find("playerCar").GetComponent<playerCar>();
        targPos = transform.position.y;

        turnTime = 1.5f;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (chasingPlayer || chasingVan)
        {
            GetComponent<SpriteRenderer>().sprite = skins[((int)turningTimer) % 2];
            turnDown.SetActive(turningTimer % 2 < 1); //turns the down blinker on if it should
            turnUp.SetActive(turningTimer % 2 > 1); //turns the down blinker on if it should
            turningTimer += Time.deltaTime * 4;

            siren.volume = controller.sfxVol * controller.masterVol;
        }

        if (chasingVan && vanOBJ == null && !crashed)
        {
            Destroy(gameObject);
        }

        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                if (chasingPlayer)
                {
                    if (crashed)
                    {
                        transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.GetComponent<main>().mph, 0, 0); //move fowards in game
                    }
                    else
                    {
                        switchTimer -= Time.deltaTime;
                        if (switchTimer < 0) //checks if its time to switch lanes
                        {
                            if (playerCarOBJ.targetPos.y != transform.position.y)
                            {
                                targPos = playerCarOBJ.targetPos.y;
                                disMove = (targPos - transform.position.y) * turnTime; //calculates the speed the car needs to go to switch lanes
                                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
                            }
                            switchTimer = Random.Range(0.75f, 1.75f); //so the car wont try to switch lanes again before its done switching currently
                        }
                        if (transform.position.x < -12.5f)
                        {
                            transform.position += new Vector3(Time.deltaTime * 3.5f, 0, 0); //move fowards in game
                        }
                        else
                        {
                            transform.position += new Vector3(Time.deltaTime * 0.25f, 0, 0); //move fowards in game
                        }
                    }
                }
                else
                {
                    if (chasingVan)
                    {
                        switchTimer -= Time.deltaTime;
                        if (switchTimer < 0) //checks if its time to switch lanes
                        {
                            if (vanOBJ.transform.position.y != transform.position.y)
                            {
                                targPos = vanOBJ.transform.position.y;
                                disMove = (targPos - transform.position.y) * turnTime * 2; //calculates the speed the car needs to go to switch lanes
                                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
                            }
                            switchTimer = Random.Range(0.75f, 1.75f); //so the car wont try to switch lanes again before its done switching currently
                        }
                        transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / vanOBJ.currSpeed), 0, 0); //move fowards in game
                    }
                    else
                    {
                        transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0); //move fowards in game
                    }
                    if(transform.position.x < -16 && controller.mph > 70)
                    {
                        chasingPlayer = true;
                        chasingVan = false;
                        siren.Play();
                        targPos = playerCarOBJ.targetPos.y;
                        transform.position = new Vector3(transform.position.x, playerCarOBJ.targetPos.y, 0);
                    }
                }
            }
            else
            {
                transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / speed), 0, 0); //car movment in the start up animation
            }

            if (transform.position.y != targPos && !crashed) //checks to see if its not in its target pos
            {
                transform.position += new Vector3(0, 4 * (Time.deltaTime / disMove), 0); //moves the car towards where they need to be

                overshoot -= Mathf.Abs(4 * Time.deltaTime / disMove); //calculate the distance it moved since getting its new current
                if (overshoot < 0) //checks if its past if target
                {
                    transform.position = new Vector3(transform.position.x, targPos, 0); //places car where it should be
                    overshoot = 0; //resets overshoot
                }
            }
        }
        else
        {
            if (chasingPlayer)
            {
                if (!crashed)
                {
                    transform.position = transform.position + new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
                }
            } else
            {
                transform.position = transform.position + new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
            }
        }

        if (transform.position.x <= -30 || transform.position.x >= 30) // checks if the car is on screen
        {
            Destroy(gameObject); // destroys it otherwise
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!crashed)
        {
            if (collision.tag == "car") //if a car hits another car
            {
                if (chasingPlayer && transform.position.x > -12.5f)
                {
                    crashed = true; //changes the speed so the cars crash
                    collision.GetComponent<cars>().speed = 6;
                    AudioSource.PlayClipAtPoint(crash, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                }
                else if (transform.position.x < collision.transform.position.x)
                {
                    speed = collision.GetComponent<cars>().speed - 1; //changes the speed so cars won't go through eachother
                }
            }
            else if (collision.tag == "Player")
            {
                crashed = true;
            }
        }
    }
}
