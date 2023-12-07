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

    public Vector3 targetPos; //where the player car has to go
    public float disMove; //speed the car has to move to get to targetPos on time
    public float overshoot; // keeps track of the distance moved so you know it wont go too far

    public float startPos;

    public AudioClip[] turns;
    public AudioClip crash1;
    public AudioClip crash2;

    void OnEnable()
    {
        pManager = GameObject.Find("playerManager").GetComponent<playerManager>();

        PlayerPrefs.SetInt("playerCarType", 0); //saves the player car type
        PlayerPrefs.SetInt("playerBody", 4); //saves the new high score
        PlayerPrefs.SetInt("wheelBody", 1); //saves the new high score
        PlayerPrefs.SetInt("windowTint", 2); //saves the new high score
        PlayerPrefs.SetInt("liveryTint", 2); //saves the new high score
        PlayerPrefs.SetInt("liveryColorTint", 17); //saves the new high score

        getPlayerCustomazation();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        setPlayerCustomazation();

        startPos = -7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            if (startPos == transform.position.x && controller.billboards[0].gameObject.name != "Start Billboard") {
                if (Input.touchCount > 0 && Time.timeScale > 0 && !tapped) // if the user touches the phone screen
                {
                    Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
                    tapped = true;

                    if (tapPoint.y < 3.0f)
                    {
                        if (tapPoint.y > transform.position.y && Mathf.Abs(tapPoint.y - transform.position.y) > 0.3) //if the tap if above the player car
                        {
                            laneUp();
                        }
                        else if (tapPoint.y < transform.position.y && Mathf.Abs(tapPoint.y - transform.position.y) > 0.3) //if the tap is below the player car
                        {
                            laneDown();
                        }
                    }
                } else
                {
                    tapped = false;
                }

                if (transform.position != targetPos) //if player car isnt where its supose to be
                {
                    transform.position += new Vector3(0, 4 * (Time.deltaTime / disMove), 0); //moves the player towards where they need to be
                    overshoot -= Mathf.Abs(4 * Time.deltaTime / disMove); //calculate the distance it moved since getting its new current
                    if (overshoot < 0) //checks if its past if target
                    {
                        transform.position = targetPos; //places player car where it should be
                        overshoot = 0; //resets overshoot
                    }
                }
            } else {
                transform.position += new Vector3(3*(moveTime) * Time.deltaTime, 0, 0);
                if(startPos - transform.position.x < 0)
                {
                    transform.position = new Vector3(startPos, transform.position.y, 0);
                    targetPos = transform.position;
                }
            }
        }

        wheelB.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
        wheelF.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * controller.mph * 10, Space.Self);
    }

    public void laneUp() //if tap is above player car
    {
        float maxLane = 0;
        if (controller.topLane)
        {
            if(controller.topLaneTime < 130)
            {
                maxLane = 0.65f;
            }
            else
            {
                maxLane = -0.6f;
            }
        }
        else
        {
            if (controller.topLaneTime < 10)
            {
                maxLane = -0.6f;
            }
            else
            {
                maxLane = 0.65f;
            }
        }
        if (targetPos.y < maxLane && Mathf.Abs(transform.position.y - targetPos.y) < 0.35f && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
        {
            targetPos += new Vector3(0, 1.25f, 0); //changes targetPos to the new lane it needs to go to
            disMove = (targetPos.y - transform.position.y) * moveTime; //calculates the speed the player car needs to go to switch lanes
            overshoot = Mathf.Abs(targetPos.y - transform.position.y); //calculates overshoot to where it needs to go
            changeOrder(-1);

            AudioSource.PlayClipAtPoint(turns[Random.Range(0, turns.Length - 1)], new Vector3(0, 0, -7), controller.masterVol * controller.sfxVol * controller.sfxVol);

            playHorn();
        }
    }

    public void laneDown()
    {
        if (targetPos.y > -4.35f && Mathf.Abs(transform.position.y - targetPos.y) < 0.35f && controller.playing) //checks if the player car is near its target lane to stops player from rapdily changing multiple lanes
        {
            targetPos += new Vector3(0, -1.25f, 0); //changes targetPos to the new lane it needs to go to
            disMove = (targetPos.y - transform.position.y) * moveTime; //calculates the speed the player car needs to go to switch lanes
            overshoot = Mathf.Abs(targetPos.y - transform.position.y); //calculates overshoot to where it needs to go
            changeOrder(1);

            AudioSource.PlayClipAtPoint(turns[Random.Range(0, turns.Length - 1)], new Vector3(0, 0, -7), controller.masterVol * controller.sfxVol);

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
            if (collision.tag == "car")
            {
                collision.GetComponent<cars>().speed = 0; //stops the car that crashes into the player (so they can file an insurence claim aganst the player)
                controller.bannedLanes.Add(collision.GetComponent<cars>().lane);
                if (collision.transform.position.y < transform.position.y)
                {
                    changeOrder(-1);
                    controller.bannedLanes.Add(collision.GetComponent<cars>().lane-1);
                    AudioSource.PlayClipAtPoint(crash2, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                }
                else if (collision.transform.position.y > transform.position.y)
                {
                    changeOrder(1);
                    controller.bannedLanes.Add(collision.GetComponent<cars>().lane+1);
                    AudioSource.PlayClipAtPoint(crash2, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                }
                crash(); //what happens when the player crashes
            }
            else if (collision.tag == "barrier")
            {
                AudioSource.PlayClipAtPoint(crash2, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                crash(); //what happens when the player crashes
            }
            else if (collision.tag == "coin")
            {
                if (!collision.GetComponent<coins>().collected)
                {
                    collision.GetComponent<coins>().pickup();
                }
            }
            else if (collision.tag == "smoke")
            {
                float smokeLevel = collision.GetComponent<manhole>().getSmokeValue() * smokeMulitplyer;
                if (smokeLevel > 500)
                {
                    smokeLevel = 500;
                }

                if (controller.screenDistortTarget < smokeLevel/500)
                {
                    
                    controller.screenDistortTarget = smokeLevel / 500;
                }
            }
        }
    }

    public void setLane(int lane)
    {
        transform.position = new Vector3(-12, (-lane * 1.25f) + 0.65f, 0);
        body.sortingOrder = 2 + lane;
        window.sortingOrder = 2 + lane;
        wheelF.sortingOrder = 2 + lane;
        wheelB.sortingOrder = 2 + lane;
        livery.sortingOrder = 2 + lane;
    }

    private void changeOrder(int change)
    {
        body.sortingOrder += change;
        window.sortingOrder += change;
        wheelF.sortingOrder += change;
        wheelB.sortingOrder += change;
        livery.sortingOrder += change;
    }

    private void getPlayerCustomazation()
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

        setCarStats(carTypeSave, wheelSave, tintSave);
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
        smokeMulitplyer = 1 - pManager.carPartsData.windowTints[tintSave].screenEffect;
    }

    private float calcStartMPH(int carTypeSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].startMPH;
    }

    private float calcUpMPH(int carTypeSave, int wheelSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].speedUp + pManager.carPartsData.wheelTypes[wheelSave].speedUp;
    }

    private float calcmoveTime(int carTypeSave, int wheelSave)
    {
        return pManager.carPartsData.carTypes[carTypeSave].moveTime + pManager.carPartsData.wheelTypes[wheelSave].moveTime;
    }

    private void playHorn()
    {
        Transform closestCar = findClosestCar();
        float closestDist = transform.position.x - closestCar.position.x;
        //Debug.Log(closestDist + " : "+ closestCar.position.x);
        if (closestDist < 2.75f && closestDist > 1)
        {
            AudioSource.PlayClipAtPoint(closestCar.GetComponent<cars>().horn, new Vector3(0, 0, -9), controller.masterVol * controller.sfxVol);
        }
    }

    private Transform findClosestCar()
    {
        Transform cardsOBJ = GameObject.Find("cars").transform;
        Transform closest = cardsOBJ.GetChild(0);
        for (int i = 1; i < cardsOBJ.childCount; i++)
        {
            Transform currCar = cardsOBJ.GetChild(i).transform;
            if(targetPos.x - currCar.position.x > 0 && currCar.position.x < closest.position.x && currCar.position.y == targetPos.y)
            {
                closest = currCar;
            }
        }
        return closest;
    }
}