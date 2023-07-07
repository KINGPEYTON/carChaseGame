using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sport_car : cars
{
    void OnEnable()
    {
        blinkTime = 0;
        speedMin = 18;
        speedMax = 23;
    }
}
