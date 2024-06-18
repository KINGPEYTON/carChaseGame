using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class incognito : MonoBehaviour
{
    public main controller;

    public float lifetime;
    public float mphTakaway;

    public bool doneSlowdown;
    public bool resetScore;
    public float mphTakawayTimer;
    public float mphTakawayTime;
    public float currMph;

    // Update is called once per frame
    void Update()
    {
        if (doneSlowdown)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                endSlowdown();
            }
        }
        else
        {
            doSlowdown();
        }
    }

    public void setSlowdown(float time, float spawnRate, bool resScore)
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        lifetime = time;
        controller.carTimerMultiplyer = spawnRate;
        controller.isSlowdown = true;
        mphTakaway = controller.playerCar.upMph * time * 2.5f;
        resetScore = resScore;
        currMph = controller.mph;
        doneSlowdown = false;

        mphTakawayTime = 1.25f;
    }

    void doSlowdown()
    {
        controller.mph = currMph - getValueScale(mphTakawayTimer, 0, mphTakawayTime, mphTakaway);
        mphTakawayTimer += Time.deltaTime;
        if(mphTakawayTimer > mphTakawayTime)
        {
            controller.mph = currMph - mphTakaway;
            if (resetScore)
            {
                controller.scoremph = currMph - mphTakaway;
            }
            doneSlowdown = true;
        }
    }

    void endSlowdown()
    {
        controller.carTimerMultiplyer = 1;
        controller.isSlowdown = false;
        Destroy(gameObject);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
