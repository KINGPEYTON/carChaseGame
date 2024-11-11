using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : buildings
{
    public bool isBigBillboard;
    public GameObject gameOverOBJ;

    public Sprite ad;
    public float adTimer;

    public GameObject adOBJ;
    public GameObject newAdOBJ;
    public Sprite[] ads; //array of ads to appear

    public bool inTransition;
    public bool newAd;
    public float transitionTimer;

    public Sprite skinCurr;

    // Start is called before the first frame update

    public override void moreStart()
    {
        setAd(adOBJ.GetComponent<SpriteRenderer>());
        adTimer = Random.Range(2, 28);

        transitionTimer = 0f;
    }

    public override void moreUpdate()
    {
        adTimer -= Time.deltaTime;
        
        if (!newAd && adTimer <= 0)
        {
            setAd(newAdOBJ.GetComponent<SpriteRenderer>());
            transitionTimer = 0;
            inTransition = true;
            newAd = true;
        }

        if (inTransition)
        {
            adOBJ.transform.localPosition = new Vector3(0, getValueScale(transitionTimer, 0, 1, -1.9f), 0);
            transitionTimer += Time.deltaTime;
            if (transitionTimer > 1)
            {
                Destroy(adOBJ);
                inTransition = false;
            }
        }
    }

    public override void setSkin(Sprite skin)
    {
        try
        {
            base.setSkin(skin);
            skinCurr = skin;
        }
        catch
        {

        }
    }

    void setAd(SpriteRenderer sr)
    {
        ad = ads[Random.Range(0, ads.Length)]; //set the skin to a random one at spawn
        sr.GetComponent<SpriteRenderer>().sprite = ad;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
