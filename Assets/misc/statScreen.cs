using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class statScreen : MonoBehaviour
{
    public main contoller;
    public statsManager statManage;

    public Image cover;
    public GameObject exitSign;
    public GameObject menuSign;

    public bool inPos;
    public bool inEnd;

    public float coverTimer;
    public float coverTime;

    public float statTimer;
    public float statTime;

    public float exitTimer;
    public float exitTime;

    public Transform scrollField;

    public TextMeshProUGUI gamesText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI longestText;
    public TextMeshProUGUI avgTimeText;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public Transform coinImg;
    public TextMeshProUGUI highText;
    public TextMeshProUGUI coinMostText;
    public Transform coinMostImg;
    public TextMeshProUGUI avgScoreText;
    public TextMeshProUGUI coinAvgText;
    public Transform coinAvgImg;
    public TextMeshProUGUI brokeText;
    public TextMeshProUGUI speedText;

    public TextMeshProUGUI turnsText;
    public TextMeshProUGUI closeText;
    public TextMeshProUGUI turnsMostText;
    public TextMeshProUGUI closeMostText;

    public TextMeshProUGUI disableText;
    public TextMeshProUGUI destroyText;
    public TextMeshProUGUI disableMostText;
    public TextMeshProUGUI destroyMostText;

    public TextMeshProUGUI lane1Text;
    public TextMeshProUGUI lane2Text;
    public TextMeshProUGUI lane3Text;
    public TextMeshProUGUI lane4Text;
    public TextMeshProUGUI lane5Text;
    public TextMeshProUGUI laneMostText;
    public TextMeshProUGUI laneSingleText;
    public TextMeshProUGUI laneSingleMostText;

    public TextMeshProUGUI powerUpText;
    public TextMeshProUGUI powerupMostText;

    public GameObject breakRow;
    public GameObject powerupSlots;
    public GameObject modSlots;

    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();
        statManage = GameObject.Find("statsManager").GetComponent<statsManager>();

        inPos = false;

        coverTime = 0.35f;
        statTime = 0.65f;
        exitTime = 0.55f;

        setStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (inPos)
        {
            if (inEnd)
            {
                exitPos();
            }
        }
        else
        {
            enterPos();
        }
    }

    void exitPos()
    {
        if (coverTimer < coverTime)
        {
            cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(coverTimer, 0, coverTime, 175)));
            menuSign.transform.localPosition = new Vector3(250, -220 - getValueScale(coverTimer, 0, coverTime, 1300), 0);
            exitSign.transform.localPosition = new Vector3(-1150 - getValueScale(coverTimer, 0, coverTime, 800), -350, 0);
            coverTimer += Time.unscaledDeltaTime;
            if (coverTimer > coverTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void enterPos()
    {
        if (coverTimer < coverTime)
        {
            cover.color = new Color32(36, 36, 36, (byte)getValueScale(coverTimer, 0, coverTime, 175));
            coverTimer += Time.unscaledDeltaTime;
            if (coverTimer > coverTime)
            {
                cover.color = new Color32(36, 36, 36, 175);
            }
        }
        if (statTimer < statTime)
        {
            menuSign.transform.localPosition = new Vector3(250, -1520 + getValueScale(statTimer, 0, statTime, 1300), 0);
            statTimer += Time.unscaledDeltaTime;
            if (statTimer > statTime)
            {
                menuSign.transform.localPosition = new Vector3(250, -220, 0);
                inPos = true;
            }
        }
        if (exitTimer < exitTime)
        {
            exitSign.transform.localPosition = new Vector3(-1950 + getValueScale(exitTimer, 0, exitTime, 800), -350, 0);
            exitTimer += Time.unscaledDeltaTime;
            if (exitTimer > statTime)
            {
                exitSign.transform.localPosition = new Vector3(-1150, -350, 0);
            }
        }
    }

    void setStats()
    {
        gamesText.text = "Games Played:\n" + statManage.pstats.games;
        timeText.text = "Total Time Played:\n" + showTime((int)(statManage.pstats.totalTime));
        longestText.text = "Longest Game:\n" + ((int)(statManage.pstats.longestGame * 100) / 100.0f) + "s";
        avgTimeText.text = "Average Game:\n" + ((int)(statManage.pstats.avgTime * 100) / 100.0f) + "s";

        scoreText.text = "Total Distance:\n" + statManage.pstats.scoreTotal + "m";
        coinText.text = "Total Coins:\n" + statManage.pstats.coins;
        coinImg.localPosition = new Vector3(34.5f - (statManage.pstats.coins.ToString().Length * 7.15f), 0, 0);
        avgScoreText.text = "Average Score:\n" + statManage.pstats.scoreAvg + "m";
        coinAvgText.text = "Average Coins:\n" + statManage.pstats.avgCoins;
        coinAvgImg.localPosition = new Vector3(34.5f - (statManage.pstats.avgCoins.ToString().Length * 7.15f), 0, 0);
        highText.text = "Highest Score:\n" + statManage.pstats.highScore + "m";
        coinMostText.text = "Most Coins:\n" + statManage.pstats.coinsInGame;
        coinMostImg.localPosition = new Vector3(34.5f - (statManage.pstats.coinsInGame.ToString().Length * 7.15f), 0, 0);
        brokeText.text = "Times Broke:\n" + statManage.pstats.highScoreBroke;
        speedText.text = "Top Speed:\n" + statManage.pstats.topSpeed + " MPH";

        turnsText.text = "Turns:\n" + statManage.pstats.turns;
        closeText.text = "Close Calls:\n" + statManage.pstats.closeHits;
        turnsMostText.text = "Single Game:\n" + statManage.pstats.turnsInGame;
        closeMostText.text = "Single Game:\n" + statManage.pstats.closeHitsInGame;

        disableText.text = "Cars Disabled:\n" + statManage.pstats.carsDisabled;
        destroyText.text = "Cars Destroyed:\n" + statManage.pstats.carsDestroyed;
        disableMostText.text = "Single Game:\n" + statManage.pstats.carsDisabledInGame;
        destroyMostText.text = "Single Game:\n" + statManage.pstats.carsDestroyedInGame;

        disableText.text = "Cars Disabled:\n" + statManage.pstats.carsDisabled;
        destroyText.text = "Cars Destroyed:\n" + statManage.pstats.carsDestroyed;
        disableMostText.text = "Single Game:\n" + statManage.pstats.carsDisabledInGame;
        destroyMostText.text = "Single Game:\n" + statManage.pstats.carsDestroyedInGame;

        laneMostText.text = "Lane Most Used:" + (int)(statManage.pstats.mostUsedLane);
        lane1Text.text = "Time in Lane 1: " + (int)(statManage.pstats.lane1Time) + "s";
        lane2Text.text = "Time in Lane 2: " + (int)(statManage.pstats.lane2Time) + "s";
        lane3Text.text = "Time in Lane 3: " + (int)(statManage.pstats.lane3Time) + "s";
        lane4Text.text = "Time in Lane 4: " + (int)(statManage.pstats.lane4Time) + "s";
        lane5Text.text = "Time in Lane 5: " + (int)(statManage.pstats.lane5Time) + "s";
        laneSingleMostText.text = "Lane:\n" + (int)(statManage.pstats.mostUsedLaneSingle);
        laneSingleText.text = "Time\n" + (int)(statManage.pstats.mostUsedLaneSingleTime) + "s";

        powerUpText.text = "Powerups:\n" + statManage.pstats.pwCollectedTotal;
        powerupMostText.text = "In Single Game:\n" + statManage.pstats.pwCollectedTotalInGame;

        powerUpManager pwManage = GameObject.Find("powerUpManager").GetComponent<powerUpManager>();
        int pwButons = (pwManage.powerupIDs.Count + 1) / 2;

        for (int i = 0; i < pwButons; i++)
        {
            Transform pwR = Instantiate(powerupSlots, scrollField).transform;
            pwR.Find("powerup 1").Find("amount").GetComponent<TextMeshProUGUI>().text = statManage.pstats.pwCollected[i * 2].ToString();
            pwR.Find("powerup 1").Find("icon").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[i * 2]][pwManage.tiers[i * 2]];
            pwR.Find("powerup 2").Find("amount").GetComponent<TextMeshProUGUI>().text = statManage.pstats.pwCollected[(i * 2) + 1].ToString();
            pwR.Find("powerup 2").Find("icon").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[(i * 2) + 1]][pwManage.tiers[(i * 2) + 1]];
        }
        Instantiate(breakRow, scrollField);

        boostManager modMang = GameObject.Find("modsManager").GetComponent<boostManager>();
        Transform mR = Instantiate(modSlots, scrollField).transform;
        mR.Find("Mod Text").GetComponent<TextMeshProUGUI>().text = "Mods Used: " + statManage.pstats.modsUsed;
        mR.Find("most used").Find("amount").GetComponent<TextMeshProUGUI>().text = "Most Used: ";
        mR.Find("most used").Find("icon").GetComponent<Image>().sprite = modMang.icons[modMang.modIDs.IndexOf(statManage.pstats.modsMost)];

        int modButons = (modMang.modIDs.Count + 1) / 2;
        for (int i = 1; i < modButons; i++)
        {
            Transform pwR = Instantiate(powerupSlots, scrollField).transform;
            pwR.Find("powerup 1").Find("amount").GetComponent<TextMeshProUGUI>().text = statManage.pstats.mods[(i * 2) - 1].ToString();
            pwR.Find("powerup 1").Find("icon").GetComponent<Image>().sprite = modMang.icons[(i * 2) - 1];
            pwR.Find("powerup 2").Find("amount").GetComponent<TextMeshProUGUI>().text = statManage.pstats.mods[i * 2].ToString();
            pwR.Find("powerup 2").Find("icon").GetComponent<Image>().sprite = modMang.icons[i * 2];
        }
    }

    string showTime(int timeUsed)
    {
        string hour = "";
        if (timeUsed > 3600)
        {
            hour = ((timeUsed % 3600)/60) + "h ";
        }
        string min = "";
        if (timeUsed > 60)
        {
            min = (timeUsed % 60) + "m ";
        }
        string sec = (timeUsed % 60) + "s";
        return hour + min + sec;
    }

    public void exit()
    {
        coverTimer = 0;
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        inEnd = true;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
