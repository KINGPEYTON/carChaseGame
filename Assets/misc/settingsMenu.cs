using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settingsMenu : MonoBehaviour
{
    public GameObject areYouSureToCreate;
    public main contoller;
    public Button prevButton;

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

    public float endTime;
    public float coverTime;
    public float mainTime;
    public float exitTime;
    public float settingsTime;

    public Scrollbar settingsSlider;
    public float settingsSliderMultiplyer;
    public float slideTargetPos;
    public float slideCurPos;

    public bool inPos;
    public float coverColor;

    public float speedTimer;

    public AudioClip clickSound;

    public Slider masterVol;
    public Slider sfxVol;
    public Slider musicVol;

    public Button resetTutorialButton;

    public statsManager statManage;
    public GameObject statScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public Transform coinImg;
    public TextMeshProUGUI highText;
    public TextMeshProUGUI speedText;

    // Start is called before the first frame update
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();

        prevButton.interactable = false;
        inPos = false;
        coverColor = 0;

        settingsTargetPos = 1137;
        exitTargetPos = 2257;
        mainTargetPos = 450;

        settingsCurPos = -1750;
        exitCurPos = 5500;
        mainCurPos = -3500;

        endTime = 0.65f;
        coverTime = 0.5f;
        settingsTime = 0.4f;
        exitTime = 0.6f;
        mainTime = 0.8f;

        settingsSliderMultiplyer = -2000;

        masterVol.value = contoller.masterVol;
        masterVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255-getValueScale(contoller.masterVol, 0, 1, 255)), 255, 255, 255);
        sfxVol.value = contoller.sfxVol;
        sfxVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255 - getValueScale(contoller.sfxVol, 0, 1, 255)), 255, (byte)(255 - getValueScale(contoller.sfxVol, 0, 1, 64)), 255);
        musicVol.value = contoller.musicVol;
        musicVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255 - getValueScale(contoller.musicVol, 0, 1, 255)), (byte)(255 - getValueScale(contoller.musicVol, 0, 1, 64)), 255, 255);

        setStats();

        if (contoller.inTutorial)
        {
            resetTutorialButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            getInPos();
        }
        else
        {
            exitPos();
        }
    }

    void getInPos()
    {
        speedTimer += Time.unscaledDeltaTime;
        cover.color = new Color32(36, 36, 36, (byte)getValueScale(getValueRanged(speedTimer, 0, coverTime), 0, coverTime, 175));
        scrollableSigns.transform.position = new Vector3(1389, mainCurPos - getValueScale(getValueRanged(speedTimer, 0, mainTime), 0, mainTime, mainCurPos - mainTargetPos), 0);
        exitSign.transform.position = new Vector3(exitCurPos - getValueScale(getValueRanged(speedTimer, 0, exitTime), 0, exitTime, exitCurPos - exitTargetPos), 450, 0);
        settingsSign.transform.position = new Vector3(settingsCurPos - getValueScale(getValueRanged(speedTimer, 0, settingsTime), 0, settingsTime, settingsCurPos - settingsTargetPos), 475, 0);
    }

    void exitPos()
    {
        speedTimer += Time.unscaledDeltaTime;
        cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, 175)));
        scrollableSigns.transform.position = new Vector3(1389, mainTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, mainCurPos - mainTargetPos), 0);
        exitSign.transform.position = new Vector3(exitTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, exitCurPos - exitTargetPos), 450, 0);
        settingsSign.transform.position = new Vector3(settingsTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, settingsCurPos - settingsTargetPos), 475, 0);
        if (speedTimer > endTime)
        {
            exitSettings();
        }
    }

    void exitSettings()
    {
        Destroy(gameObject);
    }
    

    public void exit()
    {
        inPos = true;
        speedTimer = 0;
        settingsCurPos = -1750;
        exitCurPos = 4000;
        mainCurPos = -1750;
        pauseMenu.playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol);
        settingsSlider.gameObject.SetActive(false);
        prevButton.interactable = true;
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
            masterVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255 - getValueScale(volBar.value, 0, 1, 255)), 255, 255, 255);
        }
        else if(volBar == sfxVol)
        {
            contoller.changeSfxVol(volBar.value);
            sfxVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255 - getValueScale(volBar.value, 0, 1, 255)), 255, (byte)(255 - getValueScale(volBar.value, 0, 1, 64)), 255);
        }
        else if(volBar == musicVol)
        {
            contoller.changeMusicVol(volBar.value);
            musicVol.handleRect.GetComponent<Image>().color = new Color32((byte)(255 - getValueScale(volBar.value, 0, 1, 255)), (byte)(255 - getValueScale(volBar.value, 0, 1, 64)), 255, 255);
        } else
        {
            Debug.Log("You Fucked Up Bud");
        }
    }

    public void resetTutorial()
    {
        youSure areYouSure = Instantiate(areYouSureToCreate, new Vector3(0, -1000, 0), Quaternion.identity, gameObject.transform).GetComponent<youSure>();
        areYouSure.methodToCall = newTutorial;
        areYouSure.prevButton = resetTutorialButton.GetComponent<Button>();
        areYouSure.message = "Are you sure you want to reset the tutorial. This will exit to the main menu?";
        areYouSure.displayMessage.fontSize = 35;
        pauseMenu.playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
    }

    private void newTutorial()
    {
        PlayerPrefs.SetInt("tutorialStep", 0);
        contoller.pwManage.resetTutorial();

        contoller.newGame();
    }

    void setStats()
    {
        statManage = GameObject.Find("statsManager").GetComponent<statsManager>();

        scoreText.text = "Total Distance:\n" + statManage.pstats.scoreTotal + "m";
        coinText.text = "Total Coins:\n" + statManage.pstats.coins;
        coinImg.localPosition = new Vector3(37.0f - (statManage.pstats.coins.ToString().Length * 2.35f), 14.75f, 0);
        highText.text = "Highest Score:\n" + statManage.pstats.highScore + "m";
        speedText.text = "Top Speed:\n" + statManage.pstats.topSpeed + " MPH";
    }

    public void statMenu()
    {
        pauseMenu.playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        Instantiate(statScreen);
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
