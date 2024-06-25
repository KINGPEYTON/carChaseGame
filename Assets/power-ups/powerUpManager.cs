using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpManager : MonoBehaviour
{
    public Transform pCar;
    public speedometer speedmer;

    public List<int> tiers;
    public List<Sprite> icons;

    public GameObject magneticField;
    public GameObject ram;
    public GameObject bigCoinhuna;
    public GameObject slowdown;

    void Awake()
    {
        GameObject obj = GameObject.Find("powerUpManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void collectPowerUp(int id)
    {
        pCar = GameObject.Find("playerCar").transform;
        speedmer = GameObject.Find("Speedometer").GetComponent<speedometer>();
        switch (id)
        {
            case 0:
                switch (tiers[id])
                {
                    case 0:
                        activateMagnet(15, 0.85f, false);
                        break;
                    case 1:
                        activateMagnet(15, 0.85f, true);
                        break;
                    case 2:
                        activateMagnet(25, 0.85f, true);
                        break;
                    case 3:
                        activateMagnet(25, 1.25f, true);
                        break;
                }
                break;
            case 1:
                switch (tiers[id])
                {
                    case 0:
                        activateRam(1, true, true);
                        break;
                    case 1:
                        activateRam(1, true, false);
                        break;
                    case 2:
                        activateRam(1, false, false);
                        break;
                    case 3:
                        activateRam(2, false, false);
                        break;
                }
                break;
            case 2:
                activateBoost(15, 2);
                break;
            case 3:
                switch (tiers[id])
                {
                    case 0:
                        activateCoin(10, 1, false);
                        break;
                    case 1:
                        activateCoin(10, 1, true);
                        break;
                    case 2:
                        activateCoin(10, 2, true);
                        break;
                    case 3:
                        activateCoin(15, 2, true);
                        break;
                }
                break;
            case 4:
                activateVision(15, 2);
                break;
            case 5:
                activateRandom();
                break;
            case 6:
                activateRocket(15);
                break;
            case 7:
                switch (tiers[id])
                {
                    case 0:
                        activateShield(10, true);
                        break;
                    case 1:
                        activateShield(15, true);
                        break;
                    case 2:
                        activateShield(15, false);
                        break;
                }
                break;
            case 8:
                activateTinyCars(15);
                break;
            case 9:
                switch (tiers[id])
                {
                    case 0:
                        activateSlowdown(20, 0.85f, true);
                        break;
                    case 1:
                        activateSlowdown(20, 0.65f, true);
                        break;
                    case 2:
                        activateSlowdown(20, 0.65f, false);
                        break;
                }
                break;
            case 10:
                switch (tiers[id])
                {
                    case 0:
                        activateTeleport(20, false, true);
                        break;
                    case 1:
                        activateTeleport(20, true, true);
                        break;
                    case 2:
                        activateTeleport(20, true, false);
                        break;
                }
                break;
        }
    }

    public void activateMagnet(float time, float strength, bool getHolo)
    {
        magnetField newMagnet = Instantiate(magneticField, pCar.position, Quaternion.identity, pCar).GetComponent<magnetField>();
        newMagnet.carPoint = pCar;
        newMagnet.lifetime = time;
        newMagnet.setSize(strength);
        newMagnet.getHolo = getHolo;
        speedmer.startPowerup(time, icons[0], true);
    }

    public void activateRam(int hits, bool justCars, bool headOn)
    {

        pCar.gameObject.GetComponent<playerCar>().startRam(hits, justCars, headOn);
        speedmer.startPowerup(hits, icons[1], false);
    }

    public void activateBoost(int uses, float power)
    {

    }

    public void activateCoin(int time, float spawnMultipliyer, bool startHolo)
    {
        coinhuna chuna = Instantiate(bigCoinhuna, GameObject.Find("contoller").transform).GetComponent<coinhuna>();
        chuna.setCoinhuna(time, spawnMultipliyer, startHolo);
        speedmer.startPowerup(time, icons[3], true);
    }

    public void activateVision(int time, int avoidness)
    {

    }

    public void activateRandom()
    {

    }

    public void activateShield(int time, bool autoStart)
    {
        pCar.gameObject.GetComponent<playerCar>().startShield(time, autoStart);
        speedmer.startPowerup(time, icons[6], autoStart);
    }

    public void activateRocket(int time)
    {

    }

    public void activateTinyCars(int time)
    {

    }

    public void activateSlowdown(int time, float spawns, bool affectScore)
    {
        incognito inco = Instantiate(slowdown, GameObject.Find("contoller").transform).GetComponent<incognito>();
        inco.setSlowdown(time, spawns, affectScore);
        speedmer.startPowerup(time, icons[9], true);
    }

    public void activateTeleport(int uses, bool destroyObjs, bool affectCharge)
    {
        pCar.gameObject.GetComponent<playerCar>().enterTeleport(uses, destroyObjs, affectCharge);
        speedmer.startPowerup(uses, icons[10], false);
    }
}
