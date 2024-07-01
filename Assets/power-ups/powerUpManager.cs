using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpManager : MonoBehaviour
{
    public List<int> tiers;
    public List<List<Sprite>> icons = new List<List<Sprite>>();

    public List<Sprite> magnetIcons;
    public List<Sprite> ramIcons;
    public List<Sprite> boostIcons;
    public List<Sprite> coinhunaIcons;
    public List<Sprite> visionIcons;
    public List<Sprite> randomIcons;
    public List<Sprite> shieldIcons;
    public List<Sprite> rocketIcons;
    public List<Sprite> tinyIcons;
    public List<Sprite> slowdownIcons;
    public List<Sprite> teleportIcons;
    public List<Sprite> laserIcons;

    public GameObject magneticField;
    public GameObject bigCoinhuna;
    public GameObject tinyCar;
    public GameObject slowdown;
    public Transform pCar;
    public speedometer speedmer;

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
        
        icons.Add(magnetIcons);
        icons.Add(ramIcons);
        icons.Add(boostIcons);
        icons.Add(coinhunaIcons);
        icons.Add(visionIcons);
        icons.Add(randomIcons);
        icons.Add(shieldIcons);
        icons.Add(rocketIcons);
        icons.Add(tinyIcons);
        icons.Add(slowdownIcons);
        icons.Add(teleportIcons);
        icons.Add(laserIcons);
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
                switch (tiers[id])
                {
                    case 0:
                        activateBoost(3, 1.75f, false);
                        break;
                    case 1:
                        activateBoost(3, 2.35f, false);
                        break;
                    case 2:
                        activateBoost(4, 2.35f, false);
                        break;
                    case 3:
                        activateBoost(4, 2.35f, true);
                        break;
                }
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
                activateVision(5, 2);
                break;
            case 5:
                activateRandom();
                break;
            case 6:
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
            case 7:
                switch (tiers[id])
                {
                    case 0:
                        activateRocket(4.25f, 1.5f, 1.35f, false);
                        break;
                    case 1:
                        activateRocket(6.5f, 1.5f, 1.35f, false);
                        break;
                    case 2:
                        activateRocket(6.5f, 1.5f, 1.35f, true);
                        break;
                }
                break;
            case 8:
                switch (tiers[id])
                {
                    case 0:
                        activateTinyCars(15, false);
                        break;
                    case 1:
                        activateTinyCars(15, true);
                        break;
                    case 2:
                        activateTinyCars(25, true);
                        break;
                }
                break;
            case 9:
                switch (tiers[id])
                {
                    case 0:
                        activateSlowdown(20, 0.95f, true);
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
                        activateTeleport(20, 0.35f, new Color32(115, 255, 255, 255), false, true);
                        break;
                    case 1:
                        activateTeleport(20, 0.35f, new Color32(115, 115, 255, 255), true, true);
                        break;
                    case 2:
                        activateTeleport(20, 0.35f, new Color32(115, 0, 255, 255), true, false);
                        break;
                }
                break;
            case 11:
                switch (tiers[id])
                {
                    case 0:
                        activateLaser(10, 0.65f, 1.5f, false);
                        break;
                    case 1:
                        activateLaser(10, 0.35f, 0.75f, false);
                        break;
                    case 2:
                        activateLaser(20, 0.35f, 0.75f, true);
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
        speedmer.startPowerup(time, icons[0][tiers[0]], true);
    }

    public void activateRam(int hits, bool justCars, bool headOn)
    {

        pCar.gameObject.GetComponent<playerCar>().startRam(hits, justCars, headOn);
        speedmer.startPowerup(hits, icons[1][tiers[1]], false);
    }

    public void activateBoost(int uses, float power, bool hitProt)
    {
        pCar.gameObject.GetComponent<playerCar>().startBoost(uses, power, hitProt);
        speedmer.startPowerup(uses, icons[2][tiers[2]], false);
    }

    public void activateCoin(int time, float spawnMultipliyer, bool startHolo)
    {
        coinhuna chuna = Instantiate(bigCoinhuna, GameObject.Find("contoller").transform).GetComponent<coinhuna>();
        chuna.setCoinhuna(time, spawnMultipliyer, startHolo);
        speedmer.startPowerup(time, icons[3][tiers[3]], true);
    }

    public void activateVision(int time, int avoidness)
    {
        Debug.Log("Not yet added");
    }

    public void activateRandom()
    {

    }

    public void activateShield(int time, bool autoStart)
    {
        pCar.gameObject.GetComponent<playerCar>().startShield(time, autoStart);
        speedmer.startPowerup(time, icons[6][tiers[6]], autoStart);
    }

    public void activateRocket(float power, float boostTime, float coinDist, bool allHolo)
    {
        pCar.gameObject.GetComponent<playerCar>().startRocket(power, boostTime, coinDist, allHolo);
        speedmer.startPowerup(power, icons[7][tiers[7]], false);
    }

    public void activateTinyCars(int time, bool allCars)
    {
        tinyCars tCars = Instantiate(tinyCar, GameObject.Find("contoller").transform).GetComponent<tinyCars>();
        tCars.setTinyCars(time, allCars);
        speedmer.startPowerup(time, icons[8][tiers[8]], true);
    }

    public void activateSlowdown(int time, float spawns, bool affectScore)
    {
        incognito inco = Instantiate(slowdown, GameObject.Find("contoller").transform).GetComponent<incognito>();
        inco.setSlowdown(time, spawns, affectScore);
        speedmer.startPowerup(time, icons[9][tiers[9]], true);
    }

    public void activateTeleport(int uses, float boltTimer, Color32 boltColor, bool destroyObjs, bool affectCharge)
    {
        pCar.gameObject.GetComponent<playerCar>().enterTeleport(uses, boltTimer, boltColor, destroyObjs, affectCharge);
        speedmer.startPowerup(uses, icons[10][tiers[10]], false);
    }

    public void activateLaser(int shots, float fireRate, float cooldown, bool autoShoot)
    {
        pCar.gameObject.GetComponent<playerCar>().startLaser(shots, fireRate, cooldown, autoShoot);
        speedmer.startPowerup(shots, icons[11][tiers[11]], false);
    }
}
