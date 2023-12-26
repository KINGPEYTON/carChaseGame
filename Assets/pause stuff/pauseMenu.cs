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

    public float speedTime;

    public AudioClip clickSound;

    // Use this for initialization
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();

        inPos = false;
        coverColor = 0;
        mainTargetPos = 642f;
        mainCurPos = -400.0f;
        resumeTargetPos = 364f;
        resumeCurPos = -1000.0f;
        menuTargetPos = 2404f;
        menuCurPos = 4500.0f;
        settingsTargetPos = 92f;
        settingsCurPos = -2400.0f;

        speedTime = 5000f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            if (coverColor < 175)
            {
                //print(coverColor);
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
                coverColor += (Time.unscaledDeltaTime * (speedTime/10));
            } else if (coverColor > 175)
            {
                coverColor = 175;
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
            }
            if (mainSign.transform.position.y < mainTargetPos)
            {
                mainSign.transform.position = new Vector3(1389, mainCurPos, 0);
                mainCurPos += (Time.unscaledDeltaTime * speedTime);
            }
            else if (mainSign.transform.position.y > mainTargetPos)
            {
                mainCurPos = 642;
                mainSign.transform.position = new Vector3(1389, mainCurPos, 0);
            }
            if (resumeSign.transform.position.x < resumeTargetPos)
            {
                resumeSign.transform.position = new Vector3(resumeCurPos, 357.2f, 0);
                resumeCurPos += (Time.unscaledDeltaTime * speedTime);
            }
            else if (resumeSign.transform.position.x > resumeTargetPos)
            {
                resumeCurPos = 364;
                resumeSign.transform.position = new Vector3(resumeCurPos, 357.2f, 0);
            }
            if (menuSign.transform.position.x > menuTargetPos)
            {
                menuSign.transform.position = new Vector3(menuCurPos, 437.2f, 0);
                menuCurPos -= (Time.unscaledDeltaTime * speedTime);
            }
            else if (menuSign.transform.position.x < menuTargetPos)
            {
                menuCurPos = 2404;
                menuSign.transform.position = new Vector3(menuCurPos, 437.2f, 0);
            }
            if (settingsSign.transform.position.y < settingsTargetPos)
            {
                settingsSign.transform.position = new Vector3(1150, settingsCurPos, 0);
                settingsCurPos += (Time.unscaledDeltaTime * speedTime);
            }
            else if (settingsSign.transform.position.y > settingsTargetPos)
            {
                settingsCurPos = 92;
                settingsSign.transform.position = new Vector3(1150, settingsCurPos, 0);
            }
        }
        else
        {
            if (coverColor > 0)
            {
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
                coverColor -= (Time.unscaledDeltaTime * (speedTime/6));
            }

            if (mainSign.transform.position.y > mainTargetPos)
            {
                mainSign.transform.position = new Vector3(1389, mainCurPos, 0);
                mainCurPos -= (Time.unscaledDeltaTime * speedTime);
                resumeSign.transform.position = new Vector3(resumeCurPos, 357.2f, 0);
                resumeCurPos -= (Time.unscaledDeltaTime * speedTime);
                menuSign.transform.position = new Vector3(menuCurPos, 437.2f, 0);
                menuCurPos += (Time.unscaledDeltaTime * speedTime);
                settingsSign.transform.position = new Vector3(1150, settingsCurPos, 0);
                settingsCurPos -= (Time.unscaledDeltaTime * speedTime);
            }
            else
            {
                Time.timeScale = 1;
                Destroy(gameObject);

                GameObject.Find("playerCar").GetComponent<playerCar>().tapped = true;
                contoller.menuSound.Play();
            }
        }
    }

    public void resume()
    {
        inPos = true;
        mainTargetPos = -500;
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
    }

    public void menu()
    {
        youSure areYouSure = Instantiate(areYouSureToCreate, gameObject.transform).GetComponent<youSure>();
        areYouSure.methodToCall = manuMethod;
        areYouSure.message = "Are you sure you want to exit to the main menu?";
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
    }

    public void settings()
    {
        Instantiate(settingsUI);
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
    }

    void manuMethod()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }
}
