using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    public bool playing;
    public float mph; //controls the speed of the game
    public float score;
    public float highScore;

    public float defaultmph;

    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public GameObject over; // game over ui
    public GameObject highScoreUI; // high score ui

    public GameObject playerCar;

    public GameObject divider; //divider gameobject to spawn
    public float dividerTimer;

    public GameObject building; //building gameobject to spawn
    public float buildingTimer;

    public GameObject cars; //car gameobject to spawn
    public float carTimer;

    public List<float> bannedLanes;
    public List<int> carsPast;

    // Start is called before the first frame update
    void Start()
    {
        playing = false; 
        mph = 0; //sets inital mph

        defaultmph = 30.0f;

        over.SetActive(false); //hides game over ui (such as high score and play again button)
        highScoreUI.SetActive(false);

        highScore = PlayerPrefs.GetInt("highscore", (int)highScore); //sets high score to the one saved

        playerCar = GameObject.Find("playerCar");

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) //if in a game 
        {
            mph += Time.deltaTime / 2; //graudully increeses the mph (+1 every 2 sec)
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

            buildingTimer += Time.deltaTime * mph; //timer that spawns a new builing
            if (buildingTimer > 23.5)
            {
                Instantiate(building, new Vector3(12, 1.25f, 0), Quaternion.identity, GameObject.Find("buildings").transform); //spawns new backround building
                buildingTimer = 0;
            }

            carTimer += Time.deltaTime * mph; // time that spawns a new car that speeds up depending on the speed of the game (mph)
            if (carTimer > 80)
            {
                Instantiate(cars, new Vector3(12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform);  //spawn new car in a random lane before going on screen
                carTimer = 0;
            }
        }
        else //if game isnt playing
        {
            carTimer += Time.deltaTime; //timer to spawn a new car after game is over
            if (carTimer > 1.0f) 
            {
                GameObject newCar = Instantiate(cars, new Vector3(-12, (Random.Range(0, -5) * 1.25f) + 0.65f, 0), Quaternion.identity, GameObject.Find("cars").transform); //spawn new car in a random lane behind the player
                carTimer = 0;
                newCar.GetComponent<cars>().setLane();

                if (newCar.transform.position.y < 0.65f && newCar.transform.position.y > -4.35f && !carsPast.Contains(newCar.GetComponent<cars>().lane))
                {
                    carsPast.Add(newCar.GetComponent<cars>().lane);
                    carsPast.Remove(carsPast.ToArray()[0]);
                }
            }
        }

        scoreDisplay.text = "Score: " + (int)score + "m"; //shows the score of the game

    }

    public void newGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }

    public void gameOver()
    {
        playing = false; //sets the game to no longer be playing
        mph = 0; //stops the backround from moving (as the player is supose to be still)

        over.SetActive(true); //shows the game over ui (such as high score and play again button)
        highScoreUI.SetActive(true);

        //Debug.Log("ll");

        if (score > highScore) { //check if theres a new high score
            highScore = score; // sets the new high score
            PlayerPrefs.SetInt("highscore", (int)highScore); //saves the new high score
        }
        highScoreDisplay.text = "High Score: " + (int)highScore + "m"; //displays the high score

    }

    public void StartGame()
    {
        playing = true;
        mph = defaultmph;
        playing = true;

        if (!carsPast.Contains(2))
        {
            playerCar.GetComponent<playerCar>().setLane(2);
        }
        else if (!carsPast.Contains(1))
        {
            playerCar.GetComponent<playerCar>().setLane(3);
        }
        else {
            playerCar.GetComponent<playerCar>().setLane(1);
        }
    }
}
