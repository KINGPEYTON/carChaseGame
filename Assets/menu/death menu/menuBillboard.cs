using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menuBillboard : MonoBehaviour
{
    public main controller;
    public Button myButton;

    public statsManager statManage;
    public Camera mainCamera;

    public bool inZoom;
    public float targetZoom;
    public float scaleZoom;
    public float startZoom;
    public float zoomTime;
    public Vector3 startPos;
    public Vector3 targPos;

    public Transform adOBJ;
    public bool inTransition;

    public bool inCameraPos;
    public float endTimer;

    public List<Transform> blockSigns;
    public SpriteRenderer blockBridge;

    public Image backround;
    public bool colorDir;
    public float colorVar;
    public string funFact;

    public bool scoreSet;
    public bool highScoreSet;
    public bool coinSet;
    public bool coinTotalSet;
    public bool speedSet;
    public float factTime;

    public bool scoreFire;
    public bool highScoreFire;
    public bool coinFire;
    public bool coinTotalFire;
    public bool speedFire;

    public Sprite[] circleClean;
    public RectTransform scoreOBJ;
    public RectTransform highOBJ;
    public RectTransform speedOBJ;
    public RectTransform coinsOBJ;
    public RectTransform totalOBJ;
    public RectTransform menuButton;
    public RectTransform factOBJ;

    public AudioClip deathTyping;
    public AudioClip fireEnd;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        statManage = GameObject.Find("statsManager").GetComponent<statsManager>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myButton = GetComponent<Button>();

        inTransition = true;
        zoomTime = 1.5f;
        colorVar = 100;
        targPos = new Vector3(transform.position.x, 2.85f, 0);
        startPos = mainCamera.transform.position;
        startZoom = mainCamera.orthographicSize;

        scaleZoom = cameraScaler.getScale(4.15f * targetZoom * controller.distMulti);

        if(GameObject.Find("bridge start(Clone)") != null)
        {
            SpriteRenderer bridgePiece = GameObject.Find("bridge start(Clone)").GetComponent<SpriteRenderer>();
            if (Mathf.Abs(bridgePiece.transform.position.x - transform.position.x) < targetZoom * 1.85f * controller.distMulti)
            {
                blockBridge = bridgePiece;
            }
        }

        Transform topSigns = GameObject.Find("Signs Top").transform;
        for(int i = 0; i < topSigns.childCount; i++)
        {
            Transform newSign = topSigns.GetChild(i);
            if(Mathf.Abs(newSign.transform.position.x - transform.position.x) < targetZoom * 1.85f * controller.distMulti)
            {
                blockSigns.Add(newSign);
            }
        }

        funFact = controller.worldM.getFunFact();
    }

    // Update is called once per frame
    void Update()
    {
        endTimer += Time.deltaTime;

        if (inTransition)
        {
            adOBJ.transform.localPosition = new Vector3(0, getValueScale(getValueRanged(endTimer, 0, 1), 0, 1, -40), 0);
            if (endTimer > 1)
            {
                Destroy(adOBJ.gameObject);
                AudioSource.PlayClipAtPoint(deathTyping, new Vector3(transform.position.x, transform.position.y, -10), controller.masterVol * controller.sfxVol);
                inTransition = false;
                inZoom = true;
                endTimer = 0;
                GameObject.Find("playerCar").GetComponent<playerCar>().crashSmoke.startLifetime = 0.75f;
                GameObject.Find("playerCar").GetComponent<playerCar>().crashSmoke.startSpeed = 2f;
                controller.checkTutorialDeath(menuButton.position);
            }
        }
        else
        {
            endStats();
            if (inZoom)
            {
                cameraZoom();
            }
        }
    }

    void cameraZoom()
    {
        Vector3 dis = new Vector3(targPos.x - startPos.x, targPos.y - startPos.y, 0);
        mainCamera.transform.position = calcPos(dis, startPos, endTimer, zoomTime);
        mainCamera.orthographicSize = startZoom - getValueScale(getValueRanged(endTimer, 0, zoomTime), 0, zoomTime, startZoom - scaleZoom);
        fadeSigns(255 - getValueScale(getValueRanged(endTimer, 0, zoomTime), 0, zoomTime, 200));
        if (endTimer > zoomTime)
        {
            inZoom = false;
        }
    }

    void endStats()
    {
        setStatDashboards(scoreOBJ, 0, 2f, (13 + (((int)controller.score) + "m").Length * (9 / 7.0f)), getValueScale(controller.score, 0, (statManage.pstats.scoreAvg + statManage.pstats.highScore) / 2, 1), (int)controller.score, "m", "Score", ref scoreSet, ref scoreFire, scoreIconAni);
        setStatDashboards(highOBJ, 1, 2.35f, (13 + (((int)controller.highScore) + "m").Length * (9 / 7.0f)), getValueScale(controller.score, 0, statManage.pstats.highScore, 1), statManage.pstats.highScore, "m", "High Score", ref highScoreSet, ref highScoreFire, highScoreIconAni);
        setStatDashboards(coinsOBJ, 2, 2.7f, (15.5f + controller.coins.ToString().Length * (8 / 7.0f)), getValueScale(controller.coins, 0, statManage.pstats.coinsInGame, 1), controller.coins, "", "Coins", ref coinSet, ref coinFire, coinsGainedIconAni);
        setStatDashboards(speedOBJ, 3, 3.05f, (13 + ((controller.topMPH) + " MPH").Length * (9 / 7.0f)), getValueScale(controller.topMPH, 0, statManage.pstats.topSpeed, 1), controller.topMPH, " MPH", "Top Speed", ref speedSet, ref speedFire, topSpeedIconAni);
        setStatDashboards(totalOBJ, 4, 3.4f, (15.5f + controller.totalCoins.ToString().Length * (8 / 7.0f)), getValueScale(controller.coins, 0, (statManage.pstats.avgCoins + statManage.pstats.coinsInGame) / 2, 1), controller.totalCoins, "", "Total Coins", ref coinTotalSet, ref coinTotalFire, coinsTotalIconAni);
        setMenuButton(2, menuButton.Find("Menu Buttom Outline").GetComponent<Image>(), menuButton.Find("Outline Ext Top").GetComponent<RectTransform>(), menuButton.Find("Outline Ext Bottom").GetComponent<RectTransform>(), menuButton.Find("Button Backround").GetComponent<Image>(), menuButton.Find("Button Backround").Find("Button Text").GetComponent<RectTransform>());
        setFunFact(3, factOBJ.Find("Fact Outline").GetComponent<Image>(), factOBJ.Find("Fact Backround").GetComponent<Image>(), factOBJ.Find("Fact Text").GetComponent<TextMeshProUGUI>());
    }

    void scoreIconAni()
    {
        float iconSize = 0.9f + getValueScale(Mathf.Abs(((endTimer) % 2) - 1f), 0, 1f, 0.2f);
        scoreOBJ.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>().localScale = new Vector3(iconSize, iconSize, 1);
    }

    void highScoreIconAni()
    {
        float icon1Size = 0.95f + getValueScale(Mathf.Abs(((endTimer) % 2) - 1f), 0, 1f, 0.1f);
        float icon2Size = 0.9f + getValueScale(Mathf.Abs(((endTimer) % 3) - 1.5f), 0, 1.5f, 0.2f);
        float icon3Size = 0.9f + getValueScale(Mathf.Abs(((1.5f + endTimer) % 3) - 1.5f), 0, 1.5f, 0.2f);
        highOBJ.Find("outline cicle").Find("outline icon").Find("star 1").GetComponent<RectTransform>().localScale = new Vector3(icon1Size, icon1Size, 1);
        highOBJ.Find("outline cicle").Find("outline icon").Find("star 2").GetComponent<RectTransform>().localScale = new Vector3(icon2Size, icon2Size, 1);
        highOBJ.Find("outline cicle").Find("outline icon").Find("star 3").GetComponent<RectTransform>().localScale = new Vector3(icon3Size, icon3Size, 1);
    }

    void coinsGainedIconAni()
    {
        float iconSize = 0.925f + getValueScale(Mathf.Abs(((endTimer) % 3.5f) - 1.75f), 0, 1.75f, 0.15f);
        float iconAng = getValueScale((endTimer % 7), 0, 7f, 2) * Mathf.PI;
        coinsOBJ.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>().localScale = new Vector3(iconSize, iconSize, 1);
        coinsOBJ.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>().localPosition = new Vector3((Mathf.Cos(iconAng) * 0.1f) - 0.5f, (Mathf.Sin(iconAng) * 0.15f) - 0.15f, 1);
    }

    void coinsTotalIconAni()
    {
        float iconSize = 1f + getValueScale(Mathf.Abs(((endTimer + 1.25f) % 2.5f) - 1.25f), 0, 1.25f, 0.1f);
        float coinPos = 2.4f - getValueScale(Mathf.Abs(((endTimer + 1.25f) % 2.5f) - 1.25f), 0, 1.25f, 0.4f);
        totalOBJ.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>().localScale = new Vector3(iconSize, iconSize, 1);
        totalOBJ.Find("outline cicle").Find("outline icon").Find("icon coin").GetComponent<RectTransform>().localPosition = new Vector3(0, coinPos, 1);
    }

    void topSpeedIconAni()
    {
        float iconSize = 0.975f + getValueScale(Mathf.Abs(((endTimer) % 3) - 1.5f), 0, 1.5f, 0.05f);
        float boltSize = 0.925f + getValueScale(Mathf.Abs(((endTimer) % 4) - 2f), 0, 2f, 0.15f);
        float boltAng = getValueScale((endTimer % 5), 0, 5f, 2) * Mathf.PI;
        speedOBJ.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>().localScale = new Vector3(iconSize, iconSize, 1);
        speedOBJ.Find("outline cicle").Find("outline icon").Find("icon bolt").GetComponent<RectTransform>().localScale = new Vector3(boltSize, boltSize, 1);
        speedOBJ.Find("outline cicle").Find("outline icon").Find("icon bolt").GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Cos(boltAng) * 0.25f, Mathf.Sin(boltAng) * 0.2f, 1);
    }

    void setMenuButton(float time, Image mainOutline, RectTransform outlineTop, RectTransform outlineBottom, Image backround, RectTransform menuText)
    {
        if (endTimer < time / 2)
        {
            mainOutline.fillAmount = getValueScale(endTimer, 0, time / 2, 1);
        }
        else
        {
            if (mainOutline.enabled == true)
            {
                mainOutline.enabled = false;
                menuButton.Find("Ouline Left").GetComponent<Image>().enabled = true;
                menuButton.Find("Outline Ext Top").Find("Ouline Right").GetComponent<Image>().enabled = true;
                outlineTop.GetComponent<Image>().enabled = true;
                outlineBottom.GetComponent<Image>().enabled = true;
            }
            outlineTop.sizeDelta = new Vector2(getValueScale(getValueRanged(endTimer, time / 2, time), time / 2, time, 45), 1.55f);
            outlineBottom.sizeDelta = new Vector2(getValueScale(getValueRanged(endTimer, time / 2, time), time / 2, time, 45), 1.55f);
            if (backround.fillAmount < 1)
            {
                backround.fillAmount = getValueScale(getValueRanged(endTimer, time, time * 1.5f), time, time * 1.5f, 1);
            }
            else
            {
                menuButton.GetComponent<Button>().interactable = true;
                float textSize = 0.975f + getValueScale(Mathf.Abs(((endTimer + (time * 1.5f)) % 2) - 1f), 0, 1f, 0.05f);
                menuText.localScale = new Vector3(textSize, textSize, 1);
                backround.color = wheelColor(6, endTimer - (time * 1.5f), 200);
            }
        }
    }

    void setFunFact(float time, Image outline, Image backround, TextMeshProUGUI text)
    {
        if (endTimer < time / 2)
        {
            outline.fillAmount = getValueScale(endTimer, 0, time / 2, 1);
        }
        else
        {
            outline.fillAmount = 1;
            backround.fillAmount = getValueScale(getValueRanged(endTimer, time / 2, time * 0.75f), time / 2, time * 0.75f, 1);
            if (endTimer > time * 0.75f)
            {
                if (endTimer < time * 1.5f)
                {
                    text.text = funFact.Substring(0, (int)getValueScale(endTimer, time * 0.75f, time * 1.5f, funFact.Length)) + (char)Random.Range(33, 64);
                }
                else
                {
                    text.text = funFact;
                }
            }
        }
    }

    void setStatDashboards(RectTransform obj, int colID, float time, float size, float barSize, int amountTarg, string addOn, string titleName, ref bool iconSet, ref bool fireSet, methodType iconAni)
    {
        setDashboards(iconAni, colID, ref iconSet, obj.Find("outline cicle").GetComponent<Image>(), obj.Find("outline").GetComponent<RectTransform>(), obj.Find("outline").Find("bars back mask").GetComponent<RectTransform>(), obj.Find("bars mask").GetComponent<RectTransform>(), obj.Find("bars mask").Find("bars").GetComponent<Image>(), obj.Find("outline cicle").Find("outline icon").GetComponent<RectTransform>(), obj.Find("amount text").GetComponent<TextMeshProUGUI>(), obj.Find("title text").GetComponent<TextMeshProUGUI>(), time, size, amountTarg, addOn, barSize, titleName);
        if (barSize >= 0.99f)
        {
            if(endTimer > time * 1.45f)
            {
                if (endTimer > time * 1.75f)
                {
                    ParticleSystem fireworks = obj.Find("celebrate").GetComponent<ParticleSystem>();
                    fireworks.Stop();
                }
                else
                {
                    ParticleSystem fireworks = obj.Find("celebrate").GetComponent<ParticleSystem>();
                    if (fireworks.isStopped)
                    {
                        fireworks.Play();
                        obj.Find("celebrate").GetComponent<AudioSource>().Play();
                        obj.Find("celebrate").GetComponent<AudioSource>().GetComponent<AudioSource>().volume = controller.masterVol * controller.sfxVol;
                    }
                }
            }
            if (endTimer > time * 1.5f)
            {
                fireAni(obj, obj.Find("fire").transform, time, ref fireSet);
            }
        }
    }

    void setDashboards(methodType iconAni, int colID, ref bool iconSet, Image circle, RectTransform outline, RectTransform barsBack, RectTransform bars, Image barsImg, RectTransform icon, TextMeshProUGUI amount, TextMeshProUGUI title,float time, float size, int amountTarg, string addOn, float barSize, string titleName)
    {
        if (!iconSet)
        {
            float iconSize = getValueScale(getValueRanged(endTimer, 0, time * 0.75f), 0, time * 0.75f, 1);
            icon.localScale = new Vector3(iconSize, iconSize, 1);
            if (endTimer > time * 0.75)
            {
                iconSet = true;
            }
        }
        else
        {
            iconAni();
        }

        if (endTimer < time / 2)
        {
            circle.fillAmount = getValueScale(endTimer, 0, time / 2, 1);
        }
        else
        {
            outline.sizeDelta = new Vector2(getValueScale(getValueRanged(endTimer, time / 2, time), time / 2, time, size), 2.5f);
            barsBack.sizeDelta = new Vector2(getValueScale(getValueRanged(endTimer, time / 2, time), time / 2, time, size), 5);
            if (endTimer < time * 1.25f)
            {
                title.text = titleName.Substring(0, (int)getValueScale(endTimer, time / 2, time * 1.25f, titleName.Length)) + (char)Random.Range(33, 64);
            }
            else
            {
                outline.sizeDelta = new Vector2(size, 2.5f);
                title.text = titleName;
            }
            circle.fillAmount = 1;
            circle.sprite = circleClean[colID];
            outline.Find("outline edge").GetComponent<Image>().enabled = true;

            if (endTimer >= time * 0.9f)
            {
                if (endTimer < time * 1.5f && addOn.Length > 0)
                {
                    string addText = addOn.Substring(0, (int)getValueScale(endTimer, time * 0.9f, time * 1.5f, addOn.Length)) + (char)Random.Range(33, 64);
                    amount.text = ((int)getValueScale(getValueRanged(endTimer, time * 0.9f, time * 1.5f), time * 0.9f, time * 1.5f, amountTarg)) + addText;
                }
                else
                {
                    amount.text = ((int)getValueScale(getValueRanged(endTimer, time * 0.9f, time * 1.5f), time * 0.9f, time * 1.5f, amountTarg)) + addOn;
                }
            }
            bars.sizeDelta = new Vector2(getValueScale(getValueRanged(endTimer, time * 0.75f, time * 1.45f), time * 0.75f, time * 1.45f, 11.75f * barSize), 5);
            barsImg.color = wheelColor(6, endTimer + (time * 2), 45);
        }
    }

    void fireAni(RectTransform obj, Transform fire, float time, ref bool isFire)
    {
        float scaleVal = (1.035f - getValueScale(Mathf.Abs((time * 1.65f) - getValueRanged(endTimer, time * 1.5f, time * 1.8f)), 0, time * 0.15f, 0.035f)) * 0.925f;
        obj.localScale = new Vector3(scaleVal, scaleVal, 1);
        if (!isFire)
        {
            if (endTimer > (time * 1.65f))
            {
                isFire = true;
                foreach (ParticleSystem pc in fire.GetComponentsInChildren<ParticleSystem>())
                {
                    pc.enableEmission = true;
                }
                AudioSource sndSource = GameObject.Find("secondAudio").GetComponent<AudioSource>();
                if(sndSource.clip == null)
                {
                    sndSource.clip = fireEnd;
                    sndSource.volume = controller.sfxVol * controller.masterVol;
                    sndSource.Play();
                }
            }
        }
    }

    public void click()
    {
        if (!inTransition)
        {
            controller.newGame();
        }
    }

    void fadeSigns(float value)
    {
        if (blockBridge != null)
        {
            blockBridge.color = new Color32(255, 255, 255, (byte)value);
            blockBridge.transform.Find("start ropes").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)value);
        }

        foreach (Transform t in blockSigns)
        {
            t.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)value);
            for (int i = 0; i < t.childCount; i++)
            {
                TextMeshProUGUI newText = t.GetChild(i).GetComponent<TextMeshProUGUI>();
                newText.color = new Color32((byte)(newText.color.r * 255), (byte)(newText.color.g * 255), (byte)(newText.color.b*255), (byte)value);
            }
        }

    }

    Color32 wheelColor(float colorCycle, float colorTimer, float colDif)
    {
        float colorVarR = 0;
        float colorVarG = 0;
        float colorVarB = 0;
        if (colorTimer % colorCycle < (colorCycle / 6) * 1)
        {
            colorVarR = (255 - colDif) + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 0, (colorCycle / 6) * 1, colDif);
            colorVarG = (255 - colDif);
            colorVarB = 255;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 2)
        {
            colorVarR = 255;
            colorVarG = (255 - colDif);
            colorVarB = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 1, (colorCycle / 6) * 2, colDif);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 3)
        {
            colorVarR = 255;
            colorVarG = (255 - colDif) + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 2, (colorCycle / 6) * 3, colDif);
            colorVarB = (255 - colDif);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 4)
        {
            colorVarR = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 3, (colorCycle / 6) * 4, colDif);
            colorVarG = 255;
            colorVarB = (255 - colDif);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 5)
        {
            colorVarR = (255 - colDif);
            colorVarG = 255;
            colorVarB = (255 - colDif) + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 4, (colorCycle / 6) * 5, colDif);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 6)
        {
            colorVarR = (255 - colDif);
            colorVarG = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 5, (colorCycle / 6) * 6, colDif);
            colorVarB = 255;
        }

        return new Color32((byte)colorVarR, (byte)colorVarG, (byte)colorVarB, 255);
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float scaledTimer = getValueRanged(targetTimer, 0, targetTime);
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y);
        return new Vector3(xVal, yVal, 0) + startScale;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    float getValueRanged(float val, float min, float max)
    {
        float newVal = val;
        if (newVal > max) { newVal = max; } else if (val < min) { newVal = min; }
        return newVal;
    }
}
