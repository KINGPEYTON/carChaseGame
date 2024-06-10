using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpManager : MonoBehaviour
{
    public Transform pCar;

    public List<int> tiers;

    public GameObject magneticField;
    public GameObject ram;

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
        switch (id)
        {
            case 0:
                activateMagnet(15, 0.85f);
                break;
            case 1:
                activateRam(1);
                break;
            case 2:
                activateBoost(15, 2);
                break;
            case 3:
                activateCoin(15);
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
                activateShield(15);
                break;
            case 8:
                activateTinyCars(15);
                break;
            case 9:
                activateSlowdown(15, 6);
                break;
            case 10:
                activateTeleport(15);
                break;
        }
    }

    public void activateMagnet(float time, float strength)
    {
        magnetField newMagnet = Instantiate(magneticField, pCar.position, Quaternion.identity, pCar).GetComponent<magnetField>();
        newMagnet.carPoint = pCar;
        newMagnet.lifetime = time;
        newMagnet.setSize(strength);
    }

    public void activateRam(int hits)
    {

    }

    public void activateBoost(int uses, float power)
    {

    }

    public void activateCoin(int time)
    {

    }

    public void activateVision(int time, int avoidness)
    {

    }

    public void activateRandom()
    {

    }

    public void activateShield(int time)
    {

    }

    public void activateRocket(int time)
    {

    }

    public void activateTinyCars(int time)
    {

    }

    public void activateSlowdown(int effect, int spawns)
    {

    }

    public void activateTeleport(int uses)
    {

    }
}
