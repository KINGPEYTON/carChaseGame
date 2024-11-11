using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carBoost : MonoBehaviour
{
    public playerCar pCar;
    public main controller;
    public speedometer speedo;

    public float targX;
    public float targY;
    public Vector3 startPos;
    public float startTimer;
    public bool inPos;

    public int uses;
    public float power;
    public bool hitProt;

    public float prevMPH;
    public float prevScoreMPH;
    public float targetMPH;

    public AudioSource sndSource;
    public AudioClip boostClick;
    public AudioClip boostStart;
    public AudioClip boostUse;
    public AudioClip boostEnd;
    public AudioClip boostDone;

    public bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        pCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();
        startPos = transform.localPosition;
        if(controller.mph < 100) { targetMPH = 150; }
        else { targetMPH = controller.mph * 1.5f; }
        sndSource = GameObject.Find("secondAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            Vector3 dis = new Vector3(targX - startPos.x, targY - startPos.y, 1);
            transform.localPosition = calcPos(dis, startPos, startTimer, 1);
            startTimer += Time.deltaTime;
            if (startTimer > 1)
            {
                if (!inPos)
                {
                    inPos = true;
                    transform.localPosition = new Vector3(targX, targY, 1);
                    pCar.inBoost = true;
                    AudioSource.PlayClipAtPoint(boostClick, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
                }
                GetComponent<SpriteRenderer>().sortingOrder = pCar.window.sortingOrder - 1;
            }
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

    public void setTargetPos(float x, float y)
    {
        targX = x;
        targY = y;
    }

    public void useBoost()
    {
        pCar.boosting = true;
        controller.carTimerMultiplyer = 0.15f;
        pCar.boostLeft = power;
        uses--;

        prevMPH = controller.mph;
        prevScoreMPH = controller.scoremph;

        AudioSource.PlayClipAtPoint(boostStart, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        sndSource.clip = boostUse;
        sndSource.volume = controller.sfxVol * controller.masterVol;
        sndSource.Play();
    }

    public void finishBoost()
    {
        pCar.boosting = false;
        controller.carTimerMultiplyer = 1.0f;
        controller.mph = prevMPH;
        controller.scoremph = prevScoreMPH;
        controller.updateTint(new Color32(255, 0, 0, 0));
        speedo.usePowerUp(1);
        sndSource.clip = null;
        AudioSource.PlayClipAtPoint(boostEnd, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        if (uses <= 0)
        {
            endBoost();
        }
    }

    public void takeBoost()
    {
        uses--;
        speedo.usePowerUp(1);
        if (pCar.boosting) { finishBoost(); }
        if (uses <= 0)
        {
            endBoost();
        }
    }

    public void endBoost()
    {
        uses = 0;
        destroyed = true;
        inPos = true;
        pCar.inBoost = false;
        transform.parent = null;
        if (speedo.powerupActive) { speedo.finishPowerup(); }
        AudioSource.PlayClipAtPoint(boostDone, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
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
