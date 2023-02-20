using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsMenu : MonoBehaviour
{
    public GameObject areYouSureToCreate;
    public main contoller;

    public Image cover;
    public GameObject scrollableSigns;
    public GameObject exitSign;
    public GameObject settingsSign;

    public float mainTargetPos;
    public float mainCurPos;
    public float exitTargetPos;
    public float exitCurPos;
    public float settingsTargetPos;
    public float settingsCurPos;

    public Scrollbar settingsSlider;
    public float settingsSliderMultiplyer;
    public float slideTargetPos;
    public float slideCurPos;

    public bool inPos;
    public float coverColor;

    public float speedTime;

    public AudioClip clickSound;

    public Slider masterVol;
    public Slider sfxVol;
    public Slider musicVol;

    // Start is called before the first frame update
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();

        inPos = false;
        coverColor = 0;

        speedTime = 5000f;

        settingsTargetPos = 1137;
        exitTargetPos = 2257;
        mainTargetPos = 542;

        settingsCurPos = -1750;
        exitCurPos = 5500;
        mainCurPos = -3500;

        settingsSliderMultiplyer = -2000;

        masterVol.value = contoller.masterVol;
        sfxVol.value = contoller.sfxVol;
        musicVol.value = contoller.musicVol;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            if (coverColor < 175)
            {
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
                coverColor += (Time.unscaledDeltaTime * (speedTime / 20));
            }
            else if (coverColor > 175)
            {
                coverColor = 175;
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
            }
            if (scrollableSigns.transform.position.y < mainTargetPos)
            {
                scrollableSigns.transform.position = new Vector3(1389 + slideCurPos, mainCurPos, 0);
                settingsSlider.gameObject.transform.position = new Vector3(1389, mainCurPos - 575, 0);
                mainCurPos += (Time.unscaledDeltaTime * speedTime);
            }
            else if (scrollableSigns.transform.position.y > mainTargetPos)
            {
                mainCurPos = 542;
                scrollableSigns.transform.position = new Vector3(1389 + slideCurPos, mainCurPos, 0);
                settingsSlider.gameObject.transform.position = new Vector3(1389, mainCurPos-475, 0);
            }
            if (exitSign.transform.position.x > exitTargetPos)
            {
                exitSign.transform.position = new Vector3(exitCurPos, 670, 0);
                exitCurPos -= (Time.unscaledDeltaTime * speedTime);
            }
            else if (exitSign.transform.position.x < exitTargetPos)
            {
                exitCurPos = 2257;
                exitSign.transform.position = new Vector3(exitCurPos, 670, 0);
            }
            if (settingsSign.transform.position.x < settingsTargetPos)
            {
                settingsSign.transform.position = new Vector3(settingsCurPos, 672f, 0);
                settingsCurPos += (Time.unscaledDeltaTime * speedTime);
            }
            else if (settingsSign.transform.position.x < settingsTargetPos)
            {
                settingsCurPos = 1137;
                settingsSign.transform.position = new Vector3(settingsCurPos, 672, 0);
            }


            if (slideCurPos > slideTargetPos)
            {
                scrollableSigns.transform.position = new Vector3(1389 + slideCurPos, mainCurPos, 0); ;
                slideCurPos -= (Time.unscaledDeltaTime * (speedTime/100));
                if(Mathf.Abs(slideCurPos - slideTargetPos) < 500)
                {
                    slideCurPos = slideTargetPos;
                }
            } else if (slideCurPos < slideTargetPos)
            {
                scrollableSigns.transform.position = new Vector3(1389 + slideCurPos, mainCurPos, 0); ;
                slideCurPos += (Time.unscaledDeltaTime * (speedTime/100));
                if (Mathf.Abs(slideCurPos - slideTargetPos) < 500)
                {
                    slideCurPos = slideTargetPos;
                }
            }
        }
        else
        {
            if (coverColor > 0)
            {
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
                coverColor -= (Time.unscaledDeltaTime * (speedTime / 10));
            }
            else
            {
                cover.color = new Color32(36, 36, 36, 0);
            }

            if (settingsSign.transform.position.x > settingsTargetPos)
            {
                scrollableSigns.transform.position = new Vector3(1389 + slideCurPos, mainCurPos, 0);
                settingsSlider.gameObject.transform.position = new Vector3(1389, mainCurPos - 575, 0);
                mainCurPos -= (Time.unscaledDeltaTime * speedTime);
                exitSign.transform.position = new Vector3(exitCurPos, 670, 0);
                exitCurPos += (Time.unscaledDeltaTime * speedTime);
                settingsSign.transform.position = new Vector3(settingsCurPos, 672, 0);
                settingsCurPos -= (Time.unscaledDeltaTime * speedTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        musicVol.value = contoller.musicVol;
    }

    public void exit()
    {
        inPos = true;
        settingsTargetPos = -600;
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol);
        settingsSlider.gameObject.SetActive(false);
    }

    public void slideSettings()
    {
        slideTargetPos = settingsSlider.value * settingsSliderMultiplyer;
    }

    public void changeVol(Slider volBar)
    {
        if (volBar == masterVol)
        {
            contoller.changeMasterVol(volBar.value);
        }
        else if(volBar == sfxVol)
        {
            contoller.changeSfxVol(volBar.value);
        }
        else if(volBar == musicVol)
        {
            contoller.changeMusicVol(volBar.value);
        } else
        {
            Debug.Log("You Fucked Up Bud");
        }
    }
}
