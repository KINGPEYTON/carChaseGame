using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maniac_van : cars
{
    public float currSpeed; //speed of thr car
    public float startSpeed; //speed of thr car

    public float turningSpeed;

    public SpriteRenderer party;
    public Sprite[] partySkins; //array of car skins
    public int partyList;
    public float partyBeatTimer;
    public float partyBeatTime;
    public GameObject partyMusic;
    public AudioSource musicPlayer;

    public GameObject coins;
    public float coinTimer;

    // Start is called before the first frame update
    void Start()
    {
        speedMin = 15;
        speedMax = 23;
        forceMass = 0.95f;

        startSpeed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn

        currSpeed = startSpeed;
        turningSpeed = Random.Range(0.6f, 0.9f);
        turningTimer = Random.Range(0.0f, 5.0f);

        partyBeatTime = 0.422f;
        party.sprite = partySkins[Random.Range(0, partySkins.Length)];
        musicPlayer.volume = controller.musicVol * controller.masterVol * 0.85f;
        isCar = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                if (currSpeed > 0)
                {
                    transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / currSpeed), turningSpeed * Time.deltaTime, 0); //move fowards in game
                }
                else
                {
                    transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / 3.5f), 0, 0); //move fowards in game
                }
            }
            else
            {
                if (currSpeed > 0)
                {
                    transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / currSpeed), turningSpeed * Time.deltaTime, 0); //car movment in the start up animation
                }
                else
                {
                    transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / 3.5f), 9, 0); //car movment in the start up animation
                }
            }

            spawnCoin();

            if (controller.laserOn)
            {
                getsShot();
            }
        }
        else
        {
            if (currSpeed > 0)
            {
                transform.position = transform.position + new Vector3(Time.deltaTime * (currSpeed / 2.0f), turningSpeed * Time.deltaTime, 0); // moves the across cars the screen when game isnt on (like game over screen)
            } else {
                transform.position = transform.position + new Vector3(0, 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
            }
        }

        if (transform.position.x <= -12) // checks if the car is on screen
        {
            partyMusic.transform.position -= new Vector3(0, 0, Time.deltaTime * 10);
        }

        if (transform.position.x <= -30 || transform.position.x >= 25) // checks if the car is on screen
        {
            Destroy(gameObject); // destroys it otherwise
        }

        if (currSpeed < startSpeed)
        {
            currSpeed += Time.deltaTime * 2;
        }

        changeSkin();

        musicPlayer.volume = controller.musicVol * controller.masterVol * 0.85f;

        if (isDisabled)
        {
            amDisabled();
        }
        else
        {
            checkInBounce();
        }
        if (isDestroyed)
        {
            amDestroyed();
        }

        if (makingTiny)
        {
            doTiny();
        }
        else if (makingBig)
        {
            doBig();
        }
    }

    public override void makeDisabled(float xF, float yF)
    {
        isDisabled = true;
        currSpeed = 0;
        startSpeed = 0;
        xForce = xF;
        yForce = yF;
    }

    void hitCar(Collider2D collision)
    {
        cars daCarhit = collision.GetComponent<cars>();
        if (isDisabled)
        {
            if (daCarhit.isDisabled)
            {
                if (xForce > daCarhit.xForce)
                {
                    daCarhit.xForce = xForce * Random.Range(0.65f, 0.85f);
                }
                if (yForce > daCarhit.yForce)
                {
                    daCarhit.yForce = yForce * Random.Range(0.65f, 0.85f);
                }
            }
            else
            {
                float xF = xForce; float yF = yForce;
                if (xF == 0) { xF = -1.5f; } //if(yF == 0) { yF = -0.5f; }
                daCarhit.makeDisabled(xF * 0.55f, yF * 0.55f);
                if (controller.isOver)
                {
                    controller.bannedLanes.Add(daCarhit.lane);
                }
            }

            xForce *= Random.Range(-0.9f, -0.65f);
            yForce *= -Random.Range(-0.9f, -0.65f);

            isHit = false;
        }
        else if (!isDestroyed && !daCarhit.isDestroyed && !daCarhit.isDisabled)
        {
            if (transform.position.y > (collision.transform.position.y + 0.4f))
            {
                turningSpeed *= -1;
            }
            else if (transform.position.y < (collision.transform.position.y - 0.4f))
            {
                turningSpeed *= -1;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                if (daCarhit.speed > 8)
                {
                    currSpeed = daCarhit.speed - 1; //changes the speed so cars won't go through eachother
                }
                else { daCarhit.speed = currSpeed + 1; }
            }
        }
    }

    private void spawnCoin()
    {
        if (coinTimer < 0)
        {
            Instantiate(coins, transform.position, Quaternion.identity, GameObject.Find("coins").transform);
            coinTimer = 0.75f;//Random.Range(0.5f, 1.15f);
        }
        if (isTiny) { coinTimer -= Time.deltaTime / 2; }
        else { coinTimer -= Time.deltaTime; }
    }

    private void checkInBounce()
    {
        if (transform.position.y <= -4.35 && turningSpeed > 0)
        {
            turningSpeed *= -1;
        }
        if (controller.topLane)
        {
            if (transform.position.y >= 0.65 && turningSpeed < 0)
            {
                turningSpeed *= -1;
            }
        } else
        {
            if (transform.position.y >= -0.45 && turningSpeed < 0)
            {
                turningSpeed *= -1;
            }
        }
        if (turningTimer < 0)
        {
            turningTimer = Random.Range(1.0f, 5.0f);
            turningSpeed *= -1;
        }
        turningTimer -= Time.deltaTime;
    }

    private void changeSkin()
    {
        partyBeatTimer += Time.deltaTime;
        if (partyBeatTimer > partyBeatTime)
        {
            partyList++;
            if (partyList >= partySkins.Length)
            {
                partyList = 0;
            }
            party.sprite = partySkins[partyList];
            partyBeatTimer -= partyBeatTime;
        }
    }
}
