using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menuBillboard : MonoBehaviour
{
    public main controller;
    public Button myButton;

    public Camera mainCamera;

    public bool inZoom;
    public float targetZoom;
    public float startZoom;
    public float zoomTime;
    public Vector3 startPos;
    public Vector3 targPos;

    public GameObject statics;
    public bool inStatic;
    public bool inCameraPos;
    public float endTimer;

    public List<Transform> blockSigns;

    public Image backround;
    public bool colorDir;
    public float colorVar;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public Image newHighScore;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI totalCoinText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI factText;
    public string funFact;

    public float scoreTime;
    public float highScoreTime;
    public float coinTime;
    public float coinTotalTime;
    public float speedTime;
    public float factTime;
    public float newScoreTime;

    public AudioClip staticSound;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myButton = GetComponent<Button>();

        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0,0,-10), controller.masterVol  * controller.sfxVol);
        inStatic = true;
        zoomTime = 1.5f;
        colorVar = 100;
        targPos = new Vector3(transform.position.x, 2.85f, 0);
        startPos = mainCamera.transform.position;
        startZoom = mainCamera.orthographicSize;

        Transform topSigns = GameObject.Find("Signs Top").transform;
        for(int i = 0; i < topSigns.childCount; i++)
        {
            Transform newSign = topSigns.GetChild(i);
            //Debug.Log(i + ": " + Mathf.Abs(newSign.transform.position.x - transform.position.x));
            if(Mathf.Abs(newSign.transform.position.x - transform.position.x) < targetZoom * 1.75f)
            {
                blockSigns.Add(newSign);
            }
        }

        setTexts();
    }

    // Update is called once per frame
    void Update()
    {
        endTimer += Time.deltaTime;

        if (inStatic)
        {
            if (endTimer > 1)
            {
                statics.SetActive(false);
                inStatic = false;
                inZoom = true;
                endTimer = 0;
            }
        }
        else
        {
            textAni();
            if (inZoom)
            {
                cameraZoom();
            }
        }

        backround.color = new Color32(255, (byte)colorVar, (byte)colorVar, 255);

        if (colorDir)
        {
            colorVar += Time.deltaTime * 60;
            if (colorVar > 130)
            {
                colorDir = false;
            }
        }
        else
        {
            colorVar -= Time.deltaTime * 60;
            if (colorVar < 20)
            {
                colorDir = true;
            }
        }
    }

    void cameraZoom()
    {
        Vector3 dis = new Vector3(targPos.x - startPos.x, targPos.y - startPos.y, 0);
        mainCamera.transform.position = calcPos(dis, startPos, endTimer, zoomTime);
        mainCamera.orthographicSize = startZoom - getValueScale(getValueRanged(endTimer, 0, zoomTime), 0, zoomTime, startZoom - targetZoom);
        fadeSigns(255 - getValueScale(getValueRanged(endTimer, 0, zoomTime), 0, zoomTime, 200));
        if (endTimer > zoomTime)
        {
            inZoom = false;
        }
    }

    void textAni()
    {
        if (endTimer > scoreTime)
        {
            scoreText.text = (int)controller.score + "m";
        }
        else
        {
            scoreText.text = ((int)getValueScale(endTimer, 0, scoreTime, (int)controller.score)) + "m";
        }

        if (endTimer > highScoreTime)
        {
            highScoreText.text = (int)controller.highScore + "m";
            if (controller.newHighScore)
            {
                if (endTimer > highScoreTime + newScoreTime)
                {
                    newHighScore.transform.localScale = new Vector3(1, 1, 1);
                    newHighScore.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    float scaleVal = 3 - getValueScale(endTimer, highScoreTime, highScoreTime + newScoreTime, 2);
                    newHighScore.transform.localScale = new Vector3(scaleVal, scaleVal, 1);
                    byte a = (byte)getValueScale(endTimer, highScoreTime, highScoreTime + newScoreTime, 255);
                    newHighScore.color = new Color32(255, 255, 255, a); 
                }
            }
        }
        else
        {
            highScoreText.text = ((int)getValueScale(endTimer, 0, highScoreTime, (int)controller.highScore)) + "m";
        }

        if (endTimer > coinTime)
        {
            coinText.text = controller.coins.ToString();
        }
        else
        {
            coinText.text = ((int)getValueScale(endTimer, 0, coinTime, controller.coins)).ToString();
        }

        if (endTimer > coinTotalTime)
        {
            totalCoinText.text = controller.totalCoins.ToString();
        }
        else
        {
            speedText.text = ((int)getValueScale(endTimer, 0, speedTime, controller.topMPH)).ToString();
        }

        if (endTimer > speedTime)
        {
            speedText.text = controller.topMPH.ToString();
        }
        else
        {
            totalCoinText.text = ((int)getValueScale(endTimer, 0, coinTotalTime, controller.totalCoins)).ToString();
        }

        if (endTimer > factTime)
        {
            factText.text = funFact;
        }
        else
        {
            factText.text = funFact.Substring(0, ((int)getValueScale(endTimer, 0, factTime, funFact.Length - 1)));
        }

    }

    void setTexts()
    {
        scoreTime = zoomTime;
        highScoreTime = zoomTime + 0.5f;
        coinTime = zoomTime;
        coinTotalTime = zoomTime + 0.25f;
        speedTime = zoomTime - 0.15f;
        factTime = zoomTime - 0.25f;
        funFact = controller.worldM.getFunFact();
        newScoreTime = 0.5f;
        if (controller.newHighScore)
        {
            highScoreText.color = new Color32(80, 255, 140, 255);
            scoreTime = highScoreTime;
        }
    }

    public void click()
    {
        if (!inStatic)
        {
            AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            controller.newGame();
        }
    }

    void fadeSigns(float value)
    {
        foreach (Transform t in blockSigns)
        {
            t.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)value);
            for (int i = 0; i < t.childCount; i++)
            {
                TextMeshProUGUI newText = t.GetChild(i).GetComponent<TextMeshProUGUI>();
                newText.color = new Color32((byte)(newText.color.r * 255), (byte)(newText.color.g * 255), (byte)(newText.color.b*255), (byte)value);
            }
        }
        
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float scaledTimer = getValueRanged(targetTimer, 0, targetTime);
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y);
        return new Vector3(xVal, yVal, 0) + startScale;
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
}
