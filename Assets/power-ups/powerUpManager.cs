using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class powerUpManager : MonoBehaviour
{
    public TextAsset powerupJSON;
    public pwUnlocks pwUnl = new pwUnlocks();

    public List<bool> unlocks;
    public List<int> tiers;
    public Dictionary<string, List<Sprite>> icons = new Dictionary<string, List<Sprite>>();
    public Dictionary<string, List<Sprite>> bubbles = new Dictionary<string, List<Sprite>>();

    public GameObject magneticField;
    public GameObject bigCoinhuna;
    public GameObject enhancedSense;
    public GameObject tinyCar;
    public GameObject slowdown;
    public GameObject randomBar;
    public Transform pCar;
    public speedometer speedmer;

    public RuntimeAnimatorController bubbleAni;
    public GameObject bubbleIconOBJ;

    public List<string> powerupNames;
    public List<int> unlockCosts;
    public List<string> tutorialDescription;
    public List<bool> tutorialDone;
    public List<float> powerupOdds;
    public List<float> powerupCurrOdds;

    public List<string> powerupIDs;
    public List<List<string>> tierNames = new List<List<string>>();
    public List<List<string>> tierDescription = new List<List<string>>();
    public List<List<int>> tierCosts = new List<List<int>>();
    public List<int> powerupTypes;
    public List<float> powerupRawOdds;

    public AudioClip popSound;

    public powerupReader pwReader;

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
            if (JsonDataService.fileExist<pwUnlocks>("/pwUnlock.json", pwUnl))
            {
                loadUnlocks();
                pwReader = readPowerupJSON();
            }
            else
            {
                pwReader = readPowerupJSON();
                saveUnlocks();
            }

            getIcons();
            getPowerupOdds();
        }

        createPowerup(2, new Vector3(9, -1.65f, 0));
    }

    public void collectPowerUp(string id, int level)
    {
        pCar = GameObject.Find("playerCar").transform;
        speedmer = GameObject.Find("Speedometer").GetComponent<speedometer>();
        switch (id)
        {
            case "magnet":
                switch (level)
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
            case "ram":
                switch (level)
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
            case "boost":
                switch (level)
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
            case "coin":
                switch (level)
                {
                    case 0:
                        activateCoin(20, 1.75f, false);
                        break;
                    case 1:
                        activateCoin(20, 1.75f, true);
                        break;
                    case 2:
                        activateCoin(20, 2.5f, true);
                        break;
                    case 3:
                        activateCoin(40, 2.5f, true);
                        break;
                }
                break;
            case "sense":
                switch (level)
                {
                    case 0:
                        activateVision(20, false, 1, 1f);
                        break;
                    case 1:
                        activateVision(20, true, 1, 1f);
                        break;
                    case 2:
                        activateVision(20, true, 2, 1f);
                        break;
                    case 3:
                        activateVision(20, true, 2, 0.5f);
                        break;
                }
                break;
            case "random":
                switch (level)
                {
                    case 0:
                        activateRandom(5.6f, false, false, false);
                        break;
                    case 1:
                        activateRandom(5.6f, true, false, false);
                        break;
                    case 2:
                        activateRandom(5.6f, true, true, false);
                        break;
                    case 3:
                        activateRandom(5.6f, true, true, true);
                        break;
                }
                break;
            case "shield":
                switch (level)
                {
                    case 0:
                        activateShield(10, true, false);
                        break;
                    case 1:
                        activateShield(15, true, false);
                        break;
                    case 2:
                        activateShield(15, false, true);
                        break;
                }
                break;
            case "rocket":
                switch (level)
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
            case "tiny":
                switch (level)
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
            case "slowdown":
                switch (level)
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
            case "teleport":
                switch (level)
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
            case "laser":
                switch (level)
                {
                    case 0:
                        activateLaser(10, 0.55f, 1.5f, false);
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
        speedmer.startPowerup(time, icons["magnet"][pwUnl.tiers["magnet"]], true);
    }

    public void activateRam(int hits, bool justCars, bool headOn)
    {

        pCar.gameObject.GetComponent<playerCar>().startRam(hits, justCars, headOn);
        speedmer.startPowerup(hits, icons["ram"][pwUnl.tiers["ram"]], false);
    }

    public void activateBoost(int uses, float power, bool hitProt)
    {
        pCar.gameObject.GetComponent<playerCar>().startBoost(uses, power, hitProt);
        speedmer.startPowerup(uses, icons["boost"][pwUnl.tiers["boost"]], false);
    }

    public void activateCoin(int time, float spawnMultipliyer, bool startHolo)
    {
        coinhuna chuna = Instantiate(bigCoinhuna, GameObject.Find("contoller").transform).GetComponent<coinhuna>();
        chuna.setCoinhuna(time, spawnMultipliyer, startHolo);
        speedmer.startPowerup(time, icons["coin"][pwUnl.tiers["coin"]], true);
    }

    public void activateVision(int time, bool showIcons, float turnMultiplyer, float hitBoxSize)
    {
        sense enhance = Instantiate(enhancedSense, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<sense>();
        enhance.startSense(time, showIcons, turnMultiplyer, hitBoxSize);
        speedmer.startPowerup(time, icons["sense"][pwUnl.tiers["sense"]], true);
    }

    public void activateRandom(float spinTime, bool maxTier, bool isLonger, bool isStronger)
    {
        randomWheel rWheel = Instantiate(randomBar).GetComponent<randomWheel>();
        rWheel.pwManager = this;
        rWheel.startRandom(spinTime - 1, maxTier, isLonger, isStronger);
        speedmer.startPowerup(spinTime, icons["random"][pwUnl.tiers["random"]], true);
    }

    public void activateShield(int time, bool autoStart, bool startWhenHit)
    {
        pCar.gameObject.GetComponent<playerCar>().startShield(time, autoStart, startWhenHit);
        speedmer.startPowerup(time, icons["shield"][pwUnl.tiers["shield"]], autoStart);
    }

    public void activateRocket(float power, float boostTime, float coinDist, bool allHolo)
    {
        pCar.gameObject.GetComponent<playerCar>().startRocket(power, boostTime, coinDist, allHolo);
        speedmer.startPowerup(power, icons["rocket"][pwUnl.tiers["rocket"]], false);
    }

    public void activateTinyCars(int time, bool allCars)
    {
        tinyCars tCars = Instantiate(tinyCar, GameObject.Find("contoller").transform).GetComponent<tinyCars>();
        tCars.setTinyCars(time, allCars);
        speedmer.startPowerup(time, icons["tiny"][pwUnl.tiers["tiny"]], true);
    }

    public void activateSlowdown(int time, float spawns, bool affectScore)
    {
        incognito inco = Instantiate(slowdown, GameObject.Find("contoller").transform).GetComponent<incognito>();
        inco.setSlowdown(time, spawns, affectScore);
        speedmer.startPowerup(time, icons["slowdown"][pwUnl.tiers["slowdown"]], true);
    }

    public void activateTeleport(int uses, float boltTimer, Color32 boltColor, bool destroyObjs, bool affectCharge)
    {
        pCar.gameObject.GetComponent<playerCar>().enterTeleport(uses, boltTimer, boltColor, destroyObjs, affectCharge);
        speedmer.startPowerup(uses, icons["teleport"][pwUnl.tiers["teleport"]], false);
    }

    public void activateLaser(int shots, float fireRate, float cooldown, bool autoShoot)
    {
        pCar.gameObject.GetComponent<playerCar>().startLaser(shots, fireRate, cooldown, autoShoot);
        speedmer.startPowerup(shots, icons["laser"][pwUnl.tiers["laser"]], false);
    }

    public int getPowerupTier(string id)
    {
        return pwUnl.getTier(id);
    }

    public void createPowerup(int ind, Vector3 pos)
    {
        int t = tiers[ind];
        createBubble(powerupIDs[ind], powerupTypes[ind], icons[powerupIDs[ind]][t], bubbles[powerupIDs[ind]][t], t, pos);
    }

    public void createBubble(string id, int tpId, Sprite newIcon, Sprite bubble, int tier, Vector3 pos)
    {
        GameObject newPowerup = new GameObject(id + " powerup", typeof(SpriteRenderer), typeof(powerUp), typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D));
        newPowerup.GetComponent<powerUp>().createPowerUp(id, tpId, newIcon, bubble, tier, this, bubbleIconOBJ);
        newPowerup.GetComponent<powerUp>().popSound = popSound;
        newPowerup.GetComponent<Animator>().runtimeAnimatorController = bubbleAni;
        newPowerup.GetComponent<SpriteRenderer>().sortingOrder = 18;
        newPowerup.GetComponent<Rigidbody2D>().gravityScale = 0;
        newPowerup.GetComponent<BoxCollider2D>().size = new Vector2(6, 6);
        newPowerup.GetComponent<BoxCollider2D>().isTrigger = true;
        newPowerup.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        newPowerup.transform.localPosition = pos;
        newPowerup.tag = "power-up";
    }

    private powerupReader readPowerupJSON()
    {
        powerupReader powerupDataInJson = JsonUtility.FromJson<powerupReader>(powerupJSON.text);

        foreach (standardPowerup pw in powerupDataInJson.standard)
        {
            powerupNames.Add(pw.gameName);
            powerupIDs.Add(pw.idName);
            tiers.Add(pwUnl.getTier(pw.idName));
            bool isUnlocked = pwUnl.getUnlock(pw.idName, true);
            unlocks.Add(isUnlocked);
            unlockCosts.Add(0);
            powerupRawOdds.Add(pw.odds);
            powerupTypes.Add(pw.typeID);

            bool isTaught = pwUnl.getTutorial(pw.idName);
            tutorialDone.Add(isTaught);
            tutorialDescription.Add(pw.tutorialText);

            List<string> newNameList = new List<string>();
            List<string> newDescriptionList = new List<string>();
            List<int> newCostList = new List<int>();

            newCostList.Add(0);
            newCostList.Add(pw.tierOneCost);
            newCostList.Add(pw.tierTwoCost);
            newCostList.Add(pw.tierThreeCost);
            tierCosts.Add(newCostList);

            newNameList.Add(pw.gameName);
            newNameList.Add(pw.tierOneName);
            newNameList.Add(pw.tierTwoName);
            newNameList.Add(pw.tierThreeName);
            tierNames.Add(newNameList);

            newDescriptionList.Add(pw.description);
            newDescriptionList.Add(pw.tierOneDescription);
            newDescriptionList.Add(pw.tierTwoDescription);
            newDescriptionList.Add(pw.tierThreeDescription);
            tierDescription.Add(newDescriptionList);
        }
        
        foreach (premiumPowerup pw in powerupDataInJson.premium)
        {
            powerupNames.Add(pw.gameName);
            powerupIDs.Add(pw.idName);
            tiers.Add(pwUnl.getTier(pw.idName));
            bool isUnlocked = pwUnl.getUnlock(pw.idName, false);
            unlocks.Add(isUnlocked);
            unlockCosts.Add(pw.unlockCost);
            powerupRawOdds.Add(pw.odds);
            powerupTypes.Add(pw.typeID + 2);

            bool isTaught = pwUnl.getTutorial(pw.idName);
            tutorialDone.Add(isTaught);
            tutorialDescription.Add(pw.tutorialText);

            List<string> newNameList = new List<string>();
            List<string> newDescriptionList = new List<string>();
            List<int> newCostList = new List<int>();

            newCostList.Add(pw.unlockCost);
            newCostList.Add(pw.tierOneCost);
            newCostList.Add(pw.tierTwoCost);
            tierCosts.Add(newCostList);

            newNameList.Add(pw.gameName);
            newNameList.Add(pw.tierOneName);
            newNameList.Add(pw.tierTwoName);
            tierNames.Add(newNameList);

            newDescriptionList.Add(pw.description);
            newDescriptionList.Add(pw.tierOneDescription);
            newDescriptionList.Add(pw.tierTwoDescription);
            tierDescription.Add(newDescriptionList);
        }

        return powerupDataInJson;
    }

    private void getIcons()
    {
        for (int i = 0; i < powerupIDs.Count; i++)
        {
            List<Sprite> newIcons = new List<Sprite>();
            List<Sprite> newBubble = new List<Sprite>();
            int maxTier = 2;
            if (i < pwReader.standard.Length) { maxTier = 3; }
            getIconAssets(newIcons, newBubble, maxTier, powerupIDs[i]);
            icons.Add(powerupIDs[i], newIcons);
            bubbles.Add(powerupIDs[i], newBubble);
        }
    }

    private void getIconAssets(List<Sprite> iconParts, List<Sprite> bubbleParts, int ts, string iconName)
    {
        for (int i = 0; i <= ts; i++)
        {
            Texture2D itemTexture = Resources.Load<Texture2D>("powerup/" + iconName + "/" + iconName + " T" + i);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            Texture2D bubbleTexture = Resources.Load<Texture2D>("powerup/" + iconName + "/" + iconName + " B" + i);
            Sprite bubble = Sprite.Create(bubbleTexture, new Rect(0, 0, bubbleTexture.width, bubbleTexture.height), new Vector2(0.5f, 0.5f), 100);
            iconParts.Add(item);
            bubbleParts.Add(bubble);
        }
    }

    void getPowerupOdds()
    {
        powerupOdds.Clear();
        powerupCurrOdds.Clear();
        float oddsTotal = 0;
        float oddsNum = 0;
        for (int i = 0; i < powerupRawOdds.Count; i++)
        {
            if (unlocks[i])
            {
                oddsTotal += powerupRawOdds[i];
                oddsNum++;
            }
        }
        float multi = oddsNum / oddsTotal;
        float oddsMulti = multi / oddsNum;

        for (int i = 0; i < powerupRawOdds.Count; i++)
        {
            if (unlocks[i])
            {
                powerupOdds.Add(powerupRawOdds[i] * oddsMulti);
                powerupCurrOdds.Add(PlayerPrefs.GetFloat(powerupIDs[i] + "CurrOdds", powerupRawOdds[i] * oddsMulti));
            }
            else
            {
                powerupOdds.Add(0);
                powerupCurrOdds.Add(0);
            }
        }
    }

    public void setPowerupOdds()
    {
        powerupOdds.Clear();
        powerupCurrOdds.Clear();
        float oddsTotal = 0;
        float oddsNum = 0;
        for (int i = 0; i < powerupRawOdds.Count; i++)
        {
            if (unlocks[i])
            {
                oddsTotal += powerupRawOdds[i];
                oddsNum++;
            }
        }
        float multi = oddsNum / oddsTotal;
        float oddsMulti = multi / oddsNum;

        for (int i = 0; i < powerupRawOdds.Count; i++)
        {
            if (unlocks[i])
            {
                powerupOdds.Add(powerupRawOdds[i] * oddsMulti);
                powerupCurrOdds.Add(powerupRawOdds[i] * oddsMulti);
                PlayerPrefs.SetFloat(powerupIDs[i] + "CurrOdds", powerupRawOdds[i] * oddsMulti);
            }
            else
            {
                powerupOdds.Add(0);
                powerupCurrOdds.Add(0);
            }
        }
    }

    public void changePowerupOdds(int index)
    {
        int listCount = 0;
        foreach (bool u in unlocks) { if (u) { listCount++; } }
        float diff = powerupCurrOdds[index] - (powerupOdds[index] / 2); //the odds that are being taken away from use
        float listDiff = (diff * powerupOdds[index]) / (listCount - 1.0f); //the gap of what would of been added when changing odds
        for (int i = 0; i < powerupOdds.Count; i++)
        {
            if (unlocks[i])
            {
                powerupCurrOdds[i] += (diff * powerupOdds[i]) + listDiff; //this took me way to fucking long to figue out
            }
        }

        powerupCurrOdds[index] = powerupOdds[index] / 2;

        for (int i = 0; i < powerupCurrOdds.Count; i++)
        {

            PlayerPrefs.SetFloat(powerupIDs[i] + "CurrOdds", powerupCurrOdds[i]);
        }
    }

    public void unlockPowerUp(string id)
    {
        unlocks[powerupIDs.IndexOf(id)] = true;
        pwUnl.unlocks[id] = true;
        saveUnlocks();
    }

    public void upgradePowerUp(string id)
    {
        int ind = powerupIDs.IndexOf(id);
        tiers[ind]++;
        pwUnl.tiers[id]++;
        saveUnlocks();
    }

    public void finishTutorial(string id)
    {
        tutorialDone[powerupIDs.IndexOf(id)] = true;
        pwUnl.tutorials[id] = true;
        saveUnlocks();
    }

    public void resetTutorial()
    {
        foreach (string s in powerupIDs)
        {
            tutorialDone[powerupIDs.IndexOf(s)] = false;
            pwUnl.tutorials[s] = false;
        }
        saveUnlocks();
    }

    private void loadUnlocks()
    {
        pwUnl = JsonDataService.LoadData<pwUnlocks>("/pwUnlock.json", true);
    }
    private void saveUnlocks()
    {
        JsonDataService.SaveData("/pwUnlock.json", pwUnl, true);
    }
}

[System.Serializable]
public class powerupReader
{
    public standardPowerup[] standard;
    public premiumPowerup[] premium;
}

[System.Serializable]
public class powerup
{
    public string idName;
    public string gameName;
    public string description;
    public string tutorialText;
    public int typeID;
    public float odds;
    public int tierOneCost;
    public string tierOneName;
    public string tierOneDescription;
    public int tierTwoCost;
    public string tierTwoName;
    public string tierTwoDescription;
}

[System.Serializable]
public class standardPowerup : powerup
{
    public int tierThreeCost;
    public string tierThreeName;
    public string tierThreeDescription;
}

[System.Serializable]
public class premiumPowerup : powerup
{
    public int unlockCost;
}

[System.Serializable]
public class pwUnlocks
{
    public Dictionary<string, bool> unlocks = new Dictionary<string, bool>();
    public Dictionary<string, int> tiers = new Dictionary<string, int>();
    public Dictionary<string, bool> tutorials = new Dictionary<string, bool>();

    public bool getUnlock(string id, bool unlDefault)
    {
        try
        {
            bool a = unlocks[id];
            return a;
        }
        catch
        {
            unlocks.Add(id, unlDefault);
            return unlDefault;
        }
    }

    public int getTier(string id)
    {
        try
        {
            int a = tiers[id];
            return a;
        }
        catch
        {
            tiers.Add(id, 0);
            return 0;
        }
    }

    public bool getTutorial(string id)
    {
        try
        {
            bool a = tutorials[id];
            return a;
        }
        catch
        {
            tutorials.Add(id, false);
            return false;
        }
    }
}