using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCar : MonoBehaviour
{
    public float startMph; // the mph the car will start at
    public float upMph; //how fast the car will speed up
    public float moveTime; // the time it shoul take the player car to switch lanes

    public main controller;
    public bool tapped;

    public Vector3 targetPos; //where the player car has to go
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    public Sprite reg;
    public Sprite crashed;

    public float startPos;

    private void OnEnable()
    {
        startMph = 30f;
        upMph = 0.5f;
        moveTime = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = reg; //sets it to the non crashed skin

        startPos = -7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            if (startPos == transform.position.x && controller.billboards[0].gameObject.name != "Start Billboard") {
                if (Input.touchCount > 0 && Time.timeScale > 0 && !tapped) // if the user touches the phone screen
                {
                    Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
                    tapped = true;

                    if (tapPoint.y < 3.0f)
                    {
                        if (tapPoint.y > transform.position.y && Mathf.Abs(tapPoint.y - transform.position.y) > 0.3) //if the tap if above the player car
                        {
                            laneUp();
                        }
                        else if (tapPoint.y < transform.position.y && Mathf.Abs(tapPoint.y - transform.position.y) > 0.3) //if the tap is below the player car
                        {
                            laneDown();
                        }
                    }
                } else
                {
                    tapped = false;
                }

                if (transform.position != targetPos) //if player car isnt where its supose to be
                {
                    transform.position += new Vector3(0, 4 * (Time.deltaTime / disMove), 0); //moves the player towards where they need to be
                    overshoot -= Mathf.Abs(4 * Time.deltaTime / disMove); //calculate the distance it moved since getting its new current
                    if (overshoot < 0) //checks if its past if target
                    {
                        transform.position = targetPos; //places player car where it should be
                        overshoot = 0; //resets overshoot
                    }
                }
            } else {
                transform.position += new Vector3(3*(moveTime) * Time.deltaTime, 0, 0);
                if(startPos - transform.position.x < 0)
                {
                    transform.position = new Vector3(startPos, transform.position.y, 0);
                    targetPos = transform.position;
                }
            }
        } 
    }

    public void laneUp() //if tap is above player car
    {
        if (targetPos.y < 0.65f && Mathf.Abs(transform.position.y - targetPos.y) < 0.35f && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
        {
            targetPos += new Vector3(0, 1.25f, 0); //changes targetPos to the new lane it needs to go to
            disMove = (targetPos.y - transform.position.y) * moveTime; //calculates the speed the player car needs to go to switch lanes
            overshoot = Mathf.Abs(targetPos.y - transform.position.y); //calculates overshoot to where it needs to go
            GetComponent<SpriteRenderer>().sortingOrder--;
        }
    }

    public void laneDown()
    {
        if (targetPos.y > -4.35f && Mathf.Abs(transform.position.y - targetPos.y) < 0.35f && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
        {
            targetPos += new Vector3(0, -1.25f, 0); //changes targetPos to the new lane it needs to go to
            disMove = (targetPos.y - transform.position.y) * moveTime; //calculates the speed the player car needs to go to switch lanes
            overshoot = Mathf.Abs(targetPos.y - transform.position.y); //calculates overshoot to where it needs to go
            GetComponent<SpriteRenderer>().sortingOrder++;
        }
    }

    public void crash()
    {
        GetComponent<SpriteRenderer>().sprite = crashed; //car crashed
        controller.gameOver(); //sets the game to its game over state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller.playing)
        {
            if (collision.tag == "car")
            {
                collision.GetComponent<cars>().speed = 0; //stops the car that crashes into the player (so they can file an insurence claim aganst the pkayer)
                controller.bannedLanes.Add(collision.GetComponent<cars>().lane);
                if (collision.transform.position.y < transform.position.y)
                {
                    GetComponent<SpriteRenderer>().sortingOrder--;
                    controller.bannedLanes.Add(collision.GetComponent<cars>().lane-1);
                }
                else if (collision.transform.position.y > transform.position.y)
                {
                    GetComponent<SpriteRenderer>().sortingOrder++;
                    controller.bannedLanes.Add(collision.GetComponent<cars>().lane+1);
                }
                crash(); //what happens when the player crashes
            }
        }
    }

    public void setLane(int lane)
    {
        transform.position = new Vector3(-12, (-lane * 1.25f) + 0.65f, 0);
        GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
    }
}
