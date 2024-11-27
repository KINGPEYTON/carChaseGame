using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    public playerStats pstats = new playerStats();

    // Start is called before the first frame update
    void Awake()
    {
        GameObject obj = GameObject.Find("statsManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            if (JsonDataService.fileExist<playerStats>("/playerStats.json", pstats))
            {
                loadStats();
            }
            else
            {
                getStats();
                saveStats();
            }
        }
    }

    public void setCoins(int ammount)
    {
        pstats.currCoins = ammount;
        saveStats();
    }

    void getStats()
    {
        pstats.currCoins = 0;
        pstats.games = 0;

        pstats.highScore = 0;
        pstats.highScoreBroke = 0;
        pstats.scoreTotal = 0;
        pstats.scoreAvg = 0;

        pstats.totalTime = 0;
        pstats.longestGame = 0;
        pstats.avgTime = 0;

        pstats.lane1Time = 0;
        pstats.lane2Time = 0;
        pstats.lane3Time = 0;
        pstats.lane4Time = 0;
        pstats.lane5Time = 0;
        updateMostUsedLane();
        pstats.mostUsedLaneSingleTime = 0;
        pstats.mostUsedLaneSingle = 0;

        pstats.turns = 0;
        pstats.turnsInGame = 0;

        pstats.topSpeed = 0;

        pstats.coins = 0;
        pstats.coinsInGame = 0;
        pstats.avgCoins = 0;

        pstats.closeHits = 0;
        pstats.closeHitsInGame = 0;

        pstats.carsDisabled = 0;
        pstats.carsDisabledInGame = 0;
        pstats.carsDestroyed = 0;
        pstats.carsDestroyedInGame = 0;

        pstats.pwIDs = GameObject.Find("powerUpManager").GetComponent<powerUpManager>().powerupIDs;
        pstats.pwCollectedTotal = 0;
        pstats.pwCollectedTotalInGame = 0;
        for (int i = 0; i < pstats.pwIDs.Count; i++)
        {
            pstats.pwCollected.Add(0);
            pstats.pwCollectedInGame.Add(0);
        }
        pstats.pwMostCollected = "None";

        pstats.modIDs = GameObject.Find("modsManager").GetComponent<boostManager>().modIDs;
        pstats.modsUsed = 0;
        for (int i = 0; i < pstats.modIDs.Count; i++){
            pstats.mods.Add(0);
        }
        pstats.modsMost = "None";
    }

    public void loadStats()
    {
        pstats = JsonDataService.LoadData<playerStats>("/playerStats.json", true);
    }
    public void saveStats()
    {
        JsonDataService.SaveData("/playerStats.json", pstats, true);
    }

    public void addStats(int score, float time, int coinsGained, float[] laneTimes, int carTurns, int topMPH, int closeCalls, int carsDis, int carsDest, int pw, int[] pwList, int mod)
    {
        pstats.currCoins += coinsGained;

        pstats.games++;

        pstats.scoreTotal += score;
        pstats.scoreAvg = (pstats.scoreTotal) / pstats.games;
        if (score > pstats.highScore)
        {
            pstats.highScore = score;
            pstats.highScoreBroke++;
        }

        pstats.totalTime += time;
        pstats.avgTime = (pstats.totalTime) / pstats.games;
        if (time > pstats.longestGame)
        {
            pstats.longestGame = time;
        }

        pstats.lane1Time += laneTimes[0];
        pstats.lane2Time += laneTimes[1];
        pstats.lane3Time += laneTimes[2];
        pstats.lane4Time += laneTimes[3];
        pstats.lane5Time += laneTimes[4];
        updateMostUsedLane();
        for(int i = 0; i < laneTimes.Length; i++)
        {
            if(laneTimes[i] > pstats.mostUsedLaneSingleTime)
            {
                pstats.mostUsedLaneSingleTime = laneTimes[i];
                pstats.mostUsedLaneSingle = i+1;
            }
        }

        pstats.turns += carTurns;
        if (carTurns > pstats.turnsInGame)
        {
            pstats.turnsInGame = carTurns;
        }
        if (topMPH > pstats.topSpeed)
        {
            pstats.topSpeed = topMPH;
        }

        pstats.coins += coinsGained;
        pstats.avgCoins = (pstats.coins) / (float)pstats.games;
        if (coinsGained > pstats.coinsInGame)
        {
            pstats.coinsInGame = coinsGained;
        }

        pstats.closeHits += closeCalls;
        if (closeCalls > pstats.closeHitsInGame)
        {
            pstats.closeHitsInGame = closeCalls;
        }

        pstats.carsDisabled += carsDis;
        pstats.carsDestroyed += carsDest;
        if (carsDis > pstats.carsDisabledInGame)
        {
            pstats.carsDisabledInGame = carsDis;
        }
        if (carsDest > pstats.carsDestroyedInGame)
        {
            pstats.carsDestroyedInGame = carsDest;
        }

        pstats.pwCollectedTotal += pw;
        if(pw > pstats.pwCollectedTotalInGame)
        {
            pstats.pwCollectedTotalInGame = pw;
        }
        for (int i = 0; i < pwList.Length; i++)
        {
            pstats.pwCollected[i] += pwList[i];
            if (pwList[i] > pstats.pwCollectedInGame[i])
            {
                pstats.pwCollectedInGame[i] = pwList[i];
                if(pstats.pwMostCollected == "None") { pstats.pwMostCollected = pstats.pwIDs[i]; }
                if (pwList[i] > pstats.pwCollected[pstats.pwIDs.IndexOf(pstats.pwMostCollected)])
                {
                    pstats.pwMostCollected = pstats.pwIDs[i];
                }
            }
        }

        if (mod > 0)
        {
            pstats.modsUsed++;
            pstats.mods[mod]++;
            if(pstats.mods[mod] > pstats.mods[pstats.modIDs.IndexOf(pstats.modsMost)])
            {
                pstats.modsMost = pstats.modIDs[mod];
            }
        }

        saveStats();
    }

    void updateMostUsedLane()
    {
        float hey = 0;
        if (pstats.lane1Time > hey)
        {
            hey = pstats.lane1Time;
            pstats.mostUsedLane = 1;
        }
        if (pstats.lane2Time > hey)
        {
            hey = pstats.lane2Time;
            pstats.mostUsedLane = 2;
        }
        if (pstats.lane3Time > hey)
        {
            hey = pstats.lane3Time;
            pstats.mostUsedLane = 3;
        }
        if (pstats.lane4Time > hey)
        {
            hey = pstats.lane4Time;
            pstats.mostUsedLane = 4;
        }
        if (pstats.lane5Time > hey)
        {
            hey = pstats.lane5Time;
            pstats.mostUsedLane = 5;
        }
    }
}

[System.Serializable]
public class playerStats
{
    public int games;
    public int highScore;
    public int highScoreBroke;
    public float scoreTotal;
    public float scoreAvg;

    public float totalTime;
    public float avgTime;
    public float longestGame;

    public float lane1Time;
    public float lane2Time;
    public float lane3Time;
    public float lane4Time;
    public float lane5Time;
    public int mostUsedLane;
    public int mostUsedLaneSingle;
    public float mostUsedLaneSingleTime;

    public int turns;
    public int turnsInGame;

    public int currCoins;

    public int coins;
    public int coinsInGame;
    public float avgCoins;

    public int topSpeed;

    public int closeHits;
    public int closeHitsInGame;

    public int carsDisabled;
    public int carsDestroyed;
    public int carsDisabledInGame;
    public int carsDestroyedInGame;

    public List<string> pwIDs;
    public int pwCollectedTotal;
    public int pwCollectedTotalInGame;
    public List<int> pwCollected;
    public List<int> pwCollectedInGame;
    public string pwMostCollected;

    public List<string> modIDs;
    public int modsUsed;
    public List<int> mods;
    public string modsMost;
}
