using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regular_car : cars
{
    void OnEnable()
    {
        blinkTime = -1;
        speedMin = 12;
        speedMax = 18;
        odds = 0;
    }
}
