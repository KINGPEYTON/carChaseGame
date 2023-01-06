using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : buildings
{

    public Sprite ad;

    public float adTimer;

    public GameObject adOBJ;
    public Sprite[] ads; //array of ads to appear

    public Sprite statics;
    public float staticTimer;

    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();

        setAd();
        adTimer = Random.Range(2, 28);

        staticTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        adTimer -= Time.deltaTime;
        staticTimer -= Time.deltaTime;

        if (adTimer <= 0)
        {
            setAd();
            adTimer = 100;
            staticTimer = 1f;
        }

        if(staticTimer > 0)
        {
            adOBJ.GetComponent<SpriteRenderer>().sprite = statics;
        }
        else
        {
            adOBJ.GetComponent<SpriteRenderer>().sprite = ad;
        }
    }

    void setAd()
    {
        ad = ads[Random.Range(0, ads.Length)]; //set the skin to a random one at spawn
    }
}
