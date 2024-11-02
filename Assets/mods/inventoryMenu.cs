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
    public GameObject menuSign;

    public Button equipButton;
    public Button exitButton;

    public bool inPos;
    public bool inEnd;

    public float coverTimer;
    public float coverTime;

    public float invTimer;
    public float invTime;

    public GameObject itemBox;
    public GameObject itemRow;
    public Transform itemBar;
    public List<Button> itemButtons;

    public int currSel;

    public AudioClip clickSound;
    public AudioClip modTyping;
    public AudioClip selectSound;
    public AudioClip errorSound;

    public List<Sprite> modBoxes;
    public List<Sprite> modBoxesEmpty;
    public List<Sprite> modBoxesNone;
    public Sprite modBoxCurr;
    public Sprite modBoxSel;
    public Sprite modBoxSelCurr;

    public Image selIcon;
    public float selIconMainTimer;
    public float selIconTimer;
    public float selIconTime;
    public bool selIconAni;

    public Image selOutline;
    public List<Sprite> selOutlineSprites;

    public Image selCurrOutline;
    public float selCurrTimer;
    public float selCurrTime;
    public bool selCurrAni;
    public bool selCurrAniUp;

    public TextMeshProUGUI selTitleText;
    public string selTitleCurr;
    public float selTitleTimer;
    public float selTitleTime;
    public bool selTitleAni;

    // Start is called before the first frame update
    void Start()
    {
        contoller = GameObject.Find("contoller").GetComponent<main>();
        modMang = GameObject.Find("modsManager").GetComponent<boostManager>();

        inPos = false;

        coverTime = 0.75f;
        invTime = 0.65f;

        spawnItemMod();

        currSel = modMang.currSelect;
        pickMod(currSel);

        selCurrTime = 0.85f;
        selTitleTime = 0.95f;
        selIconTime = 1.15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inPos)
        {
            if (inEnd)
            {
                if (coverTimer < coverTime)
                {
                    cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(coverTimer, 0, coverTime, 175)));
                    menuSign.transform.localPosition = new Vector3(0, -46 - getValueScale(coverTimer, 0, coverTime, 140), 0);
                    coverTimer += Time.deltaTime;
                    if (coverTimer > coverTime)
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
                    inPos = true;
                }
            }
            if (invTimer < invTime)
            {
                menuSign.transform.localPosition = new Vector3(0, -176 + getValueScale(invTimer, 0, invTime, 140), 0);
                invTimer += Time.deltaTime;
                if (invTimer > invTime)
                {
                    menuSign.transform.localPosition = new Vector3(0, -46, 0);
                }
            }
        }

        if (selCurrAni)
        {
            if (selCurrAniUp) { selCurrOutline.fillAmount = getValueScale(selCurrTimer, 0, selCurrTime, 1); }
            else { selCurrOutline.fillAmount = selCurrTime - getValueScale(selCurrTimer, 0, selCurrTime, 1); }

            selCurrTimer += Time.deltaTime;
            if (selCurrTimer > selCurrTime)
            {
                if (selCurrAniUp) { selCurrOutline.fillAmount = 1; } else { selCurrOutline.fillAmount = 0; }
                selCurrAni = false;
            }
        }

        if (selTitleAni)
        {
            if (selTitleTimer < selTitleTime / 2)
            {
                if (selTitleCurr.Length > 1)
                {
                    selTitleText.text = selTitleCurr.Substring(0, selTitleCurr.Length - (int)getValueScale(selTitleTimer, 0, selTitleTime / 2, selTitleCurr.Length)) + (char)Random.Range(33, 64);
                }
                else
                {
                    selTitleText.text = selTitleCurr + (char)Random.Range(33, 64);
                }
            }
            else { selTitleText.text = modMang.modNames[currSel].Substring(0, (int)getValueScale(selTitleTimer, selTitleTime / 2, selTitleTime, modMang.modNames[currSel].Length)) + (char)Random.Range(33, 64); }

            selTitleTimer += Time.deltaTime;
            if (selTitleTimer > selTitleTime)
            {
                selTitleText.text = modMang.modNames[currSel];
                selTitleAni = false;
            }
        }

        selIconMainTimer += Time.deltaTime;
        float iconSize = 0.9f + getValueScale(Mathf.Abs(((selIconMainTimer) % 3.5f) - 1.75f), 0, 1.75f, 0.1f);
        selIcon.transform.localScale = new Vector3(iconSize, iconSize, 1);

        if (selIconAni)
        {
            selIcon.color = new Color32(255, 255, 255, (byte)getValueScale(Mathf.Abs(selIconTimer - selIconTime / 2), 0, selIconTime / 2, 255));
            selIconTimer += Time.deltaTime;
            if (selIconTimer > selIconTime)
            {
                selIcon.color = new Color32(255, 255, 255, 255);
                selIconAni = false;
            }
            else if (selIconTimer > selIconTime / 2)
            {
                selIcon.sprite = modMang.icons[currSel];
            }
        }
    }

    void startSelCurrAni(bool fadeIn)
    {
        if (selCurrAniUp != fadeIn)
        {
            selCurrAni = true;
            selCurrAniUp = fadeIn;
            selCurrTimer = 0;
            if (fadeIn)
            {
                selCurrOutline.fillOrigin = (int)Image.Origin90.BottomRight;
            }
            else
            {
                selCurrOutline.fillOrigin = (int)Image.Origin90.TopLeft;
            }
        }
    }

    void startSelTitleAni()
    {
        selTitleAni = true;
        selTitleTimer = 0;
        selTitleCurr = selTitleText.text;
    }

    void startSelIconAni()
    {
        if (selIconAni)
        {
            if (selIconTimer > selIconTime / 2)
            {
                selIconTimer -= (selIconTime / 2);
            }
        }
        else
        {
            selIconTimer = 0;
        }
        selIconAni = true;
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

        ni.transform.Find("mod mask").Find("mod icon").GetComponent<Image>().sprite = modMang.icons[id];
        if (id > 0)
        {
            ni.transform.Find("count text").GetComponent<TextMeshProUGUI>().text = modMang.inventory[id].ToString();
        }
        else
        {
            ni.transform.Find("count text").gameObject.SetActive(false);
        }

        Image niCol = ni.GetComponent<Image>();
        if (modMang.modRarity[id] == 0)
        {
            ni.transform.Find("mod mask").GetComponent<Image>().sprite = modBoxesNone[3];
        }
        niCol.sprite = getModBox(id);

        Button niBu = ni.GetComponent<Button>();
        itemButtons.Add(niBu);
        niBu.onClick.AddListener(() => pickMod(id));
    }

    Sprite getModBox(int id)
    {
        if (modMang.modRarity[id] == 0)
        {
            return modBoxesNone[0];
        }
        else
        {
            if (modMang.inventory[id] > 0)
            {
                return modBoxes[modMang.modRarity[id] - 1];
            }
            else
            {
                return modBoxesEmpty[modMang.modRarity[id] - 1];
            }
        }
    }

    public void pickMod(int id)
    {
        if (modMang.selectMod(id))
        {
            if (invTimer > invTime)
            {
                AudioSource.PlayClipAtPoint(selectSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
                AudioSource.PlayClipAtPoint(modTyping, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
            }
            itemButtons[currSel].interactable = true;
            if (currSel != modMang.currSelect)
            {
                itemButtons[currSel].GetComponent<Image>().sprite = getModBox(currSel);
            }
            else
            {
                if (currSel > 0)
                {
                    itemButtons[currSel].GetComponent<Image>().sprite = modBoxCurr;
                }
                else
                {
                    itemButtons[currSel].GetComponent<Image>().sprite = modBoxesNone[1];
                }
            }
            currSel = id;
            itemButtons[id].interactable = false;
            if (id != modMang.currSelect)
            {
                itemButtons[id].GetComponent<Image>().sprite = modBoxSel;
                equipButton.interactable = true;
                startSelCurrAni(false);
            }
            else
            {
                equipButton.interactable = false;
                startSelCurrAni(true);
                if (id > 0)
                {
                    itemButtons[currSel].GetComponent<Image>().sprite = modBoxSelCurr;
                }
                else
                {
                    itemButtons[currSel].GetComponent<Image>().sprite = modBoxesNone[2];
                }
            }
            startSelIconAni();
            modNewTitle.newModTitleOutline(selOutlineSprites[modMang.modRarity[currSel]], selCurrTime, selOutline);
            startSelTitleAni();

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
        exitButton.interactable = false;
    }

    public void equip()
    {
        AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), contoller.masterVol * contoller.sfxVol);
        if (currSel != modMang.currSelect)
        {
            itemButtons[modMang.currSelect].GetComponent<Image>().sprite = getModBox(modMang.currSelect);
            modMang.equipMod(currSel);
            if (currSel > 0)
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxCurr;
            }
            else
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxesNone[2];
            }
            equipButton.interactable = false;
            selectMod();
        }
    }

    void selectMod()
    {
        if (currSel > 0)
        {
            if (modMang.currSelect == currSel)
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxSelCurr;
            }
            else
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxSel;
            }
        }
        else
        {
            if (modMang.currSelect == currSel)
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxesNone[2];
            }
            else
            {
                itemButtons[currSel].GetComponent<Image>().sprite = modBoxesNone[1];
            }
        }
        startSelCurrAni(true);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
