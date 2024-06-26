using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCar : MonoBehaviour
{
    public main controller;
    public playerManager pManager;

    public float startMph; // the mph the car will start at
    public float upMph; //how fast the car will speed up
    public float moveTime; // the time it shoul take the player car to switch lanes
    public float smokeMulitplyer;

    public Sprite bodySprite;
    public Sprite crashSprite;
    public Sprite windowSprite;
    public Color windowTint;
    public Sprite wheelSprite;
    public Sprite liveryMaskSprite;
    public Sprite liverySprite;
    public Color liveryColor;

    public SpriteRenderer body;
    public SpriteRenderer window;
    public SpriteMask liveryMask;
    public SpriteRenderer livery;
    public SpriteRenderer wheelF;
    public SpriteRenderer wheelB;

    public bool tapped;
    public float firstTapPoint;
    public bool inPos;
    public bool newTap;

    public float swipeDistToDetect;

    public Vector3 targetPos; //where the player car has to go
    public Vector3 turnPos; //where the player car has to go
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    public float startPos;

    public AudioClip[] turns;
    public AudioClip crash1;
    public AudioClip crash2;

    public speedometer speedo;
    public float powerTapTimer;

    public float crashForce;

    public bool inTeleport;
    public bool beginTeleport;
    public float teleTimer;
    public float teleTime;
    public float teleTime2;
    public int teleLocation;
    public int newLane;
    public float teleportTimer;
    public int teleportCharges;
    public bool affectCharge;
    public bool destroyObstacle;
    public GameObject teleportEffect;

    public bool inShield;
    public bool shieldReady;
    public float shieldTimer;

    public bool ramOn;
    public GameObject ramOBJ;
    public carRam ram;

    public bool inBoost;
    public bool boosting;
    public float boostLeft;
    public float tempMPH;
    public GameObject boostOBJ;
    public carBoost boost;

    void OnEnable()
    {
        pManager = GameObject.Find("playerManager").GetComponent<playerManager>();

        getPlayerCustomazation();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();

        startPos = -7f;
        swipeDistToDetect = 0.25f;

        crashForce = 1.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            if (startPos == transform.position.x && controller.scoreShowing && controller.textNum >= 10) {
                if (inTeleport)
                {
                    teleport();
                }
                else
                {
                    slideLanes();
                }
            } else {
                transform.position += new Vector3(3*(moveTime) * Time.deltaTime, 0, 0);
                if(startPos - transform.position.x < 0)
                {
                    transform.position = new Vector3(startPos, transform.position.y, 0);
                    targetPos = transform.position;
                    inPos = true;
                }
            }

            if (inShield)
            {
                shieldTimer -= Time.deltaTime;
                if(shieldTimer < 0)
                {
                    endShield();
                }
            }

            if (boosting)
            {
                doBoost();
            }

            checkPowerUp();
        }

        wheelB.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
        wheelF.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
    }

    void checkPowerUp()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0) // if the user touches the phone screen
        {
            Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
            if (Mathf.Abs(transform.position.x - tapPoint.x) < 2)
            {
                if (Mathf.Abs(transform.position.y - tapPoint.y) < 1.5f)
                {
                    powerTapTimer += Time.deltaTime;
                    if(powerTapTimer > 0.75f)
                    {
                        usePowerup();
                        powerTapTimer = 0;
                    }
                }
            }
        }
        else
        {
            powerTapTimer = 0;
        }
    }

    void usePowerup()
    {
        if(shieldReady && !inShield)
        {
            activateShield();
        } else if(inBoost && !boosting)
        {
            boost.useBoost();
        }
    }

    void slideLanes()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0) // if the user touches the phone screen
        {
            Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
            if (tapped)
            {
                if (firstTapPoint < 3.0f)
                {
                    if ((tapPoint.y - firstTapPoint) > swipeDistToDetect) //if the tap if above the player car
                    {
                        laneUp(1);
                    }
                    else if ((tapPoint.y - firstTapPoint) < -swipeDistToDetect) //if the tap is below the player car
                    {
                        laneDown(1);
                    }
                }
                newTap = false;
            }
            else if (newTap)
            {
                tapped = true;
                firstTapPoint = tapPoint.y;
            }
        }
        else if (Input.touchCount == 0)
        {
            tapped = false;
            newTap = true;
        }

        if (!inPos) //if player car isnt where its supose to be
        {
            slideCar();
        }
        
        if (beginTeleport && inPos)
        {
            readyTeleport();
        }
    }

    void slideCar()
    {
        Vector3 dis = new Vector3(targetPos.x - turnPos.x, targetPos.y - turnPos.y, 0);
        transform.position = calcPos(dis, turnPos, overshoot, disMove);
        overshoot += Time.deltaTime;
        if (overshoot > disMove) //checks if its past if target
        {
            transform.position = targetPos; //places player car where it should be
            inPos = true;
        }
    }

    void teleport()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            if (tapped)
            {
                if (newTap)
                {
                    Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
                    if (tapPoint.x < 0 && tapPoint.y < 2.5f)
                    {
                        setTeleport(findTapLane(tapPoint.y));
                    }
                }
            }
        }
        else
        {
            newTap = true;
        }
        if (!tapped)
        {
            doTeleport();
        }
        teleportTimer += Time.deltaTime;
        if (teleportTimer > 1)
        {
            useCharge();
            teleportTimer--;
        }
        if(teleportCharges <= 0)
        {
            endTeleport();
        }
    }

    int findTapLane(float yTap)
    {
        if (yTap < -3.65f)
        {
            return 4;
        }
        else if (yTap < -2.4f)
        {
            return 3;
        }
        else if (yTap < -1.15f)
        {
            return 2;
        }
        else if (yTap < 0.1)
        {
            return 1;
        }
        else if (controller.topLane)
        {
            if (controller.topLaneTime < 240)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (controller.topLaneTime < 10)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    void setTeleport(int teleLane)
    {
        if (newLane != teleLane)
        {
            tapped = false;
            newTap = false;
            teleTimer = 0;
            teleLocation = teleLane;
            if (affectCharge)
            {
                useCharge();
            }
        }
    }

    void doTeleport()
    {
        teleTimer += Time.deltaTime;
        if(teleTimer > teleTime && newLane != teleLocation)
        {
            teleportLane(teleLocation);
        } else if (teleTimer > teleTime2)
        {
            tapped = true;
        }
    }

    void endTeleport()
    {
        inTeleport = false;
        beginTeleport = false;
        tapped = true;
    }

    public void enterTeleport(int uses, bool destr, bool affect)
    {
        teleportCharges = uses;
        affectCharge = affect;
        beginTeleport = true;
        destroyObstacle = destr;
        teleTime = 0.15f;
        teleTime2 = 0.25f;
    }

    void readyTeleport()
    {
        inTeleport = true;
        tapped = true;
        newLane = -(int)((transform.position.y - 0.65f) / 1.25f);
    }

    void useCharge()
    {
        teleportCharges--;
        speedo.usePowerUp(1);
    }

    void teleportLane(int lane)
    {
        transform.position = new Vector3(startPos, (-lane * 1.25f) + 0.65f, 0);
        body.sortingOrder = 2 + lane;
        window.sortingOrder = 2 + lane;
        wheelF.sortingOrder = 2 + lane;
        wheelB.sortingOrder = 2 + lane;
        livery.sortingOrder = 3 + lane;
        ram.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3 + lane;
        newLane = lane;
    }

    void shieldAni()
    {

    }

    public void startShield(float time, bool autoActive)
    {
        shieldTimer = time;
        shieldReady = true;
        if (autoActive)
        {
            inShield = true;
        }
    }

    void activateShield()
    {
        inShield = true;
        speedo.powerupIsTimed = true;
    }

    void endShield()
    {
        shieldReady = false;
        inShield = false;
    }

    public void startRam(int uses, bool justCars, bool headOn)
    {
        ram = Instantiate(ramOBJ, transform.position, Quaternion.identity, transform).GetComponent<carRam>();
        int carTypeSave = PlayerPrefs.GetInt("playerCarType", 0); //grabes the id of the car type the player last used
        ram.setTargetPos(pManager.carPartsData.carTypes[carTypeSave].ramX, pManager.carPartsData.carTypes[carTypeSave].ramY);
        ram.uses = uses;
        ram.justCars = justCars;
        ram.headOn = headOn;
    }

    public void startBoost(int uses, float power, bool hitProt)
    {
        boost = Instantiate(boostOBJ, transform.position, Quaternion.identity, transform).GetComponent<carBoost>();
        int carTypeSave = PlayerPrefs.GetInt("playerCarType", 0); //grabes the id of the car type the player last used
        boost.setTargetPos(pManager.carPartsData.carTypes[carTypeSave].boostX, pManager.carPartsData.carTypes[carTypeSave].boostY);
        boost.uses = uses;
        boost.power = power;
        boost.hitProt = hitProt;
    }

    void doBoost()
    {
        float startTime = 0.35f;
        float endTime = 0.75f;
        if (boost.power - boostLeft < startTime)
        {
            tempMPH = boost.prevMPH + getValueScale(boost.power - boostLeft, 0, startTime, boost.targetMPH - boost.prevMPH);
            controller.updateTint(new Color32(255, 0, 0, (byte)getValueScale(boost.power - boostLeft, 0, startTime, 200)));
        }
        else if(boostLeft < endTime)
        {
            tempMPH = boost.prevMPH + getValueScale(boostLeft, 0, endTime, boost.targetMPH - boost.prevMPH);
            controller.updateTint(new Color32(255, 0, 0, (byte)getValueScale(boostLeft, 0, endTime, 200)));
            if (tempMPH < boost.prevScoreMPH)
            {
                controller.scoremph = boost.prevScoreMPH;
            }
            else
            {
                controller.scoremph = tempMPH;
            }
        }
        else
        {
            tempMPH = boost.targetMPH;
            controller.updateTint(new Color32(255, 0, 0, 200));
        }
        controller.mph = tempMPH;
        boostLeft -= Time.deltaTime;
        if(boostLeft < 0)
        {
            boost.finishBoost();
        }
        
    }

    public void laneUp(int multiplier) //if tap is above player car
    {
        float maxLane = 0;
        if (controller.topLane)
        {
            if (controller.topLaneTime < 130) { maxLane = 0.65f; }
            else { maxLane = -0.6f; }
        }
        else
        {
            if (controller.topLaneTime < 10) { maxLane = -0.6f; }
            else { maxLane = 0.65f; }
        }
        if (controller.inTutorial && controller.tutorialSteps < 3)
        {
            if(controller.tutorialSteps == 1 && controller.canSwipeTutorialTimer())
            {
                startSlide(true, multiplier);
            }
        }
        else
        {
            if (targetPos.y < maxLane && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
            {
                if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
                {
                    startSlide(true, multiplier);
                }
            }
        }
    }

    public void laneDown(int multiplier)
    {
        if (controller.inTutorial && controller.tutorialSteps < 3)
        {
            if (controller.tutorialSteps == 2 && controller.canSwipeTutorialTimer())
            {
                startSlide(false, multiplier);
            }
        }
        else
        {
            if (targetPos.y > -4.35f && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
            {
                startSlide(false, multiplier);
            }
        }
    }

    void startSlide(bool isUp, float multiplier)
    {
        if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
        {
            float dis = 0; 
            if (isUp) { dis = 1.25f; } else { dis = -1.25f; }
            targetPos += new Vector3(0, dis, 0); //changes targetPos to the new lane it needs to go to
            disMove = (moveTime * multiplier); //calculates the speed the player car needs to go to switch lanes
            turnPos = transform.position;
            if (isUp) { changeOrder(-1); } else { changeOrder(1); }
            overshoot = 0; //resets overshoot

            AudioSource.PlayClipAtPoint(turns[Random.Range(0, turns.Length - 1)], new Vector3(0, 0, -7), controller.masterVol * controller.sfxVol);
            inPos = false;
            tapped = false;

            playHorn();
        }
    }

    public void crash()
    {
        body.sprite = crashSprite; //car crashed
        controller.gameOver(); //sets the game to its game over state
        AudioSource.PlayClipAtPoint(crash1, new Vector3 (0,0,-10), controller.masterVol * controller.sfxVol);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller.playing)
        {
            switch (collision.tag)
            {
                case "car":
                    hitCar(collision);
                    break;
                case "barrier":
                    hitBarrier(collision);
                    break;
                case "coin":
                    hitCoin(collision);
                    break;
                case "smoke":
                    hitSmoke(collision);
                    break;
                case "power-up":
                    hitPowerUp(collision);
                    break;
            }
        }
    }

    void hitCar(Collider2D collision)
    {
        if (!inTeleport || tapped)
        {
            if (shieldReady)
            {
                if (!collision.GetComponent<cars>().isDisabled)
                {
                    if (!inShield)
                    {
                        speedo.finishPowerup();
                    }
                    float forceFactor = (collision.transform.position.y - transform.position.y) * 3.5f;
                    if (Mathf.Abs(forceFactor) < 0.45)
                    {
                        forceFactor = Random.Range(1.5f, 4.0f) * ((Random.Range(0, 2) * 2) - 1);
                    }
                    collision.GetComponent<cars>().makeDisabled(Random.Range(5, 10), Random.Range(forceFactor * 0.75f, forceFactor * 1.25f));
                }
            }
            else if (ramOn)
            {
                if (!collision.GetComponent<cars>().isDisabled)
                {
                    if (!ram.justCars || collision.GetComponent<cars>().isCar)
                    {
                        if (!ram.headOn || Mathf.Abs(transform.position.y - collision.transform.position.y) < 0.25f)
                        {
                            float forceFactor = (collision.transform.position.y - transform.position.y) * 2.5f;
                            if (Mathf.Abs(forceFactor) < 0.45)
                            {
                                forceFactor = Random.Range(1.5f, 4.0f) * ((Random.Range(0, 2) * 2) - 1);
                            }
                            collision.GetComponent<cars>().makeDisabled(Random.Range(5, 10), Random.Range(forceFactor * 0.75f, forceFactor * 1.25f));
                            ram.ramHit();
                        }
                        else
                        {
                            lethalHit(collision);
                        }
                    }
                    else
                    {
                        lethalHit(collision);
                    }
                }
            }
            if(boosting && boost.hitProt)
            {
                if (!collision.GetComponent<cars>().isDisabled) {
                    float forceFactor = (collision.transform.position.y - transform.position.y) * 2.5f;
                    if (Mathf.Abs(forceFactor) < 0.45)
                    {
                        forceFactor = Random.Range(1.5f, 4.0f) * ((Random.Range(0, 2) * 2) - 1);
                    }
                    collision.GetComponent<cars>().makeDisabled(Random.Range(5, 10), Random.Range(forceFactor * 0.75f, forceFactor * 1.25f));
                    boost.takeBoost();
                }
            }
            else
            {
                if (!collision.GetComponent<cars>().isDisabled) { lethalHit(collision); }
            }
        }
        else
        {
            collision.GetComponent<cars>().makeDestroyed();
            if (!destroyObstacle)
            {
                crash();
                controller.bannedLanes.Add(collision.GetComponent<cars>().lane);
            }
        }
    }

    void lethalHit(Collider2D collision)
    {
        controller.bannedLanes.Add(collision.GetComponent<cars>().lane);
        if (collision.transform.position.y == transform.position.y)
        {
            collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(-0.75f, 0.75f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
        }
        else if (collision.transform.position.y < transform.position.y)
        {
            changeOrder(-1);
            controller.bannedLanes.Add(collision.GetComponent<cars>().lane - 1);
            AudioSource.PlayClipAtPoint(crash2, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            float forceFactor = (collision.transform.position.y - transform.position.y) * 1.5f;
            collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(forceFactor * 0.75f, forceFactor * 1.25f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
        }
        else if (collision.transform.position.y > transform.position.y)
        {
            changeOrder(1);
            controller.bannedLanes.Add(collision.GetComponent<cars>().lane + 1);
            AudioSource.PlayClipAtPoint(crash2, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            float forceFactor = (collision.transform.position.y - transform.position.y) * 1.5f;
            collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(forceFactor * 0.75f, forceFactor * 1.25f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
        }
        crash(); //what happens when the player crashes
    }

    void hitBarrier(Collider2D collision)
    {
        laneDown(1);
    }

    void hitCoin(Collider2D collision)
    {
        if (!collision.GetComponent<coins>().collected)
        {
            collision.GetComponent<coins>().pickup();
        }
    }

    void hitSmoke(Collider2D collision)
    {
        float smokeLevel = collision.GetComponent<manhole>().getSmokeValue() * smokeMulitplyer;
        if (smokeLevel > 500)
        {
            smokeLevel = 500;
        }

        if (controller.screenDistortTarget < smokeLevel / 500)
        {

            controller.screenDistortTarget = smokeLevel / 500;
        }
    }

    void hitPowerUp(Collider2D collision)
    {
        if (!collision.GetComponent<powerUp>().popped)
        {
            collision.GetComponent<powerUp>().collect();
        }
    }

    public void setLane(int lane)
    {
        transform.position = new Vector3(-12, (-lane * 1.25f) + 0.65f, 0);
        body.sortingOrder = 2 + lane;
        window.sortingOrder = 2 + lane;
        wheelF.sortingOrder = 2 + lane;
        wheelB.sortingOrder = 2 + lane;
        livery.sortingOrder = 3 + lane;
    }

    private void changeOrder(int change)
    {
        body.sortingOrder += change;
        window.sortingOrder += change;
        wheelF.sortingOrder += change;
        wheelB.sortingOrder += change;
        livery.sortingOrder += change;
        if (ramOn) { ram.gameObject.GetComponent<SpriteRenderer>().sortingOrder += change; }
    }

    public void getPlayerCustomazation()
    {
        int carTypeSave = PlayerPrefs.GetInt("playerCarType", 0); //grabes the id of the car type the player last used
        int bodySave = PlayerPrefs.GetInt("playerBody", 0); //grabes the id of the body skin the player last used
        int wheelSave = PlayerPrefs.GetInt("wheelBody", 0); //grabes the id of the wheel the player last used
        int tintSave = PlayerPrefs.GetInt("windowTint", 0); //grabes the id of the tint the player last used
        int liverySave = PlayerPrefs.GetInt("liveryTint", 0); //grabes the id of the tint the player last used
        int liveryColorSave = PlayerPrefs.GetInt("liveryColorTint", 0); //grabes the id of the tint the player last used

        bodySprite = pManager.bodies[carTypeSave][bodySave];
        crashSprite = pManager.crashes[carTypeSave][bodySave];
        wheelSprite = pManager.wheels[wheelSave];
        windowSprite = pManager.windows[carTypeSave];
        windowTint = pManager.windowColors[tintSave];
        liveryMaskSprite = pManager.liveryMask[carTypeSave];
        liverySprite = pManager.livery[liverySave];
        liveryColor = pManager.liveryColors[liveryColorSave];

        wheelB.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[carTypeSave].wheelB, pManager.carPartsData.carTypes[carTypeSave].wheelHight, 0);
        wheelF.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[carTypeSave].wheelF, pManager.carPartsData.carTypes[carTypeSave].wheelHight, 0);

        setCarStats(carTypeSave, wheelSave, tintSave);
        setPlayerCustomazation();
    }

    private void setPlayerCustomazation()
    {
        body.sprite = bodySprite; //sets it to the non crashed skin
        wheelB.sprite = wheelSprite; //sets the back wheel to the correct skin
        wheelF.sprite = wheelSprite; //sets the front wheel to the correct skin
        window.sprite = windowSprite; //sets the window to the correct skin
        window.color = windowTint;
        liveryMask.sprite = liveryMaskSprite;
        livery.sprite = liverySprite;
        livery.color = liveryColor;
    }

    private void setCarStats(int carTypeSave, int wheelSave, int tintSave)
    {
        startMph = calcStartMPH(carTypeSave);
        upMph = calcUpMPH(carTypeSave, wheelSave);
        moveTime = calcmoveTime(carTypeSave, wheelSave);
        smokeMulitplyer = calcSmokeMulitplyer(tintSave);
    }

    public float calcStartMPH(int carTypeSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].startMPH;
    }

    public float calcUpMPH(int carTypeSave, int wheelSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].speedUp + pManager.carPartsData.wheelTypes[wheelSave].speedUp;
    }

    public float calcmoveTime(int carTypeSave, int wheelSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].moveTime + pManager.carPartsData.wheelTypes[wheelSave].moveTime;
    }

    public float calcSmokeMulitplyer(int tintSave)
    {
        return 1 - pManager.carPartsData.windowTints[tintSave].screenEffect;
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

    private void playHorn()
    {
        Transform closestCar = findClosestCar();
        if (closestCar != null)
        {
            float closestDist = transform.position.x - closestCar.position.x;
            //Debug.Log(closestDist + " : "+ closestCar.position.x);
            if ((closestDist < 2.75f && closestDist > 1) && closestCar.position.y == targetPos.y)
            {
                closestCar.gameObject.GetComponent<cars>().nearCrash();
            }
        }
    }

    private Transform findClosestCar()
    {
        Transform carsOBJ = GameObject.Find("cars").transform;
        try
        {
            Transform closest = carsOBJ.GetChild(0);
            for (int i = 1; i < carsOBJ.childCount; i++)
            {
                Transform currCar = carsOBJ.GetChild(i).transform;
                if (currCar.position.y == targetPos.y)
                {
                    if (currCar.position.x < targetPos.x)
                    {
                        if (currCar.position.x < closest.position.x)
                        {
                            closest = currCar;
                        }
                    }
                }
            }
            return closest;
        }
        catch
        {
            return null;
        }
    }
}