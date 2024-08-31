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
        controller.createSign(4);
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
                    cars c = controller.caManager.spawnRegularCar(new Vector3(12, transform.position.y, 0), GameObject.Find("cars").transform).GetComponent<cars>();
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
        GameObject newCone = Instantiate(cones, new Vector3(transform.position.x + 2, transform.position.y + 0.25f, 0), Quaternion.identity, GameObject.Find("cars").transform);
        conesLast = newCone.transform;
        coneList.Add(newCone);
        newCone.GetComponent<cone>().construc = this;
        inSight = true;
        for (int i = 0; i < 3; i++)
        {
            cars c = controller.caManager.spawnRegularCar(new Vector3(transform.position.x - 2.5f - (i * 3), transform.position.y, 0), GameObject.Find("cars").transform).GetComponent<cars>();
            c.makeTraffic(0);
            controller.checkCarEffects(c.gameObject);
        }
        cars ca = controller.caManager.spawnRegularCar(new Vector3(transform.position.x - 14.5f, transform.position.y, 0), GameObject.Find("cars").transform).GetComponent<cars>();
        ca.makeTraffic(7.5f);
        controller.checkCarEffects(ca.gameObject);
        for (int i = 0; i < 8; i++)
        {
            cars cr = controller.caManager.spawnRegularCar(new Vector3(transform.position.x - 11.5f + (i * 3.5f), transform.position.y + 1.25f, 0), GameObject.Find("cars").transform).GetComponent<cars>();
            cr.makeTraffic(0);
            controller.checkCarEffects(cr.gameObject);
        }
    }

    void spawnCones()
    {
        if (transform.position.x < -110)
        {
            GameObject newCone = Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform);
            inEnd = true;
            coneList.Add(newCone);
            newCone.GetComponent<cone>().construc = this;
        }
        if (!inEnd)
        {
            if (conesLast.transform.position.x < 12)
            {
                GameObject newCone = Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y + 0.5f, 0), Quaternion.identity, GameObject.Find("cars").transform);
                conesLast = newCone.transform;
                coneList.Add(newCone);
                newCone.GetComponent<cone>().construc = this;
            }
        }
    }

    public override void destroyCar()
    {
        if (!inEnd)
        {
            GameObject newCone = Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform);
            coneList.Add(newCone);
            newCone.GetComponent<cone>().construc = this;
        }
        controller.bannedLanes.Remove(lane);
        controller.inConstruction = false;
        controller.constructionTimer = Random.Range(450, 1375);
        base.destroyCar();
    }

    public override void makeDisabled(float xF, float yF)
    {
        if (!inEnd)
        {
            GameObject newCone = Instantiate(cones, new Vector3(conesLast.position.x + 3.5f, transform.position.y, 0), Quaternion.identity, GameObject.Find("cars").transform);
            inEnd = true;
            coneList.Add(newCone);
            newCone.GetComponent<cone>().construc = this;
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
