using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventoryMenu : MonoBehaviour
{
    public main contoller;
    public boostManager modMang;

    public Image cover;
    public GameObject exitSign;
    public GameObject equipSign;
    public GameObject menuSign;
    public GameObject menuItems;

    public bool inPos;
    public bool inEnd;

    public float coverTimer;
    public float coverTime;

    public float invTimer;
    public float invTime;

    public float exitTimer;
    public float exitTime;

    public float equipTimer;
    public float equipTime;

    public GameObject itemBox;
    public GameObject itemRow;
    public Transform itemBar;
    public List<Button> itemButtons;

    public int currSel;

    public AudioClip clickSound;
    public AudioClip selectSound;
    public AudioClip errorSound;

    // Start is called before the first frame update
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();
        modMang = GameObject.Find("modsManager").GetComponent<boostManager>();

        inPos = false;

        coverTime = 1.0f;
        invTime = 0.5f;
        equipTime = 0.75f;
        exitTime = 1.0f;

        spawnItemMod();

        currSel = modMang.currSelect;
        itemButtons[currSel].interactable = false;
        selectMod();
    }

    // Update is called once per frame
    void Update()
    {
        if (inPos)
        {
            if (inEnd)
            {
                if (coverTimer < equipTime)
                {
                    cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(coverTimer, 0, equipTime, 175)));
                    menuSign.transform.localPosition = new Vector3(0, -43 - getValueScale(coverTimer, 0, equipTime, 140), 0);
                    menuItems.transform.localPosition = new Vector3(0, 2 - getValueScale(coverTimer, 0, equipTime, 140), 0);
                    exitSign.transform.localPosition = new Vector3(-88 - getValueScale(coverTimer, 0, equipTime, 140), -28, 0);
                    equipSign.transform.localPosition = new Vector3(85 + getValueScale(coverTimer, 0, equipTime, 140), -12, 0);
                    coverTimer += Time.deltaTime;
                    if (coverTimer > equipTime)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        else { 
            if(coverTimer < coverTime)
            {
                cover.color = new Color32(36, 36, 36, (byte)getValueScale(coverTimer, 0, coverTime, 175));
                coverTimer += Time.deltaTime;
                if (coverTimer > coverTime)
                {
                    cover.color = new Color32(36, 36, 36, 175);
                }
            }
            if (invTimer < invTime)
            {
                menuSign.transform.localPosition = new Vector3(0, -173 + getValueScale(invTimer, 0, invTime, 140), 0);
                menuItems.transform.localPosition = new Vector3(0, -138 + getValueScale(invTimer, 0, invTime, 140), 0);
                invTimer += Time.deltaTime;
                if (invTimer > invTime)
                {
                    menuSign.transform.localPosition = new Vector3(0, -43, 0);
                    menuItems.transform.localPosition = new Vector3(0, 2, 0);
                }
            }
            if (equipTimer < equipTime)
            {
                equipSign.transform.localPosition = new Vector3(225 - getValueScale(equipTimer, 0, equipTime, 140), -12, 0);
                equipTimer += Time.deltaTime;
                if (equipTimer > equipTime)
                {
                    equipSign.transform.localPosition = new Vector3(85, -12, 0);
                }
            }
            if (exitTimer < exitTime)
            {
                exitSign.transform.localPosition = new Vector3(-228 + getValueScale(exitTimer, 0, exitTime, 140), -28, 0);
                exitTimer += Time.deltaTime;
                if (exitTimer > exitTime)
                {
                    exitSign.transform.localPosition = new Vector3(-88, -28, 0);
                    inPos = true;
                }
            }
        }
    }

    public void spawnItemMod()
    {
        int i = 0;
        while (i < modMang.inventory.Count)
        {
            Transform newRow = Instantiate(itemRow, itemBar).transform;
            for (int j = 0; j < 4; j++) {
                if(i < modMang.inventory.Count)
                {
                    createItemButton(i, newRow);
                    i++;
                }
            }
        }
    }

    void createItemButton(int id, Transform paren)
    {
        GameObject ni = Instantiate(itemBox, paren);

        ni.transform.Find("name text").GetComponent<TextMeshProUGUI>().text = modMang.modNames[id];
        ni.transform.Find("mod icon").GetComponent<Image>().sprite = modMang.icons[id];
        if (id > 0)
        {
            ni.transform.Find("count text").GetComponent<TextMeshProUGUI>().text = modMang.inventory[id].ToString();
            if (modMang.inventory[id] > 0)
            {
                ni.transform.Find("mod count").GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            }
            else
            {
                ni.transform.Find("mod count").GetComponent<Image>().color = new Color32(5, 5, 5, 255);
            }
        }
        else
        {
            ni.transform.Find("count text").gameObject.SetActive(false);
            ni.transform.Find("mod count").gameObject.SetActive(false);
        }

        Image niCol = ni.GetComponent<Image>();
        switch (modMang.modRarity[id])
        {
            case 0:
                niCol.color = new Color32(255, 255, 255, 255);
                break;
            case 1:
                niCol.color = new Color32(175, 255, 175, 255);
                break;
            case 2:
                niCol.color = new Color32(175, 175, 255, 255);
                break;
            case 3:
                niCol.color = new Color32(175, 0, 175, 255);
                break;
        }

        Button niBu = ni.GetComponent<Button>();
        itemButtons.Add(niBu);
        niBu.onClick.AddListener(() => pickMod(id));
    }

    public void pickMod(int id)
    {
        if (modMang.selectMod(id))
        {
            AudioSource.PlayClipAtPoint(selectSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
            if (currSel != modMang.currSelect)
            {
                itemButtons[currSel].interactable = true;
            }
            currSel = id;
            itemButtons[id].interactable = false;
        }
        else
        {
            AudioSource.PlayClipAtPoint(errorSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        }
    }

    public void exit()
    {
        coverTimer = 0;
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        inEnd = true;
    }

    public void equip()
    {
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        if (currSel != modMang.currSelect)
        {
            ColorBlock cb = itemButtons[modMang.currSelect].colors;
            cb.disabledColor = new Color32(115, 115, 115, 255);
            itemButtons[modMang.currSelect].colors = cb;
            itemButtons[modMang.currSelect].interactable = true;
            modMang.equipMod(currSel);
            selectMod();
        }
    }

    void selectMod()
    {
        ColorBlock cb = itemButtons[currSel].colors;
        cb.disabledColor = new Color32(255, 50, 50, 255);
        itemButtons[currSel].colors = cb;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
