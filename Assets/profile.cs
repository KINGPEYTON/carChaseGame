using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class profile : MonoBehaviour
{
    public int carID;
    public float highScore;


    // Use this for initialization
    void Awake()
    {
        carID = PlayerPrefs.GetInt("carID", 0); //sets high score to the one saved
        highScore = PlayerPrefs.GetInt("highscore", 0); //sets high score to the one saved
    }
}
