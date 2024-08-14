using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class construction : cars
{
    public bool inSight;
    public bool inEnd;
    public GameObject cones;
    public Transform conesLast;

    public List<GameObject> coneList;
    public Sprite coneOutline;

    public float carTime;
    public float carTimer;

    // Start is called before the first frame update
    void Start()
    {
        forceMass = 1.45f;
        blinkTime = -1;

        speed = 0;
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        targPos = transform.position.y;

        turnTime = 0.75f;
        isCar = false;
        setLane();
        controller.carsInGame.Add(gameObject);
        controller.constructionOBJ = this;

        if (controller.senseVision) { createOuline(controller.enhancedSense); }

        controller.bannedLanes.Add(lane);
        carTime = 55;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing) //checks if game is in season
        {
            transform.position = transform.position - new Vector3(((controller.mph) * Time.deltaTime / 6f), 0, 0); //move fowards in game

            if (controller.laserOn)
            {
                getsShot();
            }

            if (inEnd && conesLast == null) // checks if the car is on screen
            {
                destroyCar(); // destroys it otherwise
            }

            if (inSight && !inEnd)
            {
                spawnCones();
            }
            else
            {
                carTimer += Time.deltaTime * controller.mph;

                if (transform.position.x < 35)
                {
                    controller.carTimer = -125;
                }
                else if (carTimer > carTime)
                {
                    cars c = Instantiate(controller.getCarFromOdds(controller.carsOdds, controller.carsCurrOdds, controller.carsList), new Vector3(12, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform).GetComponent<cars>();
                    c.makeTraffic(Random.Range(10, 15.5f));
                    carTimer -= carTime;
                    controller.carTimer -= 25;
                }
                if (transform.position.x < 27)
                {
                    spawnJam();
                }
            }
        }

        GetComponent<SpriteRenderer>().sprite = skins[((int)turningTimer) % 2];
        turningTimer += Time.deltaTime * 4;

        checkStuff();
    }

    void spawnJam()
    {
        conesLast = Instantiate(cones, new Vector3(transform.position.x + 2, transform.position.y + 0.25f, 0), Quaternion.identity, GameObject.Find("cars").transform).transform;
        inSight = true;
        for (int i = 0; i < 3; i++)
        {
            cars c = Instantiate(controller.getCarFromOdds(controller.carsOdds, controller.carsCurrOdds, controller.carsList), new Vector3(transform.position.x - 2.5f - (i * 3), transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform).GetComponent<cars>();
            c.makeTraffic(0);
            controller.checkCarEffects(c.gameObject);
        }
        cars ca = Instantiate(controller.getCarFromOdds(controller.carsOdds, controller.carsCurrOdds, controller.carsList), new Vector3(transform.position.x - 14.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform).GetComponent<cars>();
        ca.makeTraffic(7.5f);
        controller.checkCarEffects(ca.gameObject);
        for (int i = 0; i < 8; i++)
        {
            cars cr = Instantiate(controller.getCarFromOdds(controller.carsOdds, controller.carsCurrOdds, controller.carsList), new Vector3(transform.position.x - 11.5f + (i * 3.5f), transform.position.y + 1.25f, 0), Quaternion.identity, GameObject.Find("cars").transform).GetComponent<cars>();
            cr.makeTraffic(0);
            controller.checkCarEffects(cr.gameObject);
        }
    }

    void spawnCones()
    {
        if (transform.position.x < -110)
        {
            Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform);
            inEnd = true;
        }
        if (!inEnd)
        {
            if (conesLast.transform.position.x < 12)
            {
                conesLast = Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y + 0.5f, 0), Quaternion.identity, GameObject.Find("cars").transform).transform;
            }
        }
    }

    public override void destroyCar()
    {
        if (!inEnd) { Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform); }
        controller.bannedLanes.Remove(lane);
        controller.inConstruction = false;
        controller.constructionTimer = Random.Range(450, 1375);
        base.destroyCar();
    }

    public override void makeDisabled(float xF, float yF)
    {
        if (!inEnd)
        {
            Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform);
            inEnd = true;
        }
        controller.bannedLanes.Remove(lane);
        controller.inConstruction = false;
        controller.constructionTimer = Random.Range(450, 1375);
        base.makeDisabled(xF, yF);
    }

    public override void checkStuff()
    {
        if (isDestroyed)
        {
            amDestroyed();
        }
        else if (isDisabled)
        {
            amDisabled();
        }
    }

    public override void nearCrash()
    {

    }
}
