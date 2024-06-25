using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minivan_car : cars
{
    void OnEnable()
    {
        blinkTime = 20;
        speedMin = 10;
        speedMax = 14;
        odds = 0.2f;
        forceMass = 0.65f;
        isCar = true;
    }
}
