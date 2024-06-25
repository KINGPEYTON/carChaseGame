using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class semi_truck : cars
{
    void OnEnable()
    {
        blinkTime = -1;
        speedMin = 10;
        speedMax = 16;
        odds = 0.2f;
        forceMass = 1.85f;
    }
}
