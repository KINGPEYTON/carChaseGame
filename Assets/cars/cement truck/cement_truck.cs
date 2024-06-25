using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cement_truck : cars
{
    void OnEnable()
    {
        blinkTime = -1;
        speedMin = 8;
        speedMax = 13;
        odds = 0.1f;
        forceMass = 1.4f;
    }
}
