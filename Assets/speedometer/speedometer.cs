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
    public Image coinImg;
    public RectTransform dial;

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
    public AudioClip tick;
    public AudioClip quickTick;

    public float endTextTimer;
    public float endTimer;

    public bool startAni;

    public float coinPulseTimer;
    public float coinPulseTime;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        startSpeed = controller.playerCar.startMph;
        maxSpeed = startSpeed + ((1 / controller.playerCar.upMph) * 77.0f);

        fadeOBJ(0);

        endTextTimer = 1;
        coinPulseTime = 0.45f;
        coinPulseTimer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playing)
        {
            mphText();

            if (!startAni)
            {
                if (controller.mph < controller.playerCar.startMph)
                {
                    coinText.text = "0";
                    fadeOBJ(getValueScale(getValueRanged(controller.mph, 0, controller.playerCar.startMph), 0, controller.playerCar.startMph, 200));
                }
                else
                {
                    fadeOBJ(200);
                    startAni = true;
                }
            }
            else
            {
                coinText.text = controller.coins.ToString();
            }
        }
        else if (controller.isOver)
        {
            if (powerupActive)
            {
                finishPowerup();
            }
            endSmoke();
        }

        if(coinPulseTimer < coinPulseTime)
        {
            coinPulseTimer += Time.deltaTime;
            float iconSize = 1.15f - getValueScale(Mathf.Abs((coinPulseTimer % coinPulseTime) - (coinPulseTime / 2)), 0, coinPulseTime, 0.15f);
            coinImg.transform.localScale = new Vector3(iconSize, iconSize, 1);
        } else if (coinImg.transform.localScale.x != 1.0f)
        {
            coinImg.transform.localScale = new Vector3(1, 1, 1);
        }

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
            powerUpUseText.color = new Color32(255, 255, 255, (byte)getValueScale(1 - iconHueTimer, 0, 1, 255));

            iconHueTimer += Time.deltaTime;
            if (iconHueTimer > 1)
            {
                fadePowerUp(0);
                powerUpUseText.color = new Color32(255, 255, 255, 0);
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
        if(powerUpUses < 4) { AudioSource.PlayClipAtPoint(quickTick, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol); }
        else { AudioSource.PlayClipAtPoint(tick, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol); }
        
        if (powerUpUses < 0) { powerUpUses = 0; }
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
        GetComponent<Image>().color = new Color32(255, 255, 255, (byte)getValueScale(value, 0, 200, 255));
        speedText.color = new Color32(255, 0, 0, (byte)value);
        coinText.color = new Color32(255, 255, 255, (byte)getValueScale(value, 0, 200, 255));
        coinImg.color = new Color32(255, 255, 255, (byte)value);
        speedMeter.color = new Color32((byte)(speedMeter.color.r * 255), (byte)(speedMeter.color.g * 255), (byte)(speedMeter.color.b * 255), (byte)value);
        speedBackround.color = new Color32(255, 255, 255, (byte)value);
        dial.Find("dial icon").GetComponent<Image>().color = new Color32(255, 255, 255, (byte)value);
        /*speed1.color = new Color32(255, 255, 255, (byte)value);
        speed2.color = new Color32(255, 255, 255, (byte)value);
        speed3.color = new Color32(255, 255, 255, (byte)value);
        speed4.color = new Color32(255, 255, 255, (byte)value);
        speed5.color = new Color32(255, 255, 255, (byte)value);
        speed6.color = new Color32(255, 255, 255, (byte)value);
        speed7.color = new Color32(255, 255, 255, (byte)value);*/
    }

    void mphText()
    {
        if (controller.mph < maxSpeed)
        {
            speedMeter.fillAmount = 0.065f + ((controller.mph / maxSpeed) * 0.87f);
            speedText.text = ((int)controller.mph).ToString();
            dial.eulerAngles = new Vector3(0, 0, -10 - getValueScale(controller.mph, 0, maxSpeed, 160));
        }
        else if (controller.mph < maxSpeed * 2)
        {
            speedMeter.fillAmount = 1;
            speedMeter.color = new Color32(255, (byte)(255 - getValueScale(controller.mph, maxSpeed, maxSpeed*2, 255)), (byte)(255 - getValueScale(controller.mph, maxSpeed, maxSpeed * 2, 255)), 200);
            speedText.text = ((int)controller.mph).ToString();
            if (dial.eulerAngles.z != -170)
            {
                dial.eulerAngles = new Vector3(0, 0, -170);
            }
        }
        else
        {
            dial.eulerAngles = new Vector3(0, 0, -10 - getValueScale(Mathf.Abs((controller.mph % 2) - 1), 0, 1, 160));
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
            fadePowerUp(30 + getValueScale(Mathf.Abs((iconHueTimer % 2) - 1), 0, 1, 170));
        }
        else
        {
            if (iconHueTimer < 2)
            {
                fadePowerUp(getValueScale(iconHueTimer, 0, 2, 200));
                powerUpUseText.color = new Color32(255, 255, 255, (byte)getValueScale(iconHueTimer, 0, 2, 255));
                iconHueTimer += Time.deltaTime;
                if(iconHueTimer > 2)
                {
                    fadePowerUp(200);
                    powerUpUseText.color = new Color32(255, 255, 255, 255);
                }
            }
        }
        
    }

    public void pulseCoin()
    {
        if (coinPulseTimer <= coinPulseTime)
        {
            if (coinPulseTimer > coinPulseTime / 2)
            {
                coinPulseTimer = coinPulseTime / 2;
            }
        }
        else
        {
            coinPulseTimer = 0;
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

    float getValueRanged(float val, float min, float max)
    {
        float newVal = val;
        if (newVal > max) { newVal = max; } else if (val < min) { newVal = min; }
        return newVal;
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
