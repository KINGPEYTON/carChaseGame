using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bus : cars
{
    public virtual void Start()
    {
        speed = Random.Range(10, 14);
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        setLane();
    }
}
