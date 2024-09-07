using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldManager : MonoBehaviour
{
    public TextAsset worldJSON;
    public worldReader wReader;

    public List<int> exitNums;
    public List<string> stNames;

    public List<string> bridgeNames;
    public List<string> bridgeCurr;

    public List<string> funFacts;
    public List<string> funFactsCurr;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject obj = GameObject.Find("worldManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        wReader = getWorldData();
    }

    public string getBridgeName()
    {
        string s = bridgeCurr[0];
        bridgeCurr.RemoveAt(0);
        if(bridgeCurr.Count == 0)
        {
            fillStringList(bridgeCurr, bridgeNames);
        }
        return s;
    }

    public string getFunFact()
    {
        string s = funFactsCurr[0];
        funFactsCurr.RemoveAt(0);
        if (funFactsCurr.Count == 0)
        {
            fillStringList(funFactsCurr, funFacts);
        }
        return s;
    }

    private void fillStringList(List<string> fillList, List<string> takeList)
    {
        fillList.Clear();
        foreach (string s in takeList)
        {
            fillList.Add(s);
        }
        ShuffleList(fillList);
    }

    private void ShuffleList<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int idx = Random.Range(0, i);
            var temp = list[i];
            list[i] = list[idx];
            list[idx] = temp;
        }
    }

    // Update is called once per frame
    private worldReader getWorldData()
    {
        worldReader worldDataInJson = JsonUtility.FromJson<worldReader>(worldJSON.text);

        foreach (exitSignData es in worldDataInJson.exitSigns)
        {
            exitNums.Add(es.exitNum);
            stNames.Add(es.streetName);
        }
        foreach (bridgeSignData br in worldDataInJson.bridgeNames)
        {
            bridgeNames.Add(br.bridgeName);
        }
        fillStringList(bridgeCurr, bridgeNames);
        foreach (funFactData br in worldDataInJson.funFacts)
        {
            funFacts.Add(br.fact);
        }
        fillStringList(funFactsCurr, funFacts);
        return worldDataInJson;
    }
}

[System.Serializable]
public class worldReader
{
    public exitSignData[] exitSigns;
    public bridgeSignData[] bridgeNames;
    public funFactData[] funFacts; 
}

[System.Serializable]
public class exitSignData
{
    public int exitNum;
    public string streetName;
}

[System.Serializable]
public class bridgeSignData
{
    public string bridgeName;
}

[System.Serializable]
public class funFactData
{
    public string fact;
}