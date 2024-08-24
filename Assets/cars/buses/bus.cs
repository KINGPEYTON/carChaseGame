using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bus : cars
{
    public GameObject adOBJ;
    public Sprite[] ads; //array of ads to appear

    void OnEnable()
    {
        adOBJ.GetComponent<SpriteRenderer>().sprite = ads[Random.Range(0, ads.Length)]; //set the ad to a random one at spawn
    }
}
