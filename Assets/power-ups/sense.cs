using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sense : MonoBehaviour
{
    public float lifetime;
    public bool showIcons;
    public float turnMulti;
    public float hitBoxSize;

    public playerCar pCar;
    public main controller;
    public speedometer speedo;

    public SpriteRenderer playerOutline;
    public SpriteRenderer playerWheelFOutline;
    public SpriteRenderer playerWheelBOutline;
    public List<SpriteRenderer> carsOutline;
    public List<GameObject> coinsOutline;
    public List<GameObject> carIcons;

    public Transform scoreBlimp;
    public TextMeshProUGUI scoreBlimpText;
    public GameObject scoreBlimpOutline;
    public TextMeshProUGUI scoreBlimpOutlineText;
    public Transform pausePlane;
    public GameObject pausePlaneOutline;
    public GameObject outlineUI;

    public bool doFadeIn;
    public bool doFadeOut;
    public float fadeVal;
    public float fadeTimer;
    public float fadeTime;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime < 0 && !doFadeOut) { startFade(false); }

        updateUI();

        if (doFadeIn) { visionFadeIn(); }
        if (doFadeOut) { visionFadeOut(); }
    }

    public void startSense(float time, bool showI, float turnMultiplyer, float hitBox)
    {
        pCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();
        scoreBlimp = GameObject.Find("Score-Blimp").transform;
        scoreBlimpText = scoreBlimp.GetChild(0).GetComponent<TextMeshProUGUI>();
        pausePlane = GameObject.Find("Pause Plane").transform;

        controller.senseVision = true;
        controller.enhancedSense = this;

        lifetime = time;
        showIcons = showI;
        turnMulti = turnMultiplyer;
        hitBoxSize = hitBox;

        fadeTime = 1.5f;
        createOutlines();
        pCar.turnMulti = turnMulti;
        pCar.changeHitBox(hitBoxSize);

        controller.carTimer -= 60;
        controller.carPlace = 20;

        startFade(true);
    }

    void createOutlines()
    {
        pCar.createOuline(this);

        foreach (GameObject c in controller.carsInGame)
        {
            c.GetComponent<cars>().createOuline(this);
        }
        foreach (GameObject c in controller.coinList)
        {
            c.GetComponent<coins>().createOuline(this);
        }

        Transform newOutline = Instantiate(outlineUI).transform;
        scoreBlimpOutline = newOutline.Find("Score Blimp Outline").gameObject;
        scoreBlimpOutlineText = scoreBlimpOutline.transform.Find("Text Outline").GetComponent<TextMeshProUGUI>();
        pausePlaneOutline = newOutline.Find("Pause Plane Outline").gameObject;
    }

    void updateUI()
    {
        scoreBlimpOutline.transform.position = scoreBlimp.position;
        pausePlaneOutline.transform.position = pausePlane.position;
        scoreBlimpOutlineText.text = scoreBlimpText.text;
    }

    void visionFadeIn()
    {
        fadeVal = (getValueScale(fadeTimer, 0, fadeTime, 235));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(235);
            doFadeIn = false;
        }
    }

    void visionFadeOut()
    {
        fadeVal = (235 - getValueScale(fadeTimer, 0, fadeTime, 235));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(0);
            controller.senseVision = false;
            pCar.turnMulti = pCar.turnMulti * (1 / turnMulti);
            pCar.changeHitBox(1);

            controller.carTimer += 60;
            controller.carPlace = 12;

            Destroy(playerOutline);
            Destroy(playerWheelFOutline);
            Destroy(playerWheelBOutline);

            Destroy(scoreBlimpOutline);
            Destroy(scoreBlimpOutlineText);
            Destroy(pausePlaneOutline);

            foreach (SpriteRenderer sr in carsOutline) { Destroy(sr.gameObject); }
            foreach (GameObject sr in coinsOutline) { Destroy(sr); }
            foreach (GameObject sr in controller.coinList) { sr.GetComponent<coins>().changeHitbox(hitBoxSize); } //fix the coin hitboxs
            foreach (GameObject sr in carIcons) { Destroy(sr); }
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
            foreach(GameObject ci in carIcons) { ci.GetComponent<carIcon>().startFade(false); }
        }
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    void fadeAni(float value)
    {
        GetComponent<SpriteRenderer>().color = new Color32(25, 25, 25, (byte)value);

        playerOutline.color = new Color32(0, 200, 200, (byte)value);
        playerWheelFOutline.color = new Color32(0, 200, 200, (byte)value);
        playerWheelBOutline.color = new Color32(0, 200, 200, (byte)value);

        scoreBlimpOutline.GetComponent<SpriteRenderer>().color = new Color32(0, 200, 200, (byte)value);
        pausePlaneOutline.GetComponent<Image>().color = new Color32(0, 200, 200, (byte)value);
        scoreBlimpOutlineText.color = new Color32(200, 200, 0, (byte)value);

        foreach (SpriteRenderer sr in carsOutline)
        {
            sr.color = new Color32(200, 0, 0, (byte)value);
        }
        foreach (GameObject sr in coinsOutline)
        {
            if (sr.transform.parent.GetComponent<coins>().isHolo)
            {
                sr.GetComponent<SpriteRenderer>().color = new Color32(180, 200, 0, (byte)value);
            }
            else
            {
                sr.GetComponent<SpriteRenderer>().color = new Color32(0, 200, 0, (byte)value);
            }
        }
    }
}
