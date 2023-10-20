using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maniac_van : cars
{
    public float currSpeed; //speed of thr car
    public float startSpeed; //speed of thr car

    public float turningSpeed;

    public SpriteRenderer party;
    public Sprite[] partySkins; //array of car skins
    public int partyList;
    public float partyBeatTimer;
    public float partyBeatTime;
    public GameObject partyMusic;
    public AudioSource musicPlayer;

    public GameObject coins;
    public float coinTimer;

    // Start is called before the first frame update
    void Start()
    {
        speedMin = 15;
        speedMax = 23;

        startSpeed = Random.Range(speedMin, speedMax);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn

        currSpeed = startSpeed;
        turningSpeed = Random.Range(0.6f, 0.9f);
        turningTimer = Random.Range(0.0f, 5.0f);

        partyBeatTime = 0.422f;
        party.sprite = partySkins[Random.Range(0, partySkins.Length)];
        musicPlayer.volume = controller.musicVol;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            if (controller.mph > controller.playerCar.startMph) // checks if its not in the game start animation
            {
                transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / currSpeed), 0, 0); //move fowards in game
            }
            else
            {
                transform.position += new Vector3((((controller.playerCar.startMph / 1.5f) - controller.mph) * 5 * Time.deltaTime / currSpeed), 0, 0); //car movment in the start up animation
            }
        }
        else
        {
            transform.position = transform.position + new Vector3(Time.deltaTime * (currSpeed / 2.0f), 0, 0); // moves the across cars the screen when game isnt on (like game over screen)
        }

        if(coinTimer < 0)
        {
            Instantiate(coins, transform.position, Quaternion.identity, GameObject.Find("coins").transform);
            coinTimer = Random.Range(0.5f, 1.15f);
        }
        coinTimer -= Time.deltaTime;

        transform.position = transform.position - new Vector3(0, turningSpeed * Time.deltaTime, 0); //move fowards in game
        if (transform.position.y <= -4.35 && turningSpeed > 0)
        {
            turningSpeed *= -1;
        }
        if (transform.position.y >= 0.65 && turningSpeed < 0)
        {
            turningSpeed *= -1;
        }
        if(turningTimer < 0)
        {
            turningTimer = Random.Range(1.0f, 5.0f);
            turningSpeed *= -1;
        }
        turningTimer -= Time.deltaTime;

        if (transform.position.x <= -12) // checks if the car is on screen
        {
            partyMusic.transform.position -= new Vector3(0, 0, Time.deltaTime * 10);
        }

        if (transform.position.x <= -18 || transform.position.x >= 25) // checks if the car is on screen
        {
            //Destroy(gameObject); // destroys it otherwise
        }

        if (currSpeed < startSpeed)
        {
            currSpeed += Time.deltaTime * 2;
        }

        partyBeatTimer += Time.deltaTime;
        if (partyBeatTimer > partyBeatTime)
        {
            partyList++;
            if(partyList >= partySkins.Length)
            {
                partyList = 0;
            }
            party.sprite = partySkins[partyList];
            partyBeatTimer -= partyBeatTime;
        }

        musicPlayer.volume = controller.musicVol;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "car") //if a car hits another car
        {
            if (transform.position.y > (collision.transform.position.y + 0.4f))
            {
                turningSpeed *= -1;
            }
            else if (transform.position.y < (collision.transform.position.y - 0.4f))
            {
                turningSpeed *= -1;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                currSpeed = collision.GetComponent<cars>().speed - 1; //changes the speed so cars won't go through eachother
            }
        }
    }
}
