using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class playerManager : MonoBehaviour
{
    public TextAsset carPartsJSON;
    public playerCarUnlocks pUnlocks;

    public List<string> carNames;
    public List<string> carIDs;
    public List<Sprite> carIcon;
    public List<AudioClip> carStart;

    public List<string> wheelNames;
    public List<string> wheelIds;
    public List<Sprite> wheels;

    public List<List<string>> bodyNames = new List<List<string>>();
    public List<List<string>> bodyIDs = new List<List<string>>();
    public List<List<Sprite>> bodies = new List<List<Sprite>>();

    public List<Sprite> windows;
    public List<string> windowNames;
    public List<string> windowIDs;
    public List<Color> windowColors;

    public List<Sprite> livery;
    public List<Sprite> liveryMask;
    public List<string> liveryNames;
    public List<string> liveryIDs;
    public List<Color> liveryColors;
    public List<string> liveryColorNames;
    public List<string> liveryColorIDs;

    public List<bool> carTypeUnlocks;
    public List<List<bool>> bodyUnlocks = new List<List<bool>>();
    public List<bool> windowUnlocks;
    public List<bool> wheelUnlocks;
    public List<bool> liveryUnlocks;
    public List<bool> liveryColorUnlocks;

    public carPartsReader carPartsData;

    public bool intro;

    void Awake()
    {
        GameObject obj = GameObject.Find("playerManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            if (JsonDataService.fileExist<playerCarUnlocks>("/playerUnlock.json", pUnlocks))
            {
                loadUnlocks();
                carPartsData = readCarDataJSON();
            }
            else
            {
                carPartsData = readCarDataJSON();
                saveUnlocks();
            }

            for (int f = 0; f < carNames.Count; f++)
            {
                getCarTypeAssets(carIDs[f], "player/cars/" + carIDs[f], f);
            }
            getCarAssets(wheels, wheelIds, "wheel ", "player/wheels");
            getCarAssets(livery, liveryIDs, "livery ", "player/livery");

            intro = true;
        }
    }

    private void getCarTypeAssets(string id, string path, int ind)
    {
        float xPiv = carPartsData.carTypes[ind].pivotX;
        float yPiv = carPartsData.carTypes[ind].pivotY;
        Texture2D SpriteTexture = Resources.Load<Texture2D>(path + "/" + id + " window");
        Sprite windo = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(xPiv, yPiv), 100);
        windows.Add(windo);

        Texture2D iconTexture = Resources.Load<Texture2D>(path + "/" + id + " icon");
        Sprite shopIcon = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f), 100);
        carIcon.Add(shopIcon);

        Texture2D maskOutlineTexture = Resources.Load<Texture2D>(path + "/" + id + " outline");
        Sprite maskOutline = Sprite.Create(maskOutlineTexture, new Rect(0, 0, maskOutlineTexture.width, maskOutlineTexture.height), new Vector2(xPiv, yPiv), 100);
        liveryMask.Add(maskOutline);

        AudioClip carStartAudio = Resources.Load<AudioClip>(path + "/" + id + " start");
        carStart.Add(carStartAudio);

        List<bool> newBodyUnlock = new List<bool>();
        newBodyUnlock.Add(true);
        for (int i = 1; i < bodyIDs.Count; i++)
        {
            newBodyUnlock.Add(PlayerPrefs.GetInt(carIDs[ind] + bodyIDs[i], 0) != 0);
        }

        getBodyAssets(bodies[ind], bodyIDs[ind], id + " ", path + "/body", xPiv, yPiv);
    }

    private void getCarAssets(List<Sprite> carParts, List<string> carPartNames, string prefix, string path)
    {
        for (int i = 0; i < carPartNames.Count; i++)
        {
            Texture2D itemTexture = Resources.Load<Texture2D>(path + "/" + prefix + carPartNames[i]);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            carParts.Add(item);
        }
    }

    private void getBodyAssets(List<Sprite> carParts, List<string> carPartNames, string prefix, string path, float xPivot, float yPivot)
    {
        for (int i = 0; i < carPartNames.Count; i++)
        {
            Texture2D itemTexture = Resources.Load<Texture2D>(path + "/" + prefix + carPartNames[i]);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(xPivot, yPivot), 100);
            carParts.Add(item);
        }
    }

    private carPartsReader readCarDataJSON()
    {
        carPartsReader carDataInJson = JsonUtility.FromJson<carPartsReader>(carPartsJSON.text);
        foreach (carTypesReader carType in carDataInJson.carTypes)
        {
            carNames.Add(carType.typeName);
            carIDs.Add(carType.idName);
            carTypeUnlocks.Add(pUnlocks.getUnlock(carType.idName + "type"));
        }
        carTypeUnlocks[0] = true;

        for (int i = 0; i < carNames.Count; i++)
        {
            bodies.Add(new List<Sprite>());
            bodyNames.Add(new List<string>());
            bodyIDs.Add(new List<string>());
            bodyUnlocks.Add(new List<bool>());
            foreach (carBodyReader bodySkin in carDataInJson.carTypes[i].bodies)
            {
                bodyNames[i].Add(bodySkin.bodyColor);
                bodyIDs[i].Add(bodySkin.colorID);
                bodyUnlocks[i].Add(pUnlocks.getUnlock(carIDs[i] + bodySkin.colorID + "body"));
            }
            bodyUnlocks[i][0] = true;
        }

        foreach (carWheelReader wheelType in carDataInJson.wheelTypes)
        {
            wheelNames.Add(wheelType.wheelName);
            wheelIds.Add(wheelType.idName);
            wheelUnlocks.Add(pUnlocks.getUnlock(wheelType.idName + "wheel"));
        }
        wheelUnlocks[0] = true;

        foreach (carWindowReader windowTint in carDataInJson.windowTints)
        {
            windowNames.Add(windowTint.tintColor);
            windowIDs.Add(windowTint.idName);
            windowColors.Add(new Color32(windowTint.ColorR, windowTint.ColorB, windowTint.ColorG, 255));
            windowUnlocks.Add(pUnlocks.getUnlock(windowTint.idName + "window"));
        }
        windowUnlocks[0] = true;

        foreach (carLiveryReader liveryType in carDataInJson.liveryTypes)
        {
            liveryNames.Add(liveryType.liveryName);
            liveryIDs.Add(liveryType.idName);
            liveryUnlocks.Add(pUnlocks.getUnlock(liveryType.idName + "livery"));
        }
        liveryUnlocks[0] = true;

        foreach (carLiveryColorReader liveryColor in carDataInJson.liveryColors)
        {
            liveryColorNames.Add(liveryColor.liveryColor);
            liveryColorIDs.Add(liveryColor.idName);
            liveryColors.Add(new Color32(liveryColor.ColorR, liveryColor.ColorB, liveryColor.ColorG, liveryColor.ColorA));
            liveryColorUnlocks.Add(pUnlocks.getUnlock(liveryColor.idName + "color"));
        }
        liveryColorUnlocks[0] = true;

        return carDataInJson;
    }

    public void resetUnlocks()
    {
        for (int i = 1; i < carTypeUnlocks.Count; i++)
        {
            carTypeUnlocks[i] = false;
            for (int j = 1; j < bodyUnlocks.Count; j++)
            {
                bodyUnlocks[i][j] = false;
            }
        }
        for (int i = 1; i < windowUnlocks.Count; i++)
        {
            windowUnlocks[i] = false;
        }
        for (int i = 1; i < wheelUnlocks.Count; i++)
        {
            wheelUnlocks[i] = false;
        }
        for (int i = 1; i < liveryUnlocks.Count; i++)
        {
            liveryUnlocks[i] = false;
        }
        for (int i = 1; i < liveryColorUnlocks.Count; i++)
        {
            liveryColorUnlocks[i] = false;
        }
    }

    public void unlockItem(string id)
    {
        pUnlocks.unlocks[id] = true;
        saveUnlocks();
    }

    private void loadUnlocks()
    {
        pUnlocks = JsonDataService.LoadData<playerCarUnlocks>("/playerUnlock.json", true);
    }
    private void saveUnlocks()
    {
        JsonDataService.SaveData("/playerUnlock.json", pUnlocks, true);
    }
}

