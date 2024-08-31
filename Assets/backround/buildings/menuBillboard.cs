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
        } else if (inZoom)
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

    public void click()
    {
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol  * controller.sfxVol);
        controller.newGame();
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
