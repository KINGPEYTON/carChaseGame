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
                speedMeter.fillAmount = 0.11f + ((controller.mph/maxSpeed) * 0.78f);
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
        speed1.text = (Mathf.Round(((int)((maxSpeed / 7) * 0.5f)) / 5) * 5).ToString();
        speed2.text = (Mathf.Round(((int)((maxSpeed / 7) * 1.5f)) / 5) * 5).ToString();
        speed3.text = (Mathf.Round(((int)((maxSpeed / 7) * 2.5f)) / 5) * 5).ToString();
        speed4.text = (Mathf.Round(((int)((maxSpeed / 7) * 3.5f)) / 5) * 5).ToString();
        speed5.text = (Mathf.Round(((int)((maxSpeed / 7) * 4.5f)) / 5) * 5).ToString();
        speed6.text = (Mathf.Round(((int)((maxSpeed / 7) * 5.5f)) / 5) * 5).ToString();
        speed7.text = (Mathf.Round(((int)((maxSpeed / 7) * 6.5f)) / 5) * 5).ToString();
    }

    string randomChar()
    {
        char one = (char)Random.Range(33, 64);
        char two = (char)Random.Range(33, 64);
        char three = (char)Random.Range(33, 64);
        return one + "" + two + "" + three;
    }
}
