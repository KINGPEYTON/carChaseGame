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

    public playerCar playerCar;
    public AudioClip startEngine;

    public GameObject road; //road gameobject to spawn
    public GameObject roadPart; //road gameobject to spawn
    public Sprite roadSkins;
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

        //topLaneTimer = Random.Range(500, 3000);
        areaTimer = Random.Range(500, 2000);

        bigPlaneTimer = Random.Range(15, 65);

        menuSound.clip = menuAmbience;
        menuSound.Play();

        setCarOdds(carsOdds, carsCurrOdds, carsList);
        setCarOdds(carsLargeOdds, carsLargeCurrOdds, carsLargeList);
        setCarOdds(carsSpecialOdds, carsSpecialCurrOdds, carsSpecialList);
        setBackBuildingOdds();
        setFrontBuildingOdds();
        setFarBuildingOdds();
        setSkylineOdds();

        if (inTutorial && tutorialSteps == 0)
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
            spawnroad();
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
            spawnFarBuilding();
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

    void spawnroad()
    {
        try
        {
            if (roadLast.position.x < 11)
            {
                //spawns a new yellow lane road for each lane
                Transform newRoad;
                if (topLane && topLaneTime <=0)
                {
                    newRoad = Instantiate(road, new Vector3(roadLast.position.x + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).transform;
                }
                else
                {
                    newRoad = Instantiate(roadPart, new Vector3(roadLast.position.x + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).transform;
                }
                roadLast = newRoad;
            }
        }
        catch {
            Transform newRoad;
            if (topLane)
            {
                newRoad = Instantiate(road, new Vector3(12+ roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).transform;
            }
            else
            {
                newRoad = Instantiate(roadPart, new Vector3(12 + roadDist, -4.35f, 0), Quaternion.identity, GameObject.Find("roads").transform).transform;
            }
            roadLast = newRoad;
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
                    Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = newBar;
                }
            }
            catch
            {
                Instantiate(bridgeBar, new Vector3(guardLast.position.x + guardDist, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                guardLast = newBar;
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
                    buildings newbuildingFront = Instantiate(buildingFront, new Vector3(frontBuildingLast.position.x + frontBuildingDist, 0.12f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); ; //spawns new backround building
                    newbuildingFront.setSkin(getbuildingFromOdds(buildingsFrontOdds, buildingsFrontCurrOdds, buildingFrontSkins));
                    frontBuildingLast = newbuildingFront.gameObject.transform;
                    buildingFrontList++;
                    frontBuildingDist = 2.95f;
                }
                else
                {
                    if (frontBillboardList < 3)
                    {
                        GameObject bboard = Instantiate(frontBillboard, new Vector3(frontBuildingLast.position.x + frontBuildingDist + 0.5f, -0.72f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<buildings>().setSkin(getbuildingFromOdds(frontBillboardOdds, frontBillboardCurrOdds, frontBillboardSkins));
                        billboards.Add(bboard);
                        frontBuildingLast = bboard.gameObject.transform;
                        buildingFrontList = 0;
                        frontBillboardList++;
                        frontBuildingDist = 3.5f;
                    }
                    else
                    {
                        GameObject bboard = Instantiate(bigFrontBillboard, new Vector3(frontBuildingLast.position.x + frontBuildingDist + 2.0f, -1.36f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
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
                    buildings newBuilding = Instantiate(building, new Vector3(buildingLast.position.x + buildingDist, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                    newBuilding.setSkin(getbuildingFromOdds(buildingsOdds, buildingsCurrOdds, buildingSkins));
                    buildingLast = newBuilding.gameObject.transform;
                    buildingList++;
                      buildingDist = 2.95f;
                }
                else
                {
                    billboard bboard = Instantiate(backBillboard, new Vector3(buildingLast.position.x + buildingDist + 0.35f, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<billboard>(); //spawns new backround building
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
            buildings newBuilding = Instantiate(building, new Vector3(12, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
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
                buildings newBuilding = Instantiate(farBuilding, new Vector3(farBuildingLast.position.x + farBuildingDist, 0.15f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                newBuilding.setSkin(getbuildingFromOdds(farBuildingsOdds, farBuildingsCurrOdds, farBuildingSkins));
                farBuildingLast = newBuilding.gameObject.transform;
            }
        }
        catch
        {
            buildings newBuilding = Instantiate(farBuilding, new Vector3(13, 0.15f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
            newBuilding.setSkin(getbuildingFromOdds(farBuildingsOdds, farBuildingsCurrOdds, farBuildingSkins));
            farBuildingLast = newBuilding.gameObject.transform;
        }
    }

    void spawnSkyline()        //skyline buildings
    {
        if (skylineLast.position.x < 10)
        {
            buildings newSkyline = Instantiate(skyline, new Vector3(skylineLast.position.x + skylineDist, 2.5f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
            newSkyline.setSkin(getbuildingFromOdds(skylineOdds, skylineCurrOdds, skylineSkins));
            skylineLast = newSkyline.gameObject.transform;
        }
    }

    void spawnCloud()        //clouds

    {
        cloudTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (cloudTimer > 925)
        {
            Instantiate(cloud, new Vector3(13.5f, Random.Range(3.25f, 5.8f), 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
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
                topLaneTime -= Time.deltaTime * mph;
                if (topLaneTime <= 0)
                {
                    Instantiate(exitLine, new Vector3(guardLast.position.x + 8.85f, 0.225f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    Instantiate(mergeText, new Vector3(guardLast.position.x + 4.0f, 0.55f, 0), Quaternion.identity, GameObject.Find("roads").transform);
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
                    Transform newGuard = Instantiate(guard, new Vector3(guardLast.position.x + guardDist, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    guardLast = newGuard;

                    Instantiate(mergeLine, new Vector3(lastGuard.position.x + 10.35f, 0.225f, 0), Quaternion.identity, GameObject.Find("roads").transform);
                    Instantiate(yelloBarrel, new Vector3(lastGuard.position.x + 1.7f, 0.4f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(mergeText, new Vector3(lastGuard.position.x + 16.25f, 0.55f, 0), Quaternion.identity, GameObject.Find("roads").transform);
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

        if (!inTutorial || (tutorialSteps > 0 ^ tutorialSteps < 3) && topLane) { areaTimer -= Time.deltaTime * mph; }
        if (areaTimer <= 0  && (topLane && topLaneTime <= 0))
        {
            if (areaEvent == 0 && buildingFrontList == 1) { areaEvent = 1; } else if (areaEvent == 1 && buildingFrontList == 0) { areaEvent = 0;  }
        }
        if (areaEvent == 1)
        {
            if (!startBridge)
            {
                Transform bridgeStarting = Instantiate(bridgeStart, new Vector3(guardLast.position.x + 2, 0.95f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                Instantiate(bridgeBar, new Vector3(guardLast.position.x + 1.5f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                frontBuildingLast = bridgeStarting;
                frontBuildingDist = 10;
                buildingDist = 10;
                Instantiate(bridgeStartConnector, new Vector3(guardLast.position.x -1, -4.8f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                Transform newBar = Instantiate(bridgeBar2, new Vector3(guardLast.position.x + 2, -4.75f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
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
                    Transform bridgeEnding = Instantiate(bridgeEnd, new Vector3(frontBuildingLast.position.x + frontBuildingDist, 0.95f, 0), Quaternion.identity, GameObject.Find("guards").transform).transform;
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x - 2.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x + 2, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x + 6.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeBar, new Vector3(frontBuildingLast.position.x + 10.25f, 1.3f, 0), Quaternion.identity, GameObject.Find("guards").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + 4, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + -0.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
                    Instantiate(bridgeSupport, new Vector3(frontBuildingLast.position.x + 0.5f, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform);
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
                }
            }
        }
    }

    void spawnBridgePillar()
    {
        if (frontBuildingLast.position.x < 5)
        {
            if (buildingFrontList < 1)
            {
                GameObject newBuilding = Instantiate(bridgePillar, new Vector3(frontBuildingLast.position.x + frontBuildingDist, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                frontBuildingLast = newBuilding.transform;
                frontBuildingDist = 7.5f;
                buildingFrontList++;
            }
            else
            {
                if (frontBillboardList < 2)
                {
                    GameObject bboard = Instantiate(bridgeBillboard, new Vector3(frontBuildingLast.position.x + frontBuildingDist + 1.0f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<billboard>().setSkin(bridgeBillboardSkin);
                    frontBuildingLast = bboard.transform;
                    frontBuildingDist = 9.5f;
                    billboards.Add(bboard);
                    buildingFrontList = 0;
                    frontBillboardList++;
                }
                else
                {
                    GameObject bboard = Instantiate(bridgeBigBillboard, new Vector3(frontBuildingLast.position.x + frontBuildingDist + 1.0f, 0.65f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
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
        if (buildingLast.position.x < 11)
        {
            Transform newSupport = Instantiate(bridgeSupport, new Vector3(buildingLast.position.x + buildingDist, 1.0f, 0), Quaternion.identity, GameObject.Find("buildings").transform).transform; //spawns new backround building
            buildingLast = newSupport;
            buildingDist = 1.5f;
            buildingList++;
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
        billboard bboard = billboards.ToArray()[0].GetComponent<billboard>();
        Image overButton = Instantiate(bboard.gameOverOBJ, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform).GetComponent<Image>();
        overButton.sprite = bboard.skinCurr;
        if (inTutorial)
        {
            activeHand = Instantiate(tutorialHandOBJ, new Vector3(bboard.gameObject.transform.position.x + 1.5f, 2.8f, 0), Quaternion.identity).GetComponent<tutorialHand>();
            activeHand.setBounce(0.45f, 0.25f, 30, 0.5f, 2);
            activeText = Instantiate(tutorialTexts[4], new Vector3(3.8f, -0.8f, 0), Quaternion.identity);
        }

        Destroy(bboard.gameObject);

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

    private void changeOdds(int index, List<float> setList, List<float> currentList)
    {
        float diff = currentList[index] - (setList[index]/2);
        float listDiff = (diff * setList[index]) / (setList.Count - 1.0f);
        for (int i = 0; i < setList.Count; i++)
        {
            currentList[i] += (diff * setList[i]) + listDiff; //this took me way to fucking long to figue out
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
