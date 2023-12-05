using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class playerManager : MonoBehaviour
{
    public TextAsset carPartsJSON;

    public List<string> carNames;

    public List<string> wheelNames;
    public List<Sprite> wheels;

    public List<List<string>> bodyNames = new List<List<string>>();
    public List<List<Sprite>> bodies = new List<List<Sprite>>();
    public List<List<Sprite>> crashes = new List<List<Sprite>>();

    public List<Sprite> windows;
    public List<string> windowNames;
    public List<Color> windowColors;

    public List<Sprite> livery;
    public List<Sprite> liveryMask;
    public List<string> liveryNames;
    public List<Color> liveryColors;

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

        foreach (string f in carNames)
        {
            getCarTypeAssets(f, "player/cars/"+f);
        }
        getCarAssets(wheels, wheelNames, "player/wheels");
        getCarStuff(livery, liveryNames, "player/livery");
    }

    private void getCarTypeAssets(string name, string path)
    {
        Texture2D SpriteTexture = Resources.Load<Texture2D>(path + "/" + name + " - window");
        Sprite windo = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100);
        windows.Add(windo);

        Texture2D maskOutlineTexture = Resources.Load<Texture2D>(path + "/" + name + " - Outline");
        Sprite maskOutline = Sprite.Create(maskOutlineTexture, new Rect(0, 0, maskOutlineTexture.width, maskOutlineTexture.height), new Vector2(0.5f, 0.5f), 100);
        liveryMask.Add(maskOutline);

        List<Sprite> newBodySprites = new List<Sprite>();
        List<string> newBodyNames = new List<string>();
        getCarStuff(newBodySprites, newBodyNames, path + "/body");

        bodies.Add(newBodySprites);
        bodyNames.Add(newBodyNames);

        List<string> newCrashNames = new List<string>();
        List<Sprite> newCrashSprites = new List<Sprite>();
        getCarStuff(newCrashSprites, newCrashNames, path + "/crashed");

        crashes.Add(newCrashSprites);
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

    private void getCarAssets(List<Sprite> carParts, List<string> carPartNames, string path)
    {
        for (int i = 0; i < carPartNames.Count; i++)
        {
            Texture2D itemTexture = Resources.Load<Texture2D>(path + "/" + carPartNames[i]);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            carParts.Add(item);
        }
    }

    private carPartsReader readCarDataJSON()
    {
        carPartsReader carDataInJson = JsonUtility.FromJson<carPartsReader>(carPartsJSON.text);

        foreach (carTypesReader carType in carDataInJson.carTypes)
        {
            //Debug.Log("Car Type: " + carType.typeName + " with Start MPH of " + carType.startMPH + " and an incress of " + carType.speedUp);
            carNames.Add(carType.typeName);
        }

        foreach (carWheelReader wheelType in carDataInJson.wheelTypes)
        {
            //Debug.Log("Wheel Type: " + wheelType.wheelName + " modifying the speed incress by " + wheelType.speedUp + " changing and a move speed by " + wheelType.moveTime);
            wheelNames.Add(wheelType.wheelName);
        }

        foreach (carWindowReader windowTint in carDataInJson.windowTints)
        {
            //Debug.Log("Window Tint: " + windowTint.tintColor + " chancing the effect by " + windowTint.screenEffect);
            windowNames.Add(windowTint.tintColor);
            windowColors.Add(new Color32(windowTint.ColorR, windowTint.ColorB, windowTint.ColorG, 255));
        }

        return carDataInJson;
    }
}

[System.Serializable]
public class carPartsReader
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public carTypesReader[] carTypes;
    public carWheelReader[] wheelTypes;
    public carWindowReader[] windowTints;
}

[System.Serializable]
public class carTypesReader
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string typeName;
    public float startMPH;
    public float speedUp;
    public float moveTime;
}

[System.Serializable]
public class carWheelReader
{
    public string wheelName;
    public float speedUp;
    public float moveTime;
}

[System.Serializable]
public class carWindowReader
{
    public string tintColor;
    public float screenEffect;
    public byte ColorR;
    public byte ColorG;
    public byte  ColorB;
}