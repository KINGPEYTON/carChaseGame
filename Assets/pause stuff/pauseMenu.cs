using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class pauseMenu : MonoBehaviour
{
    public GameObject areYouSureToCreate;

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

    // Use this for initialization
    void Start()
    {
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
                coverColor += (Time.fixedDeltaTime * (speedTime/10));
            }
            if (mainSign.transform.position.y < mainTargetPos)
            {
                mainSign.transform.position = new Vector3(1389, mainCurPos, 0);
                mainCurPos += (Time.fixedDeltaTime * speedTime);

            }
            if (resumeSign.transform.position.x < resumeTargetPos)
            {
                resumeSign.transform.position = new Vector3(resumeCurPos, 357.2f, 0);
                resumeCurPos += (Time.fixedDeltaTime * speedTime);
            }
            if (menuSign.transform.position.x > menuTargetPos)
            {
                menuSign.transform.position = new Vector3(menuCurPos, 437.2f, 0);
                menuCurPos -= (Time.fixedDeltaTime * speedTime);
            }
            if (settingsSign.transform.position.y < settingsTargetPos)
            {
                settingsSign.transform.position = new Vector3(1150, settingsCurPos, 0);
                settingsCurPos += (Time.fixedDeltaTime * speedTime);

            }
        }
        else
        {
            if (coverColor > 0)
            {
                cover.color = new Color32(36, 36, 36, (byte)coverColor);
                coverColor -= (Time.fixedDeltaTime * (speedTime/6));
            }

            if (mainSign.transform.position.y > mainTargetPos)
            {
                mainSign.transform.position = new Vector3(1389, mainCurPos, 0);
                mainCurPos -= (Time.fixedDeltaTime * speedTime);
                resumeSign.transform.position = new Vector3(resumeCurPos, 357.2f, 0);
                resumeCurPos -= (Time.fixedDeltaTime * speedTime);
                menuSign.transform.position = new Vector3(menuCurPos, 437.2f, 0);
                menuCurPos += (Time.fixedDeltaTime * speedTime);
                settingsSign.transform.position = new Vector3(1150, settingsCurPos, 0);
                settingsCurPos -= (Time.fixedDeltaTime * speedTime);
            }
            else
            {
                Time.timeScale = 1;
                Destroy(gameObject);

                GameObject.Find("playerCar").GetComponent<playerCar>().tapped = true;
            }
        }
    }

    public void resume()
    {
        inPos = true;
        mainTargetPos = -500; 
    }

    public void menu()
    {
        youSure areYouSure = Instantiate(areYouSureToCreate, gameObject.transform).GetComponent<youSure>();
        areYouSure.methodToCall = manuMethod;
        areYouSure.message = "you want to exit to the main menu?";
    }

    public void settings()
    {
        
    }

    void manuMethod()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }
}
