using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flatbed_truck : cars
{
    public Sprite[] backCars;
    public SpriteRenderer sr;

    void OnEnable()
    {
        sr.sprite = backCars[Random.Range(0, backCars.Length)];
    }

    public override void setLane()
    {
        lane = Mathf.Abs((int)((transform.position.y / 1.25f) - 0.65f));
        GetComponent<SpriteRenderer>().sortingOrder = 3 + lane;
        sr.sortingOrder = 2 + lane;
        targPos = transform.position.y;

        if (blinkTime > -1)
        {
            turnUp.GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
            turnDown.GetComponent<SpriteRenderer>().sortingOrder = 4 + lane;
        }
    }
}
