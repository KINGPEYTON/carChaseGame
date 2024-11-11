using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class pauseMenu : MonoBehaviour
{
    public GameObject areYouSureToCreate;
    public GameObject settingsUI;
    public main contoller;

    public Image cover;
    public GameObject mainSign;
    public GameObject resumeSign;
    public GameObject menuSign;
    public GameObject settingsSign;

    public bool inPos;
    public float coverColor;

    public float mainTargetPos;
    public float mainCurPos;
    public float resumeTargetPos;
    public float resumeCurPos;
    public float menuTargetPos;
    public float menuCurPos;
    public float settingsTargetPos;
    public float settingsCurPos;

    public float endTime;

    public float speedTimer;
    public float coverTime;
    public float mainTime;
    public float resumeTime;
    public float menuTime;
    public float settingsTime;

    public AudioClip clickSound;

    // Use this for initialization
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();

        inPos = false;
        coverColor = 0;
        mainTargetPos = 402;
        mainCurPos = -1000.0f;
        resumeTargetPos = 324f;
        resumeCurPos = -1000.0f;
        menuTargetPos = 2404f;
        menuCurPos = 4500.0f;
        settingsTargetPos = -32f;
        settingsCurPos = -2400.0f;

        endTime = 0.35f;
        coverTime = 0.5f;
        mainTime = 0.4f;
        resumeTime = 0.65f;
        menuTime = 0.8f;
        settingsTime = 0.95f;
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
        mainSign.transform.position = new Vector3(1389, mainCurPos - getValueScale(getValueRanged(speedTimer, 0, mainTime), 0, mainTime, mainCurPos - mainTargetPos), 0);
        resumeSign.transform.position = new Vector3(resumeCurPos - getValueScale(getValueRanged(speedTimer, 0, resumeTime), 0, resumeTime, resumeCurPos - resumeTargetPos), 357.2f, 0);
        menuSign.transform.position = new Vector3(menuCurPos - getValueScale(getValueRanged(speedTimer, 0, menuTime), 0, menuTime, menuCurPos - menuTargetPos), 437.2f, 0);
        settingsSign.transform.position = new Vector3(1150, settingsCurPos - getValueScale(getValueRanged(speedTimer, 0, settingsTime), 0, settingsTime, settingsCurPos - settingsTargetPos), 0);
    }

    void exitPos()
    {

        speedTimer += Time.unscaledDeltaTime;
        cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, 175)));
        mainSign.transform.position = new Vector3(1389, mainTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, mainCurPos - mainTargetPos), 0);
        resumeSign.transform.position = new Vector3(resumeTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, resumeCurPos - resumeTargetPos), 357.2f, 0);
        menuSign.transform.position = new Vector3(menuTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, menuCurPos - menuTargetPos), 437.2f, 0);
        settingsSign.transform.position = new Vector3(1150, settingsTargetPos + getValueScale(getValueRanged(speedTimer, 0, endTime), 0, endTime, settingsCurPos - settingsTargetPos), 0);
        if(speedTimer > endTime)
        {
            exitPause();
        }
    }

    void exitPause()
    {
        Time.timeScale = 1;
        Destroy(gameObject);

        GameObject.Find("playerCar").GetComponent<playerCar>().tapped = true;
        contoller.menuSound.Play();
        if(contoller.playerCar.sndSource != null) { contoller.playerCar.sndSource.Play(); }

    }

    public void resume()
    {
        inPos = true;
        speedTimer = 0;
        playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        resumeSign.GetComponent<Button>().interactable = false;
    }

    public void menu()
    {
        youSure areYouSure = Instantiate(areYouSureToCreate, new Vector3(0,-1000,0), Quaternion.identity, gameObject.transform).GetComponent<youSure>();
        areYouSure.methodToCall = manuMethod;
        areYouSure.prevButton = menuSign.GetComponent<Button>();
        areYouSure.message = "Are you sure you want to exit to the main menu?";
        playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
    }

    public void settings()
    {
        playSound(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        contoller.settingsButton(settingsSign.GetComponent<Button>());
    }

    void manuMethod()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }

    public static void playSound(AudioClip clip, Vector3 pos, float vol)
    {
        float currTime = Time.timeScale;
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(clip, pos, vol);
        Time.timeScale = currTime;
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
