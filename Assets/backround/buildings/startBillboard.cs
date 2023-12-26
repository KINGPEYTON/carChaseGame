using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class startBillboard : MonoBehaviour
{
    public main controller;
    public Camera mainCamera;
    public Camera sideCamera;
    public bool inStore;
    public bool intro;
    public GameObject buttonCoverImage;

    public GameObject parantOBJ;
    public GameObject areYouSureToCreate;
    public Transform sideTransform;
    public GameObject signs;
    public GameObject playerDisplay;

    public float targetZoom;
    public float zoomSpeed;
    public Vector3 targetPos;
    public Vector3 targetSpeed;
    public bool inPos;

    public Button myButton;
    public Button shopButton;
    public Button settingsButton;
    public GameObject bigBillboard;

    public GameObject statics;
    public float staticTimer;
    public bool inStatic;
    public Image backround;
    public bool colorDir;
    public float colorVar;

    public AudioClip staticSound;

    public playerManager pManager;
    public playerCar playerCar;

    public int displayCarType;
    public SpriteRenderer displayBody;
    public SpriteRenderer displayWheelF;
    public SpriteRenderer displayWheelB;
    public SpriteRenderer displayWindow;
    public SpriteRenderer displayLivery;
    public SpriteMask displayMask;

    public float displayStartMPH;
    public float displayUpMPH;
    public float displayMoveTime;
    public float displaySmokeMulitplyer;

    public Transform shopItemsTransform;
    public Transform shopCategoryTransform;
    public TextMeshProUGUI coinText;
    public Button equipbutton;

    public GameObject itemButton;
    public List<Button> itemButtonList;
    public Button categoryButton;
    public List<Button> categoryButtonList;

    public Sprite window;
    public Button activeCategoryButton;
    public Button selectedCategoryButton;
    public int catergoryID;
    public int itemID;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        myButton = GetComponent<Button>();
        statics.SetActive(false);
        colorVar = 100;
        shopButtonFunc(0.1f);

        pManager = GameObject.Find("playerManager").GetComponent<playerManager>();
        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();
    }

    // Update is called once per frame
    void Update()
    {
        backround.color = new Color32((byte)colorVar, (byte)colorVar, 255, 255);

        if (colorDir)
        {
            colorVar += Time.deltaTime * 60;
            if(colorVar > 180)
            {
                colorDir = false;
            }
        }
        else
        {
            colorVar -= Time.deltaTime * 60;
            if (colorVar < 70)
            {
                colorDir = true;
            }
        }

        staticTimer -= Time.deltaTime;

        if (controller.playing)
        {
            parantOBJ.transform.position = parantOBJ.transform.position - new Vector3(Time.deltaTime / 8 * controller.mph, 0, 0); //moves building across the screen
            if (transform.position.x <= -14) //checks if its offscreen
            {
                Destroy(parantOBJ);
            }
        }

        if (staticTimer <= 0)
        {
            if (inStatic)
            {
                inStatic = false;
                statics.SetActive(false);
                if (inStore)
                {
                    playerDisplay.SetActive(true);
                    getItemButtons(0);
                }
            }
        }

        if (!inPos)
        {
            storeAnimation();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            addCoins(1000);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            pManager.resetUnlocks();
        }
    }

    void storeAnimation()
    {
        sideCamera.transform.position += new Vector3(targetSpeed.x * Time.deltaTime, targetSpeed.y * Time.deltaTime, 0);
        sideCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        if (targetSpeed.x < 0)
        {
            if (sideCamera.transform.position.x < targetPos.x)
            {
                sideCamera.transform.position = targetPos;
                inPos = true;
                sideCamera.orthographicSize = targetZoom;
                if (!inStore)
                {
                    mainCamera.enabled = true;
                    sideCamera.enabled = false;
                }
            }
        }
        else if (targetSpeed.x > 0)
        {
            if (sideCamera.transform.position.x > targetPos.x)
            {
                sideCamera.transform.position = targetPos;
                inPos = true;
                sideCamera.orthographicSize = targetZoom;
                if (!inStore)
                {
                    mainCamera.enabled = true;
                    sideCamera.enabled = false;
                }
            }
        }
        else
        {
            if (targetSpeed.y < 0)
            {
                if (sideCamera.transform.position.y < targetPos.y)
                {
                    sideCamera.transform.position = targetPos;
                    inPos = true;
                    sideCamera.orthographicSize = targetZoom;
                    if (!inStore)
                    {
                        mainCamera.enabled = true;
                        sideCamera.enabled = false;
                    }
                }
            }
            else if (targetSpeed.y > 0)
            {
                if (sideCamera.transform.position.y > targetPos.y)
                {
                    sideCamera.transform.position = targetPos;
                    inPos = true;
                    sideCamera.orthographicSize = targetZoom;
                    if (!inStore)
                    {
                        mainCamera.enabled = true;
                        sideCamera.enabled = false;
                    }
                }
            }
        }
    }

    public void click()
    {
        controller.StartGame();
        shopButton.interactable = false;
        settingsButton.interactable = false;
        statics.SetActive(true);
        staticTimer = 1f;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0,0,-10), controller.masterVol * controller.sfxVol);
    }

    public void shopButtonFunc(float speed)
    {
        AudioSource.PlayClipAtPoint(controller.clickSound, transform.position, controller.masterVol * controller.sfxVol);
        inStore = true;
        mainCamera.enabled = false;
        sideCamera.enabled = true;
        targetPos = new Vector3(-4, 3, -10);
        targetSpeed = setTargetSpeed(targetPos, speed, sideCamera.transform.position);
        inPos = false;
        targetZoom = 1.55f;
        zoomSpeed = -(sideCamera.orthographicSize - targetZoom) / speed;
        buttonCoverImage.SetActive(true);
        signs.SetActive(false);
        playerDisplay.SetActive(false);

        statics.SetActive(true);
        staticTimer = 1f;
        inStatic = true;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        coinText.text = controller.totalCoins.ToString();
    }

    public Vector3 setTargetSpeed(Vector3 target, float speed, Vector3 currPos)
    {
        return new Vector3((target.x - currPos.x) / speed, (target.y - currPos.y) / speed, (target.z - currPos.z) / speed);
    }

    public void exitShop(float speed)
    {
        AudioSource.PlayClipAtPoint(controller.clickSound, transform.position, controller.masterVol * controller.sfxVol);
        inStore = false;
        targetPos = new Vector3(0, 0, -10);
        targetSpeed = setTargetSpeed(targetPos, speed, sideCamera.transform.position);
        inPos = false;
        targetZoom = 5;
        zoomSpeed = -(sideCamera.orthographicSize - targetZoom) / speed;
        buttonCoverImage.SetActive(false);
        signs.SetActive(true);
        playerDisplay.SetActive(false);

        statics.SetActive(true);
        staticTimer = 1f;
        inStatic = true;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    public void equipItem()
    {
        bool didEquip = false;
        switch (catergoryID)
        {
            case 0:
                if (pManager.carTypeUnlocks[itemID])
                {
                    PlayerPrefs.SetInt("playerCarType", itemID); //saves the player car type
                    PlayerPrefs.SetInt("playerBody", 0); //saves the new high score
                    didEquip = true;
                }
                else
                {
                    if (buyItem(pManager.carPartsData.carTypes[itemID].cost, pManager.carTypeUnlocks, pManager.carNames[itemID] + "Type"))
                    {
                        PlayerPrefs.SetInt("playerCarType", itemID); //saves the player car type
                        PlayerPrefs.SetInt("playerBody", 0); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
            case 1:
                if (pManager.windowUnlocks[itemID])
                {
                    PlayerPrefs.SetInt("windowTint", itemID); //saves the new high score
                    didEquip = true;
                }
                else
                {
                    if (buyItem(pManager.carPartsData.windowTints[itemID].cost, pManager.windowUnlocks, pManager.windowNames[itemID] + "Window"))
                    {
                        PlayerPrefs.SetInt("windowTint", itemID); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
            case 2:
                if (pManager.bodyUnlocks[PlayerPrefs.GetInt("playerCarType", 0)][itemID])
                {
                    PlayerPrefs.SetInt("playerBody", itemID); //saves the new high score
                    didEquip = true;
                }
                else {
                    if(buyItem(pManager.bodyCosts[PlayerPrefs.GetInt("playerCarType", 0)][itemID].cost, pManager.bodyUnlocks[PlayerPrefs.GetInt("playerCarType", 0)], pManager.carNames[PlayerPrefs.GetInt("playerCarType", 0)] + pManager.bodyNames[PlayerPrefs.GetInt("playerCarType", 0)][itemID]))
                    {
                        PlayerPrefs.SetInt("playerBody", itemID); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
            case 3:
                if (pManager.wheelUnlocks[itemID])
                {
                    PlayerPrefs.SetInt("wheelBody", itemID); //saves the new high score
                    didEquip = true;
                }
                else
                {
                    if (buyItem(pManager.carPartsData.wheelTypes[itemID].cost, pManager.wheelUnlocks, pManager.wheelNames[itemID] + "Wheel"))
                    {
                        PlayerPrefs.SetInt("wheelBody", itemID); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
            case 4:
                if (pManager.liveryUnlocks[itemID])
                {
                    PlayerPrefs.SetInt("liveryTint", itemID); //saves the new high score
                    didEquip = true;
                }
                else
                {
                    if (buyItem(pManager.carPartsData.liveryTypes[itemID].cost, pManager.liveryUnlocks, pManager.liveryNames[itemID] + "livery"))
                    {
                        PlayerPrefs.SetInt("liveryTint", itemID); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
            case 5:
                if (pManager.liveryColorUnlocks[itemID])
                {
                    PlayerPrefs.SetInt("liveryColorTint", itemID); //saves the new high score
                    didEquip = true;
                }
                else
                {
                    if (buyItem(pManager.carPartsData.liveryColors[itemID].cost, pManager.liveryColorUnlocks, pManager.liveryColorNames[itemID] + "Color"))
                    {
                        PlayerPrefs.SetInt("liveryColorTint", itemID); //saves the new high score
                        didEquip = true;
                    }
                }
                break;
        }
        if (didEquip)
        {
            playerCar.getPlayerCustomazation();
            updateItemButtons(catergoryID);
            coinText.text = controller.totalCoins.ToString();

            selectedCategoryButton.interactable = true;
            ColorBlock cb = selectedCategoryButton.colors;
            cb.disabledColor = new Color32(0, 0, 175, 255);
            cb.normalColor = new Color32(200, 200, 255, 255);
            selectedCategoryButton.colors = cb;

            selectedCategoryButton = activeCategoryButton;
            ColorBlock cb2 = selectedCategoryButton.colors;
            cb2.disabledColor = new Color32(175, 0, 0, 255);
            cb2.normalColor = new Color32(175, 0, 0, 255);
            cb2.highlightedColor = new Color32(255, 50, 50, 255);
            selectedCategoryButton.colors = cb2;

            equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
            ColorBlock cb3 = equipbutton.colors;
            cb3.normalColor = new Color32(75, 75, 255, 255);
            cb3.selectedColor = new Color32(75, 75, 255, 255);
            equipbutton.colors = cb3;
        }
    }

    bool buyItem(int cost, List<bool> save, string saveName)
    {
        if (cost < controller.totalCoins)
        {
            controller.totalCoins -= cost;
            PlayerPrefs.SetInt("coins", controller.totalCoins); //saves the total coins
            save[itemID] = true;
            PlayerPrefs.SetInt(saveName, 1);
            return true;
        }
        else
        {
            youSure areYouSure = Instantiate(areYouSureToCreate, sideTransform).GetComponent<youSure>();
            areYouSure.methodToCall = buyMoreCoins;
            areYouSure.message = "Not enough coins. Would you like to buy some more?";
            return false;
        }
    }

    public void buyMoreCoins()
    {
        Debug.Log("no coins yet");
    }

    public void selectCustomizationItem(int buttonID, SpriteRenderer toChange, Sprite spriteChange, int cost, bool unlocked)
    {
        itemID = buttonID;
        activeCategoryButton.interactable = true;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.pressedColor;
        activeCategoryButton = itemButtonList[buttonID].GetComponent<Button>();
        activeCategoryButton.interactable = false;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;

        toChange.sprite = spriteChange;

        if (unlocked)
        {
            if (activeCategoryButton == selectedCategoryButton)
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(75, 75, 255, 255);
                cb.selectedColor = new Color32(75, 75, 255, 255);
                equipbutton.colors = cb;
            }
            else
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equip";

                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(150, 150, 255, 255);
                cb.selectedColor = new Color32(150, 150, 255, 255);
                equipbutton.colors = cb;
            }
        }
        else
        {
            equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Buy";

            if (cost < controller.totalCoins)
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(45, 225, 45, 255);
                cb.selectedColor = new Color32(45, 225, 45, 255);
                equipbutton.colors = cb;
            } else
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(225, 45, 45, 255);
                cb.selectedColor = new Color32(225, 45, 45, 255);
                equipbutton.colors = cb;
            }
        }
    }

    public void selectCustomizationItem(int buttonID, SpriteRenderer toChange, SpriteRenderer toChange2, Sprite spriteChange, int cost, bool unlocked)
    {
        itemID = buttonID;
        activeCategoryButton.interactable = true;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.pressedColor;
        activeCategoryButton = itemButtonList[buttonID].GetComponent<Button>();
        activeCategoryButton.interactable = false;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;

        toChange.sprite = spriteChange;
        toChange2.sprite = spriteChange;

        if (unlocked)
        {
            if (activeCategoryButton == selectedCategoryButton)
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(75, 75, 255, 255);
                cb.selectedColor = new Color32(75, 75, 255, 255);
                equipbutton.colors = cb;
            }
            else
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equip";

                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(150, 150, 255, 255);
                cb.selectedColor = new Color32(150, 150, 255, 255);
                equipbutton.colors = cb;
            }
        }
        else
        {
            equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Buy";

            if (cost < controller.totalCoins)
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(45, 225, 45, 255);
                cb.selectedColor = new Color32(45, 225, 45, 255);
                equipbutton.colors = cb;
            }
            else
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(225, 45, 45, 255);
                cb.selectedColor = new Color32(225, 45, 45, 255);
                equipbutton.colors = cb;
            }
        }
    }

    public void selectCustomizationItem(int buttonID, SpriteRenderer toChange, Color colorChange, int cost, bool unlocked)
    {
        itemID = buttonID;
        activeCategoryButton.interactable = true;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.pressedColor;
        activeCategoryButton = itemButtonList[buttonID].GetComponent<Button>();
        activeCategoryButton.interactable = false;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;

        toChange.color = colorChange;

        if (unlocked)
        {
            if (activeCategoryButton == selectedCategoryButton)
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(75, 75, 255, 255);
                cb.selectedColor = new Color32(75, 75, 255, 255);
                equipbutton.colors = cb;
            }
            else
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equip";

                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(150, 150, 255, 255);
                cb.selectedColor = new Color32(150, 150, 255, 255);
                equipbutton.colors = cb;
            }
        }
        else
        {
            equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Buy";

            if (cost < controller.totalCoins)
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(45, 225, 45, 255);
                cb.selectedColor = new Color32(45, 225, 45, 255);
                equipbutton.colors = cb;
            }
            else
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(225, 45, 45, 255);
                cb.selectedColor = new Color32(225, 45, 45, 255);
                equipbutton.colors = cb;
            }
        }
    }

    public void selectCustomizationItem(int buttonID, int cost, bool unlocked)
    {
        itemID = buttonID;
        activeCategoryButton.interactable = true;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.pressedColor;
        activeCategoryButton = itemButtonList[buttonID].GetComponent<Button>();
        activeCategoryButton.interactable = false;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;

        setDisplayCarType(buttonID);

        if (unlocked)
        {
            if (activeCategoryButton == selectedCategoryButton)
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(75, 75, 255, 255);
                cb.selectedColor = new Color32(75, 75, 255, 255);
                equipbutton.colors = cb;
            }
            else
            {
                equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equip";

                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(150, 150, 255, 255);
                cb.selectedColor = new Color32(150, 150, 255, 255);
                equipbutton.colors = cb;
            }
        }
        else
        {
            equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Buy";

            if (cost < controller.totalCoins)
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(45, 225, 45, 255);
                cb.selectedColor = new Color32(45, 225, 45, 255);
                equipbutton.colors = cb;
            }
            else
            {
                ColorBlock cb = equipbutton.colors;
                cb.normalColor = new Color32(225, 45, 45, 255);
                cb.selectedColor = new Color32(225, 45, 45, 255);
                equipbutton.colors = cb;
            }
        }
    }

    public void getItemButtons(int buttonID)
    {
        selectCustomizationCategories(buttonID, true);
    }

    public void updateItemButtons(int buttonID)
    {
        selectCustomizationCategories(buttonID, false);
    }

    public void selectCustomizationCategories(int buttonID, bool resetItemBar)
    {
        catergoryID = buttonID;
        switch (buttonID)
        {
            case 0:
                activateButtons(pManager.carIcon, pManager.carNames, PlayerPrefs.GetInt("playerCarType", 0), pManager.carPartsData.carTypes, pManager.carTypeUnlocks);
                break;
            case 1:
                activateButtons(pManager.windowColors, pManager.windowNames, window, displayWindow, PlayerPrefs.GetInt("windowTint", 0), pManager.carPartsData.windowTints, pManager.windowUnlocks);
                break;
            case 2:
                activateButtons(pManager.bodies[PlayerPrefs.GetInt("playerCarType", 0)], pManager.bodyNames[PlayerPrefs.GetInt("playerCarType", 0)], displayBody, PlayerPrefs.GetInt("playerBody", 0), pManager.bodyCosts[PlayerPrefs.GetInt("playerCarType", 0)], pManager.bodyUnlocks[PlayerPrefs.GetInt("playerCarType", 0)]);
                break;
            case 3:
                activateButtons(pManager.wheels, pManager.wheelNames, displayWheelF, displayWheelB, PlayerPrefs.GetInt("wheelBody", 0), pManager.carPartsData.wheelTypes, pManager.wheelUnlocks);
                break;
            case 4:
                activateButtons(pManager.livery, pManager.liveryNames, displayLivery, PlayerPrefs.GetInt("liveryTint", 0), pManager.carPartsData.liveryTypes, pManager.liveryUnlocks);
                break;
            case 5:
                activateButtons(pManager.liveryColors, pManager.liveryColorNames, pManager.livery[PlayerPrefs.GetInt("liveryTint", 0)], displayLivery, PlayerPrefs.GetInt("liveryColorTint", 0), pManager.carPartsData.liveryColors, pManager.liveryColorUnlocks);
                break;
        }

        categoryButton.interactable = true;
        categoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 180, 255);
        categoryButton = categoryButtonList[buttonID];
        categoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(0, 125, 255, 255);
        categoryButton.interactable = false;

        if (resetItemBar)
        {
            //slideItem();
        }
    }

    public void activateButtons(List<Sprite> iconList, List<string> nameList, SpriteRenderer toChange, int currSelect, carPart[] cost, List<bool> activeList)
    {
        int numButtons = iconList.Count;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = createButton(iconList[i], nameList[i], i, toChange, cost[i].cost, activeList[i]);
            newButton.transform.Find("Image").GetComponent<Image>().preserveAspect = true;
            newButton.transform.localPosition = new Vector3(-25 + (15 * i), 0.5f, 0);
            itemButtonList.Add(newButton);
        }
        activeCategoryButton = itemButtonList[currSelect];
        selectedCategoryButton = itemButtonList[currSelect];
        activeCategoryButton.interactable = false;
        ColorBlock cb = selectedCategoryButton.colors;
        cb.normalColor = new Color32(175, 0, 0, 255);
        cb.disabledColor = new Color32(255, 50, 50, 255);
        cb.highlightedColor = new Color32(255, 150, 150, 255);
        cb.pressedColor = new Color32(125, 0, 0, 255);
        selectedCategoryButton.colors = cb;
        selectedCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;
        setDisplayCar(PlayerPrefs.GetInt("playerCarType", 0));

        equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
        ColorBlock cb2 = equipbutton.colors;
        cb2.normalColor = new Color32(75, 75, 255, 255);
        cb2.selectedColor = new Color32(75, 75, 255, 255);
        equipbutton.colors = cb2;
    }

    public void activateButtons(List<Sprite> iconList, List<string> nameList, SpriteRenderer toChange, SpriteRenderer toChange2, int currSelect, carPart[] cost, List<bool> activeList)
    {
        int numButtons = iconList.Count;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = createButton(iconList[i], nameList[i], i, toChange, toChange2, cost[i].cost, activeList[i]);
            newButton.transform.Find("Image").GetComponent<Image>().preserveAspect = true;
            newButton.transform.localPosition = new Vector3(-25 + (15 * i), 0.5f, 0);
            itemButtonList.Add(newButton);
        }
        activeCategoryButton = itemButtonList[currSelect];
        selectedCategoryButton = itemButtonList[currSelect];
        activeCategoryButton.interactable = false;
        ColorBlock cb = selectedCategoryButton.colors;
        cb.normalColor = new Color32(175, 0, 0, 255);
        cb.disabledColor = new Color32(255, 50, 50, 255);
        cb.highlightedColor = new Color32(255, 150, 150, 255);
        cb.pressedColor = new Color32(125, 0, 0, 255);
        selectedCategoryButton.colors = cb;
        selectedCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;
        setDisplayCar(PlayerPrefs.GetInt("playerCarType", 0));

        equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
        ColorBlock cb2 = equipbutton.colors;
        cb2.normalColor = new Color32(75, 75, 255, 255);
        cb2.selectedColor = new Color32(75, 75, 255, 255);
        equipbutton.colors = cb2;
    }

    public void activateButtons(List<Color> iconList, List<string> nameList, Sprite icon, SpriteRenderer toChange, int currSelect, carPart[] cost, List<bool> activeList)
    {
        int numButtons = iconList.Count;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = createButton(icon, nameList[i], i, toChange, iconList[i], cost[i].cost, activeList[i]);
            newButton.transform.Find("Image").GetComponent<Image>().color = iconList[i];
            newButton.transform.Find("Image").GetComponent<Image>().preserveAspect = true;
            newButton.transform.localPosition = new Vector3(-25 + (15 * i), 0.5f, 0);
            itemButtonList.Add(newButton);
        }
        activeCategoryButton = itemButtonList[currSelect];
        selectedCategoryButton = itemButtonList[currSelect];
        activeCategoryButton.interactable = false;
        ColorBlock cb = selectedCategoryButton.colors;
        cb.normalColor = new Color32(175, 0, 0, 255);
        cb.disabledColor = new Color32(255, 50, 50, 255);
        cb.highlightedColor = new Color32(255, 150, 150, 255);
        cb.pressedColor = new Color32(125, 0, 0, 255);
        selectedCategoryButton.colors = cb;
        selectedCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;
        setDisplayCar(PlayerPrefs.GetInt("playerCarType", 0));

        equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
        ColorBlock cb2 = equipbutton.colors;
        cb2.normalColor = new Color32(75, 75, 255, 255);
        cb2.selectedColor = new Color32(75, 75, 255, 255);
        equipbutton.colors = cb2;
    }

    public void activateButtons(List<Sprite> iconList, List<string> nameList, int currSelect, carPart[] cost, List<bool> activeList)
    {
        int numButtons = iconList.Count;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = createButton(iconList[i], nameList[i], i, cost[i].cost, activeList[i]);
            newButton.transform.Find("Image").GetComponent<Image>().preserveAspect = true;
            newButton.transform.localPosition = new Vector3(-25 + (15 * i), 0.5f, 0);
            itemButtonList.Add(newButton);
        }
        activeCategoryButton = itemButtonList[currSelect];
        selectedCategoryButton = itemButtonList[currSelect];
        activeCategoryButton.interactable = false;
        ColorBlock cb = selectedCategoryButton.colors;
        cb.normalColor = new Color32(175, 0, 0, 255);
        cb.disabledColor = new Color32(255, 50, 50, 255);
        cb.highlightedColor = new Color32(255, 150, 150, 255);
        cb.pressedColor = new Color32(125, 0, 0, 255);
        selectedCategoryButton.colors = cb;
        selectedCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;
        setDisplayCar(PlayerPrefs.GetInt("playerCarType", 0));

        equipbutton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = "Equiped";
        ColorBlock cb2 = equipbutton.colors;
        cb2.normalColor = new Color32(75, 75, 255, 255);
        cb2.selectedColor = new Color32(75, 75, 255, 255);
        equipbutton.colors = cb2;
    }

    Button createButton(Sprite img, string text, int id, SpriteRenderer toChange, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectCustomizationItem(id, toChange, img, cost, unlocked));
        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
        newButton.transform.Find("Image").GetComponent<Image>().sprite = img;
        if (unlocked)
        {
            newButton.transform.Find("Cost").gameObject.SetActive(false);
            newButton.transform.Find("Coin").gameObject.SetActive(false);
        }
        else
        {
            TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
            newButton.transform.Find("Coin").localPosition = new Vector3(7.3f - (cost.ToString().Length * 2.25f), 6, 0);

            ColorBlock cb = newButton.colors;

            if (cost < controller.totalCoins)
            {
                cb.normalColor = new Color32(150, 225, 150, 255);
                cb.disabledColor = new Color32(75, 150, 150, 255);
                cb.highlightedColor = new Color32(0, 255, 225, 255);
                cb.pressedColor = new Color32(75, 150, 150, 255);
            }
            else
            {
                costText.color = new Color32(175, 0, 0, 255);
                cb.normalColor = new Color32(150, 150, 150, 255);
                cb.disabledColor = new Color32(75, 75, 150, 255);
                cb.highlightedColor = new Color32(125, 125, 255, 255);
                cb.pressedColor = new Color32(100, 100, 100, 255);
            }
            newButton.colors = cb;
            newButton.transform.Find("Text Backround").GetComponent<Image>().color = cb.pressedColor;
        }
        toChange.sprite = img;
        return newButton;
    }

    Button createButton(Sprite img, string text, int id, SpriteRenderer toChange, SpriteRenderer toChange2, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectCustomizationItem(id, toChange, toChange2, img, cost, unlocked));
        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
        newButton.transform.Find("Image").GetComponent<Image>().sprite = img;
        if (unlocked)
        {
            newButton.transform.Find("Cost").gameObject.SetActive(false);
            newButton.transform.Find("Coin").gameObject.SetActive(false);
        }
        else
        {
            TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
            newButton.transform.Find("Coin").localPosition = new Vector3(7.3f - (cost.ToString().Length * 2.25f), 6, 0);

            ColorBlock cb = newButton.colors;

            if (cost < controller.totalCoins)
            {
                cb.normalColor = new Color32(150, 225, 150, 255);
                cb.disabledColor = new Color32(75, 150, 150, 255);
                cb.highlightedColor = new Color32(0, 255, 225, 255);
                cb.pressedColor = new Color32(75, 150, 150, 255);
            }
            else
            {
                costText.color = new Color32(175, 0, 0, 255);
                cb.normalColor = new Color32(150, 150, 150, 255);
                cb.disabledColor = new Color32(75, 75, 150, 255);
                cb.highlightedColor = new Color32(125, 125, 255, 255);
                cb.pressedColor = new Color32(100, 100, 100, 255);
            }
            newButton.colors = cb;
            newButton.transform.Find("Text Backround").GetComponent<Image>().color = cb.pressedColor;
        }
        toChange.sprite = img;
        return newButton;
    }

    Button createButton(Sprite img, string text, int id, SpriteRenderer toChange, Color col, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectCustomizationItem(id, toChange, col, cost, unlocked));
        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
        newButton.transform.Find("Image").GetComponent<Image>().sprite = img;
        if (unlocked)
        {
            newButton.transform.Find("Cost").gameObject.SetActive(false);
            newButton.transform.Find("Coin").gameObject.SetActive(false);
        }
        else
        {
            TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
            newButton.transform.Find("Coin").localPosition = new Vector3(7.3f - (cost.ToString().Length * 2.25f), 6, 0);

            ColorBlock cb = newButton.colors;

            if (cost < controller.totalCoins)
            {
                cb.normalColor = new Color32(150, 225, 150, 255);
                cb.disabledColor = new Color32(75, 150, 150, 255);
                cb.highlightedColor = new Color32(0, 255, 225, 255);
                cb.pressedColor = new Color32(75, 150, 150, 255);
            }
            else
            {
                costText.color = new Color32(175, 0, 0, 255);
                cb.normalColor = new Color32(150, 150, 150, 255);
                cb.disabledColor = new Color32(75, 75, 150, 255);
                cb.highlightedColor = new Color32(125, 125, 255, 255);
                cb.pressedColor = new Color32(100, 100, 100, 255);
            }
            newButton.colors = cb;
            newButton.transform.Find("Text Backround").GetComponent<Image>().color = cb.pressedColor;
        }
        return newButton;
    }

    Button createButton(Sprite img, string text, int id, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectCustomizationItem(id, cost, unlocked));
        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
        newButton.transform.Find("Image").GetComponent<Image>().sprite = img;
        if (unlocked)
        {
            newButton.transform.Find("Cost").gameObject.SetActive(false);
            newButton.transform.Find("Coin").gameObject.SetActive(false);
        }
        else
        {
            TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
            newButton.transform.Find("Coin").localPosition = new Vector3(7.3f - (cost.ToString().Length * 2.25f), 6, 0);

            ColorBlock cb = newButton.colors;

            if (cost < controller.totalCoins)
            {
                cb.normalColor = new Color32(150, 225, 150, 255);
                cb.disabledColor = new Color32(75, 150, 150, 255);
                cb.highlightedColor = new Color32(0, 255, 225, 255);
                cb.pressedColor = new Color32(75, 150, 150, 255);
            }
            else
            {
                costText.color = new Color32(175, 0, 0, 255);
                cb.normalColor = new Color32(150, 150, 150, 255);
                cb.disabledColor = new Color32(75, 75, 150, 255);
                cb.highlightedColor = new Color32(125, 125, 255, 255);
                cb.pressedColor = new Color32(100, 100, 100, 255);
            }
            newButton.colors = cb;
            newButton.transform.Find("Text Backround").GetComponent<Image>().color = cb.pressedColor;
        }
        return newButton;
    }

    void setDisplayCar(int typeID)
    {
        setDisplayCarType(typeID);

        displayBody.sprite = playerCar.bodySprite;
        displayWheelB.sprite = playerCar.wheelSprite;
        displayWheelF.sprite = playerCar.wheelSprite;
        displayLivery.sprite = playerCar.liverySprite;

        displayWindow.color = playerCar.windowTint;
        displayLivery.color = playerCar.liveryColor;
    }

    void setDisplayCarType(int typeID)
    {
        displayBody.sprite = pManager.bodies[typeID][0];
        displayWindow.sprite = pManager.windows[typeID];
        displayMask.sprite = pManager.liveryMask[typeID];

        displayWheelB.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[typeID].wheelB, pManager.carPartsData.carTypes[typeID].wheelHight, 0);
        displayWheelF.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[typeID].wheelF, pManager.carPartsData.carTypes[typeID].wheelHight, 0);
    }

    void getDisplayCarStats(int typeID)
    {
        displayStartMPH = playerCar.startMph;
    }

    void addCoins(int ammount)
    {
        controller.totalCoins += ammount;
    }
}