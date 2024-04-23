using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
    public bool playing;
    public bool isOver;
    public float mph; //controls the speed of the game
    public float score;
    public float highScore;

    public float screenDistort;
    public float screenDistortTarget;

    public float masterVol;
    public float sfxVol;
    public float musicVol;
    public float radioVol;

    public bool inTutorial;
    public int tutorialSteps;
    public GameObject tutorialHandOBJ;
    public tutorialHand activeHand;
    public GameObject[] tutorialTexts;
    public GameObject activeText;

    public int coins; //amount of coins a player has collected in a game
    public int totalCoins; //amount of coins a player has in total

    public GameObject[] coin; //coin gameobject to 
    public GameObject[] coin2; //coin gameobject to spawn
    public float coinTimer; //

    public GameObject pauseMenu;

    public GameObject scoreBlimp;
    public Vector3 blimpSpeed;
    public Vector3 newBlimpLocation;
    public TextMeshProUGUI scoreText;

    public float textTimer;
    public int textNum;
    public bool scoreShowing;

    public GameObject overBoard;
    public GameObject overBigBoard;

    public playerCar playerCar;
    public AudioClip startEngine;

    public GameObject divider; //divider gameobject to spawn
    public GameObject dividerPart; //divider gameobject to spawn
    public float dividerTimer;

    public GameObject manhole;
    public float manholeTimer;
    public float manholeSpawn;

    public GameObject planeAd;
    public float planeAdTimer;

    public GameObject bigPlane;
    public float bigPlaneTimer;

    public GameObject building; //building gameobject to spawn
    public GameObject billboard; //building gameobject to spawn
    public GameObject bigBillboard; //building gameobject to spawn
    public List<Sprite> buildingSkins;
    public List<float> buildingsOdds;
    public List<float> buildingsCurrOdds;
    public float buildingList;
    public float billboardList;
    public float buildingTimer;

    public GameObject buildingFront; //building gameobject to spawn
    public GameObject frontBillboard; //building gameobject to spawn
    public GameObject bigFrontBillboard; //building gameobject to spawn
    public List<Sprite> buildingFrontSkins;
    public List<float> buildingsFrontOdds;
    public List<float> buildingsFrontCurrOdds;
    public float buildingFrontList;
    public float frontBillboardList;
    public float buildingFrontTimer;

    public GameObject skyline;
    public int skylineList;
    public float skylineTimer;

    public GameObject crain;
    public float crainTimer;

    public GameObject cloud;
    public float cloudTimer;

    public List<GameObject> billboards;

    public GameObject guard; //rail guard gameobject to spawn
    public GameObject guard2; //rail guard gameobject to spawn
    public float guardTimer;

    public GameObject[] milestoneSigns;
    public GameObject milestoneBigSign;
    public int milestone;

    public List<GameObject> carsList;
    public List<float> carsOdds;
    public List<float> carsCurrOdds;
    public int carList; //how many cars have spawned since the last bus
    public float carTimer;
    public float largeCarOdds;
    public float specalCarOdds;

    public List<GameObject> carsLargeList;
    public List<float> carsLargeOdds;
    public List<float> carsLargeCurrOdds;

    public GameObject bus; //bus gmaeobject to spawn
    public GameObject overBus; //bus gmaeobject to spawn

    public List<GameObject> carsSpecialList;
    public List<float> carsSpecialOdds;
    public List<float> carsSpecialCurrOdds;

    public GameObject policeCar;

    public List<float> bannedLanes;
    public List<int> carsPast;

    public GameObject settingsUI;

    public AudioClip menuAmbience;
    public AudioClip gameAmbience;
    public AudioSource menuSound;

    public AudioClip clickSound;

    public bool topLane;
    public float topLaneTime;
    public float topLaneTimer;

    public GameObject topCurrSide;
    public GameObject topSide;
    public GameObject topCurrBar;
    public GameObject topBar;
    public GameObject topCurrSide2;
    public GameObject topSide2;
    public GameObject topCurrBar2;
    public GameObject topBar2;
    public GameObject topCurrRoad;
    public GameObject topRoad;

    public GameObject topLaneRoadL;
    public GameObject topLaneRoadR;
    public GameObject topLaneCurrRoad;
    public GameObject topLaneLineL;
    public GameObject topLaneLineR;
    public float topLaneLineTimer;
    public GameObject yelloBarrel;

    public byte areaEvent;
    public float areaTime;
    public float areaTimer;

    public bool startBridge;

    public GameObject bridgeBar;
    public GameObject bridgeBar2;
    public GameObject bridgeStart;
    public GameObject bridgeEnd;
    public GameObject bridgeStartConnector;
    public GameObject bridgeEndConnector;
    public GameObject bridgePillar;
    public GameObject bridgeSupport;
    public GameObject backroundPort;
    public float bridgeLineTimer;

    public GameObject bridgeBillboard;
    public GameObject bridgeBigBillboard;
    public GameObject bridgeOverBoard;
    public GameObject bridgeOverBigBoard;


    // Start is called before the first frame update
    void OnEnable()
    {
        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        menuSound = GameObject.Find("ambience").GetComponent<AudioSource>();

        masterVol = PlayerPrefs.GetFloat("masterVol", 1); //sets high score to the one saved
        sfxVol = PlayerPrefs.GetFloat("sfxVol", 1); //sets high score to the one saved
        musicVol = PlayerPrefs.GetFloat("musicVol", 1); //sets the music volume to the one it was last on
        radioVol = PlayerPrefs.GetFloat("radioVol", 1); //sets the radio volume to the one it was last on

        tutorialSteps = PlayerPrefs.GetInt("tutorialStep", 0); //sets the tutorial to the last step
        if(tutorialSteps < 5)
        {
            inTutorial = true;
        }

        playing = false;
        isOver = false;
        scoreShowing = false;
        mph = 0; //sets inital mph

        coins = 0;
        highScore = PlayerPrefs.GetInt("highscore", 0); //sets high score to the one saved
        totalCoins = PlayerPrefs.GetInt("coins", 0); //sets high score to the one saved

        largeCarOdds = 0.05f;
        specalCarOdds = 0.01f;

        milestone = 0;
        blimpSpeed = new Vector3(0.1f, 0.05f, 0);

        topLaneTimer = Random.Range(500, 3000);
        areaTimer = Random.Range(500, 2000);

        bigPlaneTimer = Random.Range(15, 65);

        menuSound.clip = menuAmbience;
        menuSound.Play();

        setCarOdds(carsOdds, carsCurrOdds, carsList);
        setCarOdds(carsLargeOdds, carsLargeCurrOdds, carsLargeList);
        setCarOdds(carsSpecialOdds, carsSpecialCurrOdds, carsSpecialList);
        setBuildingOdds();
        setbuildingFrontOdds();

        if(inTutorial && tutorialSteps == 0)
        {
            startTutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) //if in a game 
        {
            if (mph < playerCar.startMph) //checks if the game is in its starting animation
            {
                mph += (mph/2 + 15.0f) * Time.deltaTime;
            }
            else
            {
                mph += playerCar.upMph * Time.deltaTime; //graudully increeses the mph (+1 every 2 sec)
            }

            score += (mph * 0.44704f) * Time.deltaTime; //increses the score based on how far the player has gone

            //make road elements
            spawnDivider();
            spawnGuard();

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
            spawnCrain();
            spawnSkyline();
            spawnCloud();

            //make sky elements
            spawnAdPlane();
            spawnBigPlane();

            //make game element
            if (!inTutorial || (tutorialSteps > 0 ^ tutorialSteps < 3))
            {
                spawnCoin();
                newTopLane();
                changeArea();
                spawnGameCar();
            }
            else
            {
                spawnTutorialCar();
                swipeTutorial();
            }
            updateBillbord();

            //set blimp text
            blimpText();

            if (inTutorial && tutorialSteps == 3)
            {
                swipeTutorial();
                if (canSwipeTutorialTimer() && Input.touchCount > 0)
                {
                    nextTutorialStep();
                }
            }
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

        //updates the smoke effict on the players screen
        updateSmokeScreen();

        // update blimp
        if ((scoreBlimp.transform.position.x < -7.79f || scoreBlimp.transform.position.x > -4.82f) && Mathf.Abs(scoreBlimp.transform.position.x - newBlimpLocation.x) > 1.0) //if blimp went out of bounce on the X
        {
            updateBlimpX(); // find new target location for blimp
        }
        if ((scoreBlimp.transform.position.y > 4.58f || scoreBlimp.transform.position.y < 3.86f) && Mathf.Abs(scoreBlimp.transform.position.y - newBlimpLocation.y) > 0.5) //if blimp went out of bounce on the X
        {
            updateBlimpY(); // find new target location for blimp
        }

        scoreBlimp.transform.position += blimpSpeed * Time.deltaTime; // updates blimp position

        //makes milestone sign
        if (score > (milestone + 210))
        {
            setMilestone();
        }

        menuSound.volume = masterVol;
    }

    void spawnDivider()
    {
        dividerTimer += Time.deltaTime * mph; //timer to spawn new lane divider
        if (dividerTimer > 12)
        {
            //spawns a new yellow lane divider for each lane
            if (topLane)
            {
                Instantiate(divider, new Vector3(12, -1.8f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            }
            else
            {
                Instantiate(dividerPart, new Vector3(12, -2.5f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            }
            dividerTimer = 0;
        }
    }

    void spawnGuard()
    {
        guardTimer += Time.deltaTime * mph; //timer to spawn new road guard 
        if (areaEvent == 0)
        {
            if (guardTimer > 2)
            {
                //spawns a new rail guard for the edge of the road
                if (topLane)
                {
                    Instantiate(guard2, new Vector3(12, 1.36f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                }
                else
                {
                    if (topLaneTime >= 0)
                    {
                        Instantiate(guard2, new Vector3(12, 1.36f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    }
                    else
                    {
                        Instantiate(guard2, new Vector3(12, 0.55f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    }
                }
                Instantiate(guard, new Vector3(12, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                guardTimer = 0;
            }
        }
        else if (areaEvent == 1)
        {
            if (guardTimer > 15)
            {
                //spawns a new rail guard for the edge of the road
                Instantiate(bridgeBar, new Vector3(12, 1.3f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                Instantiate(bridgeBar2, new Vector3(12, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                guardTimer = 0;
            }
        }
    }

    void spawnManhole()
    {
        manholeTimer += Time.deltaTime * mph; //timer to spawn new road guard 
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
            newManhole.transform.localScale = new Vector3((0.4f + (0.05f * manholeLayer)), (0.4f + (0.05f * manholeLayer)), 1);
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
        buildingFrontTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (buildingFrontTimer > 34.5)
        {
            if (buildingFrontList < 3)
            {
                buildings newbuildingFront = Instantiate(buildingFront, new Vector3(13, 0.36f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); ; //spawns new backround building
                newbuildingFront.setSkin(getbuildingFrontFromOdds());
                buildingFrontList++;
                buildingFrontTimer = 0;
            }
            else
            {
                if (frontBillboardList < 3)
                {
                    GameObject bboard = Instantiate(frontBillboard, new Vector3(13, -2.15f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBillboard = true;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    buildingFrontTimer = 0;
                    frontBillboardList++;
                }
                else
                {
                    GameObject bboard = Instantiate(bigFrontBillboard, new Vector3(14.125f, -1.64f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBigBillboard = true;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    buildingFrontTimer = -18.5f;
                    frontBillboardList = 0;
                }
            }
        }
    }

    void spawnBackBuilding()        //backround buildings
    {
        buildingTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (buildingTimer > 25.5)
        {
            if (buildingList < 6)
            {
                buildings newBuilding = Instantiate(building, new Vector3(12, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                newBuilding.setSkin(getbuildingFromOdds());
                buildingList++;
                buildingTimer = 0;
            }
            else
            {
                buildings bboard = Instantiate(bigBillboard, new Vector3(13, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                bboard.isBigBillboard = true;
                buildingList = 0; buildingTimer = -25;
                billboardList = 0;
            }
        }
    }

    void spawnCrain()        //skyline buildings
    {
        crainTimer -= Time.deltaTime * mph; //timer that spawns a new builing
        if (crainTimer < 0)
        {
            Instantiate(crain, new Vector3(13.5f, 2.5f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
            crainTimer = Random.Range(250, 825);
        }
    }

    void spawnSkyline()        //skyline buildings
    {
        skylineTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (skylineTimer > 150)
        {
            Instantiate(skyline, new Vector3(13.5f, 2.5f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
            skylineTimer = 0;
        }
    }

    void spawnCloud()        //clouds

    {
        cloudTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (cloudTimer > 925)
        {
            Instantiate(cloud, new Vector3(13.5f, Random.Range(2.5f, 5.8f), 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
            cloudTimer = 0;
        }
    }

    void newTopLane()
    {
        if (areaEvent == 0) { topLaneTimer -= Time.deltaTime * mph; }
        if (topLaneTimer <= 0 && topLaneTime <= 0)
        {
            topLane = !topLane;
            topLaneTime = 250.0f;
        }
        if (!topLane)
        {
            if (topLaneTime > 0)
            {
                if (topLaneCurrRoad == null)
                {
                    topLaneCurrRoad = Instantiate(topLaneRoadL, new Vector3(24.5f, 0, 0), Quaternion.identity);
                    Instantiate(topLaneLineL, new Vector3(12, 0.6f, 0), Quaternion.identity);
                }
                topLaneTime -= Time.deltaTime * mph;
                if (topLaneTime <= 0)
                {
                    topCurrBar.GetComponent<sideBar>().movingOut = true;
                    topCurrSide.GetComponent<sideBar>().movingOut = true;
                    topCurrRoad.GetComponent<sideBar>().movingOut = true;
                    topLaneCurrRoad.GetComponent<sideBar>().movingOut = true;
                    topCurrSide = Instantiate(topSide, new Vector3(24.5f, 0.33f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    topCurrBar = Instantiate(topBar, new Vector3(24.5f, 0.80f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    Instantiate(yelloBarrel, new Vector3(12f, 0.50f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    topLaneTimer = Random.Range(250, 750);
                }
                else
                {
                    if (topLaneLineTimer > 12)
                    {
                        Instantiate(topLaneLineL, new Vector3(12, 0.6f, 0), Quaternion.identity);
                        topLaneLineTimer = 0;
                    }
                    topLaneLineTimer += Time.deltaTime * mph;
                }

            }
        }
        else
        {
            if (topLaneTime > 0)
            {
                if (topLaneCurrRoad == null)
                {
                    topLaneCurrRoad = Instantiate(topLaneRoadR, new Vector3(24.5f, 0, 0), Quaternion.identity);
                    topCurrRoad = Instantiate(topRoad, new Vector3(24, -2, 0), Quaternion.identity);
                    topCurrBar.GetComponent<sideBar>().movingOut = true;
                    topCurrSide.GetComponent<sideBar>().movingOut = true;
                    topCurrSide = Instantiate(topSide, new Vector3(24.5f, 1.14f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    topCurrBar = Instantiate(topBar, new Vector3(24.5f, 1.61f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    Instantiate(yelloBarrel, new Vector3(12f, 0.50f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                }
                topLaneTime -= Time.deltaTime * mph;
                if (topLaneTime <= 0)
                {
                    topLaneCurrRoad.GetComponent<sideBar>().movingOut = true;
                    topLaneTimer = Random.Range(750, 2500);
                }
                else
                {
                    if (topLaneLineTimer > 12)
                    {
                        Instantiate(topLaneLineR, new Vector3(12, 0.6f, 0), Quaternion.identity);
                        topLaneLineTimer = 0;
                    }
                    topLaneLineTimer += Time.deltaTime * mph;
                }

            }
        }
    }

    void changeArea()
    {
        if (!inTutorial || (tutorialSteps > 0 ^ tutorialSteps < 3)) { areaTimer -= Time.deltaTime * mph; }
        if (areaTimer <= 0 && buildingFrontList == 1)
        {
            if (areaEvent == 0) { areaEvent = 1; } else { areaEvent = 0;  }
        }
        if (areaEvent == 1 && (topLane && topLaneTime <= 0))
        {
            if (!startBridge)
            {
                topCurrBar.GetComponent<sideBar>().movingOut = true;
                topCurrSide.GetComponent<sideBar>().movingOut = true;
                topCurrBar2.GetComponent<sideBar>().movingOut = true;
                topCurrSide2.GetComponent<sideBar>().movingOut = true;
                Instantiate(bridgeStart, new Vector3(14.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                Instantiate(bridgeStartConnector, new Vector3(11.5f, -4.8f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                guardTimer = 5;
                buildingTimer = -34;
                buildingFrontTimer = -10;
                buildingFrontList = 2;
                frontBillboardList = 1;
                areaTimer = Random.Range(600, 900);
                startBridge = true;
            }
            else
            {
                if (bridgeLineTimer > 12)
                {
                    //Instantiate(topLaneLineL, new Vector3(12, 0.6f, 0), Quaternion.identity);
                    bridgeLineTimer = 0;
                }
                bridgeLineTimer += Time.deltaTime * mph;
            }

        }
        else
        {
            if (areaEvent == 0)
            {
                if (startBridge)
                {
                    topCurrSide = Instantiate(topSide, new Vector3(29.5f, 1.14f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    topCurrBar = Instantiate(topBar, new Vector3(29.5f, 1.61f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    topCurrSide2 = Instantiate(topSide2, new Vector3(29.5f, -5.07f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    topCurrBar2 = Instantiate(topBar2, new Vector3(29.5f, -4.6f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    Instantiate(bridgeEnd, new Vector3(14.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    Instantiate(bridgeEndConnector, new Vector3(18.5f, -4.8f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    Instantiate(bridgeBar, new Vector3(12.5f, 1.3f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    Instantiate(bridgeBar, new Vector3(15.5f, 1.3f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                    Instantiate(bridgeBar2, new Vector3(12, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    Instantiate(bridgeBar2, new Vector3(15, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    Instantiate(bridgeBar2, new Vector3(16.5f, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                    guardTimer = -35;
                    buildingFrontList = 2;
                    startBridge = false;
                    areaTimer = Random.Range(2000, 4000);
                }
                if (bridgeLineTimer > 12)
                {
                    //Instantiate(topLaneLineR, new Vector3(12, 0.6f, 0), Quaternion.identity);
                    bridgeLineTimer = 0;
                }
                bridgeLineTimer += Time.deltaTime * mph;
            }
        }
    }

    void spawnBridgePillar()
    {
        buildingFrontTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (buildingFrontTimer > 50)
        {
            if (buildingFrontList < 1)
            {
                if (areaTimer > 10)
                {
                    Instantiate(bridgePillar, new Vector3(13, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                }
                buildingFrontList++;
                buildingFrontTimer = 0;
            }
            else
            {
                if (frontBillboardList < 2)
                {
                    GameObject bboard = Instantiate(bridgeBillboard, new Vector3(13, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBillboard = true;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    buildingFrontTimer = -10;
                    frontBillboardList++;
                }
                else
                {
                    GameObject bboard = Instantiate(bridgeBigBillboard, new Vector3(14.125f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBigBillboard = true;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    buildingFrontTimer = -10;
                    frontBillboardList = 0;
                }
            }
        }
    }

    void spawnBridgeSupport()
    {
        buildingTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (buildingTimer > 7)
        {
            Instantiate(bridgeSupport, new Vector3(12, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
            buildingList++;
            buildingTimer = 0;
        }
    }

    void updateSmokeScreen()
    {
        if (screenDistort == screenDistortTarget)
        {
            screenDistortTarget = 0;
        }
        else
        {
            if (screenDistort < screenDistortTarget)
            {
                screenDistort += Time.deltaTime * 0.95f;
                if (screenDistort > screenDistortTarget)
                {
                    screenDistort = screenDistortTarget;
                }
            }
            else if (screenDistort > screenDistortTarget)
            {
                screenDistort -= Time.deltaTime * 0.95f;
                if(screenDistort < screenDistortTarget)
                {
                    screenDistort = screenDistortTarget;
                }
            }
        }
    }

    void spawnGameCar()
    {
        carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
        if (carTimer > 80)
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

    void spawnTutorialCar()
    {
        carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
        if (carTimer > 100)
        {
            float newLane = (Random.Range(0, -2) * 5.0f) + 0.65f;
            Instantiate(getCarFromOdds(carsOdds, carsCurrOdds, carsList), new Vector3(12, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
            carTimer = 0;
        }
    }

    void spawnMenuCar()
    {
        float carTime = 1.0f;
        if(!inTutorial || tutorialSteps < 3)
        {
            carTime = 1.75f;
        }
        carTimer += Time.deltaTime; //timer to spawn a new car after game is over
        if (carTimer > carTime)
        {
            GameObject newCar = this.gameObject;
            if (!isOver)
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
                float specalCar = Random.Range(0.0f, 1.0f);
                if (specalCar > largeCarOdds + specalCarOdds)
                {
                    newCar = Instantiate(getCarFromOdds(carsOdds, carsCurrOdds, carsList), new Vector3(-12, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                    largeCarOdds += 0.1f;
                    specalCarOdds += 0.00175f;
                }
                else if (specalCar > largeCarOdds)
                {
                    newCar = Instantiate(policeCar, new Vector3(-12, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                    specalCarOdds = 0.01f;
                }
                else
                {
                    newCar = Instantiate(getCarFromOdds(carsLargeOdds, carsLargeCurrOdds, carsLargeList), new Vector3(-14, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                    largeCarOdds = 0.05f;
                }
                carTimer = 0;
            }
            else
            {
                if (carList < 3)
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
                    newCar = Instantiate(getCarFromOdds(carsOdds, carsCurrOdds, carsList), new Vector3(-14, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
                    carList++;
                }
                else
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
                    newCar = Instantiate(overBus, new Vector3(-14, newLane, 0), Quaternion.identity, GameObject.Find("Extra").transform); //spawn new car in a random lane behind the player
                    carList = 0;
                }
            }

            newCar.GetComponent<cars>().setLane();

            if (!inTutorial && newCar.transform.position.y < 0.65f && newCar.transform.position.y > -4.35f && !carsPast.Contains(newCar.GetComponent<cars>().lane))
            {
                carsPast.Add(newCar.GetComponent<cars>().lane);
                carsPast.Remove(carsPast[0]);
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
        Instantiate(getCarFromOdds(carsOdds, carsCurrOdds, carsList), new Vector3(12, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
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
        Instantiate(getCarFromOdds(carsLargeOdds, carsLargeCurrOdds, carsLargeList), new Vector3(13, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
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
        Instantiate(getCarFromOdds(carsSpecialOdds, carsSpecialCurrOdds, carsSpecialList), new Vector3(25, newLane, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
    }

    void spawnCoin()
    {
        coinTimer += Time.deltaTime * mph;
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
        if (billboards.ToArray()[0].transform.position.x < -9.25f)
        {
            billboards.Remove(billboards.ToArray()[0]);
        }
    }

    GameObject getCarFromOdds(List<float> oddsList, List<float> currOddsList, List<GameObject> carList)
    {
        float newCarOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < oddsList.Count; i++)
        {
            oddsAccum += currOddsList[i];
            if (oddsAccum > newCarOdds)
            {
                changeOdds(i, oddsList, currOddsList);
                return carList[i];
            }
        }

        changeOdds(0, oddsList, currOddsList);
        return carList[0];
    }

    Sprite getbuildingFromOdds()
    {
        float newBuildingOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < buildingsOdds.Count; i++)
        {
            oddsAccum += buildingsCurrOdds[i];
            if (oddsAccum > newBuildingOdds)
            {
                changeOdds(i, buildingsOdds, buildingsCurrOdds);
                return buildingSkins[i];
            }
        }

        changeOdds(0, buildingsOdds, buildingsCurrOdds);
        return buildingSkins[0];
    }

    Sprite getbuildingFrontFromOdds()
    {
        float newBuildingFrontOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < buildingsFrontOdds.Count; i++)
        {
            oddsAccum += buildingsFrontCurrOdds[i];
            if (oddsAccum > newBuildingFrontOdds)
            {
                changeOdds(i, buildingsFrontOdds, buildingsFrontCurrOdds);
                return buildingFrontSkins[i];
            }
        }

        changeOdds(0, buildingsFrontOdds, buildingsFrontCurrOdds);
        return buildingFrontSkins[0];
    }

    void blimpText()
    {
        if (!scoreShowing)
        {
            string scoretxt = scoreText.text = "Don't Crash Bro";
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

        if (textTimer >= 0.10f)
        {
            textTimer = 0;
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

        if (score > highScore) { //check if theres a new high score
            highScore = score; // sets the new high score
            PlayerPrefs.SetInt("highscore", (int)highScore); //saves the new high score
        }

        isOver = true;

        GameObject.Find("Main Camera").GetComponent<AudioLowPassFilter>().cutoffFrequency = 2500;

        screenDistortTarget = 0;

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

        AudioSource.PlayClipAtPoint(startEngine, new Vector3(-0.5f, 0, -10), masterVol * sfxVol);
        menuSound.clip = gameAmbience;
        menuSound.Play();

        if(inTutorial && tutorialSteps == 0)
        {
            nextTutorialStep();
        }
    }

    public void updateBlimpX()
    {
        blimpSpeed = new Vector3(blimpSpeed.x * -1, blimpSpeed.y, 0);
        newBlimpLocation = new Vector3(scoreBlimp.transform.position.x, newBlimpLocation.y, 0);
    }

    public void updateBlimpY()
    {
        blimpSpeed = new Vector3(blimpSpeed.x, blimpSpeed.y * -1, 0);
        newBlimpLocation = new Vector3(newBlimpLocation.x, scoreBlimp.transform.position.y, 0);
    }

    public void gameGameOverButton()
    {
        GameObject bboard = billboards.ToArray()[0];

        if (bboard.GetComponent<buildings>().isBigBillboard)
        {
            if (areaEvent == 0)
            {
                Instantiate(overBigBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
            } else if(areaEvent == 1)
            {
                Instantiate(bridgeOverBigBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
            }
            if (inTutorial)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(bboard.transform.position.x + 1.5f, 2.8f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.45f, 0.25f, 30, 0.5f, 2);
                activeText = Instantiate(tutorialTexts[4], new Vector3(3.8f, -0.8f, 0), Quaternion.identity);
            }
        }
        else
        {
            if (areaEvent == 0)
            {
                Instantiate(overBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
            }
            else if (areaEvent == 1)
            {
                Instantiate(bridgeOverBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
            }
            if (inTutorial)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(bboard.transform.position.x + 2.7f, 2.8f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.45f, 0.25f, 30, 0.5f, 2);
                activeText = Instantiate(tutorialTexts[4], new Vector3(3.8f, -0.8f, 0), Quaternion.identity);
            }
        }

        if (inTutorial)
        {
            if(tutorialSteps < 4)
            {
                tutorialSteps = 4;
            }
        }

        Destroy(bboard);

        menuSound.clip = menuAmbience;
        menuSound.Play();
    }

    public void pauseButton()
    {
        if(Time.deltaTime > 0 && playing)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
            Time.timeScale = 0;
            Instantiate(pauseMenu);
            menuSound.Pause();
        }
    }

    public void settingsButton()
    {
        AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
        Instantiate(settingsUI);
    }

    public void setMilestone()
    {
        milestone += 250;

        GameObject sign = this.gameObject;
        if (milestone % 1000 == 0) {
            sign = Instantiate(milestoneBigSign, GameObject.Find("Signs").transform);
        }
        else
        {
            sign = Instantiate(milestoneSigns[Random.Range(0, milestoneSigns.Length)], GameObject.Find("Signs").transform);
        }
        sign.GetComponentInChildren<TextMeshProUGUI>().text = milestone + "m";
    }

    public void collectCoin(int ammount)
    {
        coins += ammount;
        totalCoins += ammount;
        PlayerPrefs.SetInt("coins", totalCoins); //saves the total coins
    }

    

    public void changeMasterVol(float newVol)
    {
        masterVol = newVol;
        PlayerPrefs.SetFloat("masterVol", masterVol); //saves the master volume level
    }

    public void changeSfxVol(float newVol)
    {
        sfxVol = newVol;
        PlayerPrefs.SetFloat("sfxVol", sfxVol); //saves the master volume level
    }

    public void changeMusicVol(float newVol)
    {
        musicVol = newVol;
        PlayerPrefs.SetFloat("musicVol", musicVol); //saves the master volume level
    }


    public void changeRadioVol(float newVol)
    {
        radioVol = newVol;
        PlayerPrefs.SetFloat("radioVol", radioVol); //saves the master volume level
    }

    private void setCarOdds(List<float> oddsList, List<float> currOddsList, List<GameObject> carList)
    {
        for (int i = 0; i < carList.Count; i++)
        {
            float addOdds = carList[i].GetComponent<cars>().odds;
            oddsList.Add(addOdds);
            currOddsList.Add(addOdds);
        }
    }

    private void setSpecialCarOdds()
    {
        for (int i = 0; i < carsList.Count; i++)
        {
            float addOdds = carsSpecialList[i].GetComponent<cars>().odds;
            carsSpecialOdds.Add(addOdds);
            carsCurrOdds.Add(addOdds);
        }
    }

    private void setBuildingOdds()
    {
        float[] buildingOddsNew = { 0.175f, 0.125f, 0.225f, 0.125f, 0.875f, 0.875f, 0.875f, 0.875f };
        for (int i = 0; i < buildingSkins.Count; i++)
        {
            buildingsOdds.Add(buildingOddsNew[i]);
            buildingsCurrOdds.Add(buildingOddsNew[i]);
        }
    }

    private void setbuildingFrontOdds()
    {
        float[] buildingFrontOddsNew = { 0.3f, 0.3f, 0.4f};
        for (int i = 0; i < buildingFrontSkins.Count; i++)
        {
            buildingsFrontOdds.Add(buildingFrontOddsNew[i]);
            buildingsFrontCurrOdds.Add(buildingFrontOddsNew[i]);
        }
    }

    private void changeOdds(int index, List<float> setList, List<float> currentList)
    {
        for(int i = 0; i < setList.Count; i++)
        {
            currentList[i] += setList[i] / 3;
        }

        currentList[index] = setList[index] / 2;
    }

    public void nextTutorialStep()
    {
        tutorialSteps++;
        Destroy(activeHand.gameObject);
        Destroy(activeText.gameObject);

        if (tutorialSteps >= 5)
        {
            inTutorial = false;
        }

        PlayerPrefs.SetInt("tutorialStep", tutorialSteps);
    }

    public void startTutorial()
    {
        activeHand = Instantiate(tutorialHandOBJ, new Vector3(1.2f, -2.8f, 0), Quaternion.identity).GetComponent<tutorialHand>();
        activeHand.setBounce(0.45f, 0.25f, 30, 0.5f, 2);
        activeText = Instantiate(tutorialTexts[0], new Vector3(3.8f, -0.8f, 0), Quaternion.identity);
    }

    private float swipeTutorialTimer = 0;
    private void swipeTutorial()
    {
        if (canSwipeTutorialTimer() && activeHand == null)
        {
            if (tutorialSteps == 1)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(0.0f, -1.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.0f, 0.55f, 0, 0, 0.5f);
                activeText = Instantiate(tutorialTexts[1], new Vector3(2.2f, -1.8f, 0), Quaternion.identity);
            }
            else if (tutorialSteps == 2)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(0.0f, -0.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeHand.setBounce(0.0f, 0.55f, 0, 0, 2);
                activeText = Instantiate(tutorialTexts[2], new Vector3(2.2f, -1.8f, 0), Quaternion.identity);
            }
            else if (tutorialSteps == 3)
            {
                activeHand = Instantiate(tutorialHandOBJ, new Vector3(100.0f, -0.5f, 0), Quaternion.identity).GetComponent<tutorialHand>();
                activeText = Instantiate(tutorialTexts[3], new Vector3(2.2f, -1.8f, 0), Quaternion.identity);
            }
        }
        else
        {
            swipeTutorialTimer += Time.deltaTime;
        }

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
}
