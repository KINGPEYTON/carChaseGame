using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class overBus : bus
{
    public TextMeshProUGUI highscore;

    // Start is called before the first frame update
    void OnEnable()
    {
        speedMin = 9;
        speedMax = 9;

        controller = GameObject.Find("contoller").GetComponent<main>();
        highscore.text = "High Score: " + (int)controller.highScore + "m";
    }

}
