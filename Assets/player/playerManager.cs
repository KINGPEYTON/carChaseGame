using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class playerManager : MonoBehaviour
{
    public TextAsset carPartsJSON;

    public List<string> carNames;
    public List<Sprite> carIcon;

    public List<string> wheelNames;
    public List<Sprite> wheels;

    public List<List<string>> bodyNames = new List<List<string>>();
    public List<List<Sprite>> bodies = new List<List<Sprite>>();
    public List<List<Sprite>> crashes = new List<List<Sprite>>();
    public List<carPart[]> bodyCosts = new List<carPart[]>();

    public List<Sprite> windows;
    public List<string> windowNames;
    public List<Color> windowColors;

    public List<Sprite> livery;
    public List<Sprite> liveryMask;
    public List<string> liveryNames;
    public List<Color> liveryColors;
    public List<string> liveryColorNames;

    public List<bool> carTypeUnlocks;
    public List<List<bool>> bodyUnlocks = new List<List<bool>>();
    public List<bool> windowUnlocks;
    public List<bool> wheelUnlocks;
    public List<bool> liveryUnlocks;
    public List<bool> liveryColorUnlocks;

    public carPartsReader carPartsData;

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
        }

        carPartsData = readCarDataJSON();

        for(int f = 0; f < carNames.Count; f++)
        {
            getCarTypeAssets(carNames[f], "player/cars/"+ carNames[f], f);
        }
        getCarAssets(wheels, wheelNames, " Wheel", "player/wheels");
        getCarAssets(livery, liveryNames, " Livery", "player/livery");
    }

    private void getCarTypeAssets(string name, string path, int ind)
    {
        Texture2D SpriteTexture = Resources.Load<Texture2D>(path + "/" + name + " - window");
        Sprite windo = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100);
        windows.Add(windo);

        Texture2D iconTexture = Resources.Load<Texture2D>(path + "/" + name + " - icon");
        Sprite shopIcon = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f), 100);
        carIcon.Add(shopIcon);

        Texture2D maskOutlineTexture = Resources.Load<Texture2D>(path + "/" + name + " - outline");
        Sprite maskOutline = Sprite.Create(maskOutlineTexture, new Rect(0, 0, maskOutlineTexture.width, maskOutlineTexture.height), new Vector2(0.5f, 0.5f), 100);
        liveryMask.Add(maskOutline);

        List<Sprite> newBodySprites = new List<Sprite>();
        List<string> newBodyNames = new List<string>();
        getCarStuff(newBodySprites, newBodyNames, path + "/body");

        List<bool> newBodyUnlock = new List<bool>();
        newBodyUnlock.Add(true);
        for (int i = 1; i < newBodyNames.Count; i++)
        {
            newBodyUnlock.Add(PlayerPrefs.GetInt(carNames[ind] + newBodyNames[i], 0) != 0);
        }

        bodies.Add(newBodySprites);
        bodyNames.Add(newBodyNames);
        bodyUnlocks.Add(newBodyUnlock);

        List<string> newCrashNames = new List<string>();
        List<Sprite> newCrashSprites = new List<Sprite>();
        getCarStuff(newCrashSprites, newCrashNames, path + "/crashed");

        crashes.Add(newCrashSprites);

        carPart[] bodyCostsArr = new carPart[newBodySprites.Count];
        for(int i = 0; i < newBodySprites.Count; i++)
        {
            bodyCostsArr[i] = new carPart(carPartsData.carTypes[ind].cost / 4);
        }
        bodyCosts.Add(bodyCostsArr);
    }

    private void getCarStuff(List<Sprite> carParts, List<string> carPartNames, string path)
    {
        var carBodyList = Resources.LoadAll(path, typeof(Texture2D));

        for (int i = 0; i < carBodyList.Length; i++)
        {
            Texture2D itemTexture = carBodyList[i] as Texture2D;
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            string name = carBodyList[i].name;
            string color = name.Substring(name.IndexOf(" - ") + 3);
            carParts.Add(item);
            carPartNames.Add(color);
        }
    }

    private void getCarAssets(List<Sprite> carParts, List<string> carPartNames, string addOn, string path)
    {
        for (int i = 0; i < carPartNames.Count; i++)
        {
            Texture2D itemTexture = Resources.Load<Texture2D>(path + "/" + carPartNames[i] + addOn);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            carParts.Add(item);
        }
    }

    private carPartsReader readCarDataJSON()
    {
        carPartsReader carDataInJson = JsonUtility.FromJson<carPartsReader>(carPartsJSON.text);

        foreach (carTypesReader carType in carDataInJson.carTypes)
        {
            carNames.Add(carType.typeName);
            carTypeUnlocks.Add(PlayerPrefs.GetInt(carType.typeName + "Type", 0) != 0);
        }
        carTypeUnlocks[0] = true;

        foreach (carWheelReader wheelType in carDataInJson.wheelTypes)
        {
            wheelNames.Add(wheelType.wheelName);
            wheelUnlocks.Add(PlayerPrefs.GetInt(wheelType.wheelName + "Wheel", 0) != 0);
        }
        wheelUnlocks[0] = true;

        foreach (carWindowReader windowTint in carDataInJson.windowTints)
        {
            windowNames.Add(windowTint.tintColor);
            windowColors.Add(new Color32(windowTint.ColorR, windowTint.ColorB, windowTint.ColorG, 255));
            windowUnlocks.Add(PlayerPrefs.GetInt(windowTint.tintColor + "Window", 0) != 0);
        }
        windowUnlocks[0] = true;

        foreach (carLiveryReader liveryType in carDataInJson.liveryTypes)
        {
            liveryNames.Add(liveryType.liveryName);
            liveryUnlocks.Add(PlayerPrefs.GetInt(liveryType.liveryName + "livery", 0) != 0);
        }
        liveryUnlocks[0] = true;

        foreach (carLiveryColorReader liveryColor in carDataInJson.liveryColors)
        {
            liveryColorNames.Add(liveryColor.liveryColor);
            liveryColors.Add(new Color32(liveryColor.ColorR, liveryColor.ColorB, liveryColor.ColorG, 255));
            liveryColorUnlocks.Add(PlayerPrefs.GetInt(liveryColor.liveryColor + "Color", 0) != 0);
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
}

[System.Serializable]
public class carPartsReader
{
    //employees is case sensitive and must match the string "employees" in the JSON.
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
    }
}

[System.Serializable]
public class carTypesReader : carPart
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string typeName;
    public float startMPH;
    public float speedUp;
    public float moveTime;
    public float wheelHight;
    public float wheelF;
    public float wheelB;
}

[System.Serializable]
public class carWheelReader : carPart
{
    public string wheelName;
    public float speedUp;
    public float moveTime;
}

[System.Serializable]
public class carWindowReader : carPart
{
    public string tintColor;
    public float screenEffect;
    public byte ColorR;
    public byte ColorG;
    public byte  ColorB;
}

[System.Serializable]
public class carLiveryReader : carPart
{
    public string liveryName;
}

[System.Serializable]
public class carLiveryColorReader : carPart
{
    public string liveryColor;
    public byte ColorR;
    public byte ColorG;
    public byte ColorB;
}