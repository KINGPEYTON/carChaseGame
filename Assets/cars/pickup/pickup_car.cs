using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_car : cars
{
    void OnEnable()
    {
        blinkTime = 8;
        speedMin = 9;
        speedMax = 12;
        odds = 0.15f;
    }
}
