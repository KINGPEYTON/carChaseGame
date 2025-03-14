using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
    public bool playing;
    public bool isOver;
    public float mph; //controls the speed of the game
    public float scoremph; //controls the speed of the scoreIncrese
    public bool inStartup;
    public float score;
    public float highScore;
    public bool newHighScore;

    public float upMPHmod;
    public float startMPHmod;
    public float boostDist;

    public float screenDistort;
    public float screenDistortTarget;

    public float masterVol;
    public float sfxVol;
    public float musicVol;
    public float radioVol;

    public Image screenTint;

    public bool inTutorial;
    public int tutorialSteps;
    public GameObject tutorialHandOBJ;
    public tutorialHand activeHand;
    public List<string> tutorialTexts;
    public GameObject activeText;
    public float stepTimer;
    public GameObject tutorialCoin; //coin gameobject to 
    public GameObject tutorialPowerup; //coin gameobject to 

    public int coins; //amount of coins a player has collected in a game
    public int totalCoins; //amount of coins a player has in total

    public GameObject[] coin; //coin gameobject to 
    public GameObject[] coin2; //coin gameobject to spawn
    public float coinTimer; //
    public List<GameObject> coinList;

    public float coinSpawnMultiplier;
    public bool allHolo;
    public bool isBigCoinhuna;

    public bool laserOn;

    public GameObject pauseMenu;

    public GameObject scoreBlimp;
    public Vector3 blimpSpeed;
    public Vector3 newBlimpLocation;
    public TextMeshProUGUI scoreText;

    public float blimpMinX;
    public float blimpMaxX;
    public float blimpMinY;
    public float blimpMaxY;

    public float textTimer;
    public int textNum;
    public bool scoreShowing;

    public playerCar playerCar;
    public AudioClip startEngine;

    public powerUpManager pwManage;
    public bool powerupActive;
    public string activePowerup;
    public float powerupTimer;
    public float powerupMultiplier;
    public float powerupTime;

    public boostManager modMang;
    public bool modActive;
    public AudioClip modActivete;

    public GameObject road; //road gameobject to spawn
    public GameObject roadPart; //road gameobject to spawn
    public List<Sprite> roadSkins;
    public List<Sprite> roadPartSkins;
    public List<float> roadOdds;
    public List<float> roadOddsCurr;
    public Transform roadLast;
    public float roadDist = 2.0f;

    public GameObject manhole;
    public float manholeTimer;
    public float manholeSpawn;

    public GameObject planeAd;
    public float planeAdTimer;

    public GameObject bigPlane;
    public float bigPlaneTimer;

    public GameObject building; //building gameobject to spawn
    public List<Sprite> buildingSkins;
    public List<float> buildingsOdds;
    public GameObject backBillboard; //building gameobject to spawn
    public List<Sprite> backBillboardSkins;
    public List<float> buildingsCurrOdds;
    public float buildingList;

    public float buildingDist = 2.95f;
    public Transform buildingLast;

    public float distMulti;

    public GameObject buildingFront; //building gameobject to spawn
    public List<Sprite> buildingFrontSkins;
    public List<float> buildingsFrontOdds;
    public List<float> buildingsFrontCurrOdds;

    public GameObject frontBillboard; //building gameobject to spawn
    public GameObject bigFrontBillboard; //building gameobject to spawn
    public List<Sprite> frontBillboardSkins;
    public List<Sprite> frontBigBillboardSkins;
    public List<float> frontBillboardOdds;
    public List<float> frontBillboardCurrOdds;
    public float buildingFrontList;
    public float frontBillboardList;

    public float frontBuildingDist = 2.0f;
    public Transform frontBuildingLast;

    public GameObject skyline;
    public List<Sprite> skylineSkins;
    public List<float> skylineOdds;
    public List<float> skylineCurrOdds;
    public Transform skylineLast;
    public float skylineDist = 4.5f;

    public GameObject farBuilding;
    public List<Sprite> farBuildingSkins;
    public List<float> farBuildingsOdds;
    public List<float> farBuildingsCurrOdds;
    public float farBuildingDist = 3.25f;
    public Transform farBuildingLast;

    public GameObject cloud;
    public float cloudTimer;

    public List<GameObject> billboards;

    public GameObject guard; //rail guard gameobject to spawn
    public GameObject guard2; //rail guard gameobject to spawn
    public Transform guardLast;
    public float guardDist = 3.25f;

    public worldManager worldM;
    public List<float> signOdds;
    public List<float> signCurrOdds;
    public List<GameObject> funSigns;
    public float signTimer;
    public float signTime;

    public GameObject[] milestoneSigns;
    public GameObject milestoneBigSign;
    public int milestone;
    public bool topSign;

    public GameObject exitSign;
    public int exitNum;
    public bool exitNumUp;
    public GameObject bridgeSign;

    public List<GameObject> carsInGame;
    public bool inTinyCars;
    public bool allTinyCars;

    public bool senseVision;
    public sense enhancedSense;

    public float obstaclesMultiplyer;

    public carsManager caManager;
    public int carList; //how many cars have spawned since the last bus
    public float carTimer;
    public float carTime;
    public float carTimerMultiplyer;
    public bool isSlowdown;
    public float largeCarOdds;
    public float specalCarOdds;
    public float carPlace;

    public List<GameObject> carsLargeList;
    public List<float> carsLargeOdds;
    public List<float> carsLargeCurrOdds;

    public GameObject bus; //bus gmaeobject to spawn
    public GameObject overBus; //bus gmaeobject to spawn

    public List<GameObject> carsSpecialList;
    public List<float> carsSpecialOdds;
    public List<float> carsSpecialCurrOdds;

    public List<float> bannedLanes;
    public List<int> carsPast;

    public GameObject settingsUI;
    public GameObject inventoryUI;

    public AudioClip menuAmbience;
    public AudioClip gameAmbience;
    public AudioSource menuSound;

    public AudioClip clickSound;

    public bool inConstruction;
    public construction constructionOBJ;
    public GameObject constructionArrow;
    public float constructionTimer;
    public GameObject workSign;

    public bool topLane;
    public float topLaneTime;
    public float topLaneTimer;

    public GameObject topCurrRoad;
    public GameObject topRoad;

    public GameObject exitLine;
    public GameObject mergeLine;
    public GameObject topLaneWhiteLine;
    public Transform topLaneLineLast;
    public float topLaneLineDist = 3.25f;

    public GameObject exitText;
    public GameObject mergeText;
    public int exitCount;

    public GameObject yelloBarrel;

    public byte areaEvent;
    public float areaTime;
    public float areaTimer;

    public bool startBridge;

    public GameObject bridgeBar;
    public GameObject bridgeBar2;
    public GameObject bridgebar3;
    public GameObject bridgeStart;
    public GameObject bridgeEnd;
    public GameObject bridgeStartConnector;
    public GameObject bridgeEndConnector;
    public GameObject bridgePillar;
    public GameObject bridgeSupport;
    public GameObject backroundPort;
    public float bridgeLineTimer;

    public GameObject bridgeBillboard;
    public Sprite bridgeBillboardSkin;
    public GameObject bridgeBigBillboard;
    public Sprite bridgeBigBillboardSkin;

    public statsManager statManage;
    public int topMPH;
    public float timePlayed;
    public int closeHits;
    public float[] laneTimes = new float[5];
    public int carsDisabled;
    public int carsDestroyed;
    public int powerUpsCollected;
    public int[] pwCollected;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        menuSound = GameObject.Find("ambience").GetComponent<AudioSource>();
        pwManage = GameObject.Find("powerUpManager").GetComponent<powerUpManager>();
        modMang = GameObject.Find("modsManager").GetComponent<boostManager>();
        caManager = GameObject.Find("carsManager").GetComponent<carsManager>();
        worldM = GameObject.Find("worldManager").GetComponent<worldManager>();
        statManage = GameObject.Find("statsManager").GetComponent<statsManager>();

        masterVol = PlayerPrefs.GetFloat("masterVol", 1); //sets high score to the one saved
        sfxVol = PlayerPrefs.GetFloat("sfxVol", 1); //sets high score to the one saved
        musicVol = PlayerPrefs.GetFloat("musicVol", 1); //sets the music volume to the one it was last on
        radioVol = PlayerPrefs.GetFloat("radioVol", 0.65f); //sets the radio volume to the one it was last on

        foreach (string s in worldM.tutText) { tutorialTexts.Add(s); }
        tutorialSteps = PlayerPrefs.GetInt("tutorialStep", 0); //sets the tutorial to the last step
        if (tutorialSteps < 10) 
        {
            inTutorial = true;
        }

        playing = false;
        isOver = false;
        scoreShowing = false;
        mph = 0; //sets inital mph
        inStartup = true;

        upMPHmod = 1;
        startMPHmod = 1;

        coins = 0;
        highScore = statManage.pstats.highScore;
        totalCoins = statManage.pstats.currCoins;

        carTimerMultiplyer = 1;
        obstaclesMultiplyer = 1;
        coinSpawnMultiplier = 1;
        powerupMultiplier = 1;

        largeCarOdds = 0.05f;
        specalCarOdds = 0.01f;

        carTime = 80;
        carPlace = 12;
        powerupTime = Random.Range(450, 1600);

        milestone = 0;
        signTime = 175f;
        blimpSpeed = new Vector3(0.1f, 0.05f, 0);

        topLaneTimer = Random.Range(550, 2250);
        areaTimer = Random.Range(750, 2500);
        constructionTimer = Random.Range(850, 1475);

        bigPlaneTimer = Random.Range(15, 65);

        menuSound.clip = menuAmbience;
        menuSound.Play();

        setRoadOdds();
        setBackBuildingOdds();
        setFrontBuildingOdds();
        setFarBuildingOdds();
        setSkylineOdds();
        setSignOdds();

        if (inTutorial)
        {
            if (tutorialSteps == 0)
            {
                startTutorial();
            }
            else if(tutorialSteps == 9)
            {
                swipeTutorialTimer = 5;
                swipeTutorial();
            }

        }

        caManager.spawnRegularCar(new Vector3(0, 0.65f, 0), GameObject.Find("cars").transform);
        caManager.spawnRegularCar(new Vector3(6, -4.35f, 0), GameObject.Find("cars").transform);
        caManager.spawnRegularCar(new Vector3(12, -0.6f, 0), GameObject.Find("cars").transform);
        caManager.spawnRegularCar(new Vector3(-6.5f, -3.1f, 0), GameObject.Find("cars").transform);
        //caManager.spawnPoliceCar(new Vector3(-6.5f, -0.6f, 0), GameObject.Find("cars").transform);

        exitNum = Random.Range(0, 100);
        if(exitNum < 30) { exitNumUp = true; }
        else if(exitNum > 70) { exitNumUp = false; }
        else { exitNumUp = Random.Range(0, 2) != 0; }

        pwCollected = new int[pwManage.powerupIDs.Count];
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) //if in a game 
        {
            timePlayed += Time.deltaTime;
            if (inStartup) //checks if the game is in its starting animation
            {
                mph += (mph/2 + 15.0f) * Time.deltaTime * upMPHmod;
                if(mph > playerCar.startMph * startMPHmod)
                {
                    inStartup = false;
                }
            }
            else
            {
                if (!inTutorial)
                {
                    if (!isSlowdown)
                    {
                        mph += playerCar.upMph * Time.deltaTime * upMPHmod; //graudully increeses the mph (+1 every 2 sec)
                    }
                    else
                    {
                        mph += playerCar.upMph * Time.deltaTime * 2.5f * upMPHmod;
                    }
                    if (mph > scoremph) { scoremph = mph; }
                    if ((int)mph > topMPH)
                    {
                        if (!(playerCar.boosting || playerCar.beginBoost))
                        {
                            topMPH = (int)mph;
                        }
                    }
                }
                else if(tutorialSteps > 6)
                {
                    mph += 1.25f * Time.deltaTime;
                }
            }

            if (!inTutorial)
            {
                score += (scoremph * 0.44704f) * Time.deltaTime; //increses the score based on how far the player has gone
            }

            //make backround elements
            if (areaEvent == 0)
            {
                spawnFrontBuilding();
                spawnBackBuilding();
                spawnManhole();
            }
            else if (areaEvent == 1)
            {
                spawnBridgeSupport();
                spawnBridgePillar();
            }
            spawnFarBuilding();
            spawnSkyline();
            spawnCloud();

            //make sky elements
            spawnAdPlane();
            //spawnBigPlane();

            //make game element
            if (!inTutorial || tutorialSteps > 6)
            {
                spawnCoin();
                newConstruction();
                newTopLane();
                changeArea();
                spawnGameCar();
                spawnPowerup();

                if (inTutorial)
                {
                    swipeTutorial();
                    switch (tutorialSteps)
                    {
                        case 7:
                            isStepTime(7.5f);
                            break;
                    }
                }
            }
            else
            {
                spawnTutorialWorld();
            }
            updateBillbord();

            //make road elements
            spawnroad();
            spawnGuard();

            //set blimp text
            blimpText();
        }
        else //if game isnt playing
        {
            spawnMenuCar();
            if (bannedLanes.Count == 0 && inTutorial && tutorialSteps < 3)
            {
                bannedLanes.Add(1);
                bannedLanes.Add(2);
                bannedLanes.Add(3);
            }
        }
        // update blimp
        if ((scoreBlimp.transform.localPosition.x < -9.5f + (distMulti * 3.5f) || scoreBlimp.transform.localPosition.x > -7.2f + (distMulti * 3)) && Mathf.Abs(scoreBlimp.transform.localPosition.x - newBlimpLocation.x) > 1.0) //if blimp went out of bounce on the X
        {
            updateBlimpX(); // find new target location for blimp
        }
        if ((scoreBlimp.transform.localPosition.y > 3.05f * (distMulti * 1.5f) || scoreBlimp.transform.localPosition.y < 2.9f * (distMulti * 1.35f)) && Mathf.Abs(scoreBlimp.transform.localPosition.y - newBlimpLocation.y) > 0.5) //if blimp went out of bounce on the X
        {
            updateBlimpY(); // find new target location for blimp
        }

        scoreBlimp.transform.localPosition += blimpSpeed * Time.deltaTime; // updates blimp position

        //makes milestone sign
        if (score > (milestone + 175))
        {
            createSign(0);
        }

        //makes fun sign
        signTimer -= Time.deltaTime * mph;
        if (signTimer < 0)
        {
            createSign(1);
            signTimer = signTime * Random.Range(0.85f, 1.15f);
        }

        menuSound.volume = masterVol;
    }

    void spawnroad()
    {
        try
        {
            if (roadLast.position.x < 11)
            {
                //spawns a new yellow lane road for each lane
                buildings newRoad;
                if (topLane && topLaneTime <= 0)
                {
                    newRoad = Instantiate(road, new Vector3(roadLast.position.x + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).GetComponent<buildings>();
                    newRoad.setSkin(getbuildingFromOdds(roadOdds, roadOddsCurr, roadSkins));
                }
                else
                {
                    newRoad = Instantiate(roadPart, new Vector3(roadLast.position.x + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).GetComponent<buildings>();
                    newRoad.setSkin(getbuildingFromOdds(roadOdds, roadOddsCurr, roadPartSkins));
                }
                roadLast = newRoad.gameObject.transform;
            }
        }
        catch {
            buildings newRoad;
            if (topLane)
            {
                newRoad = Instantiate(road, new Vector3(12+ roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).GetComponent<buildings>();
                newRoad.setSkin(getbuildingFromOdds(roadOdds, roadOddsCurr, roadSkins));
            }
            else
            {
                newRoad = Instantiate(roadPart, new Vector3(12 + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).GetComponent<buildings>();
                newRoad.setSkin(getbuildingFromOdds(roadOdds, roadOddsCurr, roadPartSkins));
            }
            roadLast = newRoad.gameObject.transform;
        }
    }

    void spawnGuard()
    {
        if (areaEvent == 0)
        {
            try
            {
                if (guardLast.position.x < 10)
                {
                    //spawns a new rail guard for the edge of the road
                    if (topLane)
                    {
                        Instantiate(guard2, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    }
                    else
                    {
                        if (topLaneTime >= 0)
                        {
                            Instantiate(guard2, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                        }
                        else
                        {
                            Instantiate(guard2, new Vector3(guardLast.position.x + guardDist, 0.25f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                        }
                    }
                    Transform newGuard = Instantiate(guard, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = newGuard;
                }
            }
            catch
            {
                if (topLane)
                {
                    Instantiate(guard2, new Vector3(12 + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                }
                else
                {
                    if (topLaneTime >= 0)
                    {
                        Instantiate(guard2, new Vector3(12 + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    }
                    else
                    {
                        Instantiate(guard2, new Vector3(12 + guardDist, 0.25f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    }
                }
                Transform newGuard = Instantiate(guard, new Vector3(12 + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                guardLast = newGuard;
            }
        }
        else if (areaEvent == 1)
        {
            try
            {
                if (guardLast.position.x < 12)
                {
                    //spawns a new rail guard for the edge of the road
                    Instantiate(bridgeBar, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgebar3, new Vector3(guardLast.position.x + (guardDist * 2), (8 * distMulti), 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = newBar;
                }
            }
            catch
            {
                Instantiate(bridgeBar, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                Instantiate(bridgebar3, new Vector3(guardLast.position.x + (guardDist * 2 * distMulti), (8 * distMulti), 0), Quaternion.identity, GameObject.Find("guards").transform);
                Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                guardLast = newBar;
            }
        }
    }

    void spawnManhole()
    {
        manholeTimer += Time.deltaTime * mph * obstaclesMultiplyer; //timer to spawn new road guard 
        if (manholeTimer > manholeSpawn)
        {
            int manholeLayer = 0;
            if (topRoad)
            {
                manholeLayer = Random.Range(1, 5);
            }
            else
            {
                manholeLayer = Random.Range(0, 5);
            }
            GameObject newManhole = Instantiate(manhole, new Vector3(12, (manholeLayer * -1.25f) + 0.55f, 0), Quaternion.identity, GameObject.Find("misc backround").transform);
            newManhole.gameObject.GetComponentInChildren<ParticleSystem>(false).GetComponent<Renderer>().sortingOrder = 2 + manholeLayer;
            //newManhole.transform.localScale = new Vector3((0.4f + (0.05f * manholeLayer)), (0.4f + (0.05f * manholeLayer)), 1);
            manholeTimer = 0;
            manholeSpawn = Random.Range(100, 350);
        }
    }

    void spawnAdPlane()
    {
        planeAdTimer += Time.deltaTime; //timer to spawn new road guard 
        if (planeAdTimer > 31)
        {
            //spawns a new rail guard for the edge of the road
            Instantiate(planeAd, new Vector3(12, 4.5f, 0), Quaternion.identity, GameObject.Find("misc backround").transform);
            planeAdTimer = 0;
        }
    }

    void spawnBigPlane()
    {
        bigPlaneTimer -= Time.deltaTime;
        if (bigPlaneTimer <= 0)
        {
            //spawns a new rail guard for the edge of the road
            Instantiate(bigPlane, GameObject.Find("misc backround").transform);
            bigPlaneTimer = Random.Range(28, 81);
        }
    }

    void spawnFrontBuilding()        //front buildings
    {
        try
        {
            if (frontBuildingLast.position.x < 10)
            {
                if (buildingFrontList < 4)
                {
                    buildings newbuildingFront = Instantiate(buildingFront, new Vector3(frontBuildingLast.position.x + (frontBuildingDist * distMulti), 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); ; //spawns new backround building
                    newbuildingFront.setSkin(getbuildingFromOdds(buildingsFrontOdds, buildingsFrontCurrOdds, buildingFrontSkins));
                    frontBuildingLast = newbuildingFront.gameObject.transform;
                    buildingFrontList++;
                    frontBuildingDist = 2.95f;
                }
                else
                {
                    if (frontBillboardList < 3)
                    {
                        GameObject bboard = Instantiate(frontBillboard, new Vector3(frontBuildingLast.position.x + ((frontBuildingDist + 0.5f) * distMulti), -0.75f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<buildings>().setSkin(getbuildingFromOdds(frontBillboardOdds, frontBillboardCurrOdds, frontBillboardSkins));
                        billboards.Add(bboard);
                        frontBuildingLast = bboard.gameObject.transform;
                        buildingFrontList = 0;
                        frontBillboardList++;
                        frontBuildingDist = 3.5f;
                    }
                    else
                    {
                        GameObject bboard = Instantiate(bigFrontBillboard, new Vector3(frontBuildingLast.position.x + ((frontBuildingDist + 2.0f) * distMulti), -1.4f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<billboard>().isBigBillboard = true;
                        bboard.GetComponent<buildings>().setSkin(getbuildingFromOdds(frontBigBillboardSkins));
                        billboards.Add(bboard);
                        frontBuildingLast = bboard.gameObject.transform;
                        buildingFrontList = 0;
                        frontBillboardList = 0;
                        frontBuildingDist = 5.0f;
                    }
                }
            }
        }
        catch
        {
            buildings newbuildingFront = Instantiate(buildingFront, new Vector3(13, 0.12f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); ; //spawns new backround building
            newbuildingFront.setSkin(getbuildingFromOdds(buildingsFrontOdds, buildingsFrontCurrOdds, buildingFrontSkins));
            frontBuildingLast = newbuildingFront.gameObject.transform;
            buildingFrontList++;
            frontBuildingDist = 2.75f;
        }
    }

    void spawnBackBuilding()        //backround buildings
    {
        try
        {
            if (buildingLast.position.x < 10)
            {
                if (buildingList < 6)
                {
                    buildings newBuilding = Instantiate(building, new Vector3(buildingLast.position.x + (buildingDist * distMulti), 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                    newBuilding.setSkin(getbuildingFromOdds(buildingsOdds, buildingsCurrOdds, buildingSkins));
                    buildingLast = newBuilding.gameObject.transform;
                    buildingList++;
                      buildingDist = 2.95f;
                }
                else
                {
                    billboard bboard = Instantiate(backBillboard, new Vector3(buildingLast.position.x + (buildingDist * distMulti) + 0.35f, 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<billboard>(); //spawns new backround building
                    bboard.isBigBillboard = true;
                    bboard.GetComponent<buildings>().setSkin(getbuildingFromOdds(backBillboardSkins));
                    buildingLast = bboard.gameObject.transform;
                    buildingList = 0;
                    buildingDist = 3.05f;
                }
            }
        }
        catch
        {
            buildings newBuilding = Instantiate(building, new Vector3(12, 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
            newBuilding.setSkin(getbuildingFromOdds(buildingsOdds, buildingsCurrOdds, buildingSkins));
            buildingLast = newBuilding.gameObject.transform;
            buildingList++;
            buildingDist = 2.95f;
        }
    }

    void spawnFarBuilding()        //far buildings
    {
        try
        {
            if (farBuildingLast.position.x < 10)
            {
                buildings newBuilding = Instantiate(farBuilding, new Vector3(farBuildingLast.position.x + farBuildingDist, 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                newBuilding.setSkin(getbuildingFromOdds(farBuildingsOdds, farBuildingsCurrOdds, farBuildingSkins));
                farBuildingLast = newBuilding.gameObject.transform;
            }
        }
        catch
        {
            buildings newBuilding = Instantiate(farBuilding, new Vector3(13, 0.05f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
            newBuilding.setSkin(getbuildingFromOdds(farBuildingsOdds, farBuildingsCurrOdds, farBuildingSkins));
            farBuildingLast = newBuilding.gameObject.transform;
        }
    }

    void spawnSkyline()        //skyline buildings
    {
        if (skylineLast.position.x < 10)
        {
            buildings newSkyline = Instantiate(skyline, new Vector3(skylineLast.position.x + (skylineDist * distMulti), skylineLast.position.y, 0), Quaternion.identity, GameObject.Find("skyline").transform).GetComponent<buildings>(); //spawns new backround building
            newSkyline.setSkin(getbuildingFromOdds(skylineOdds, skylineCurrOdds, skylineSkins));
            skylineLast = newSkyline.gameObject.transform;
        }
    }

    void spawnCloud()        //clouds

    {
        cloudTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (cloudTimer > 925)
        {
            Instantiate(cloud, new Vector3(13.5f, Random.Range(3.25f, 5.8f), 0), Quaternion.identity, GameObject.Find("clouds").transform); //spawns new backround building
            cloudTimer = 0;
        }
    }

    void newConstruction()
    {
        if (!inConstruction)
        {
            if (topLane) { constructionTimer -= Time.deltaTime * mph * obstaclesMultiplyer; }
            if (constructionTimer < 0)
            {
                inConstruction = true;
                GameObject newCar = Instantiate(constructionArrow, new Vector3(65, -4.35f, 0), Quaternion.identity, GameObject.Find("cars").transform);
                checkCarEffects(newCar);
            }
        }
    }

    void newTopLane()
    {
        if (areaEvent == 0 && !inConstruction) { topLaneTimer -= Time.deltaTime * mph; }
        if (topLaneTimer <= 0 && topLaneTime <= 0)
        {
            topLane = !topLane;
            topLaneTime = 250.0f;
        }
        if (!topLane)
        {
            if (topLaneTime > 0)
            {
                topLaneTime -= Time.deltaTime * mph;
                if (topLaneTime <= 0)
                {
                    Instantiate(exitLine, new Vector3(guardLast.position.x + 8.85f, 0.225f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    createSign(2);
                    if (exitNumUp) { exitNum++; } else { exitNum--; }
                    Instantiate(exitText, new Vector3(guardLast.position.x + 4.0f, 0.55f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    Instantiate(guard2, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard2, new Vector3(guardLast.position.x + guardDist + 3.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard, new Vector3(guardLast.position.x + guardDist + 3.25f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard2, new Vector3(guardLast.position.x + guardDist + 6.5f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard, new Vector3(guardLast.position.x + guardDist + 6.5f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard2, new Vector3(guardLast.position.x + guardDist + 9.75f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard, new Vector3(guardLast.position.x + guardDist + 9.75f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard2, new Vector3(guardLast.position.x + guardDist + 13.0f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(guard, new Vector3(guardLast.position.x + guardDist + 13.0f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Transform lastGuard = Instantiate(guard2, new Vector3(guardLast.position.x + guardDist + 16.25f, 0.25f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    Transform newGuard = Instantiate(guard, new Vector3(guardLast.position.x + guardDist + 16.25f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = newGuard;
                    topCurrRoad.GetComponent<sideBar>().movingOut = true;
                    topCurrRoad.GetComponent<sideBar>().lastGuard = lastGuard;
                    Instantiate(yelloBarrel, new Vector3(guardLast.position.x - 1.85f, 0.4f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    topLaneTimer = Random.Range(250, 750);
                }
                else
                {
                    try
                    {
                        if (topLaneLineLast.position.x < 10)
                        {
                            Transform newLine = Instantiate(topLaneWhiteLine, new Vector3(topLaneLineLast.position.x + topLaneLineDist, 0, 0), Quaternion.identity).transform;
                            topLaneLineLast = newLine;
                            increseExitCount(exitText, 1);
                        }
                    }
                    catch
                    {
                        Transform newLine = Instantiate(topLaneWhiteLine, new Vector3(guardLast.position.x + 1.7f, 0, 0), Quaternion.identity).transform;
                        topLaneLineLast = newLine;
                        increseExitCount(exitText, 0);
                        createSign(2);
                    }
                }

            }
        }
        else
        {
            if (topLaneTime > 0)
            {
                if (topCurrRoad == null)
                {
                    topCurrRoad = Instantiate(topRoad, new Vector3(guardLast.position.x + 15.75f, -2.02f, 0), Quaternion.identity);
                    Transform lastGuard = Instantiate(guard2, new Vector3(guardLast.position.x + guardDist, 0.25f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = Instantiate(guard, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;

                    Instantiate(mergeLine, new Vector3(lastGuard.position.x + 10.35f, 0.225f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    Instantiate(yelloBarrel, new Vector3(lastGuard.position.x + 1.7f, 0.4f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(mergeText, new Vector3(lastGuard.position.x + 16.25f, 0.55f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    buildings newRoad = Instantiate(roadPart, new Vector3(roadLast.position.x + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).GetComponent<buildings>();
                    newRoad.setSkin(getbuildingFromOdds(roadOdds, roadOddsCurr, roadPartSkins));
                    roadLast = newRoad.gameObject.transform;
                    Transform newLine = Instantiate(topLaneWhiteLine, new Vector3(lastGuard.position.x + 15.85f, 0, 0), Quaternion.identity).transform;
                    topLaneLineLast = newLine;
                    exitCount = 0;
                }
                topLaneTime -= Time.deltaTime * mph;
                if (topLaneTime <= 0)
                {
                    topLaneTimer = Random.Range(750, 2500);
                }
                else
                {
                    try
                    {
                        if (topLaneLineLast.position.x < 10)
                        {
                            Transform newLine = Instantiate(topLaneWhiteLine, new Vector3(topLaneLineLast.position.x + topLaneLineDist, 0, 0), Quaternion.identity).transform;
                            topLaneLineLast = newLine;
                            increseExitCount(mergeText, 1);
                        }
                    }
                    catch
                    {
                        Transform newLine = Instantiate(topLaneWhiteLine, new Vector3(guardLast.position.x + 1.7f, 0, 0), Quaternion.identity).transform;
                        topLaneLineLast = newLine;
                        increseExitCount(mergeText, 0);
                    }
                }

            }
        }
    }

    void increseExitCount(GameObject exitThingm, int count)
    {
        if (exitCount >= count)
        {
            Instantiate(exitThingm, new Vector3(topLaneLineLast.position.x, 0.55f, 0), Quaternion.identity);
            exitCount = 0;
        }
        else
        {
            exitCount++;
        }
    }

    void changeArea()
    {

        if ((!inTutorial || (tutorialSteps > 0 ^ tutorialSteps < 3)) && topLane) { areaTimer -= Time.deltaTime * mph; }
        if (areaTimer <= 0  && (topLane && topLaneTime <= 0))
        {
            if (areaEvent == 0 && buildingFrontList == 1) { areaEvent = 1; }
        }
        if (areaEvent == 1)
        {
            if (!startBridge)
            {
                Transform bridgeStarting = Instantiate(bridgeStart, new Vector3(guardLast.position.x + 2, 0.95f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform;
                Transform newSupport = Instantiate(bridgeSupport, new Vector3(bridgeStarting.position.x + 4.75f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform;
                buildingLast = newSupport;
                buildingDist = 1.5f;
                Instantiate(bridgeBar, new Vector3(guardLast.position.x + 1.5f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                Instantiate(bridgebar3, new Vector3(guardLast.position.x + 8.25f, 8 * distMulti, 0), Quaternion.identity, GameObject.Find("guards").transform);
                GameObject newBuilding = Instantiate(bridgePillar, new Vector3(bridgeStarting.position.x + 4.5f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                frontBuildingLast = newBuilding.transform;
                frontBuildingDist = 7.5f;
                Instantiate(bridgeStartConnector, new Vector3(guardLast.position.x - 1, -4.8f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                createSign(3);
                Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + 1.5f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                guardLast = newBar;
                guardDist = 4.3665f;
                buildingFrontList = 3;
                frontBillboardList = 1;
                areaTimer = Random.Range(600, 900);
                startBridge = true;
            }

        }
        else
        {
            if (areaEvent == 0)
            {
                if (startBridge)
                {
                    Transform bridgeEnding = Instantiate(bridgeEnd, new Vector3(frontBuildingLast.position.x + 6.5f, 0.95f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform;
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x - 2.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x + 2, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x + 6.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + -0.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + 0.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + 1.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
                    frontBuildingLast = bridgeEnding.transform;
                    frontBuildingDist = 1.75f;
                    Transform bridgeEndConn = Instantiate(bridgeEndConnector, new Vector3(frontBuildingLast.position.x + 4.25f, -4.8f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = bridgeEndConn;
                    guardDist = 3.25f;

                    Instantiate(bridgeBar2, new Vector3(frontBuildingLast.position.x - 7.75f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar2, new Vector3(frontBuildingLast.position.x - 4.5f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar2, new Vector3(frontBuildingLast.position.x - 1.25f, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar2, new Vector3(frontBuildingLast.position.x + 2, - 4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    buildingFrontList = 3;
                    startBridge = false;
                    areaTimer = Random.Range(2000, 4000);

                    GameObject bar3 = null;
                    Transform barTra = GameObject.Find("guards").transform;
                    for (int i = 0; i < barTra.childCount; i++)
                    {
                        if (barTra.GetChild(i).name == "bridge bar3(Clone)")
                        {
                            bar3 = barTra.GetChild(i).gameObject;
                        }
                    }
                    Destroy(bar3);
                }
            }
        }
    }

    void spawnBridgePillar()
    {
        try
        {
            if (frontBuildingLast.position.x < 8)
            {
                if (buildingFrontList < 1)
                {
                    GameObject newBuilding = Instantiate(bridgePillar, new Vector3(frontBuildingLast.position.x + (frontBuildingDist * distMulti), 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    frontBuildingLast = newBuilding.transform;
                    frontBuildingDist = 7.5f;
                    buildingFrontList++;
                }
                else
                {
                    if (frontBillboardList < 2)
                    {
                        GameObject bboard = Instantiate(bridgeBillboard, new Vector3(frontBuildingLast.position.x + ((frontBuildingDist + 1.0f) * distMulti), 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<billboard>().setSkin(bridgeBillboardSkin);
                        frontBuildingLast = bboard.transform;
                        frontBuildingDist = 9.5f;
                        billboards.Add(bboard);
                        buildingFrontList = 0;
                        frontBillboardList++;
                    }
                    else
                    {
                        GameObject bboard = Instantiate(bridgeBigBillboard, new Vector3(frontBuildingLast.position.x + ((frontBuildingDist + 2.5f) * distMulti), 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<billboard>().setSkin(bridgeBigBillboardSkin);
                        frontBuildingLast = bboard.transform;
                        frontBuildingDist = 11f;
                        bboard.GetComponent<billboard>().isBigBillboard = true;
                        billboards.Add(bboard);
                        buildingFrontList = 0;
                        frontBillboardList = 0;
                    }
                    if (areaEvent == 1 && areaTimer <= 0) { areaEvent = 0; }
                }
            }
        }
        catch
        {
            if (buildingFrontList < 1)
            {
                GameObject newBuilding = Instantiate(bridgePillar, new Vector3(13, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                frontBuildingLast = newBuilding.transform;
                frontBuildingDist = 7.5f;
                buildingFrontList++;
            }
            else
            {
                if (frontBillboardList < 2)
                {
                    GameObject bboard = Instantiate(bridgeBillboard, new Vector3(13 + 1.0f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<billboard>().setSkin(bridgeBillboardSkin);
                    frontBuildingLast = bboard.transform;
                    frontBuildingDist = 9.5f;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    frontBillboardList++;
                }
                else
                {
                    GameObject bboard = Instantiate(bridgeBigBillboard, new Vector3(13 + 1.0f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<billboard>().setSkin(bridgeBigBillboardSkin);
                    frontBuildingLast = bboard.transform;
                    frontBuildingDist = 9.5f;
                    bboard.GetComponent<billboard>().isBigBillboard = true;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    frontBillboardList = 0;
                }
            }
        }
    }

    void spawnBridgeSupport()
    {
        try
        {
            if (buildingLast.position.x < 11)
            {
                Transform newSupport = Instantiate(bridgeSupport, new Vector3(buildingLast.position.x + (buildingDist * distMulti), 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform; //spawns new backround building
                buildingLast = newSupport;
                buildingDist = 1.5f;
                buildingList++;
            }
        }
        catch
        {
            Transform newSupport = Instantiate(bridgeSupport, new Vector3(13, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform; //spawns new backround building
            buildingLast = newSupport;
            buildingDist = 1.5f;
            buildingList++;
        }
    }

    void spawnPowerup()
    {
        if (!powerupActive)
        {
            powerupTimer += Time.deltaTime * mph * powerupMultiplier;
            if (powerupTimer > powerupTime)
            {
                float newLane = 0;
                int maxLane = 1; if (topLane) { maxLane = 0; }
                int minLane = 5; if (inConstruction) { minLane = 4; }
                newLane = (Random.Range(-maxLane, -minLane) * 1.25f) + 0.85f;

                pwManage.createPowerup(getPowerupFromOdds(pwManage.powerupOdds, pwManage.powerupCurrOdds), new Vector3(15, newLane, 0));
                powerupTimer = 0;
                powerupTime = Random.Range(1200, 2250);
            }
        }
    }

    public void updateTint(Color32 color)
    {
        screenTint.color = color;
    }

    void spawnGameCar()
    {
        carTimer += (Time.deltaTime * mph) * carTimerMultiplyer * obstaclesMultiplyer; // time that spawns a new car that speeds up depending on the speed of the game (mph)
        if (carTimer > carTime)
        {
            float specalCar = Random.Range(0.0f, 1.0f);
            if (specalCar > largeCarOdds + specalCarOdds)
            {
                spawnNormalCar();
                largeCarOdds += 0.1f;
                specalCarOdds += 0.00175f;
            }
            else if (specalCar > largeCarOdds)
            {
                spawnSpecalCar();
                specalCarOdds = 0.01f;
            }
            else
            {
                spawnLargeCar();
                largeCarOdds = 0.05f;
            }
            carTimer = 0;
        }
    }

    void spawnNormalCar()
    {
        float newLane = 0;
        if (topLane)
        {
            newLane = (Random.Range(0, -5) * 1.25f) + 0.65f;
        }
        else
        {
            newLane = (Random.Range(-1, -5) * 1.25f) + 0.65f;
        }
        GameObject newCar = caManager.spawnRegularCar(new Vector3(carPlace, newLane, 0), GameObject.Find("cars").transform).gameObject;  //spawn new car in a random lane before going on screen
        checkCarEffects(newCar);
    }

    void spawnLargeCar()
    {
        float newLane = 0;
        if (topLane)
        {
            newLane = (Random.Range(0, -5) * 1.25f) + 0.65f;
        }
        else
        {
            newLane = (Random.Range(-1, -5) * 1.25f) + 0.65f;
        }
        GameObject newCar = caManager.spawnLargeCar(new Vector3(carPlace + 1, newLane, 0), GameObject.Find("cars").transform).gameObject;  //spawn new car in a random lane before going on screen
        checkCarEffects(newCar);
    }

    void spawnSpecalCar()
    {
        float newLane = 0;
        if (topLane)
        {
            newLane = (Random.Range(0, -5) * 1.25f) + 0.65f;
        }
        else
        {
            newLane = (Random.Range(-1, -5) * 1.25f) + 0.65f;
        }
        GameObject newCar = caManager.spawnSpecialCar(new Vector3(carPlace + 13, newLane, 0), GameObject.Find("cars").transform).gameObject;  //spawn new car in a random lane before going on screen
        checkCarEffects(newCar);
    }

    public void checkCarEffects(GameObject newCar)
    {
        if (senseVision && enhancedSense.showIcons) { newCar.GetComponent<cars>().createIcon(enhancedSense); }
        if (inTinyCars)
        {
            makeCarTiny(newCar, true);
        }
    }

    void spawnCoin()
    {
        coinTimer += Time.deltaTime * coinSpawnMultiplier * mph;
        if (coinTimer > 350)
        {
            if (topLane)
            {
                Instantiate(coin[Random.Range(0, coin.Length)], new Vector3(12, -1.85f, 0), Quaternion.identity, GameObject.Find("coins").transform);  //spawn new car in a random lane before going on screen
            }
            else
            {
                Instantiate(coin2[Random.Range(0, coin2.Length)], new Vector3(12, -1.85f, 0), Quaternion.identity, GameObject.Find("coins").transform);  //spawn new car in a random lane before going on screen
            }
            coinTimer = 0;
        }
    }

    void updateBillbord()
    {
        if (billboards.ToArray()[0].transform.position.x < -8.75f)
        {
            billboards.Remove(billboards.ToArray()[0]);
        }
    }

    Sprite getbuildingFromOdds(List<float> buildingsOddsList, List<float> buildingsCurrOddsList, List<Sprite> buildingSkinsList)
    {
        float newBuildingOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < buildingsOddsList.Count; i++)
        {
            oddsAccum += buildingsCurrOddsList[i];
            if (oddsAccum > newBuildingOdds)
            {
                changeOdds(i, buildingsOddsList, buildingsCurrOddsList);
                return buildingSkinsList[i];
            }
        }

        changeOdds(0, buildingsOddsList, buildingsCurrOddsList);
        return buildingSkinsList[0];
    }

    Sprite getbuildingFromOdds(List<Sprite> buildingSkinsList)
    {
        float newBuildingOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;
        float evenOdds = 1.0f / buildingSkinsList.Count;

        for (int i = 0; i < buildingSkinsList.Count; i++)
        {
            oddsAccum += evenOdds;
            if (oddsAccum > newBuildingOdds)
            {
                return buildingSkinsList[i];
            }
        }

        return buildingSkinsList[0];
    }

    int getPowerupFromOdds(List<float> oddsList, List<float> currOddsList)
    {
        float newBuildingOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < oddsList.Count; i++)
        {
            oddsAccum += currOddsList[i];
            if (oddsAccum > newBuildingOdds)
            {
                pwManage.changePowerupOdds(i);
                return i;
            }
        }

        pwManage.changePowerupOdds(0);
        return 0;
    }

    void blimpText()
    {
        if (!scoreShowing)
        {
            string scoretxt = scoreText.text = " Crashopolis";
            scoreText.text = scoretxt.Substring(textNum);

            if (textNum >= scoretxt.Length - 1)
            {
                textNum = 0;
                scoreShowing = true;
            }

        }
        else
        {
            string scoretxt = "Score: " + (int)score + "m";
            if (inTutorial) { scoretxt = "In Tutorial"; }

            if (textNum <= scoretxt.Length - 1)
            {
                scoreText.text = scoretxt.Substring(0, textNum);
            }
            else
            {
                scoreText.text = scoretxt;
            }
        }

        textTimer += Time.deltaTime;

        if (textTimer >= 0.1f)
        {
            textTimer -= 0.1f;
            textNum++;
        }
    }

    public void newGame()
    {
        if (inTutorial)
        {
            nextTutorialStep();
        }
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }

    public void gameOver()
    {
        playing = false; //sets the game to no longer be playing
        mph = 0; //stops the backround from moving (as the player is supose to be still)

        if (senseVision) { enhancedSense.startFade(false); }

        if (score > highScore) { //check if theres a new high score
            highScore = score; // sets the new high score
            newHighScore = true;
        }

        totalCoins += coins;

        isOver = true;

        GameObject.Find("Main Camera").GetComponent<AudioLowPassFilter>().cutoffFrequency = 2500;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().bypassListenerEffects = false;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().bypassEffects = false;

        screenDistortTarget = 0;

        statManage.addStats((int)score, timePlayed, coins, laneTimes, playerCar.statTurns, topMPH, closeHits, carsDisabled, carsDestroyed, powerUpsCollected, pwCollected, modMang.currSelect);

        gameGameOverButton();
    }

    public void StartGame() //offically start game
    {
        playing = true;

        if (!carsPast.Contains(2))
        {
            playerCar.setLane(2);
        }
        else if (!carsPast.Contains(1))
        {
            playerCar.setLane(1);
        }
        else {
            playerCar.setLane(3);
        }

        carTimer = 0;

        AudioSource.PlayClipAtPoint(playerCar.engineStart, new Vector3(0, 0, -10), masterVol * sfxVol);
        menuSound.clip = gameAmbience;
        menuSound.Play();

        if (inTutorial)
        {
            if (tutorialSteps == 0)
            {
                nextTutorialStep();
                playerCar.startMph = 30;
            }
            else if (tutorialSteps == 9)
            {
                nextTutorialStep();
                inTutorial = false;
            }
        }
        if(modMang.currSelect > 0)
        {
            modMang.useMod();
            AudioSource.PlayClipAtPoint(modActivete, new Vector3(0, 0, -10), masterVol * sfxVol);
            modActive = true;
            startMod();
        }
    }

    void startMod()
    {
        switch (modMang.currSelect)
        {
            case 1:
                upMPHmod = 2;
                break;
            case 2:
                startMPHmod = 2;
                break;
            case 3:
                obstaclesMultiplyer = 0.6f;
                break;
            case 4:
                playerCar.startBeginBoost(1000, 600);
                break;
            case 5:
                playerCar.autoShield = true;
                break;
            case 6:
                coinSpawnMultiplier = 2.25f;
                break;
            case 7:
                powerupMultiplier = 2.15f;
                powerupTime = 0;
                break;
            case 8:
                playerCar.startBeginBoost(4000, 1100);
                break;
            case 9:
                allHolo = true;
                playerCar.makePermMagnet();
                break;
            case 10:
                playerCar.turnMulti *= 1.5f;
                break;
            case 11:
                playerCar.hits += 5;
                break;
            case 12:
                playerCar.startBeginBoost(8000, 1750);
                break;
        }
    }

    public void updateBlimpX()
    {
        blimpSpeed = new Vector3(blimpSpeed.x * -1, blimpSpeed.y, 0);
        newBlimpLocation = new Vector3(scoreBlimp.transform.localPosition.x, newBlimpLocation.y, 0);
    }

    public void updateBlimpY()
    {
        blimpSpeed = new Vector3(blimpSpeed.x, blimpSpeed.y * -1, 0);
        newBlimpLocation = new Vector3(newBlimpLocation.x, scoreBlimp.transform.localPosition.y, 0);
    }

    public void gameGameOverButton()
    {
        billboard bboard = billboards.ToArray()[0].GetComponent<billboard>();
        Image overButton = Instantiate(bboard.gameOverOBJ, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform).GetComponent<Image>();
        overButton.sprite = bboard.skinCurr;
        overButton.GetComponent<menuBillboard>().adOBJ.GetComponent<Image>().sprite = bboard.ad;
        
        Destroy(bboard.gameObject);

        menuSound.clip = menuAmbience;
        menuSound.Play();
    }

    public void checkTutorialDeath(Vector3 menuButtonPos)
    {

        if (inTutorial)
        {
            if (tutorialSteps == 7)
            {
                tutorialSteps++;
            }
            if (tutorialSteps == 8)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(menuButtonPos.x + 0.5f, menuButtonPos.y - 0.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.35f, 0.15f, 30, 0.5f, 2);
                activeHand.transform.localScale = new Vector3(0.05f, 0.05f, 1);
                setTutorialText(tutorialTexts[7], new Vector3(5.2f, 1.55f, 0));
                activeText.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
    }

    public void pauseButton()
    {
        if(Time.deltaTime > 0 && playing)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
            Time.timeScale = 0;
            Instantiate(pauseMenu);
            menuSound.Pause();
            playerCar.sndSource.Pause();
        }
    }

    public void settingsButton(Button sign)
    {
        AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
        settingsMenu sm = Instantiate(settingsUI).GetComponent<settingsMenu>();
        sm.prevButton = sign;
    }

    public void inventoryButton()
    {
        AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
        Instantiate(inventoryUI);
    }

    public void createSign(int type)
    {
        float pos = 0;
        Transform trans = null;
        if (type < 2)
        {
            if (topSign) { pos = -95; trans = GameObject.Find("Signs Bottom").transform; }
            else
            {
                if (topLane)
                {
                    if (topLaneTime < 245)
                    {
                        pos = 60;
                    }
                    else
                    {
                        pos = 20;
                    }
                }
                else
                {
                    if (topLaneTime < 10)
                    {
                        pos = 20;
                    }
                    else
                    {
                        pos = 60;
                    }
                }
                trans = GameObject.Find("Signs Top").transform;
            }
        }

        if (type == 0)
        {
            setMilestone(trans, new Vector3(500, pos, 0));
        }
        else if (type == 1)
        {
            setFunSign(trans, new Vector3(500, pos, 0));
        }
        else if (type == 2)
        {
            setExitSign(GameObject.Find("Signs Top").transform, new Vector3(500, 60, 0));
        }
        else if (type == 3)
        {
            setBridgeSigns();
        }
        else if (type == 4)
        {
            setWorkSign(GameObject.Find("Signs Bottom").transform, new Vector3(500, -95, 0));
        }
        topSign = !topSign;
    }

    private void setMilestone(Transform par, Vector3 pos)
    {
        milestone += 250;

        signs sign = null;
        if (milestone % 1000 == 0)
        {
            sign = Instantiate(milestoneBigSign, par).GetComponent<signs>();
        }
        else
        {
            sign = Instantiate(milestoneSigns[Random.Range(0, milestoneSigns.Length)], par).GetComponent<signs>();
        }
        sign.transform.localPosition = pos;
        sign.setMilestoneText(milestone);
    }

    private void setFunSign(Transform par, Vector3 pos)
    {
        signs sign = Instantiate(getSignFromOdds(signOdds, signCurrOdds, funSigns), par).GetComponent<signs>();
        sign.transform.localPosition = pos;
    }

    private void setExitSign(Transform par, Vector3 pos)
    {
        signs sign = Instantiate(exitSign, par).GetComponent<signs>();
        sign.transform.localPosition = pos;
        sign.exitSign(worldM.exitNums[exitNum], worldM.stNames[exitNum]);
    }

    private void setBridgeSigns()
    {
        string bName = worldM.getBridgeName();
        signs sign1 = Instantiate(bridgeSign, GameObject.Find("Signs Bottom").transform).GetComponent<signs>();
        sign1.transform.localPosition = new Vector3(500, -95, 0);
        sign1.setBridgeText(bName);
        signs sign2 = Instantiate(bridgeSign, GameObject.Find("Signs Top").transform).GetComponent<signs>();
        sign2.transform.localPosition = new Vector3(500, 60, 0);
        sign2.setBridgeText(bName);
    }

    private void setWorkSign(Transform par, Vector3 pos)
    {
        signs sign = Instantiate(workSign, par).GetComponent<signs>();
        sign.transform.localPosition = pos;
    }

    GameObject getSignFromOdds(List<float> signOddsList, List<float> signCurrOddsList, List<GameObject> signSkinsList)
    {
        float newBuildingOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < signOddsList.Count; i++)
        {
            oddsAccum += signCurrOddsList[i];
            if (oddsAccum > newBuildingOdds)
            {
                changeOdds(i, signOddsList, signCurrOddsList);
                return signSkinsList[i];
            }
        }

        changeOdds(0, signOddsList, signCurrOddsList);
        return signSkinsList[0];
    }

    public void collectCoin(int ammount)
    {
        coins += ammount;

        if(inTutorial && tutorialSteps == 3)
        {
            resetSwipeTutorialTimer(2);
        }
    }

    public void makeCoinsHolo()
    {
        foreach (GameObject c in coinList)
        {
            c.GetComponent<coins>().makingHolo = true;
        }
    }

    public void endCoinhuna()
    {
        isBigCoinhuna = false;
        if (!allHolo)
        {
            foreach (GameObject co in coinList)
            {
                coins c = co.GetComponent<coins>();
                if (!c.isHolo)
                {
                    c.makingNormal = true;
                }
            }
        }
        if (modMang.currSelect == 6)
        {
            coinSpawnMultiplier = 2.25f;
        }
        else
        {
            coinSpawnMultiplier = 1;
        }
    }

    void makeCarTiny(GameObject tinyCar, bool doInstant)
    {
        cars c = tinyCar.GetComponent<cars>();
        if(allTinyCars || c.isCar)
        {
            c.makeTiny(doInstant);
        }
    }

    void makeCarNormal(GameObject tinyCar)
    {
        cars c = tinyCar.GetComponent<cars>();
        if (allTinyCars || c.isCar)
        {
            c.makeNormal();
        }
    }

    public void startTinyCars(bool allTiny)
    {
        inTinyCars = true;
        allTinyCars = allTiny;
        foreach (GameObject ca in carsInGame)
        {
            makeCarTiny(ca, false);
        }
    }

    public void endTinyCars()
    {
        inTinyCars = false;
        foreach (GameObject ca in carsInGame)
        {
            makeCarNormal(ca);
        }
    }

    public void changeMasterVol(float newVol)
    {
        masterVol = newVol;
        PlayerPrefs.SetFloat("masterVol", masterVol); //saves the master volume level
    }

    public void changeSfxVol(float newVol)
    {
        sfxVol = newVol;
        PlayerPrefs.SetFloat("sfxVol", sfxVol); //saves the SFX volume level
    }

    public void changeMusicVol(float newVol)
    {
        musicVol = newVol;
        PlayerPrefs.SetFloat("musicVol", musicVol); //saves the music volume level
    }


    public void changeRadioVol(float newVol)
    {
        radioVol = newVol;
        PlayerPrefs.SetFloat("radioVol", radioVol); //saves the radio volume level
    }

    private void setBackBuildingOdds()
    {
        float[] buildingOddsNew = { 0.025f, 0.025f, 0.045f, 0.045f, 0.06f, 0.06f, 0.05f, 0.05f, 0.09f, 0.09f, 0.1f, 0.1f, 0.055f, 0.055f, 0.0375f, 0.0375f, 0.0375f, 0.0375f };
        for (int i = 0; i < buildingSkins.Count; i++)
        {
            buildingsOdds.Add(buildingOddsNew[i]);
            buildingsCurrOdds.Add(buildingOddsNew[i]);
        }
    }

    private void setFrontBuildingOdds()
    {
        float[] buildingFrontOddsNew = { 0.0875f, 0.0875f, 0.0875f, 0.0875f, 0.15f, 0.15f, 0.05f, 0.05f, 0.05f, 0.075f, 0.075f, 0.025f, 0.025f };
        for (int i = 0; i < buildingFrontSkins.Count; i++)
        {
            buildingsFrontOdds.Add(buildingFrontOddsNew[i]);
            buildingsFrontCurrOdds.Add(buildingFrontOddsNew[i]);
        }

        float[] billboardFrontOddsNew = { 0.3f, 0.4f, 0.3f };
        for (int i = 0; i < frontBillboardSkins.Count; i++)
        {
            frontBillboardOdds.Add(billboardFrontOddsNew[i]);
            frontBillboardCurrOdds.Add(billboardFrontOddsNew[i]);
        }
    }

    private void setFarBuildingOdds()
    {
        float[] farBuildingOddsNew = { 0.2f, 0.2f, 0.2f, 0.1f, 0.1f, 0.025f, 0.025f, 0.025f, 0.025f, 0.05f, 0.05f };
        for (int i = 0; i < farBuildingSkins.Count; i++)
        {
            farBuildingsOdds.Add(farBuildingOddsNew[i]);
            farBuildingsCurrOdds.Add(farBuildingOddsNew[i]);
        }
    }

    private void setSkylineOdds()
    {
        float[] skylineOddsNew = { 0.35f, 0.3f, 0.35f };
        for (int i = 0; i < skylineSkins.Count; i++)
        {
            skylineOdds.Add(skylineOddsNew[i]);
            skylineCurrOdds.Add(skylineOddsNew[i]);
        }
    }

    private void setRoadOdds()
    {
        float[] roadOddsNew = { 0.25f, 0.3f, 0.25f, 0.2f };
        for (int i = 0; i < roadSkins.Count; i++)
        {
            roadOdds.Add(roadOddsNew[i]);
            roadOddsCurr.Add(roadOddsNew[i]);
        }
    }

    private void setSignOdds()
    {
        float[] signOddsNew = { 0.06f, 0.06f, 0.06f, 0.12f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.1f, 0.09f, 0.09f };
        for (int i = 0; i < funSigns.Count; i++)
        {
            signOdds.Add(signOddsNew[i]);
            signCurrOdds.Add(signOddsNew[i]);
        }
    }

    private void changeOdds(int index, List<float> setList, List<float> currentList)
    {
        float diff = currentList[index] - (setList[index]/2); //the odds that are being taken away from use
        float listDiff = (diff * setList[index]) / (setList.Count - 1.0f); //the gap of what would of been added when changing odds
        for (int i = 0; i < setList.Count; i++)
        {
            currentList[i] += (diff * setList[i]) + listDiff; //this took me way to fucking long to figue out
        }

        currentList[index] = setList[index] / 2;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    private void spawnTutorialWorld()
    {
        spawnTutorialCar();
        swipeTutorial();
        switch (tutorialSteps)
        {
            case 3:
                spawnTutorialCoin();
                break;
            case 4:
                isStepTime(7.5f);
                break;
            case 5:
                spawnTutorialPowerup();
                if (mph < 50) { mph += Time.deltaTime; }
                break;
            case 6:
                if (!powerupActive)
                {
                    spawnTutorialPowerup();
                }
                spawnTutorialPowerupCoins();
                if (mph < 50) { mph += Time.deltaTime; }
                break;
        }
}

    public void nextTutorialStep()
    {
        tutorialSteps++;
        if (activeHand != null) { Destroy(activeHand.gameObject); }
        activeText.SetActive(false);

        PlayerPrefs.SetInt("tutorialStep", tutorialSteps);
    }

    public void startTutorial()
    {
        activeHand = Instantiate(tutorialHandOBJ, new Vector3(1.2f, -2.8f, 0), Quaternion.identity).GetComponent<tutorialHand>();
        activeHand.setBounce(0.45f, 0.25f, 30, 0.5f, 2);
        setTutorialText(tutorialTexts[0], new Vector3(3.8f, -0.8f, 0));
    }

    private float swipeTutorialTimer = 0;
    private void swipeTutorial()
    {
        if (canSwipeTutorialTimer() && (!activeText.activeInHierarchy))
        {
            if (tutorialSteps == 1)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(0.0f, -0.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.0f, 0.55f, 0, 0, 0.5f);
                setTutorialText(tutorialTexts[1], new Vector3(3.2f, 1.3f, 0));
            }
            else if (tutorialSteps == 2)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(0.0f, -0.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.0f, 0.55f, 0, 0, 2);
                setTutorialText(tutorialTexts[2], new Vector3(3.2f, -3.8f, 0));
            }
            else if (tutorialSteps == 3)
            {
                setTutorialText(tutorialTexts[3], new Vector3(2.2f, -1.8f, 0));
            }
            else if (tutorialSteps == 4)
            {
                setTutorialText(tutorialTexts[4], new Vector3(2.2f, -1.8f, 0));
            }
            else if (tutorialSteps == 5)
            {
                setTutorialText(tutorialTexts[5], new Vector3(2.2f, -0.8f, 0));
            }
            else if (tutorialSteps == 7)
            {
                setTutorialText(tutorialTexts[6], new Vector3(1.2f, -1.8f, 0));
            }
            else if (tutorialSteps == 9)
            {
                setTutorialText(tutorialTexts[8], new Vector3(2.8f, -0.3f, 0));
            }
        }
        else
        {
            swipeTutorialTimer += Time.deltaTime;
        }

    }

    public void setTutorialText(string text, Vector3 newPos)
    {
        activeText.SetActive(true);
        activeText.GetComponent<TextMeshProUGUI>().text = text;
        activeText.transform.position = newPos;
        activeText.GetComponent<tutorialText>().startPoint = newPos.y;
    }

    public bool canSwipeTutorialTimer()
    {
        return swipeTutorialTimer > 4;
    }

    public void resetSwipeTutorialTimer(float swipeTime)
    {
        swipeTutorialTimer = swipeTime;
        nextTutorialStep();
    }

    private bool isStepTime(float time)
    {
        stepTimer += Time.deltaTime;

        if(stepTimer > time)
        {
            stepTimer = 0;
            resetSwipeTutorialTimer(2);
            return true;
        }
        return false;
    }

    private void spawnTutorialCoin()
    {
        coinTimer += Time.deltaTime;
        if (coinTimer > 6)
        {
            Instantiate(tutorialCoin, new Vector3(12, -1.85f, 0), Quaternion.identity, GameObject.Find("coins").transform);  //spawn new car in a random lane before going on screen
            coinTimer = 0;
        }
    }

    void spawnTutorialPowerup()
    {
        if (tutorialPowerup == null && GameObject.Find("power up icon(Clone)") == null)
        {
            pwManage.createPowerup(0, new Vector3(15, -1.85f, 0));
            tutorialPowerup = GameObject.Find("magnet powerup");
        }
    }

    void spawnTutorialPowerupCoins()
    {
        coinTimer += Time.deltaTime;
        if (coinTimer > 3.5f)
        {
            Instantiate(coin[Random.Range(0, 3)], new Vector3(12, -1.85f, 0), Quaternion.identity, GameObject.Find("coins").transform);  //spawn new car in a random lane before going on screen
            coinTimer = 0;
        }
    }


    void spawnTutorialCar()
    {
        carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
        if (carTimer > 100)
        {
            float newLane = (Random.Range(0, -2) * 5.0f) + 0.65f;
            caManager.spawnRegularCar(new Vector3(12, newLane, 0), GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
            carTimer = 0;
        }
    }

    void spawnMenuCar()
    {
        float carTime = 1.0f;
        if (!inTutorial || tutorialSteps < 3)
        {
            carTime = 1.75f;
        }
        carTimer += Time.deltaTime; //timer to spawn a new car after game is over
        if (carTimer > carTime)
        {
            cars newCar = null;
            float newLane = 0;
            if (topLane)
            {
                if (topLaneTime < 240)
                {
                    newLane = (Random.Range(0, -5) * 1.25f) + 0.65f;
                }
                else
                {
                    newLane = (Random.Range(-1, -5) * 1.25f) + 0.65f;
                }
            }
            else
            {
                if (topLaneTime < 10)
                {
                    newLane = (Random.Range(-1, -5) * 1.25f) + 0.65f;
                }
                else
                {
                    newLane = (Random.Range(0, -5) * 1.25f) + 0.65f;
                }
            }
            float specalCar = Random.Range(0.0f, 1.0f);
            if (specalCar > largeCarOdds + specalCarOdds)
            {
                newCar = caManager.spawnRegularCar(new Vector3(-12, newLane, 0), GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                largeCarOdds += 0.1f;
                specalCarOdds += 0.00175f;
            }
            else if (specalCar > largeCarOdds)
            {
                newCar = caManager.spawnPoliceCar(new Vector3(-12, newLane, 0), GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                specalCarOdds = 0.01f;
            }
            else
            {
                newCar = caManager.spawnLargeCar(new Vector3(-14, newLane, 0), GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                largeCarOdds = 0.05f;
            }
            carTimer = 0;

            newCar.setLane();

            if ((!inTutorial || tutorialSteps > 6) && newCar.transform.position.y < 0.65f && newCar.transform.position.y > -4.35f && !carsPast.Contains(newCar.GetComponent<cars>().lane))
            {
                carsPast.Add(newCar.GetComponent<cars>().lane);
                carsPast.Remove(carsPast[0]);
            }

            carTimer = 0;
        }
    }

}