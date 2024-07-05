using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomWheel : MonoBehaviour
{
    public List<Sprite> iconsToUse;
    public List<Sprite> iconsIn;

    public bool maxTier;
    public bool makeLonger;
    public bool makeStronger;

    public powerUpManager pwManager;
    public SpriteRenderer sr;
    public SpriteRenderer selectSr;

    public float iconTimer;
    public float iconTime;
    public bool iconSpining;
    public List<GameObject> icons;
    public GameObject iconOBJ;
    public int iconAmount;
    public int totalSpace;

    public bool doFadeIn;
    public bool doFadeOut;
    public float fadeVal;
    public float fadeTimer;
    public float fadeTime;

    // Update is called once per frame
    void Update()
    {
        if (iconSpining) { iconSpin(); }

        if (doFadeIn) { wheelFadeIn(); }
        if (doFadeOut) { wheelFadeOut(); }
    }

    public void startRandom(float spinTime, bool topTier, bool isLonger, bool isStronger)
    {
        maxTier = topTier;
        makeLonger = isLonger;
        makeStronger = isStronger;

        fadeTime = 1.5f;
        sr = GetComponent<SpriteRenderer>();

        iconTime = Mathf.Pow(spinTime, 4);
        iconAmount = (int)(iconTime / 2.5f);
        totalSpace = ((iconAmount * 10) - 40);

        setIconsToUse();
        setIcons();
        startFade(true);
        iconSpining = true;
    }

    public void pickPowerup(int id)
    {
        startFade(false);
        if (makeStronger)
        {
            getStrongerPowerUp(id);
        }
        else if(makeLonger)
        {
            getLongerPowerUp(id);
        }
        else if(maxTier)
        {
            getMaxPowerUp(id);
        }
        else
        {
            pwManager.collectPowerUp(id);
        }
    }

    void getMaxPowerUp(int id)
    {
        int currTier = pwManager.tiers[id];
        if (id < 6) {
            pwManager.tiers[id] = 3;
        }
        else
        {
            pwManager.tiers[id] = 2;
        }

        pwManager.collectPowerUp(id);
        pwManager.tiers[id] = currTier;
    }

    void getLongerPowerUp(int id)
    {
        switch (id)
        {
            case 0:
                pwManager.activateMagnet(35, 1.25f, true);
                break;
            case 1:
                pwManager.activateRam(3, false, false);
                break;
            case 2:
                pwManager.activateBoost(6, 2.35f, true);
                break;
            case 3:
                pwManager.activateCoin(22, 2, true);
                break;
            case 4:
                Debug.Log("aa");
                break;
            case 6:
                pwManager.activateShield(22, false, true);
                break;
            case 7:
                pwManager.activateRocket(10, 1.5f, 1.35f, true);
                break;
            case 8:
                pwManager.activateTinyCars(35, true);
                break;
            case 9:
                pwManager.activateSlowdown(30, 0.65f, false);
                break;
            case 10:
                pwManager.activateTeleport(30, 0.35f, new Color32(115, 0, 255, 255), true, false);
                break;
            case 11:
                pwManager.activateLaser(30, 0.35f, 0.75f, true);
                break;
        }
    }

    void getStrongerPowerUp(int id)
    {

        switch (id)
        {
            case 0:
                pwManager.activateMagnet(35, 2.05f, true);
                break;
            case 1:
                pwManager.activateRam(4, false, false);
                break;
            case 2:
                pwManager.activateBoost(6, 3.85f, true);
                break;
            case 3:
                pwManager.activateCoin(22, 3, true);
                break;
            case 4:
                Debug.Log("aa");
                break;
            case 6:
                pwManager.activateShield(30, false, true);
                break;
            case 7:
                pwManager.activateRocket(10, 0.95f, 0.65f, true);
                break;
            case 8:
                pwManager.activateTinyCars(50, true);
                break;
            case 9:
                pwManager.activateSlowdown(30, 0.45f, false);
                break;
            case 10:
                pwManager.activateTeleport(40, 0.35f, new Color32(115, 0, 255, 255), true, false);
                break;
            case 11:
                pwManager.activateLaser(35, 0.15f, 0.45f, true);
                break;
        }
    }

    void setIcons()
    {
        List<int> iconSet = new List<int>();
        while (iconsIn.Count < iconAmount)
        {
            getIconSet(iconSet);
            addIcons(iconSet);
        }

        for(int i = 0; i < iconsIn.Count; i++)
        {
            icons.Add(Instantiate(iconOBJ));
            icons[i].GetComponent<SpriteRenderer>().sprite = iconsIn[i];
            setIconAlpha(icons[i], 0);
            icons[i].transform.parent = transform;
            icons[i].transform.localPosition = new Vector3(35 - (i * 10), 0, 0);
        }

        iconTimer = iconTime;
    }

    void addIcons(List<int> iconSet)
    {
        for (int i = 0; iconSet.Count > 3; i++)
        {
            if (iconsIn.Count < iconAmount)
            {
                iconsIn.Add(iconsToUse[iconSet[0]]);
            }
            iconSet.RemoveAt(0);
        }
    }

    void getIconSet(List<int> iconSet)
    {
        List<int> newSet = new List<int>();
        for (int i = 0; i < iconsToUse.Count; i++)
        {
            if (!iconSet.Contains(i))
            {
                newSet.Add(i);
            }
        }
        ShuffleIcons(newSet);
        for (int i = 0; i < newSet.Count; i++)
        {
            iconSet.Add(newSet[i]);
        }
    }

    void iconSpin()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].transform.localPosition = new Vector3(totalSpace - (10 * i) - getValueScale(iconTimer, 0, iconTime, totalSpace - 35), 0, 0);
            setIconAlpha(icons[i], 255);
        }

        if (iconTimer > 2) { iconTimer -= Time.deltaTime * iconTimer; }
        else { iconTimer -= Time.deltaTime * 2; }

        if(iconTimer < 0)
        {
            iconSpining = false;
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].transform.localPosition = new Vector3(totalSpace - (10 * i), 0, 0);
                setIconAlpha(icons[i], 255);
            }
            int iconPlace = iconAmount - 4;
            int iconId = iconsToUse.IndexOf(iconsIn[iconPlace]);
            if(iconId > 4) { iconId++; } // to accomedate the gap of ids without the random powerup
            icons[iconPlace].GetComponent<randomIcon>().id = iconId;
            icons[iconPlace].GetComponent<randomIcon>().select(this);
        }
    }

    void setIconsToUse()
    {
        if (maxTier)
        {
            iconsToUse.Add(pwManager.magnetIcons[3]);
            iconsToUse.Add(pwManager.ramIcons[3]);
            iconsToUse.Add(pwManager.boostIcons[3]);
            iconsToUse.Add(pwManager.coinhunaIcons[3]);
            iconsToUse.Add(pwManager.visionIcons[3]);
            iconsToUse.Add(pwManager.shieldIcons[2]);
            iconsToUse.Add(pwManager.rocketIcons[2]);
            iconsToUse.Add(pwManager.tinyIcons[2]);
            iconsToUse.Add(pwManager.slowdownIcons[2]);
            iconsToUse.Add(pwManager.teleportIcons[2]);
            iconsToUse.Add(pwManager.laserIcons[2]);
        }
        else
        {

            iconsToUse.Add(pwManager.magnetIcons[pwManager.tiers[0]]);
            iconsToUse.Add(pwManager.ramIcons[pwManager.tiers[1]]);
            iconsToUse.Add(pwManager.boostIcons[pwManager.tiers[2]]);
            iconsToUse.Add(pwManager.coinhunaIcons[pwManager.tiers[3]]);
            iconsToUse.Add(pwManager.visionIcons[pwManager.tiers[4]]);
            iconsToUse.Add(pwManager.shieldIcons[pwManager.tiers[6]]);
            iconsToUse.Add(pwManager.rocketIcons[pwManager.tiers[7]]);
            iconsToUse.Add(pwManager.tinyIcons[pwManager.tiers[8]]);
            iconsToUse.Add(pwManager.slowdownIcons[pwManager.tiers[9]]);
            iconsToUse.Add(pwManager.teleportIcons[pwManager.tiers[10]]);
            iconsToUse.Add(pwManager.laserIcons[pwManager.tiers[11]]);
        }
    }

    void ShuffleIcons<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int idx = Random.Range(0, i);
            var temp = list[i];
            list[i] = list[idx];
            list[idx] = temp;
        }
    }

    void setIconAlpha(GameObject icon, float alpha)
    {
        float newAlpha = 255;
        if (icon.transform.localPosition.x > 25)
        {
            if (icon.transform.localPosition.x > 45) { newAlpha = 0; }
            else
            {
                newAlpha = 255 - getValueScale(icon.transform.localPosition.x, 25, 45, 255);
            }
        }
        else if (icon.transform.localPosition.x < -25)
        {
            if (icon.transform.localPosition.x < -45) { newAlpha = 0; }
            else
            {
                newAlpha = 255 - getValueScale(icon.transform.localPosition.x, -25, -45, 255);
            }
        }
        newAlpha = getValueScale(newAlpha * alpha, 0, 65025, 255);
        icon.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte) newAlpha);
    }

    void wheelFadeIn()
    {
        fadeVal = (getValueScale(fadeTimer, 0, fadeTime, 255));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(255);
            doFadeIn = false;
        }
    }

    void wheelFadeOut()
    {
        fadeVal = (255 - getValueScale(fadeTimer, 0, fadeTime, 255));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(0);
            Destroy(gameObject);
        }
    }

    public void startFade(bool fIn)
    {
        fadeTimer = 0;
        if (fIn)
        {
            doFadeIn = true;
        }
        else
        {
            doFadeOut = true;
        }
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
    void fadeAni(float value)
    {
        sr.color = new Color32(255, 255, 255, (byte)value);
        selectSr.color = new Color32(255, 255, 255, (byte)value);
        for (int i = 0; i < icons.Count; i++)
        {
            setIconAlpha(icons[i], (byte)value);
        }
    }
}
