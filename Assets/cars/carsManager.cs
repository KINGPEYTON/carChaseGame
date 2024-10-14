using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carsManager : MonoBehaviour
{
    public TextAsset carsJSON;
    public carsReader carReader;

    public List<List<string>> idNames = new List<List<string>>();
    public List<List<float>> oddsRaw = new List<List<float>>();
    public List<int> blinkTime;
    public List<float> speedMin;
    public List<float> speedMax;
    public List<float> forceMass;
    public List<bool> isCar;
    public List<int> hitPoint;

    public List<List<GameObject>> carsOBJs = new List<List<GameObject>>();

    public List<List<float>> carOdds = new List<List<float>>();
    public List<List<float>> carOddsCurr = new List<List<float>>();
    private int lastSelID;

    void Awake()
    {
        GameObject obj = GameObject.Find("carsManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        carReader = readCarsJSON();
        addCarOBJ();
        setCarOdds();
    }

    public cars spawnRegularCar(Vector3 pos, Transform par)
    {
        cars ca = spawnCar(0, pos, par).GetComponent<cars>();

        setCarStuff(ca, lastSelID);
        return ca;
    }

    public cars spawnLargeCar(Vector3 pos, Transform par)
    {
        cars ca = spawnCar(1, pos, par).GetComponent<cars>();

        setCarStuff(ca, lastSelID + carsOBJs[0].Count);
        return ca;
    }

    public cars spawnSpecialCar(Vector3 pos, Transform par)
    {
        cars ca = spawnCar(2, pos, par).GetComponent<cars>();

        setCarStuff(ca, lastSelID + carsOBJs[0].Count + carsOBJs[1].Count);
        return ca; 
    }

    public cars spawnPoliceCar(Vector3 pos, Transform par)
    {
        cars ca = Instantiate(carsOBJs[2][0], pos, Quaternion.identity, par).GetComponent<cars>();

        setCarStuff(ca, 0 + carsOBJs[0].Count + carsOBJs[1].Count);
        return ca;
    }

    private GameObject spawnCar(int typeID, Vector3 pos, Transform par)
    {
        return Instantiate(getCarFromOdds(carOdds[typeID], carOddsCurr[typeID], carsOBJs[typeID]), pos, Quaternion.identity, par);  //spawn new car in a random lane before going on screen
    }

    private void setCarStuff(cars obj, int id)
    {
        obj.blinkTime = blinkTime[id];
        obj.speedMin = speedMin[id];
        obj.speedMax = speedMax[id];
        obj.forceMass = forceMass[id];
        obj.isCar = isCar[id];
        obj.hitPoint = hitPoint[id];
    }

    private void setCarOdds()
    {
        for(int i = 0; i < 3; i++)
        {
            carOdds.Add(new List<float>());
            carOddsCurr.Add(new List<float>());
            setCarTypeOdds(i);
        }
    }

    private void setCarTypeOdds(int type)
    {
        float oddsTotal = 0;
        float oddsNum = 0;
        for (int i = 0; i < oddsRaw[type].Count; i++)
        {
            oddsTotal += oddsRaw[type][i];
            oddsNum++;
        }
        float multi = oddsNum / oddsTotal;
        float oddsMulti = multi / oddsNum;

        for (int i = 0; i < oddsRaw[type].Count; i++)
        {
            carOdds[type].Add(oddsRaw[type][i] * oddsMulti);
            carOddsCurr[type].Add(oddsRaw[type][i] * oddsMulti);
        }
    }

    private GameObject getCarFromOdds(List<float> oddsList, List<float> currOddsList, List<GameObject> carList)
    {
        float newCarOdds = Random.Range(0.0f, 1.0f);
        float oddsAccum = 0.0f;

        for (int i = 0; i < oddsList.Count; i++)
        {
            oddsAccum += currOddsList[i];
            if (oddsAccum > newCarOdds)
            {
                lastSelID = i;
                changeOdds(i, oddsList, currOddsList);
                return carList[i];
            }
        }

        changeOdds(0, oddsList, currOddsList);
        return carList[0];
    }

    private void changeOdds(int index, List<float> setList, List<float> currentList)
    {
        float diff = currentList[index] - (setList[index] / 2); //the odds that are being taken away from use
        float listDiff = (diff * setList[index]) / (setList.Count - 1.0f); //the gap of what would of been added when changing odds
        for (int i = 0; i < setList.Count; i++)
        {
            currentList[i] += (diff * setList[i]) + listDiff; //this took me way to fucking long to figue out
        }

        currentList[index] = setList[index] / 2;
    }

    private carsReader readCarsJSON()
    {
        carsReader carDataInJson = JsonUtility.FromJson<carsReader>(carsJSON.text);

        List<string> normalNameList = new List<string>();
        List<float> normalRawOdds = new List<float>();
        foreach (carData ca in carDataInJson.normal)
        {
            normalNameList.Add(ca.idName);
            normalRawOdds.Add(ca.odds);
            blinkTime.Add(ca.blinkTime);
            speedMin.Add(ca.speedMin);
            speedMax.Add(ca.speedMax);
            forceMass.Add(ca.forceMass);
            isCar.Add(ca.isCar);
            hitPoint.Add(ca.hitPoint);
        }
        List<string> largelNameList = new List<string>();
        List<float> largeRawOdds = new List<float>();
        foreach (carData ca in carDataInJson.large)
        {
            largelNameList.Add(ca.idName);
            largeRawOdds.Add(ca.odds);
            blinkTime.Add(ca.blinkTime);
            speedMin.Add(ca.speedMin);
            speedMax.Add(ca.speedMax);
            forceMass.Add(ca.forceMass);
            isCar.Add(ca.isCar);
            hitPoint.Add(ca.hitPoint);
        }
        List<string> specialNameList = new List<string>();
        List<float> specialRawOdds = new List<float>();
        foreach (carData ca in carDataInJson.special)
        {
            specialNameList.Add(ca.idName);
            specialRawOdds.Add(ca.odds);
            blinkTime.Add(ca.blinkTime);
            speedMin.Add(ca.speedMin);
            speedMax.Add(ca.speedMax);
            forceMass.Add(ca.forceMass);
            isCar.Add(ca.isCar);
            hitPoint.Add(ca.hitPoint);
        }
        idNames.Add(normalNameList);
        idNames.Add(largelNameList);
        idNames.Add(specialNameList);
        oddsRaw.Add(normalRawOdds);
        oddsRaw.Add(largeRawOdds);
        oddsRaw.Add(specialRawOdds);

        return carDataInJson;
    }

    private void addCarOBJ()
    {
        List<GameObject> normalCars = new List<GameObject>();
        List<GameObject> largeCars = new List<GameObject>();
        List<GameObject> specialCars = new List<GameObject>();
        for (int i = 0; i < idNames[0].Count; i++)
        {
            addCar(0, i, normalCars);
        }
        for (int i = 0; i < idNames[1].Count; i++)
        {
            addCar(1, i, largeCars);
        }
        for (int i = 0; i < idNames[2].Count; i++)
        {
            addCar(2, i, specialCars);
        }
        carsOBJs.Add(normalCars);
        carsOBJs.Add(largeCars);
        carsOBJs.Add(specialCars);
    }

    private void addCar(int list, int id, List<GameObject> addList)
    {
        GameObject newCarOBJ = Resources.Load<GameObject>("cars/" + idNames[list][id]);
        addList.Add(newCarOBJ);

    }
}

[System.Serializable]
public class carsReader
{
    public carData[] normal;
    public carData[] large;
    public carData[] special;
}

[System.Serializable]
public class carData
{
    public string idName;
    public int hitPoint;
    public int blinkTime;
    public float speedMin;
    public float speedMax;
    public float odds;
    public bool isCar;
    public float forceMass;
}
