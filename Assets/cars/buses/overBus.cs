using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class overBus : bus
{
    public TextMeshProUGUI highscore;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        speed = 9;

        highscore.text = "High Score: " + (int)controller.highScore + "m";
    }

}
