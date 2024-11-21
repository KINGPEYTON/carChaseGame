using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class police_Car : cars
{
    public playerCar playerCarOBJ;

    public bool chasingPlayer;

    public AudioSource siren;
    public AudioClip playerChase;
    public AudioClip vanChase;
    public float speedLimit;

    public bool chasingVan;
    public maniac_van vanOBJ;

    public GameObject iconOBJ;
    public GameObject iconInGame;
    public bool iconInPos;
    public float iconTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (chasingVan)
        {
            siren.clip = vanChase;
            siren.Play();
            GameObject.Find("carsManager").GetComponent<carsManager>();
            iconSpawn(vanOBJ.transform);
        }

        speed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        playerCarOBJ = GameObject.Find("playerCar").GetComponent<playerCar>();
        targPos = transform.position.y;

        controller.carsInGame.Add(gameObject);
        normalVal = 0.45f;

        turnTime = 0.75f;

        speedLimit = 80;

        if (controller.senseVision) {
            createOuline(controller.enhancedSense);
            if (chasingVan) { createIcon(controller.enhancedSense); }
        }

        isCar = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (chasingPlayer || chasingVan && !isDestroyed)
        {
            GetComponent<SpriteRenderer>().sprite = skins[((int)turningTimer) % 2];
            turnDown.SetActive(turningTimer % 2 < 1); //turns the down blinker on if it should
            turnUp.SetActive(turningTimer % 2 > 1); //turns the down blinker on if it should
            turningTimer += Time.deltaTime * 4;

            siren.volume = controller.sfxVol * controller.masterVol;
        }

        if (chasingVan && vanOBJ == null && !isDisabled)
        {
            destroyCar();
        }

        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                if (chasingPlayer)
                {
                    chasePlayer();
                }
                else if (!chasingVan || isDisabled)
                {
                    if (speed > 6f)
                    {
                        transform.position -= new Vector3(((controller.mph * Time.deltaTime) / speed), 0, 0); //move fowards in game
                    }
                    else
                    {
                        transform.position -= new Vector3(((controller.mph) * Time.deltaTime / 6f), 0, 0); //move fowards in game
                    }
                }
                if (transform.position.x < -16 && controller.mph > speedLimit && !playerCarOBJ.boosting && !playerCarOBJ.inRocket && !chasingPlayer)
                {
                    startPlayerChase();
                }

                if (controller.laserOn)
                {
                    getsShot();
                }
            }
            else
            {
                transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / speed), 0, 0); //car movment in the start up animation
            }
        }
        else
        {
            if (chasingPlayer)
            {
                slowdown(9.5f);
            }
            else if (!chasingVan || isDisabled)
            {
                transform.position += new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
            }
        }

        if (chasingVan && !isDisabled)
        {
            if (vanOBJ.isDisabled)
            {
                transform.parent = GameObject.Find("cars").transform;
                slowdown(4.5f);
            }
            else
            {
                chaseVan();
            }
        }

        if (transform.position.x <= -30 || (!controller.playing && transform.position.x >= 30)) // checks if the car is on screen
        {
            destroyCar(); // destroys it otherwise
        }

        if (iconInPos)
        {
            iconAni();
        }

        checkStuff();
    }

    void chasePlayer()
    {
        if (isDisabled)
        {
            transform.position -= new Vector3((Time.deltaTime / 6) * controller.mph, 0, 0); //move fowards in game
        }
        else
        {
            if (switchUp)
            {
                if (switchTimer > 0)
                {
                    switchTimer -= Time.deltaTime;
                    if (switchTimer < 0)
                    {
                        startSwitch(playerCarOBJ.targetPos.y, (2.0f / 3.0f));
                    }
                }
                else
                {
                    turnLanes();
                }
            }
            else
            {
                if (playerCarOBJ.targetPos.y != targPos)
                {
                    switchUp = true;
                }
            }

            if (transform.position.x < -12.5f)
            {
                transform.position += new Vector3(Time.deltaTime * 3.5f, 0, 0); //move fowards in game
            }
            else
            {
                transform.position += new Vector3(Time.deltaTime * 0.15f, 0, 0); //move fowards in game
            }
        }
    }

    void chaseVan()
    {
        if (switchDown)
        {
            if (switchTimer > 0)
            {
                switchTimer -= Time.deltaTime;
                if (switchTimer < 0)
                {
                    startSwitch(vanOBJ.transform.position.y, 2.85f);
                }
            }
            else
            {
                turnLanes();
            }
        }
        else
        {
            if (vanOBJ.transform.position.y != targPos)
            {
                switchDown = true;
            }
        }
        float lastY = transform.position.y;
        transform.position = new Vector3(vanOBJ.gameObject.transform.position.x - 5, lastY, 1);
    }

    void turnLanes()
    {

        float newY = 0;
        if (disMove != overshoot) { newY = startTurnPos + getValueScale(overshoot, 0, disMove, targPos - startTurnPos); }
        transform.position = new Vector3(transform.position.x, newY, 0);
        overshoot += Time.deltaTime;
        if (overshoot > disMove) //checks if its past if target
        {
            transform.position = new Vector3(transform.position.x, targPos, 0); //places car where it should be
            overshoot = disMove;
            switchDown = false; //turns down blinker off
            switchUp = false; //turns up blinker off
            setLane(); //update so the car knows what lane its in
            switchTimer = 0.35f + getValueScale(getValueRanged(controller.mph, speedLimit, 130), speedLimit, 130, 0.65f);
        }
    }

    void startSwitch(float newTarg, float mulitpliyer)
    {
        targPos = newTarg;

        disMove = Mathf.Abs(targPos - transform.position.y) * mulitpliyer; //calculates the speed the car needs to go to switch lanes
        overshoot = 0; //calculates overshoot to where it needs to go
        startTurnPos = transform.position.y;
    }

    void startPlayerChase()
    {
        chasingPlayer = true;
        chasingVan = false;
        transform.parent = GameObject.Find("cars").transform;
        siren.clip = playerChase;
        siren.Play();
        switchTimer = 0.35f + getValueScale(getValueRanged(controller.mph, speedLimit, 130), speedLimit, 130, 0.65f);
        targPos = playerCarOBJ.targetPos.y;
        transform.position = new Vector3(transform.position.x, playerCarOBJ.targetPos.y, 0);

        if(iconInGame != null) { Destroy(iconInGame); }
        iconSpawn(playerCarOBJ.transform);
    }

    void slowdown(float speeddown)
    {
        if (speed > 3)
        {
            speed -= Time.deltaTime * speeddown;
            if (speed < 3)
            {
                speed = 0;
            }
        }
        if (!isDisabled)
        {
            if (speed > 0)
            {
                transform.position += new Vector3(Time.deltaTime * (speed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
            }
            else
            {
                transform.position += new Vector3(0, 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            hitCar(collision);
        }
        else if (collision.tag == "barrier")
        {
            hitBarrier();
        }
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
                if (!isTiny || daCarhit.isTiny)
                {
                    float xF = xForce; float yF = yForce;
                    if (xF == 0) { xF = -1.5f; } //if(yF == 0) { yF = -0.5f; }
                    daCarhit.makeDisabled(xF * 0.55f, yF * 0.55f);
                    if (controller.isOver)
                    {
                        controller.bannedLanes.Add(daCarhit.lane);
                    }
                }
            }

            xForce *= Random.Range(-0.9f, -0.65f);
            yForce *= -Random.Range(-0.9f, -0.65f);

            isHit = false;
        }
        else if (!isDestroyed && !daCarhit.isDestroyed && !daCarhit.isDisabled)
        {
            if (chasingPlayer && transform.position.x > -12.5f)
            {
                makeDisabled(-0.4f, 0); //changes the speed so the cars crash
            }
            else if (chasingVan)
            {
                if (transform.position.y > collision.transform.position.y && !(daCarhit.switchUp || daCarhit.isDisabled) && daCarhit.lane > 0)
                {
                    daCarhit.switchDown = true;
                    daCarhit.startTurnPos = daCarhit.transform.position.y;
                    daCarhit.targPos -= 1.25f;
                    daCarhit.disMove = daCarhit.turnTime;
                    daCarhit.overshoot = 0; //calculates overshoot to where it needs to go
                }
                else if (transform.position.y < collision.transform.position.y && !(daCarhit.switchDown || daCarhit.isDisabled) && daCarhit.lane < 4)
                {
                    daCarhit.switchUp = true;
                    daCarhit.startTurnPos = daCarhit.transform.position.y;
                    daCarhit.targPos += 1.25f;
                    daCarhit.disMove = daCarhit.turnTime;
                    daCarhit.overshoot = 0; //calculates overshoot to where it needs to go
                }
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                if (daCarhit.inTraffic)
                {
                    speed = daCarhit.speed;
                    inTraffic = true;
                }
                else if (daCarhit.speed > 13 && !inTraffic)
                {
                    speed = daCarhit.speed - 1; //changes the speed so cars won't go through eachother
                }
                else { daCarhit.speed = speed + 1; }
            }
        }
    }

    public override void makeDisabled(float xF, float yF)
    {
        base.makeDisabled(xF, yF);
        if (chasingPlayer || chasingVan) { iconInPos = true; }
    }

    public override void destroyCar()
    {
        Destroy(iconInGame);
        base.destroyCar();
    }

    private void iconAni()
    {
        if (!isDisabled) { iconInGame.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)getValueScale(iconTimer, 0, 1, 220)); }
        else { iconInGame.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)(220 - getValueScale(iconTimer, 0, 1, 220))); }
        iconTimer += Time.deltaTime;
        if(iconTimer > 1)
        {
            iconTimer = 0;
            iconInPos = false;
            if (!isDisabled)
            {
                iconInGame.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 220);
            }
            else
            {
                Destroy(iconInGame);
            }
        }
    }

    private void iconSpawn(Transform iconCar)
    {
        iconInGame = Instantiate(iconOBJ, iconCar);
        iconInGame.transform.localPosition = new Vector3(0, 1.65f, 0);
        iconTimer = 0;
        iconInPos = true;
    }

    public override void createIcon(sense sen)
    {
        GameObject newOutline = new GameObject("Car Icon", typeof(SpriteRenderer), typeof(carIcon));

        newOutline.transform.localScale = new Vector3(0.165f, 0.165f, 1);
        if (chasingVan) { newOutline.transform.position = new Vector3(7, transform.position.y, 0); }
        else { newOutline.transform.position = new Vector3(9, transform.position.y, 0); }
        newOutline.GetComponent<SpriteRenderer>().sprite = warningIcon;
        newOutline.GetComponent<SpriteRenderer>().sortingOrder = 154;
        sen.carIcons.Add(newOutline);
        newOutline.GetComponent<carIcon>().carAttached = this;
        newOutline.GetComponent<carIcon>().sen = sen;
    }

    public override void nearCrash()
    {
        base.nearCrash();
        startPlayerChase();
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    float getValueRanged(float val, float min, float max)
    {
        float newVal = val;
        if (newVal > max) { newVal = max; } else if (val < min) { newVal = min; }
        return newVal;
    }
}