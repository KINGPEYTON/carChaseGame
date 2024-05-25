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
    public Sprite[] ads; //array of ads to appear

    public Sprite statics;
    public float staticTimer;
    public AudioClip staticSound;

    public Sprite skinCurr;

    // Start is called before the first frame update

    public override void moreStart()
    {
        setAd();
        adTimer = Random.Range(2, 28);

        staticTimer = 0f;
    }

    public override void moreUpdate()
    {
        adTimer -= Time.deltaTime;
        staticTimer -= Time.deltaTime;

        if (adTimer <= 0)
        {
            setAd();
            adTimer = 100;
            staticTimer = 1f;
            AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        }

        if (staticTimer > 0)
        {
            adOBJ.GetComponent<SpriteRenderer>().sprite = statics;
        }
        else
        {
            adOBJ.GetComponent<SpriteRenderer>().sprite = ad;
        }
    }

    public override void setSkin(Sprite skin)
    {
        base.setSkin(skin);
        skinCurr = skin;
    }

    void setAd()
    {
        ad = ads[Random.Range(0, ads.Length)]; //set the skin to a random one at spawn
    }
}
