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
    public int hits;
    public int hitsStart;

    public Sprite bodySprite;
    public Sprite windowSprite;
    public Color windowTint;
    public Sprite wheelSprite;
    public Sprite liveryMaskSprite;
    public Sprite liverySprite;
    public Color liveryColor;
    public AudioClip engineStart;

    public SpriteRenderer body;
    public SpriteRenderer window;
    public SpriteMask liveryMask;
    public SpriteRenderer livery;
    public SpriteRenderer wheelF;
    public SpriteRenderer wheelB;
    public ParticleSystem crashSmoke;

    public bool tapped;
    public Vector3 firstTapPoint;
    public bool inPos;
    public bool slidingUp;
    public bool slidingDown;
    public bool newTap;
    public Transform nearCar;

    public float swipeDistToDetect;

    public Vector3 targetPos; //where the player car has to go
    public Vector3 turnPos; //where the player car has to go
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    public float startPos;

    public AudioClip turnsUp;
    public AudioClip turnsDown;
    public AudioClip[] hitsAudio;

    public speedometer speedo;

    public float crashForce;

    public float hitboxSizeX;
    public float hitboxSizeY;
    public BoxCollider2D hitbox;

    public AudioClip manhole;

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
    public float teleBoltTime;
    public float teleBoltTimer;
    public GameObject teleBolt;
    public Color32 teleBoltColor;
    public AudioClip teleportSound;
    public AudioClip teleportHumm;
    public AudioClip teleportStart;
    public AudioClip teleportEnd;

    public bool inShield;
    public bool shieldReady;
    public bool shieldWhenHit;
    public float shieldTimer;
    public GameObject shieldAniOBJ;
    public shieldAni shieldAni;
    public AudioClip shieldForce;
    public AudioClip shieldStart;
    public AudioClip shieldEnd;

    public bool ramOn;
    public GameObject ramOBJ;
    public carRam ram;

    public bool inBoost;
    public bool boosting;
    public float boostLeft;
    public float tempMPH;
    public GameObject boostOBJ;
    public carBoost boost;

    public bool inRocket;
    public bool rocketLanding;
    public float landingTimer;
    public GameObject rocketOBJ;
    public rocketBoost rocket;

    public GameObject laserGun;
    public laser carLaser;
    public bool laserAutoShoot;

    public float turnMulti;

    public bool autoShield;
    public bool beginBoost;
    public int beginBoostTarget;
    public float beginBoostTime;
    public float beginBoostScoreMPH;

    public int statLane;
    public int statTurns;

    public GameObject crashAudio;

    public AudioSource sndSource;

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
        hitbox = GetComponent<BoxCollider2D>();

        hitboxSizeX = hitbox.size.x;
        hitboxSizeY = hitbox.size.y;
        turnMulti = 1;

        startPos = -7f;
        swipeDistToDetect = 0.25f;

        crashForce = 1.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            if (!beginBoost)
            {
                controller.laneTimes[statLane] += Time.deltaTime;
                if (startPos == transform.position.x && controller.scoreShowing && controller.textNum >= 10)
                {
                    if (!(inRocket && rocket.boosting))
                    {
                        if (inTeleport)
                        {
                            teleport();
                        }
                        else
                        {
                            slideLanes();
                        }
                    }
                }
                else
                {
                    transform.position += new Vector3(3 * (moveTime) * Time.deltaTime, 0, 0);
                    if (startPos - transform.position.x < 0)
                    {
                        transform.position = new Vector3(startPos, transform.position.y, 0);
                        targetPos = transform.position;
                        inPos = true;
                    }
                }

                if (inShield)
                {
                    shieldTimer -= Time.deltaTime;

                    if (shieldTimer < 1 && !shieldAni.doFadeOut)
                    {
                        shieldAni.startFade(false);
                    }

                    if (shieldTimer < 0)
                    {
                        endShield();
                    }
                }

                if (boosting)
                {
                    doBoost();
                }

                if (controller.laserOn && laserAutoShoot && !carLaser.inCooldown)
                {
                    shootCloseCar(1.35f);
                }

                checkPowerUp();
            }
            else
            {
                doBeginBoost();
            }
        }

        if (rocketLanding)
        {
            landingRocket();
        }

        if (!inRocket)
        {
            wheelB.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
            wheelF.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
        }
    }

    void checkPowerUp()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0) // if the user touches the phone screen
        {
            if (tapped)
            {
                Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
                if ((tapPoint.x - firstTapPoint.x) > 0.45f)
                {
                    if (Mathf.Abs(transform.position.y - tapPoint.y) < 1.5f)
                    {
                        usePowerup();
                    }
                }
            }
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
                if (firstTapPoint.y < 3.0f || inRocket)
                {
                    if ((tapPoint.y - firstTapPoint.y) > swipeDistToDetect) //if the tap if above the player car
                    {
                        laneUp(turnMulti);
                    }
                    else if ((tapPoint.y - firstTapPoint.y) < -swipeDistToDetect) //if the tap is below the player car
                    {
                        laneDown(turnMulti);
                    }
                }
                newTap = false;
            }
            else if (newTap)
            {
                tapped = true;
                firstTapPoint = tapPoint;
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
            if(nearCar != null) { controller.closeHits++; }
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
        if(teleportCharges <= 0 && tapped)
        {
            endTeleport();
        }
        if (teleportCharges > 1)
        {
            boltAni();
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
            Instantiate(teleportEffect, transform.position, Quaternion.identity, GameObject.Find("misc backround").transform);
            teleLocation = teleLane;
            Animator newEffect = Instantiate(teleportEffect, new Vector3(transform.position.x, (teleLane * -1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("misc backround").transform).GetComponent<Animator>();
            newEffect.Play("tele light exit");
            AudioSource.PlayClipAtPoint(teleportSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);

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
        targetPos = transform.position;
        inPos = true;
        AudioSource.PlayClipAtPoint(teleportEnd, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        sndSource.clip = null;
    }

    public void enterTeleport(int uses, float boltTime, Color32 boltColor, bool destr, bool affect)
    {
        teleportCharges = uses;
        affectCharge = affect;
        beginTeleport = true;
        destroyObstacle = destr;
        teleTime = 0.25f;
        teleTime2 = 0.35f;
        teleBoltTime = boltTime;
        teleBoltTimer = boltTime;
        teleBoltColor = boltColor;

        AudioSource.PlayClipAtPoint(teleportStart, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        sndSource.clip = teleportHumm;
        sndSource.volume = controller.sfxVol * controller.masterVol;
        sndSource.Play();
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
        setOrder(lane);
        newLane = lane;
    }

    bool boltSide = false;
    void boltAni()
    {
        teleBoltTimer += Time.deltaTime;
        if(teleBoltTimer > teleBoltTime)
        {
            float boltPosX = Random.Range(2.0f, 4.5f);
            float boltPosY = 1.5f - getValueScale(boltPosX, 2, 4.5f, 1.5f);
            float boltRos = 120 - getValueScale(boltPosX, 2, 4.5f, 70);
            if (boltSide)
            {
                boltPosX *= -1;
                boltRos = 310 - boltRos;
            }
            boltSide = !boltSide;
            GameObject bolt = Instantiate(teleBolt, transform);
            bolt.transform.localPosition = new Vector3(boltPosX, boltPosY, 0);
            bolt.transform.localEulerAngles = new Vector3(0, 0, boltRos);
            bolt.GetComponent<SpriteRenderer>().color = teleBoltColor;
            teleBoltTimer = 0;
        }
    }

    public void startShield(float time, bool autoActive, bool activeWhenHit)
    {
        shieldTimer = time;
        shieldReady = true;
        shieldAni = Instantiate(shieldAniOBJ, transform.position, Quaternion.identity, transform).GetComponent<shieldAni>();
        shieldAni.startFade(true);
        if (autoActive)
        {
            inShield = true;
            shieldAni.startAni();
            sndSource.clip = shieldForce;
            sndSource.volume = controller.sfxVol * controller.masterVol;
            sndSource.Play();
        }
        shieldWhenHit = activeWhenHit;

        AudioSource.PlayClipAtPoint(shieldStart, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    void useAutoShield()
    {
        inShield = true;
        shieldTimer = 20;
        shieldReady = true;
        shieldAni = Instantiate(shieldAniOBJ, transform.position, Quaternion.identity, transform).GetComponent<shieldAni>();
        shieldAni.startFade(true);
        inShield = true;
        shieldAni.startAni();
        autoShield = false;
    }

    public void makePermMagnet()
    {
        magnetField newMagnet = Instantiate(controller.pwManage.magneticField, transform.position, Quaternion.identity, transform).GetComponent<magnetField>();
        newMagnet.carPoint = transform;
        newMagnet.isPerm = true;
        newMagnet.lifetime = 1;
        newMagnet.setSize(1.95f);
        newMagnet.getHolo = true;
    }

    public void startBeginBoost(int targScore, float scoremph)
    {
        beginBoost = true;
        beginBoostTarget = targScore;
        beginBoostTime = 0.75f;
        beginBoostScoreMPH = scoremph;
    }

    void doBeginBoost()
    {
        if (!(startPos == transform.position.x && controller.scoreShowing && controller.textNum >= 10))
        {
            transform.position += new Vector3(3 * (moveTime) * Time.deltaTime, 0, 0);
            if (startPos - transform.position.x < 0)
            {
                transform.position = new Vector3(startPos, transform.position.y, 0);
                targetPos = transform.position;
                inPos = true;
            }
        }

        float endTime = 0.75f;
        if (controller.score > beginBoostTarget - 50)
        {
            tempMPH = startMph + getValueScale(beginBoostTime, 0, endTime, 200 - startMph);
            controller.updateTint(new Color32(255, 0, 0, (byte)getValueScale(beginBoostTime, 0, endTime, 200)));
            beginBoostTime -= Time.deltaTime;
            if (tempMPH < startMph)
            {
                controller.scoremph = startMph;
            }
            else
            {
                controller.scoremph = tempMPH;
            }
        }
        else
        {
            tempMPH = 250;
            controller.scoremph = beginBoostScoreMPH;
            controller.updateTint(new Color32(255, 0, 0, 200));
        }
        controller.mph = tempMPH;
        if (beginBoostTime < 0)
        {
            endbeginBoost();
        }
    }

    void endbeginBoost()
    {
        beginBoost = false;
    }

    void activateShield()
    {
        inShield = true;
        shieldAni.startAni();
        speedo.powerupIsTimed = true;
        sndSource.clip = shieldForce;
        sndSource.volume = controller.sfxVol * controller.masterVol;
        sndSource.Play();
    }

    void endShield()
    {
        shieldReady = false;
        inShield = false;
        if (!shieldAni.doFadeOut)
        {
            shieldAni.startFade(false);
        }
        AudioSource.PlayClipAtPoint(shieldEnd, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        sndSource.clip = null;
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

    public void startRocket(float power, float boostTime, float coinRate, bool makeHolo)
    {
        rocket = Instantiate(rocketOBJ, transform.position, Quaternion.identity, transform).GetComponent<rocketBoost>();
        int carTypeSave = PlayerPrefs.GetInt("playerCarType", 0); //grabes the id of the car type the player last used
        rocket.setTargetPos(pManager.carPartsData.carTypes[carTypeSave].rocketX, pManager.carPartsData.carTypes[carTypeSave].rocketY, pManager.carPartsData.carTypes[carTypeSave].rocketX2);
        rocket.startBoost(power, boostTime, coinRate, makeHolo);
        setOrder(statLane + 5);
    }

    void landingRocket()
    {
        landingTimer += Time.deltaTime;
        if(landingTimer > 0.55f)
        {
            landRocket();
        }
    }

    void landRocket()
    {
        inRocket = false;
        targetPos = transform.position;
        rocketLanding = false;
        rocket.destroyed = true;
        rocket.transform.parent = null;
        rocket.setAni("rocket standby");
        rocket.newSound(rocket.boostEnd);
        sndSource.clip = null;
        AudioSource.PlayClipAtPoint(rocket.boostEnd, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        setOrder(findTapLane(targetPos.y));
    }

    public void startLaser(int shots, float fireRate, float cooldown, bool autoShoot)
    {
        carLaser = Instantiate(laserGun, transform.position, Quaternion.identity, transform).GetComponent<laser>();
        carLaser.startLaser(shots, fireRate, cooldown);
        int carTypeSave = PlayerPrefs.GetInt("playerCarType", 0); //grabes the id of the car type the player last used
        carLaser.setTargetPos(pManager.carPartsData.carTypes[carTypeSave].laserX, pManager.carPartsData.carTypes[carTypeSave].laserY);
        laserAutoShoot = autoShoot;
    }

    public void createOuline(sense sen)
    {
        SpriteRenderer newOutline = new GameObject("Sense Outline", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        newOutline.gameObject.transform.parent = transform;
        newOutline.sprite = liveryMaskSprite;
        newOutline.sortingOrder = 153;

        newOutline.transform.localScale = new Vector3(1, 1, 1);
        newOutline.transform.localPosition = new Vector3(0, 0, 0);

        newOutline.color = new Color32(0, 200, 200, 0);

        sen.playerOutline = newOutline;

        SpriteRenderer newWheelFOutline = new GameObject("Sense Outline", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        newWheelFOutline.gameObject.transform.parent = wheelF.gameObject.transform;
        newWheelFOutline.sprite = wheelSprite;
        newWheelFOutline.sortingOrder = 153;
        newWheelFOutline.transform.localScale = new Vector3(1, 1, 1);
        newWheelFOutline.transform.localPosition = new Vector3(0, 0, 0);
        newWheelFOutline.color = new Color32(0, 200, 200, 0);
        sen.playerWheelFOutline = newWheelFOutline;

        SpriteRenderer newWheelBOutline = new GameObject("Sense Outline", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        newWheelBOutline.gameObject.transform.parent = wheelB.gameObject.transform;
        newWheelBOutline.sprite = wheelSprite;
        newWheelBOutline.sortingOrder = 153;
        newWheelBOutline.transform.localScale = new Vector3(1, 1, 1);
        newWheelBOutline.transform.localPosition = new Vector3(0, 0, 0);
        newWheelBOutline.color = new Color32(0, 200, 200, 0);
        sen.playerWheelBOutline = newWheelBOutline;
    }

    public void changeHitBox(float multi)
    {
        hitbox.size = new Vector2(hitboxSizeX * multi, hitboxSizeY * multi);
    }

    public void laneUp(float multiplier) //if tap is above player car
    {
        float maxLane = 0;
        if (inRocket && !rocketLanding) { maxLane = 10.64f; }
        else if (controller.topLane)
        {
            if (controller.topLaneTime < 130) { maxLane = 0.64f; }
            else { maxLane = -0.61f; }
        }
        else
        {
            if (controller.topLaneTime < 10) { maxLane = -0.61f; }
            else { maxLane = 0.64f; }
        }

        if (controller.playing)
        {
            if (controller.inTutorial && controller.tutorialSteps < 7)
            {
                    if ((controller.tutorialSteps > 2 || (controller.tutorialSteps == 1 && controller.canSwipeTutorialTimer())) && targetPos.y < -0.61f)
                    {
                        startSlide(true, multiplier);
                    }
            }
            else
            {
                if (targetPos.y < maxLane) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
                {
                    if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
                    {
                        startSlide(true, multiplier);
                    }
                }
            }
        }
    }

    public void laneDown(float multiplier)
    {
        if (controller.playing)
        {
            if (controller.inTutorial && controller.tutorialSteps < 7)
            {
                if ((controller.tutorialSteps > 2 || (controller.tutorialSteps == 2 && controller.canSwipeTutorialTimer())) && targetPos.y > -3.05f)
                {
                    startSlide(false, multiplier);
                }
            }
            else
            {
                if (inRocket)
                {
                    if (targetPos.y > 5.66f) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
                    {
                        if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
                        {
                            startSlide(false, multiplier);
                        }
                    }
                }
                else
                {
                    if (targetPos.y > -4.34f) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
                    {
                        if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
                        {
                            startSlide(false, multiplier);
                        }
                    }
                }
            }
        }
    }

    void startSlide(bool isUp, float multiplier)
    {
        if (Mathf.Abs(transform.position.y - targetPos.y) < 0.35f)
        {
            float dis = 0;
            if (isUp)
            {
                dis = 1.25f;
                changeOrder(-1);
                AudioSource.PlayClipAtPoint(turnsUp, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            }
            else
            {
                dis = -1.25f;
                changeOrder(1);
                AudioSource.PlayClipAtPoint(turnsDown, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            }
            targetPos += new Vector3(0, dis, 0); //changes targetPos to the new lane it needs to go to
            disMove = (moveTime * (1 / multiplier)); //calculates the speed the player car needs to go to switch lanes
            turnPos = transform.position;
            overshoot = 0; //resets overshoot

            slidingUp = isUp;
            slidingDown = !isUp;
            inPos = false;
            tapped = false;

            statTurns++;
            almostHit();

            if (controller.inTutorial)
            {
                if((controller.tutorialSteps == 1 && isUp) || (controller.tutorialSteps == 2 && !isUp))
                {
                    controller.resetSwipeTutorialTimer(2);
                }
            }
        }
    }


    void forceSlide(bool isUp, float multiplier)
    {
        float dis = 0; if (isUp)
        {
            dis = 1.25f;
            changeOrder(-1);
            AudioSource.PlayClipAtPoint(turnsUp, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        }
        else
        {
            dis = -1.25f;
            changeOrder(1);
            AudioSource.PlayClipAtPoint(turnsDown, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        }
        targetPos += new Vector3(0, dis, 0); //changes targetPos to the new lane it needs to go to
        disMove = (moveTime * (1 / multiplier)); //calculates the speed the player car needs to go to switch lanes
        turnPos = transform.position;
        overshoot = 0; //resets overshoot

        slidingUp = isUp;
        slidingDown = !isUp;
        inPos = false;
        tapped = false;

        statTurns++;
        almostHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller.playing && !(inRocket && rocket.boosting))
        {
            switch (collision.tag)
            {
                case "car":
                    hitCar(collision);
                    break;
                case "barrier":
                    hitBarrier(collision);
                    break;
                case "cone":
                    hitCone(collision);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (rocketLanding)
        {
            if (collision.tag == "car")
            {
                collision.GetComponent<cars>().makeDestroyed();
            }
        }
    }

    void hitCar(Collider2D collision)
    {
        cars carHit = collision.GetComponent<cars>();
        if (!inTeleport || tapped)
        {
            if (shieldReady)
            {
                if (!carHit.isDisabled)
                {
                    if (!inShield)
                    {
                        if (shieldWhenHit) { activateShield(); }
                        else
                        {
                            speedo.finishPowerup();
                            endShield();
                        }
                    }
                    disableCar(collision, 3.5f);
                }
            }
            else if(boosting && boost.hitProt)
            {
                if (!carHit.isDisabled) {

                    disableCar(collision, 2.15f);
                    boost.takeBoost();
                }
            } else if (carHit.isTiny && !carHit.isDisabled)
            {
                disableCar(collision, 7.5f);
            }
            else if (rocketLanding)
            {
                carHit.makeDestroyed();
            } else if (beginBoost)
            {
                disableCar(collision, 2.15f);
            }
            else if (ramOn)
            {
                if (!carHit.isDisabled)
                {
                    if (!ram.justCars || carHit.isCar)
                    {
                        if (!ram.headOn || Mathf.Abs(transform.position.y - collision.transform.position.y) < 0.35f)
                        {
                            disableCar(collision, 2.5f);
                            ram.ramHit();
                        }
                        else
                        {
                            takeHit(collision, carHit.hitPoint);
                        }
                    }
                    else
                    {
                        takeHit(collision, carHit.hitPoint);
                    }
                }
            }
            else
            {
                if (!carHit.isDisabled) { takeHit(collision, carHit.hitPoint); }
            }
        }
        else
        {
            carHit.makeDestroyed();
            if (!destroyObstacle)
            {
                takeHit(collision, carHit.hitPoint);
            }
        }
    }

    void takeHit(Collider2D collision, int hitPoint)
    {
        if (Mathf.Abs(collision.transform.position.magnitude - transform.position.magnitude) < 4)
        {
            hits -= hitPoint;
            if (hits < hitsStart)
            {
                crashSmoke.emissionRate = getValueScale(hitsStart - hits, 0, hitsStart, hitsStart * 3);
            }
            if (hits <= 0)
            {
                lethalHit(collision);
            }
            else
            {
                AudioSource.PlayClipAtPoint(hitsAudio[Random.Range(0, hitsAudio.Length)], new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            }

            if (collision.transform.position.y == transform.position.y)
            {
                collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(-0.75f, 0.75f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
            }
            else if (collision.transform.position.y < transform.position.y)
            {
                float forceFactor = (collision.transform.position.y - transform.position.y) * 1.5f;
                collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(forceFactor * 0.75f, forceFactor * 1.25f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
            }
            else if (collision.transform.position.y > transform.position.y)
            {
                float forceFactor = (collision.transform.position.y - transform.position.y) * 1.5f;
                collision.GetComponent<cars>().makeDisabled(crashForce, Random.Range(forceFactor * 0.75f, forceFactor * 1.25f)); //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
            }
        }
    }

    void lethalHit(Collider2D collision)
    {
        if (autoShield)
        {
            useAutoShield();
        }
        else
        {
            crashSmoke.emissionRate = 24;
            Instantiate(crashAudio).GetComponent<AudioSource>().volume = controller.masterVol * controller.sfxVol;
            controller.bannedLanes.Add(collision.GetComponent<cars>().lane);
            if (collision.transform.position.y < transform.position.y)
            {
                changeOrder(-1);
                controller.bannedLanes.Add(collision.GetComponent<cars>().lane - 1);
            }
            else if (collision.transform.position.y > transform.position.y)
            {
                changeOrder(1);
                controller.bannedLanes.Add(collision.GetComponent<cars>().lane + 1);
            }

            controller.gameOver(); //sets the game to its game over state

            GameObject rWheel = GameObject.Find("random bar(Clone)");
            if (rWheel != null && !rWheel.GetComponent<randomWheel>().doFadeOut)
            {
                rWheel.GetComponent<randomWheel>().startFade(false);
            }
        }
    }

    void disableCar(Collider2D collision, float multiplyer)
    {
        float forceFactor = (collision.transform.position.y - transform.position.y) * multiplyer;
        if (Mathf.Abs(forceFactor) < 0.45)
        {
            forceFactor = Random.Range(1.5f, 4.0f) * ((Random.Range(0, 2) * 2) - 1);
        }
        collision.GetComponent<cars>().makeDisabled(Random.Range(5, 10), Random.Range(forceFactor * 0.75f, forceFactor * 1.25f));
    }

    void hitBarrier(Collider2D collision)
    {
        if (transform.position.y > 0 && !slidingDown)
        {
            forceSlide(false, turnMulti * 2);
        }
    }

    void hitCone(Collider2D collision)
    {
        if (targetPos.y < transform.position.y && controller.inConstruction && !slidingUp)
        {
            forceSlide(true, turnMulti * 2);
        }
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
        float smokeLevel = collision.GetComponent<manhole>().getSmokeValue();
        if (smokeLevel > 500)
        {
            smokeLevel = 500;
        }

        if (!inShield)
        {
            if (controller.screenDistortTarget < smokeLevel / 500)
            {
                AudioSource.PlayClipAtPoint(manhole, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                controller.screenDistortTarget = smokeLevel / 500;
            }
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

        statLane = lane;
    }

    private void changeOrder(int change)
    {
        body.sortingOrder += change;
        window.sortingOrder += change;
        wheelF.sortingOrder += change;
        wheelB.sortingOrder += change;
        livery.sortingOrder += change;
        if (ramOn) { ram.gameObject.GetComponent<SpriteRenderer>().sortingOrder += change; }
        if (controller.laserOn) { carLaser.gameObject.GetComponent<SpriteRenderer>().sortingOrder += change; }

        statLane += change;
    }

    private void setOrder(int lane)
    {
        body.sortingOrder = 3 + lane;
        window.sortingOrder = 3 + lane;
        wheelF.sortingOrder = 3 + lane;
        wheelB.sortingOrder = 3 + lane;
        livery.sortingOrder = 4 + lane;
        if (ramOn) { ram.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3 + lane; }
        if (controller.laserOn) { carLaser.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3 + lane; }

        if (lane < 5)
        {
            statLane = lane;
        }
        else
        {
            statLane = lane % 5;
        }
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
        wheelSprite = pManager.wheels[wheelSave];
        windowSprite = pManager.windows[carTypeSave];
        windowTint = pManager.windowColors[tintSave];
        liveryMaskSprite = pManager.liveryMask[carTypeSave];
        liverySprite = pManager.livery[liverySave];
        liveryColor = pManager.liveryColors[liveryColorSave];
        engineStart = pManager.carStart[carTypeSave];

        wheelB.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[carTypeSave].wheelB, pManager.carPartsData.carTypes[carTypeSave].wheelHight, 0);
        wheelF.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[carTypeSave].wheelF, pManager.carPartsData.carTypes[carTypeSave].wheelHight, 0);
        livery.transform.localScale = new Vector3(pManager.carPartsData.carTypes[carTypeSave].scale, pManager.carPartsData.carTypes[carTypeSave].scale, 1);
        livery.transform.localPosition = new Vector3(0, pManager.carPartsData.carTypes[carTypeSave].liveryHight, 0);

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
        hits = calcHealth(carTypeSave);
        hitsStart = hits;
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

    public int calcHealth(int carTypeSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].hits;
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

    private void shootCloseCar(float min)
    {
        Transform closestCar = findNearestCar(min, transform.position.y, 1.35f);
        if (carLaser.carToDestroy == null && closestCar != null)
        {
            float closestDistX = closestCar.position.x - transform.position.x;
            float closestDistY = closestCar.position.y - transform.position.y;
            if ((closestDistX < 5.75f && closestDistX > min) && Mathf.Abs(closestDistY) < 1.35f)
            {
                carLaser.makeTarget(closestCar.gameObject.GetComponent<cars>());
            }
        }
    }

    private void almostHit()
    {
        Transform closestCar = findClosestCar();
        if (closestCar != null)
        {
            float closestDist = transform.position.x - closestCar.position.x;
            if ((closestDist < closestCar.gameObject.GetComponent<cars>().cutoffSize && closestDist > 1) && Mathf.Abs(closestCar.position.y - targetPos.y) < 0.1f)
            {
                nearCar = closestCar;
                closestCar.gameObject.GetComponent<cars>().nearCrash();
            }
        }
    }

    private Transform findNearestCar(float min, float yVal, float yTol)
    {
        try
        {
            Transform closest = controller.carsInGame[0].transform;
            for (int i = 1; i < controller.carsInGame.Count; i++)
            {
                Transform currCar = controller.carsInGame[i].transform;
                if (Mathf.Abs(currCar.position.y - yVal) < yTol)
                {
                    if (currCar.position.x > transform.position.x + min)
                    {
                        if (currCar.position.x < closest.position.x || closest.position.x < transform.position.x + min)
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

    private Transform findClosestCar()
    {
        Transform carsOBJ = GameObject.Find("cars").transform;
        try
        {
            Transform closest = carsOBJ.GetChild(0);
            for (int i = 1; i < carsOBJ.childCount; i++)
            {
                Transform currCar = carsOBJ.GetChild(i).transform;
                if (Mathf.Abs(currCar.position.y - targetPos.y) < 0.1f) // check position with floating point tolorence
                {
                    if (currCar.position.x < targetPos.x)
                    {
                        if (Mathf.Abs(closest.position.y - targetPos.y) < 0.1f && closest.position.x < targetPos.x) //checks if the current car is eligable
                        {
                            if (currCar.position.x > closest.position.x)
                            {
                                closest = currCar;
                            }
                        }
                        else
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