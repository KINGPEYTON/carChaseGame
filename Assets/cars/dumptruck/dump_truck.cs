using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dump_truck : cars
{
    void OnEnable()
    {
        blinkTime = -1;
        speedMin = 11;
        speedMax = 12;
        odds = 0.10f;
    }
}
