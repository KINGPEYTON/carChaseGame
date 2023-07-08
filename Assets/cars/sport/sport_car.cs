using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sport_car : cars
{
    void OnEnable()
    {
        blinkTime = 1;
        speedMin = 18;
        speedMax = 23;
        odds = 0.05f;
    }
}