[System.Serializable]
public class carPartsReader
{
    public carTypesReader[] carTypes;
    public carWheelReader[] wheelTypes;
    public carWindowReader[] windowTints;
    public carLiveryReader[] liveryTypes;
    public carLiveryColorReader[] liveryColors;
}

[System.Serializable]
public class carPart
{
    public int cost;

    public carPart(int co)
    {
        cost = co;
    }

    public carPart()
    {
        cost = 0;
    }
}

[System.Serializable]
public class carTypesReader : carPart
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string typeName;
    public string idName;
    public float startMPH;
    public float speedUp;
    public float moveTime;
    public int hits;
    public float wheelHight;
    public float wheelF;
    public float wheelB;
    public float liveryHight;
    public float pivotX;
    public float pivotY;

    public float scale;

    public float ramX;
    public float ramY;
    public float boostX;
    public float boostY;
    public float rocketX;
    public float rocketY;
    public float rocketX2;
    public float laserX;
    public float laserY;

    public float displayWheelY;
    public float displayWheelX1;
    public float displayWheelX2;
    public float displayWheelScale;

    public carBodyReader[] bodies;
}

[System.Serializable]
public class carBodyReader : carPart
{
    public string bodyColor;
    public string colorID;
}

[System.Serializable]
public class carWheelReader : carPart
{
    public string wheelName;
    public string idName;
    public float speedUp;
    public float moveTime;
}

[System.Serializable]
public class carWindowReader : carPart
{
    public string tintColor;
    public string idName;
    public byte ColorR;
    public byte ColorG;
    public byte  ColorB;
}

[System.Serializable]
public class carLiveryReader : carPart
{
    public string liveryName;
    public string idName;
}

[System.Serializable]
public class carLiveryColorReader : carPart
{
    public string liveryColor;
    public string idName;
    public byte ColorR;
    public byte ColorG;
    public byte ColorB;
    public byte ColorA;
}

[System.Serializable]
public class playerCarUnlocks
{
    public Dictionary<string, bool> unlocks = new Dictionary<string, bool>();

    public bool getUnlock(string id)
    {
        try
        {
            return unlocks[id];
        }
        catch
        {
            unlocks.Add(id, false);
            return false;
        }
    }
}