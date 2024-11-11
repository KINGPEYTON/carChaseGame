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

    public AudioClip powerupSound;
    public AudioClip wheelSpinSound;

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

        main controller = GameObject.Find("contoller").GetComponent<main>();
        AudioSource.PlayClipAtPoint(wheelSpinSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    public void pickPowerup(string id)
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
            pwManager.collectPowerUp(id, pwManager.tiers[pwManager.getPowerupTier(id)]);
        }
    }

    void getMaxPowerUp(string id)
    {
        int MaxTier = 2;
        if (pwManager.powerupIDs.IndexOf(id) < pwManager.pwReader.standard.Length) { MaxTier = 3; }
        pwManager.collectPowerUp(id, MaxTier);
    }

    void getLongerPowerUp(string id)
    {
        switch (id)
        {
            case "magnet":
                pwManager.activateMagnet(35, 1.25f, true);
                break;
            case "ram":
                pwManager.activateRam(3, false, false);
                break;
            case "boost":
                pwManager.activateBoost(6, 2.35f, true);
                break;
            case "coin":
                pwManager.activateCoin(55, 2.5f, true);
                break;
            case "sense":
                pwManager.activateVision(30, true, 2, 0.5f);
                break;
            case "shield":
                pwManager.activateShield(22, false, true);
                break;
            case "rocket":
                pwManager.activateRocket(10, 1.5f, 1.35f, true);
                break;
            case "tiny":
                pwManager.activateTinyCars(35, true);
                break;
            case "slowdown":
                pwManager.activateSlowdown(30, 0.65f, false);
                break;
            case "teleport":
                pwManager.activateTeleport(30, 0.35f, new Color32(115, 0, 255, 255), true, false);
                break;
            case "laser":
                pwManager.activateLaser(30, 0.35f, 0.75f, true);
                break;
        }
    }

    void getStrongerPowerUp(string id)
    {

        switch (id)
        {
            case "magnet":
                pwManager.activateMagnet(35, 2.05f, true);
                break;
            case "ram":
                pwManager.activateRam(4, false, false);
                break;
            case "boost":
                pwManager.activateBoost(6, 3.85f, true);
                break;
            case "coin":
                pwManager.activateCoin(55, 3.75f, true);
                break;
            case "sense":
                pwManager.activateVision(30, true, 3, 0.3f);
                break;
            case "shield":
                pwManager.activateShield(30, false, true);
                break;
            case "rocket":
                pwManager.activateRocket(10, 0.95f, 0.65f, true);
                break;
            case "tiny":
                pwManager.activateTinyCars(50, true);
                break;
            case "slowdown":
                pwManager.activateSlowdown(30, 0.45f, false);
                break;
            case "teleport":
                pwManager.activateTeleport(40, 0.35f, new Color32(115, 0, 255, 255), true, false);
                break;
            case "laser":
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
            int newID = iconsToUse.IndexOf(iconsIn[iconPlace]);
            if (newID > pwManager.powerupIDs.IndexOf("random")) { newID++; } // to accomedate the gap of ids without the random powerup
            string iconId = pwManager.powerupIDs[newID];
            icons[iconPlace].AddComponent(typeof(randomIcon));
            icons[iconPlace].GetComponent<randomIcon>().id = iconId;
            icons[iconPlace].GetComponent<randomIcon>().select(this);
            main controller = GameObject.Find("contoller").GetComponent<main>();
            AudioSource.PlayClipAtPoint(powerupSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        }
    }

    void setIconsToUse()
    {
        if (maxTier)
        {
            int stdLen = pwManager.pwReader.standard.Length;
            for (int i = 0; i < stdLen - 1; i++)
            {
                iconsToUse.Add(pwManager.icons[pwManager.powerupIDs[i]][3]);
            }
            for(int i = stdLen; i < pwManager.pwReader.premium.Length + stdLen; i++)
            {
                iconsToUse.Add(pwManager.icons[pwManager.powerupIDs[i]][2]);
            }
        }
        else
        {

            for (int i = 0; i < pwManager.powerupIDs.Count; i++)
            {
                if(i == pwManager.pwReader.standard.Length - 1) { i++; } //skips the random icon
                iconsToUse.Add(pwManager.icons[pwManager.powerupIDs[i]][pwManager.getPowerupTier(pwManager.powerupIDs[i])]);
            }
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
