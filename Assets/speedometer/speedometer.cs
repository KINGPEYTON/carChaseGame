using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class speedometer : MonoBehaviour
{
    public main controller;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI coinText;
    public Image speedMeter;
    public Image speedBackround;
    public ParticleSystem smoke;

    public float startSpeed;
    public float maxSpeed;

    public TextMeshProUGUI speed1;
    public TextMeshProUGUI speed2;
    public TextMeshProUGUI speed3;
    public TextMeshProUGUI speed4;
    public TextMeshProUGUI speed5;
    public TextMeshProUGUI speed6;
    public TextMeshProUGUI speed7;

    public Image powerUpIcon;
    public TextMeshProUGUI powerUpUseText;
    public int powerUpStartUses;
    public int powerUpUses;

    public bool powerupActive;
    public bool powerupIsTimed;
    public float powerUpTimer;
    public float iconHueTimer;

    public float endTextTimer;
    public float endTimer;

    public float coinTextTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        startSpeed = controller.playerCar.startMph;
        maxSpeed = startSpeed + ((1 / controller.playerCar.upMph) * 77.0f);

        smoke.enableEmission = false;

        endTextTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            mphText();

            if (coinTextTimer < 1.5)
            {
                coinText.text = ((int)((controller.totalCoins / 1.5) * (1.5 - coinTextTimer))).ToString();
                coinTextTimer += Time.deltaTime;
            }
            else
            {
                coinText.text = controller.coins.ToString();
            }
        }
        else if (controller.isOver)
        {
            endSmoke();
        }
        else
        {
            speedText.text = "---";
            coinText.text = controller.totalCoins.ToString();
        }
        setSpeedText();

        if (powerupActive)
        {
            startFade();
            if (powerupIsTimed)
            {
                powerUpTimer -= Time.deltaTime;
                if ((int)powerUpTimer + 1 < powerUpUses)
                {
                    usePowerUp(1);
                }
            }
        }
        else if (iconHueTimer < 1)
        {
            if (powerUpIcon.color.a > getValueScale(1 - iconHueTimer, 0, 1, 200))
            {
                fadePowerUp(getValueScale(1 - iconHueTimer, 0, 1, 200));
            }
            powerUpUseText.color = new Color32(32, 217, 255, (byte)getValueScale(1 - iconHueTimer, 0, 1, 200));

            iconHueTimer += Time.deltaTime;
            if (iconHueTimer > 1)
            {
                fadePowerUp(0);
                powerUpUseText.color = new Color32(32, 217, 255, 0);
            }
        }
    }

    public void startPowerup(float uses, Sprite icon, bool isTimed)
    {
        if (controller.playing)
        {
            powerUpIcon.sprite = icon;
            powerupIsTimed = isTimed;
            powerUpTimer = uses;
            powerUpUses = (int)uses;
            powerUpStartUses = (int)uses;
            powerUpUseText.text = powerUpUses.ToString();
            iconHueTimer = 0;
            powerupActive = true;
            controller.powerupActive = true;
        }
    }

    public void usePowerUp(int uses)
    {
        powerUpUses -= uses;
        if(powerUpUses < 0) { powerUpUses = 0; }
        powerUpUseText.text = powerUpUses.ToString();
        if (powerUpUses == 0)
        {
            finishPowerup();
        }
    }

    void setSpeedText()
    {
        speed1.text = (Mathf.Round(((int)((maxSpeed / 7) * 0.5f)) / 5) * 5).ToString();
        speed2.text = (Mathf.Round(((int)((maxSpeed / 7) * 1.5f)) / 5) * 5).ToString();
        speed3.text = (Mathf.Round(((int)((maxSpeed / 7) * 2.5f)) / 5) * 5).ToString();
        speed4.text = (Mathf.Round(((int)((maxSpeed / 7) * 3.5f)) / 5) * 5).ToString();
        speed5.text = (Mathf.Round(((int)((maxSpeed / 7) * 4.5f)) / 5) * 5).ToString();
        speed6.text = (Mathf.Round(((int)((maxSpeed / 7) * 5.5f)) / 5) * 5).ToString();
        speed7.text = (Mathf.Round(((int)((maxSpeed / 7) * 6.5f)) / 5) * 5).ToString();
    }

    void endSmoke()
    {
        if (endTextTimer > 0.25f)
        {
            speedText.text = randomChar();
            endTextTimer = 0;
        }
        endTextTimer += Time.deltaTime;
        if(endTimer > 1)
        {
            fadeOBJ(200 - getValueScale(endTimer, 1, 3, 200));
        }
        endTimer += Time.deltaTime;
        if(endTimer >= 3)
        {
            fadeOBJ(0);
        }

    }

    void fadeOBJ(float value)
    {
        GetComponent<Image>().color = new Color32(255, 255, 255, (byte)value);
        speedText.color = new Color32(255, 0, 0, (byte)value);
        coinText.color = new Color32(30, 215, 255, (byte)value);
        speedMeter.color = new Color32((byte)speedMeter.color.r, (byte)speedMeter.color.g, (byte)speedMeter.color.b, (byte)value);
        speedBackround.color = new Color32(255, 255, 255, (byte)value);
        speed1.color = new Color32(255, 255, 255, (byte)value);
        speed2.color = new Color32(255, 255, 255, (byte)value);
        speed3.color = new Color32(255, 255, 255, (byte)value);
        speed4.color = new Color32(255, 255, 255, (byte)value);
        speed5.color = new Color32(255, 255, 255, (byte)value);
        speed6.color = new Color32(255, 255, 255, (byte)value);
        speed7.color = new Color32(255, 255, 255, (byte)value);
    }

    void mphText()
    {
        if (controller.mph < maxSpeed)
        {
            speedMeter.fillAmount = 0.11f + ((controller.mph / maxSpeed) * 0.78f);
            speedText.text = ((int)controller.mph).ToString();
        }
        else if (controller.mph < maxSpeed * 2)
        {
            speedMeter.fillAmount = 1;
            speedMeter.color = new Color32((byte)(255 - (controller.mph - maxSpeed) * 2), 0, 0, 200);
            speedText.text = ((int)controller.mph).ToString();
        }
        else
        {
            speedText.text = "WTF";
        }
    }

    void fadePowerUp(float value)
    {
        powerUpIcon.color = new Color32(255, 255, 255, (byte)value);
    }

    void startFade()
    {
        if (powerUpUses * 1.0f / powerUpStartUses < 0.28f || (powerUpUses == 1 && powerUpStartUses > 1))
        {
            iconHueTimer += Time.deltaTime;
            fadePowerUp(getValueScale(Mathf.Abs((iconHueTimer % 2) - 1), 0, 1, 200));
        }
        else
        {
            if (iconHueTimer < 2)
            {
                fadePowerUp(getValueScale(iconHueTimer, 0, 2, 200));
                powerUpUseText.color = new Color32(32, 217, 255, (byte)getValueScale(iconHueTimer, 0, 2, 200));
                iconHueTimer += Time.deltaTime;
                if(iconHueTimer > 2)
                {
                    fadePowerUp(200);
                    powerUpUseText.color = new Color32(32, 217, 255, 200);
                }
            }
        }
        
    }

    string randomChar()
    {
        char one = (char)Random.Range(33, 64);
        char two = (char)Random.Range(33, 64);
        char three = (char)Random.Range(33, 64);
        return one + "" + two + "" + three;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    public void finishPowerup()
    {
        powerupActive = false;
        iconHueTimer = 0;
        powerUpTimer = 0;
        powerUpUses = 0;
        controller.powerupActive = false;
    }
}
