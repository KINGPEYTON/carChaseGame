using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour
{
    public main controller;

    public Sprite[] skins; //array of car skins

    public float speed; //speed of thr car
    public int lane; //lane of car
    public bool isCar;

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

    public bool isDestroyed;
    public float destroyedTimer;
    public float destroyedTime;
    public GameObject destroyedCar;
    public GameObject carDebris;
    public bool isDisabled;

    public float xForce;
    public float yForce;
    public float forceMass;
    public bool isHit;

    public bool isTiny;
    public bool makingTiny;
    public bool makingBig;
    public float tinyVal;
    public float normalVal;
    public float tinyTimer;

    public float laserTimer;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn

        controller.carsInGame.Add(gameObject);
        setLane();

        turnTime = 3.5f;
        switchLane(blinkTime);

        destroyedTime = 0.35f;
        normalVal = 0.45f;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                if (speed > 0)
                {
                    transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / speed), 0, 0); //move fowards in game
                }
                else
                {
                    transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / 3.5f), 0, 0); //move fowards in game
                }
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
                if (speed > 0)
                {
                    transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / speed), 0, 0); //car movment in the start up animation
                }
                else
                {
                    transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / 3.5f), 0, 0); //car movment in the start up animation
                }
            }

            if (blinkTime > -1) // checks if its a car that can turn
            {
                turnDown.SetActive(switchDown && turningTimer % 1 < 0.5f); //turns the down blinker on if it should
                turnUp.SetActive(switchUp && turningTimer % 1 < 0.5f); //turns the up blinker on if it should
                if (switchDown || switchUp) //checks if one of the blinkers are on
                {
                    turningTimer += Time.deltaTime;
                }

                if (!isDisabled)
                {
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

            if (controller.laserOn)
            {
                getsShot();
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
        }

        if (transform.position.x <= -15 || transform.position.x >= 15) // checks if the car is on screen
        {
            destroyCar(); // destroys it otherwise
        }

        if (isDestroyed)
        {
            amDestroyed();
        }
        else if (isDisabled)
        {
            amDisabled();
        }

        if (makingTiny)
        {
            doTiny();
        } else if (makingBig)
        {
            doBig();
        }
        
    }

    public void amDisabled()
    {

        turningTimer += Time.deltaTime;
        turnDown.SetActive(turningTimer % 1 < 0.5f); //turns the down blinker on if it should
        turnUp.SetActive(turningTimer % 1 < 0.5f); //turns the up blinker on if it should

        applyForce();
    }

    public void amDestroyed()
    {
        destroyedTimer += Time.deltaTime;
        if(destroyedTimer > destroyedTime)
        {
            Instantiate(carDebris, transform.position, Quaternion.identity, transform.parent);
            destroyCar();
        }
    }

    public void makeDisabled(float xF, float yF)
    {
        isDisabled = true;
        speed = 0;
        xForce = xF;
        yForce = yF;
    }

    public void makeDestroyed()
    {
        isDestroyed = true;
        isDisabled = true;
        speed = 0;
        destroyedTimer = 0;
        Instantiate(destroyedCar, transform.position, Quaternion.identity, transform.parent);
    }

    void applyForce()
    {
        if (xForce > 0)
        {
            xForce -= Time.deltaTime;
            if (controller.playing) { xForce -= Time.deltaTime * 2; }
            if (xForce < 0)
            {
                xForce = 0;
            }
        }
        else if (xForce < 0)
        {
            xForce += Time.deltaTime;
            if (xForce > 0)
            {
                xForce = 0;
            }
        }

        if (yForce > 0)
        {
            yForce -= Time.deltaTime;
            if (yForce < 0)
            {
                yForce = 0;
            }
        }
        else if (yForce < 0)
        {
            yForce += Time.deltaTime;
            if (yForce > 0)
            {
                yForce = 0;
            }
        }
        transform.position += new Vector3(xForce * Time.deltaTime / forceMass, yForce * Time.deltaTime / forceMass, 0);
    }

    public void getsShot()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            laser laserGun = controller.playerCar.carLaser;
            if (hit.collider != null && hit.collider.transform == transform && laserGun.carToDestroy == null)
            {
                if (!(laserGun.inCooldown || laserGun.inShot))
                    laserTimer += Time.deltaTime;
                if (laserTimer > 0.25f)
                {
                    laserGun.makeTarget(this);
                }
            }
            else
            {
                laserTimer = 0;
            }
        }
    }

    public void makeTiny(bool doInstant)
    {
        tinyVal = 0.22f;
        if (doInstant)
        {
            isTiny = true;
            transform.localScale = new Vector3(tinyVal, tinyVal, 1);
        }
        else
        {
            makingTiny = true;
            tinyTimer = 0;
        }
    }

    public void makeNormal()
    {
        isTiny = false;
        makingBig = true;
        tinyTimer = 0;
    }

    public void doTiny()
    {
        float tinyDiff = normalVal - tinyVal;
        float diffVal = 5f;
        tinyTimer += Time.deltaTime;

        if (tinyTimer < 0.5f)
        {
            float scaleValue = getValueScale(tinyTimer, 0, 0.5f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue + (tinyDiff * (2/3.0f));
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else if(tinyTimer < 1.0f)
        {
            float scaleValue = getValueScale(tinyTimer, 0.5f, 1.0f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue + (tinyDiff * (1 / 3.0f));
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else if(tinyTimer < 1.5f)
        {
            float scaleValue = getValueScale(tinyTimer, 1.0f, 1.5f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue;
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else {
            isTiny = true;
            makingTiny = false;
            transform.localScale = new Vector3(tinyVal, tinyVal, 1);
        }
    }

    public void doBig()
    {
        float tinyDiff = normalVal - tinyVal;
        float diffVal = 5f;
        tinyTimer += Time.deltaTime;

        if (tinyTimer < 0.5f)
        {
            float scaleValue = (tinyDiff / diffVal) - getValueScale(tinyTimer, 0, 0.5f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue;
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else if (tinyTimer < 1.0f)
        {
            float scaleValue = (tinyDiff / diffVal) - getValueScale(tinyTimer, 0.5f, 1.0f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue + (tinyDiff * (1 / 3.0f));
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else if (tinyTimer < 1.5f)
        {
            float scaleValue = (tinyDiff / diffVal) - getValueScale(tinyTimer, 1.0f, 1.5f, tinyDiff / diffVal);
            float valToScale = tinyVal + scaleValue + (tinyDiff * (2 / 3.0f));
            transform.localScale = new Vector3(valToScale, valToScale, 1);
        }
        else
        {
            makingBig = false;
            transform.localScale = new Vector3(normalVal, normalVal, 1);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            stayCar(collision);
        }
        else if (collision.tag == "Player" && isDisabled) //if a car hits another car
        {
            doIKMS();
        }
    }

    void hitBarrier()
    {
        if (isDisabled)
        {
            if(!isHit)
            yForce *= -1;
            isHit = true;
        }
        else
        {

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
                if(yForce > daCarhit.yForce)
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
            if (transform.position.y > collision.transform.position.y)
            {
                if (disMove < 0)
                {
                    targPos += 1.25f;
                    disMove *= -1;
                }
                switchUp = false;
                overshoot = Mathf.Abs(targPos - transform.position.y); //calculates overshoot to where it needs to go
            }
            else if (transform.position.y < collision.transform.position.y)
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
                if (speed > 8)
                {
                    speed = daCarhit.speed - 1; //changes the speed so cars won't go through eachother
                }
                else { daCarhit.speed = speed + 1; }
            }
        }
    }

    void stayCar(Collider2D collision)
    {
        if (isDisabled)
        {
            doIKMS();
        }
        else
        {
            if (transform.position.y == collision.transform.position.y && transform.position.x < collision.transform.position.x)
            {
                speed -= Time.deltaTime * 3;
                collision.GetComponent<cars>().speed += Time.deltaTime * 4; //changes the speed so cars won't go through eachother
            }
        }
    }

    void doIKMS()
    {
        destroyedTimer += Time.deltaTime;
        if(destroyedTimer > 0.5f)
        {
            //makeDestroyed();
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

    void destroyCar()
    {
        controller.carsInGame.Remove(gameObject);
        Destroy(gameObject);
    }

    public virtual void nearCrash()
    {
        AudioSource.PlayClipAtPoint(horn, new Vector3(0, 0, -9), controller.masterVol * controller.sfxVol);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y);
        return new Vector3(xVal, yVal, 1) + startScale;
    }
}
