using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boostManager : MonoBehaviour
{
    public List<string> modIDs;
    public List<int> inventory;
    public List<Sprite> icons;
    public List<int> modRarity;
    public List<int> modCosts;
    public List<string> modNames;
    public List<string> modDescription;

    public int currSelect;
    public TextAsset modsJSON;
    public Sprite noneIcon;

    public modReader mdReader;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject obj = GameObject.Find("modsManager");

        if (obj != null && obj != gameObject)
        {
            obj.GetComponent<boostManager>().currSelect = 0;
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        mdReader = readModsJSON();
    }

    public bool selectMod(int id)
    {
        if (inventory[id] > 0 || id == 0)
        {
            return true;
        }
        return false;
    }

    public void equipMod(int id)
    {
        currSelect = id;
        GameObject.Find("Inventory Button Icon").GetComponent<Image>().sprite = icons[id];
    }

    public void addMod(int id)
    {
        if (id > 0)
        {
            inventory[id]++;
            PlayerPrefs.SetInt(modIDs[id] + "Ammount", inventory[id]);
        }
    }

    public int useMod()
    {
        if(currSelect > 0)
        {
            inventory[currSelect]--;
            PlayerPrefs.SetInt(modIDs[currSelect] + "Ammount", inventory[currSelect]);
        }
        return currSelect;
    }

    private modReader readModsJSON()
    {
        modNames.Add("None");
        modIDs.Add("empty");
        modRarity.Add(0);
        inventory.Add(0);
        modCosts.Add(0);
        modDescription.Add("");
        icons.Add(noneIcon);

        modReader modsDataInJson = JsonUtility.FromJson<modReader>(modsJSON.text);

        foreach (mods md in modsDataInJson.boosts)
        {
            modNames.Add(md.gameName);
            modIDs.Add(md.idName);
            modRarity.Add(md.rarity);
            inventory.Add(PlayerPrefs.GetInt(md.idName + "Ammount", 0));
            modCosts.Add(md.cost);
            modDescription.Add(md.description);

            Texture2D itemTexture = Resources.Load<Texture2D>("mods/boosts/" + md.idName);
            Sprite item = Sprite.Create(itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
            icons.Add(item);
        }

        return modsDataInJson;
    }
}

[System.Serializable]
public class modReader
{
    public mods[] boosts;
    public mods[] challenges;
    public mods[] premium;
}

[System.Serializable]
public class mods
{
    public string idName;
    public string gameName;
    public int rarity;
    public string description;
    public int cost;
}