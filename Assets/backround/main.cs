using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
    public profile prof;

    public bool playing;
    public bool isOver;
    public float mph; //controls the speed of the game
    public float score;
    public float highScore;

    public float masterVol;
    public float sfxVol;
    public float musicVol;

    public int coins; //amount of coins a player has collected in a game
    public int totalCoins; //amount of coins a player has in total

    public GameObject[] coin; //coin gameobject to spawn
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

    public GameObject divider1; //divider gameobject to spawn
    public GameObject divider2; //divider gameobject to spawn
    public GameObject divider3; //divider gameobject to spawn
    public GameObject divider4; //divider gameobject to spawn
    public float dividerTimer;

    public GameObject building; //building gameobject to spawn
    public GameObject billboard; //building gameobject to spawn
    public GameObject bigBillboard; //building gameobject to spawn
    public List<Sprite> buildingSkins;
    public List<float> buildingsOdds;
    public List<float> buildingsCurrOdds;
    public float buildingList;
    public float billboardList;
    public float buildingTimer;

    public GameObject building1; //building gameobject to spawn
    public GameObject billboard1; //building gameobject to spawn
    public GameObject bigBillboard1; //building gameobject to spawn
    public List<Sprite> building1Skins;
    public List<float> buildings1Odds;
    public List<float> buildings1CurrOdds;
    public float buildingList1;
    public float billboardList1;
    public float buildingTimer1;

    public GameObject skyline;
    public int skylineList;
    public float skylineTimer;

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
    public float specalCarOdds;

    public GameObject bus; //bus gmaeobject to spawn
    public GameObject overBus; //bus gmaeobject to spawn

    public List<float> bannedLanes;
    public List<int> carsPast;

    public GameObject settingsUI;

    public AudioClip menuAmbience;
    public AudioClip gameAmbience;
    public AudioSource menuSound;

    public AudioClip clickSound;

    // Start is called before the first frame update
    void OnEnable()
    {
        //prof = GameObject.Find("profile").GetComponent<profile>();

        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        menuSound = GameObject.Find("ambience").GetComponent<AudioSource>();

        masterVol = PlayerPrefs.GetFloat("masterVol", 1); //sets high score to the one saved
        sfxVol = PlayerPrefs.GetFloat("sfxVol", 1); //sets high score to the one saved
        musicVol = PlayerPrefs.GetFloat("musicVol", 1); //sets the radio volume to the one it was last on

        playing = false;
        isOver = false;
        scoreShowing = false;
        mph = 0; //sets inital mph

        coins = 0;
        highScore = PlayerPrefs.GetInt("highscore", 0); //sets high score to the one saved
        totalCoins = PlayerPrefs.GetInt("coins", 0); //sets high score to the one saved

        specalCarOdds = 0.05f;

        milestone = 0;
        blimpSpeed = new Vector3(0.1f, 0.05f, 0);

        menuSound.clip = menuAmbience;
        menuSound.Play();

        setCarOdds();
        setBuildingOdds();
        setBuilding1Odds();
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
            spawnFrontBuilding();
            spawnBackBuilding();
            spawnSkyline();
            spawnCloud();

            //make game element
            spawnGameCar();
            spawnCoin();

            //set blimp text
            blimpText();
        }
        else //if game isnt playing
        {
            spawnMenuCar();
        }


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
    }

    void spawnDivider()
    {
        dividerTimer += Time.deltaTime * mph; //timer to spawn new lane divider
        if (dividerTimer > 15)
        {
            //spawns a new yellow lane divider for each lane
            Instantiate(divider1, new Vector3(12, 0.0f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            Instantiate(divider2, new Vector3(12, -1.25f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            Instantiate(divider3, new Vector3(12, -2.5f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            Instantiate(divider4, new Vector3(12, -3.75f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
            dividerTimer = 0;
        }
    }

    void spawnGuard()
    {
        guardTimer += Time.deltaTime * mph; //timer to spawn new road guard 
        if (guardTimer > 2)
        {
            //spawns a new rail guard for the edge of the road
            Instantiate(guard2, new Vector3(12, 1.36f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
            Instantiate(guard, new Vector3(12, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
            guardTimer = 0;
        }
    }

    void spawnFrontBuilding()        //front buildings
    {
        buildingTimer1 += Time.deltaTime * mph; //timer that spawns a new builing
        if (buildingTimer1 > 30.5)
        {
            if (buildingList1 < 4)
            {
                buildings newBuilding1 = Instantiate(building1, new Vector3(12, 0.36f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); ; //spawns new backround building
                newBuilding1.setSkin(getbuilding1FromOdds());
                buildingList1++;
                buildingTimer1 = 0;
            }
            else
            {
                if (billboardList1 < 3)
                {
                    GameObject bboard = Instantiate(billboard1, new Vector3(12, -2.15f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBillboard = true;
                    billboards.Add(bboard);
                    buildingList1 = 0;
                    buildingTimer1 = 0;
                    billboardList1++;
                }
                else
                {
                    GameObject bboard = Instantiate(bigBillboard1, new Vector3(13.125f, -1.64f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    bboard.GetComponent<buildings>().isBigBillboard = true;
                    billboards.Add(bboard);
                    buildingList1 = 0;
                    buildingTimer1 = -18.5f;
                    billboardList1 = 0;
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
                if (billboardList < 3)
                {
                    buildings bboard = Instantiate(billboard, new Vector3(12, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform).GetComponent<buildings>(); //spawns new backround building
                    bboard.isBillboard = true;
                    buildingList = 0; buildingTimer = -25;
                    billboardList++;
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
    }

    void spawnSkyline()        //skyline buildings
    {
        skylineTimer += Time.deltaTime * mph; //timer that spawns a new builing
        if (skylineTimer > 150)
        {
            Instantiate(skyline, new Vector3(13.5f, 2.8f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
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

    void spawnGameCar()
    {
        carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
        if (carTimer > 80)
        {
            float specalCar = Random.Range(0.0f, 1.0f);
            if (specalCar > specalCarOdds)
            {
                spawnNormalCar();
                specalCarOdds += 0.1f;
            }
            else
            {
                spawnSpecalCar();
                specalCarOdds = 0.05f;
            }
            carTimer = 0;
        }
    }

    void spawnMenuCar()
    {
        carTimer += Time.deltaTime; //timer to spawn a new car after game is over
        if (carTimer > 1.0f)
        {
            GameObject newCar = this.gameObject;
            if (!isOver)
            {
                newCar = Instantiate(getCarFromOdds(), new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
                carTimer = 0;
            }
            else
            {
                if (carList < 3)
                {
                    newCar = Instantiate(getCarFromOdds(), new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
                    carList++;
                }
                else
                {
                    newCar = Instantiate(overBus, new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("Extra").transform); //spawn new car in a random lane behind the player
                    carList = 0;
                }
            }

            newCar.GetComponent<cars>().setLane();

            if (newCar.transform.position.y < 0.65f && newCar.transform.position.y > -4.35f && !carsPast.Contains(newCar.GetComponent<cars>().lane))
            {
                carsPast.Add(newCar.GetComponent<cars>().lane);
                carsPast.Remove(carsPast[0]);
            }

            carTimer = 0;
        }
    }

    void spawnNormalCar()
    {
        Instantiate(getCarFromOdds(), new Vector3(12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
    }

    void spawnSpecalCar()
    {
        Instantiate(bus, new Vector3(13, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
    }

    void spawnCoin()
    {
        coinTimer += Time.deltaTime * mph;
        if (coinTimer > 350)
        {
            Instantiate(coin[Random.Range(0, coin.Length)], new Vector3(12, -1.85f, 0), Quaternion.identity, GameObject.Find("coins").transform);  //spawn new car in a random lane before going on screen
            coinTimer = 0;
        }

        if (billboards.ToArray()[0].transform.position.x < -9.25f)
        {
            billboards.Remove(billboards.ToArray()[0]);
        }
    }

    GameObject getCarFromOdds()
    {
        float newCarOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < carsOdds.Count; i++)
        {
            oddsAccum += carsCurrOdds[i];
            if (oddsAccum > newCarOdds)
            {
                changeOdds(i, carsOdds, carsCurrOdds);
                return carsList[i];
            }
        }

        changeOdds(0, carsOdds, carsCurrOdds);
        return carsList[0];
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

    Sprite getbuilding1FromOdds()
    {
        float newBuilding1Odds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < buildings1Odds.Count; i++)
        {
            oddsAccum += buildings1CurrOdds[i];
            if (oddsAccum > newBuilding1Odds)
            {
                changeOdds(i, buildings1Odds, buildings1CurrOdds);
                return building1Skins[i];
            }
        }

        changeOdds(0, buildings1Odds, buildings1CurrOdds);
        return building1Skins[0];
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
            Instantiate(overBigBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
        }
        else
        {
            Instantiate(overBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("Front Billboards").transform);
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

    public void shopButton()
    {

        AudioSource.PlayClipAtPoint(clickSound, transform.position, masterVol * sfxVol);
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

    private void setCarOdds()
    {
        for (int i = 0; i < carsList.Count; i++)
        {
            float addOdds = carsList[i].GetComponent<cars>().odds;
            carsOdds.Add(addOdds);
            carsCurrOdds.Add(addOdds);
        }
    }

    private void setBuildingOdds()
    {
        float[] buildingOddsNew = { 0.25f, 0.3f, 0.45f };
        for (int i = 0; i < buildingSkins.Count; i++)
        {
            buildingsOdds.Add(buildingOddsNew[i]);
            buildingsCurrOdds.Add(buildingOddsNew[i]);
        }
    }

    private void setBuilding1Odds()
    {
        float[] building1OddsNew = {1.0f};
        for (int i = 0; i < building1Skins.Count; i++)
        {
            buildings1Odds.Add(building1OddsNew[i]);
            buildings1CurrOdds.Add(building1OddsNew[i]);
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
}
