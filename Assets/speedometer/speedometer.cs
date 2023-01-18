using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class speedometer : MonoBehaviour
{
    public main controller;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI unitText;
    public Image speedMeter;

    public float startSpeed;
    public float maxSpeed;

    public TextMeshProUGUI speed1;
    public TextMeshProUGUI speed2;
    public TextMeshProUGUI speed3;
    public TextMeshProUGUI speed4;
    public TextMeshProUGUI speed5;
    public TextMeshProUGUI speed6;
    public TextMeshProUGUI speed7;

    public float endTextTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        startSpeed = controller.playerCar.startMph;
        maxSpeed = startSpeed + ((1 / controller.playerCar.upMph) * 77.0f);

        endTextTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            if (controller.mph < maxSpeed)
            {
                speedMeter.fillAmount = 0.1f + ((controller.mph/maxSpeed) * 0.8f);
                speedText.text = ((int)controller.mph).ToString();
            }
            else if (controller.mph < maxSpeed + 127)
            {
                speedMeter.fillAmount = 1;
                speedMeter.color = new Color32((byte)(255 - (controller.mph - maxSpeed)*2), 0, 0, 200);
                speedText.text = ((int)controller.mph).ToString();
            }
            else
            {
                speedText.text = "WTF";
            }
        }
        else if (controller.isOver)
        {
            if (endTextTimer > 0.5f)
            {
                speedText.text = randomChar();
                endTextTimer = 0;
            }
            endTextTimer += Time.deltaTime;
        }
        else
        {
            speedText.text = "---";
        }
        speed1.text = ((int)maxSpeed / 7).ToString();
        speed2.text = ((int)(maxSpeed / 7) * 2).ToString();
        speed3.text = (((int)maxSpeed / 7) * 3).ToString();
        speed4.text = (((int)maxSpeed / 7) * 4).ToString();
        speed5.text = (((int)maxSpeed / 7) * 5).ToString();
        speed6.text = (((int)maxSpeed / 7) * 6).ToString();
        speed7.text = maxSpeed.ToString();
    }

    string randomChar()
    {
        char one = (char)Random.Range(33, 64);
        char two = (char)Random.Range(33, 64);
        char three = (char)Random.Range(33, 64);
        return one + "" + two + "" + three;
    }
}
