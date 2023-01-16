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

    public GameObject divider; //divider gameobject to spawn
    public float dividerTimer;

    public GameObject building; //building gameobject to spawn
    public GameObject billboard; //building gameobject to spawn
    public GameObject bigBillboard; //building gameobject to spawn
    public float buildingList;
    public float billboardList;
    public float buildingTimer;

    public List<GameObject> billboards;

    public GameObject guard; //rail guard gameobject to spawn
    public GameObject guard2; //rail guard gameobject to spawn
    public float guardTimer;

    public GameObject[] milestoneSigns;
    public GameObject milestoneBigSign;
    public int milestone;

    public GameObject cars; //car gameobject to spawn
    public float carList; //how many cars have spawned since the last bus
    public float carTimer;

    public GameObject bus; //bus gmaeobject to spawn
    public GameObject overBus; //bus gmaeobject to spawn

    public List<float> bannedLanes;
    public List<int> carsPast;

    // Start is called before the first frame update
    void Start()
    {
        //prof = GameObject.Find("profile").GetComponent<profile>();

        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();

        playing = false;
        isOver = false;
        scoreShowing = false;
        mph = 0; //sets inital mph

        highScore = PlayerPrefs.GetInt("highscore", (int)highScore); //sets high score to the one saved

        milestone = 0;
        blimpSpeed = new Vector3(0.1f, 0.05f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) //if in a game 
        {
            if (mph < playerCar.startMph)
            {
                mph += (mph/2 + 15.0f) * Time.deltaTime;
            }
            else
            {
                mph += playerCar.upMph * Time.deltaTime; //graudully increeses the mph (+1 every 2 sec)
            }

            score += (mph * 0.44704f) * Time.deltaTime; //increses the score based on how far the player has gone

            dividerTimer += Time.deltaTime * mph; //timer to spawn new lane divider
            if (dividerTimer > 15)
            {
                //spawns a new yellow lane divider for each lane
                Instantiate(divider, new Vector3(12, 0.0f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
                Instantiate(divider, new Vector3(12, -1.25f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
                Instantiate(divider, new Vector3(12, -2.5f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
                Instantiate(divider, new Vector3(12, -3.75f, 0), Quaternion.identity, GameObject.Find("dividers").transform);
                dividerTimer = 0;
            }

            guardTimer += Time.deltaTime * mph; //timer to spawn new lane divider
            if (guardTimer > 2)
            {
                //spawns a new rail guard for the edge of the road
                Instantiate(guard2, new Vector3(12, 1.36f, 0), Quaternion.identity, GameObject.Find("top guards").transform);
                Instantiate(guard, new Vector3(12, -4.85f, 0), Quaternion.identity, GameObject.Find("bottom guards").transform);
                guardTimer = 0;
            }

            buildingTimer += Time.deltaTime * mph; //timer that spawns a new builing
            if (buildingTimer > 25.5)
            {
                if (buildingList < 6)
                {
                    Instantiate(building, new Vector3(12, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                    buildingList++;
                    buildingTimer = 0;
                }
                else
                {
                    if (billboardList < 3)
                    {
                        GameObject bboard = Instantiate(billboard, new Vector3(12, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<buildings>().isBillboard = true;
                        billboards.Add(bboard);
                        buildingList = 0;
                        buildingTimer = 0;
                        billboardList++;
                    }
                    else
                    {
                        GameObject bboard = Instantiate(bigBillboard, new Vector3(13, 0.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                        bboard.GetComponent<buildings>().isBigBillboard = true;
                        billboards.Add(bboard);
                        buildingList = 0; buildingTimer = -25;
                        billboardList = 0;
                    }
                }
            }

            carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
            if (carTimer > 80)
            {
                if (carList < 7) {
                    Instantiate(cars, new Vector3(12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                    carList++;
                }
                else
                {
                    Instantiate(bus, new Vector3(13, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                    carList = 0;
                }
                carTimer = 0;
            }

            if (billboards.ToArray()[0].transform.position.x < -8.5f)
            {
                billboards.Remove(billboards.ToArray()[0]);
            }

            if (!scoreShowing)
            {
                string scoretxt = scoreText.text = "Don't Crash Bro";
                scoreText.text = scoretxt.Substring(textNum);

                if (textNum >= scoretxt.Length-1)
                {
                    textNum = 0;
                    scoreShowing = true;
                }

            } else
            {
                string scoretxt = "Score: " + (int)score + "m";
                if (textNum <= scoretxt.Length-1)
                {
                    scoreText.text = scoretxt.Substring(0, textNum);
                } else
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
        else //if game isnt playing
        {
            carTimer += Time.deltaTime; //timer to spawn a new car after game is over
            if (carTimer > 1.0f) 
            {
                GameObject newCar = this.gameObject;
                if (!isOver)
                {
                    newCar = Instantiate(cars, new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
                    carTimer = 0;
                } else
                {
                    if (carList < 3)
                    {
                        newCar = Instantiate(cars, new Vector3(-14, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
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
                    carsPast.Remove(carsPast.ToArray()[0]);
                }

                carTimer = 0;
            }
        }


        if ((scoreBlimp.transform.position.x < -7.79f || scoreBlimp.transform.position.x > -4.82f) && Mathf.Abs(scoreBlimp.transform.position.x - newBlimpLocation.x) > 1.0) //if blimp went out of bounce on the X
        {
            updateBlimpX(); // find new target location for blimp
        }
        if ((scoreBlimp.transform.position.y > 4.58f || scoreBlimp.transform.position.y < 3.78f) && Mathf.Abs(scoreBlimp.transform.position.y - newBlimpLocation.y) > 0.5) //if blimp went out of bounce on the X
        {
            updateBlimpY(); // find new target location for blimp
        }

        scoreBlimp.transform.position += blimpSpeed * Time.deltaTime;

        if (score > (milestone + 210))
        {
            setMilestone();
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
            Instantiate(overBigBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("score Blimp").transform);
        }
        else
        {
            Instantiate(overBoard, bboard.transform.position, Quaternion.identity, GameObject.Find("score Blimp").transform);
        }

        Destroy(bboard);
    }

    public void pauseButton()
    {
        if(Time.deltaTime > 0 && playing)
        {
            Time.timeScale = 0;
            Instantiate(pauseMenu);
        }
    }

    public void shopButton()
    {

    }

    public void settingsButton()
    {

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

}
