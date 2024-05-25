using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suv_car : cars
{
    void OnEnable()
    {
        blinkTime = 15;
        speedMin = 11;
        speedMax = 16;
        odds = 0.25f;
    }
}
