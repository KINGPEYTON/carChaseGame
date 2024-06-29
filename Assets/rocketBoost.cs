using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketBoost : MonoBehaviour
{
    public playerCar pCar;
    public main controller;
    public speedometer speedo;

    public Transform player;
    public Transform camera;
    public Transform skyline;
    public Transform clouds;
    public Transform blimp;
    public Transform pPlane;

    public float targX;
    public float targY;
    public float targX2;
    public Vector3 startPos;
    public float startTimer;
    public bool inPos;

    public float power;
    public bool allHolo;

    public bool boosting;
    public float boostLeft;
    public bool inSky;
    public float boostTimer;
    public float boostTime;
    public float useTimer;

    public GameObject coin;
    public GameObject holoCoin;
    public float coinDist;
    public Transform coinLast;
    public float coinLane;
    public float coinLaneOdds;

    public bool destroyed;
    public Transform rocket2;
    public Animator ani;
    public Animator ani2;

    // Start is called before the first frame update
    void Start()
    {
        pCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();

        player = pCar.gameObject.transform;
        camera = GameObject.Find("Main Camera").transform;
        skyline = GameObject.Find("skyline").transform;
        clouds = GameObject.Find("clouds").transform;
        blimp = GameObject.Find("score Blimp").transform;
        pPlane = GameObject.Find("pause Plane").transform;

        rocket2 = transform.GetChild(0);
        ani = GetComponent<Animator>();
        ani2 = rocket2.gameObject.GetComponent<Animator>();

        coinLane = 5.65f + (Random.Range(0, 5) * 1.25f);
        GameObject coinToMake = coin;
        if (allHolo)
        {
            coinToMake = holoCoin;
        }
        coinLast = Instantiate(coinToMake, new Vector3(14, coinLane, 0), Quaternion.identity, GameObject.Find("coins").transform).transform;
        coinLaneOdds = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            Vector3 dis = new Vector3(targX - startPos.x, targY - startPos.y, 1);
            Vector3 dis2 = new Vector3(targX2, 0, 1);
            transform.localPosition = calcPos(dis, startPos, startTimer, 1);
            rocket2.localPosition = calcPos(dis2, new Vector3(0, 0, 0), startTimer, 1);
            startTimer += Time.deltaTime;
            if (startTimer > 1)
            {
                inPos = true;
                transform.localPosition = new Vector3(targX, targY, 1);
                rocket2.localPosition = new Vector3(targX2, 0, 1);
                pCar.inRocket = true;
                boosting = true;
                startPos = player.position;
                targY = pCar.targetPos.y + 10;
                setAni("rocket boost");
            }
        } else if (boosting)
        {
            if (inSky)
            {
                boostDown();
            }
            else
            {
                boostUp();
            }
            spawnCoins();
        }
        else if (inSky)
        {
            sky();
            spawnCoins();
        }
        else if (destroyed)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.mph, 0, 0); //moves guard across the screen
            if (transform.position.x <= -13) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
    }

    public void setTargetPos(float x, float y, float x2)
    {
        targX = x;
        targY = y;
        targX2 = x2;
    }

    public void startBoost(float boostPower, float boostingTime, float coinRate, bool coinsHolo)
    {
        power = boostPower;
        boostLeft = boostPower;
        boostTime = boostingTime;
        coinDist = coinRate;
        allHolo = coinsHolo;
        useTimer = boostPower - Mathf.Floor(boostPower);
    }

    public void setAni(string an)
    {
        ani.Play(an);
        ani2.Play(an);
    }

    void boostUp()
    {
        Vector3 disPlayer = new Vector3(0, targY - startPos.y, 0);
        Vector3 disCamera = new Vector3(0, 8, 0);
        Vector3 disSkyLine = new Vector3(0, 3, 0);
        Vector3 disClouds = new Vector3(0, 7, 0);
        Vector3 disBlimp = new Vector3(0, 2, 0);
        Vector3 disPlane = new Vector3(0, 1.5f, 0);
        player.position = calcPos(disPlayer, startPos, boostTimer, boostTime);
        camera.position = calcPos(disCamera, new Vector3(0, 0, -10), boostTimer, boostTime);
        skyline.localPosition = calcPos(disSkyLine, new Vector3(0, 0, 0), boostTimer, boostTime);
        clouds.localPosition = calcPos(disClouds, new Vector3(0, 0, 0), boostTimer, boostTime);
        blimp.position = calcPos(disBlimp, new Vector3(0, 0, 0), boostTimer, boostTime);
        pPlane.position = calcPos(disPlane, new Vector3(0, 0, 0), boostTimer, boostTime);
        boostTimer += Time.deltaTime;
        if(boostTimer > boostTime)
        {
            boosting = false;
            inSky = true;
            player.position = new Vector3(player.position.x, targY, -9);
            camera.position = new Vector3(camera.position.x, 8, -10);
            skyline.position = new Vector3(skyline.position.x, 2, 0);
            clouds.position = new Vector3(clouds.position.x, 6, 0);
            blimp.position = new Vector3(blimp.position.x, 2, 0);
            pPlane.position = new Vector3(blimp.position.x, 1.5f, 0);
            pCar.targetPos = player.position;
            setAni("rocket sky");
            pCar.inPos = true;
        }
    }

    void sky()
    {
        boostLeft -= Time.deltaTime;
        useTimer += Time.deltaTime;
        if(useTimer > 1 && boostLeft > 0.5f)
        {
            useTimer--;
            speedo.usePowerUp(1);
        }
        if(boostLeft < 0)
        {
            boosting = true;
            startPos = player.position;
            targY = pCar.targetPos.y - 10;
            setAni("rocket standby");
            boostTimer = 0;
        }
    }

    void boostDown()
    {
        Vector3 disPlayer = new Vector3(0, targY - startPos.y, 0);
        Vector3 disCamera = new Vector3(0, 8, 0);
        Vector3 disSkyLine = new Vector3(0, 3, 0);
        Vector3 disClouds = new Vector3(0, 7, 0);
        Vector3 disBlimp = new Vector3(0, 2, 0);
        Vector3 disPlane = new Vector3(0, 1.5f, 0);
        player.position = calcPos(disPlayer, startPos, boostTimer, boostTime);
        camera.position = disCamera - calcPos(disCamera, new Vector3(0, 0, 10), boostTimer, boostTime);
        skyline.localPosition = disSkyLine - calcPos(disSkyLine, new Vector3(0, 0, 0), boostTimer, boostTime);
        clouds.localPosition = disClouds - calcPos(disClouds, new Vector3(0, 0, 0), boostTimer, boostTime);
        blimp.position = disBlimp - calcPos(disBlimp, new Vector3(0, 0, 0), boostTimer, boostTime);
        pPlane.position = disPlane - calcPos(disPlane, new Vector3(0, 0, 0), boostTimer, boostTime);
        boostTimer += Time.deltaTime;
        if (boostTimer > boostTime)
        {
            boosting = false;
            inSky = false;
            player.position = new Vector3(player.position.x, targY, -9);
            camera.position = new Vector3(camera.position.x, 0, -10);
            skyline.position = new Vector3(skyline.position.x, -1, 0);
            clouds.position = new Vector3(clouds.position.x, -1, 0);
            blimp.position = new Vector3(blimp.position.x, 0, 0);
            pPlane.position = new Vector3(blimp.position.x, 0, 0);
            endBoost();
        }
    }

    void spawnCoins()
    {
        if(coinLast.position.x < 12)
        {
            makeCoin();
        }
    }

    void addCoinOdds()
    {
        float newCoinOdds = Random.Range(0.0f, 1.0f);
        if(newCoinOdds < coinLaneOdds)
        {
            if (coinLane == 5.65f)
            {
                coinLane += 1.25f;
            }
            else if (coinLane == 10.65f)
            {
                coinLane -= 1.25f;
            }
            else
            {
                coinLane += 1.25f * ((Random.Range(0, 2) * 2) - 1);
                coinLaneOdds = 0.1f;
            }
        }
        else
        {
            coinLaneOdds *= 1.25f;
        }
    }

    void makeCoin()
    {
        GameObject coinToMake = coin;
        if (allHolo)
        {
            coinToMake = holoCoin;
        }
        coinLast = Instantiate(coinToMake, new Vector3(coinLast.position.x + coinDist, coinLane, 0), Quaternion.identity, GameObject.Find("coins").transform).transform;
        addCoinOdds();
    }

    void endBoost()
    {
        pCar.rocketLanding = true;
        pCar.landingTimer = 0;
        speedo.finishPowerup();
        pCar.inPos = true;
        setAni("rocket sky");
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y);
        return new Vector3(xVal, yVal, 0) + startScale;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
