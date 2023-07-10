using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bus : cars
{
    public GameObject adOBJ;
    public Sprite[] ads; //array of ads to appear

    void OnEnable()
    {
        blinkTime = -1;
        speedMin = 10;
        speedMax = 14;
        adOBJ.GetComponent<SpriteRenderer>().sprite = ads[Random.Range(0, ads.Length)]; //set the ad to a random one at spawn
    }
}
