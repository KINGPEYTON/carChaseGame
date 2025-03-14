using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.Purchasing;

public class shopBillboard : MonoBehaviour
{
    public main controller;
    public Camera mainCamera;
    public cameraScaler cameraScale;
    public bool inStore;
    public GameObject buttonCoverImage;

    public GameObject mainCoverPage;
    public GameObject playerCoverPage;
    public GameObject powerupCoverPage;
    public GameObject uiCoverPage;
    public GameObject boostsCoverPage;
    public GameObject buyCoinsCoverPage;

    public bool intro;
    public float introTimer;
    public bool canStart;

    public GameObject parantOBJ;
    public GameObject areYouSureToCreate;
    public Transform sideTransform;
    public GameObject menuSigns;
    public Button startSign;
    public Button modSign;
    public Button settingsSign;
    public GameObject speedUI;

    public float targetZoom;
    public float zoomSpeed;
    public Vector3 targetPos;
    public Vector3 targetSpeed;
    public bool inPos;

    public Button myButton;
    public Button shopButton;
    public Button settingsButton;
    public Button inventoryButton;
    public GameObject bigBillboard;

    public GameObject statics;
    public float staticTimer;
    public bool inStatic;

    public Image backround;
    public Image shopTextBackround;
    public Image shopTextOutline;
    public float colorTimer;

    public GameObject backCoin;
    public Transform coinAni;
    public float backCoinTimer;

    public GameObject starGlimmer;
    public Transform stars;
    public float starTimer;

    public Image frontCoinImage;
    public TextMeshProUGUI frontCoinText;

    public AudioClip staticSound;

    public playerManager pManager;
    public powerUpManager pwManage;
    public boostManager modManage;
    public playerCar playerCar;

    public int displayCarType;
    public Image displayBody;
    public Image displayWheelF;
    public Image displayWheelB;
    public Image displayWindow;
    public Image displayLivery;
    public Image displayMask;

    public float displayStartMPH;
    public float displayUpMPH;
    public float displayMoveTime;
    public float displayHealth;

    public float displayStartMPHMin;
    public float displayUpMPHMin;
    public float displayMoveTimeMin;
    public float displayHealthMin;
    public float displayStartMPHMax;
    public float displayUpMPHMax;
    public float displayMoveTimeMax;
    public float displayHealthMax;
    public GameObject displayStartMPHBar;
    public GameObject displayUpMPHBar;
    public GameObject displayMoveTimeBar;
    public GameObject displayHealthBar;
    public GameObject displayStartMPHBarChange;
    public GameObject displayUpMPHBarChange;
    public GameObject displayMoveTimeBarChange;
    public GameObject displayHealthBarChange;

    public Transform shopCategoryTransform;
    public Transform shopItemsTransform;
    public Button equipbutton;

    public Transform playerCategoryTransform;
    public Transform uiCategoryTransform;
    public Transform inventoryCategoryTransform;
    public Transform powerupCategoryTransform;

    public Transform playerItemsTransform;
    public Transform uiItemsTransform;
    public Transform modShopItemsTransform;
    public Transform inventoryItemsTransform;
    public Transform powerupItemsTransform;

    public GameObject itemButton;
    public List<Button> itemButtonList;
    public GameObject categoryButton;
    public GameObject powerupButton;
    public GameObject modButton;

    public Button playerCategoryButton;
    public List<Button> playerCategoryButtonList;
    public Button powerUpCategoryButton;
    public List<Button> powerUpCategoryButtonList;

    public GameObject modMenu;
    public GameObject modShop;
    public GameObject modInventory;

    public GameObject modItem;
    public Button modBuyButton;
    public Image modBuyBackround;
    public TextMeshProUGUI modBuyText;

    public GameObject modIconOBJ;
    public GameObject modIconCurr;

    public GameObject inventoryRow;
    public GameObject inventoryItem;

    public TextMeshProUGUI coinTextMain;
    public TextMeshProUGUI coinTextPlayer;
    public TextMeshProUGUI coinTextUI;
    public TextMeshProUGUI coinTextPowerup;
    public TextMeshProUGUI coinTextBoost;

    public TextMeshProUGUI coinTextModShop;
    public TextMeshProUGUI coinTextInventory;

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

        pManager = GameObject.Find("playerManager").GetComponent<playerManager>();
        pwManage = GameObject.Find("powerUpManager").GetComponent<powerUpManager>();
        modManage = GameObject.Find("modsManager").GetComponent<boostManager>();
        playerCar = GameObject.Find("playerCar").GetComponent<playerCar>();

        updateCoinText();

        cameraScale = mainCamera.GetComponent<cameraScaler>();
        float scaleFactor = ((cameraScale.cameraSize - 5) / 7.5f) + 1;
        controller.distMulti = scaleFactor;
        GameObject.Find("buildings").transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        GameObject.Find("Front Billboards").transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        GameObject.Find("score Blimp").transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        GameObject.Find("pause Plane").transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        GameObject.Find("Pause Plane").transform.localPosition = new Vector3(1.22f, 6.68f + (cameraScale.cameraHight / 2), 0);

        if (pManager.intro) // delete this to make the start animation again
        {
            startup(scaleFactor);
            pManager.intro = false; // makes it so it wont go in the start animation again without closing the app
        }
        else {
            canStart = true;
            controller.scoreBlimp.transform.position = new Vector3(-7.75f + (1 * scaleFactor) , 1.75f * (scaleFactor * 2.75f), 0);
            mainCamera.orthographicSize = cameraScale.cameraSize;
            mainCamera.transform.position = new Vector3(0, cameraScale.cameraHight, -10);
        }

        //shopButtonFunc(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        backroundColors();
        
        if (inStatic) { staticTimer -= Time.deltaTime; }

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
                    openMainStoreScreen();
                }
            }
        }

        if (!inPos)
        {
            storeAnimation();
        }

        if (intro)
        {
            introTimer += Time.deltaTime;
            if (introTimer > 2.0f)
            {
                startAnimation(2);
            }
        }
    }

    void backroundColors()
    {
        colorTimer += Time.deltaTime;

        shopTextBackround.color = shopTextColor(6);
        shopTextOutline.color = shopOutlineColor(12);
        backround.color = shopBackroundColor(12);

        displayCoins(1.5f);
        backroundCoins(2);
        backroundstars(0.25f);
    }

    void displayCoins(float cycleTime)
    {
        float sizeScale = 0.975f + getValueScale(Mathf.Abs(colorTimer % cycleTime - (cycleTime / 2)), 0, (cycleTime / 2), 0.065f);

        shopTextBackround.transform.localScale = new Vector3(sizeScale, sizeScale, 1);
        shopTextOutline.transform.localScale = new Vector3(sizeScale * 1.005f, (sizeScale) * 1.005f, 1);

        coinTextColor(cycleTime);
    }

    void backroundCoins(float cycleTime)
    {
        backCoinTimer += Time.deltaTime;
        if (backCoinTimer > cycleTime)
        {
            for (int i = -50; i < 50; i += 4)
            {
                Transform t = Instantiate(backCoin, coinAni).transform;
                t.localPosition = new Vector3(i * 4, 22, 0);
            }
            backCoinTimer -= cycleTime;
        }
    }

    void backroundstars(float cycleTime)
    {
        starTimer += Time.deltaTime;
        if (starTimer > cycleTime)
        {
            Transform t = Instantiate(starGlimmer, stars).transform;
            t.localPosition = new Vector3(Random.Range(-45, 10), Random.Range(-15, 5), 0);
            starTimer -= cycleTime;
        }
    }

    Color32 shopTextColor(float colorCycle)
    {
        float colorVarR = 0;
        float colorVarG = 0;
        float colorVarB = 0;
        if (colorTimer % colorCycle < (colorCycle / 6) * 1)
        {
            colorVarR = 225 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 0, (colorCycle / 6) * 1, 30);
            colorVarG = 225;
            colorVarB = 255;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 2)
        {
            colorVarR = 255;
            colorVarG = 225;
            colorVarB = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 1, (colorCycle / 6) * 2, 30);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 3)
        {
            colorVarR = 255;
            colorVarG = 225 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 2, (colorCycle / 6) * 3, 30);
            colorVarB = 225;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 4)
        {
            colorVarR = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 3, (colorCycle / 6) * 4, 30);
            colorVarG = 255;
            colorVarB = 225;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 5)
        {
            colorVarR = 225;
            colorVarG = 255;
            colorVarB = 225 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 4, (colorCycle / 6) * 5, 30);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 6)
        {
            colorVarR = 225;
            colorVarG = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 5, (colorCycle / 6) * 6, 30);
            colorVarB = 255;
        }

        return new Color32((byte)colorVarR, (byte)colorVarG, (byte)colorVarB, 255);
    }

    Color32 shopOutlineColor(float colorCycle)
    {
        float colorVarR = 0;
        float colorVarG = 0;
        float colorVarB = 0;
        if (colorTimer % colorCycle < (colorCycle / 6) * 1)
        {
            colorVarR = 0 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 0, (colorCycle / 6) * 1, 255);
            colorVarG = 0;
            colorVarB = 255;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 2)
        {
            colorVarR = 255;
            colorVarG = 0;
            colorVarB = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 1, (colorCycle / 6) * 2, 255);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 3)
        {
            colorVarR = 255;
            colorVarG = 0 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 2, (colorCycle / 6) * 3, 255);
            colorVarB = 0;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 4)
        {
            colorVarR = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 3, (colorCycle / 6) * 4, 255);
            colorVarG = 255;
            colorVarB = 0;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 5)
        {
            colorVarR = 0;
            colorVarG = 255;
            colorVarB = 0 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 4, (colorCycle / 6) * 5, 255);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 6)
        {
            colorVarR = 0;
            colorVarG = 255 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 5, (colorCycle / 6) * 6, 255);
            colorVarB = 255;
        }

        return new Color32((byte)colorVarR, (byte)colorVarG, (byte)colorVarB, 255);
    }

    Color32 shopBackroundColor(float colorCycle)
    {
        float colorVarR = 0;
        float colorVarG = 0;
        float colorVarB = 0;
        if (colorTimer % colorCycle < (colorCycle / 6) * 1)
        {
            colorVarR = 230 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 0, (colorCycle / 6) * 1, 80);
            colorVarG = 230;
            colorVarB = 150;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 2)
        {
            colorVarR = 150;
            colorVarG = 230;
            colorVarB = 150 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 1, (colorCycle / 6) * 2, 80);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 3)
        {
            colorVarR = 150;
            colorVarG = 230 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 2, (colorCycle / 6) * 3, 80);
            colorVarB = 230;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 4)
        {
            colorVarR = 150 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 3, (colorCycle / 6) * 4, 80);
            colorVarG = 150;
            colorVarB = 230;
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 5)
        {
            colorVarR = 230;
            colorVarG = 150;
            colorVarB = 230 - getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 4, (colorCycle / 6) * 5, 80);
        }
        else if (colorTimer % colorCycle < (colorCycle / 6) * 6)
        {
            colorVarR = 230;
            colorVarG = 150 + getValueScale(colorTimer % colorCycle, (colorCycle / 6) * 5, (colorCycle / 6) * 6, 80);
            colorVarB = 150;
        }

        return new Color32((byte)colorVarR, (byte)colorVarG, (byte)colorVarB, 255);
    }

    void coinTextColor(float cycleTime)
    {
        if (inStatic || inStore)
        {
            frontCoinText.color = new Color32(0, 0, 0, 0);
        }
        else
        {
            if (controller.playing)
            {
                float a = 255 - getValueScale(getValueRanged(controller.mph, 0, playerCar.startMph), 0, playerCar.startMph, 255);
                frontCoinText.color = new Color32(255, 255, 255, (byte)a);
                frontCoinImage.color = new Color32(255, 255, 255, (byte)a);
            }
            else if (controller.isOver)
            {
                frontCoinText.color = new Color32(255, 255, 255, 0);
                frontCoinImage.color = new Color32(255, 255, 255, 0);
            }
            else
            {
                frontCoinText.color = new Color32(255, 255, 255, 255);
            }
            Color32 topLeft = new Color32(150, (byte)(255 - getValueScale(Mathf.Abs(colorTimer % cycleTime - cycleTime / 2), 0, cycleTime / 2, 100)), 255, 255);
            Color32 topRight = new Color32((byte)(155 + getValueScale(Mathf.Abs(colorTimer % cycleTime - cycleTime / 2), 0, cycleTime / 2, 100)), 155, 255, 255);
            Color32 bottomLeft = new Color32(255, 155, (byte)(255 - getValueScale(Mathf.Abs(colorTimer % cycleTime - cycleTime / 2), 0, cycleTime / 2, 100)), 255);
            Color32 bottomRight = new Color32(155, (byte)(150 + getValueScale(Mathf.Abs(colorTimer % cycleTime - cycleTime / 2), 0, cycleTime / 2, 100)), 255, 255);
            frontCoinText.colorGradient = new TMPro.VertexGradient(topLeft, topRight, bottomLeft, bottomRight);
        }
    }

    void storeAnimation()
    {
        mainCamera.transform.position += new Vector3(targetSpeed.x * Time.deltaTime, targetSpeed.y * Time.deltaTime, 0);
        mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        if (targetSpeed.x < 0)
        {
            if (mainCamera.transform.position.x < targetPos.x)
            {
                mainCamera.transform.position = targetPos;
                inPos = true;
                mainCamera.orthographicSize = targetZoom;
                if (!inStore)
                {
                    canStart = true;
                    setMenuUI(true);
                }
                else
                {
                    menuSigns.SetActive(false);
                }
            }
        }
        else if (targetSpeed.x > 0)
        {
            if (mainCamera.transform.position.x > targetPos.x)
            {
                mainCamera.transform.position = targetPos;
                inPos = true;
                mainCamera.orthographicSize = targetZoom;
                if (!inStore)
                {
                    canStart = true;
                    setMenuUI(true);
                }
                else
                {
                    menuSigns.SetActive(false);
                }
            }
        }
        else
        {
            if (targetSpeed.y < 0)
            {
                if (mainCamera.transform.position.y < targetPos.y)
                {
                    mainCamera.transform.position = targetPos;
                    inPos = true;
                    mainCamera.orthographicSize = targetZoom;
                    if (!inStore)
                    {
                        canStart = true;
                        setMenuUI(true);
                    }
                    else
                    {
                        menuSigns.SetActive(false);
                    }
                }
            }
            else if (targetSpeed.y > 0)
            {
                if (mainCamera.transform.position.y > targetPos.y)
                {
                    mainCamera.transform.position = targetPos;
                    inPos = true;
                    mainCamera.orthographicSize = targetZoom;
                    if (!inStore)
                    {
                        canStart = true;
                        setMenuUI(true);
                    }
                    else
                    {
                        menuSigns.SetActive(false);
                    }
                }
            }
        }
    }

    public void startGame()
    {
        if (canStart)
        {
            controller.StartGame();
            shopButton.interactable = false;
            settingsButton.interactable = false;
            inventoryButton.interactable = false;
            staticTimer = 1f;
        }
    }

    void setMenuUI(bool val)
    {
        modSign.interactable = val;
        settingsSign.interactable = val;
        speedUI.SetActive(val);
    }

    public void shopButtonFunc(float speed)
    {
        if (!controller.playing)
        {
            AudioSource.PlayClipAtPoint(controller.clickSound, transform.position, controller.masterVol * controller.sfxVol);
            inStore = true;
            canStart = false;
            targetPos = new Vector3(4 * controller.distMulti, 3 * controller.distMulti, -10);
            targetSpeed = setTargetSpeed(targetPos, speed, mainCamera.transform.position);
            inPos = false;
            targetZoom = cameraScaler.getScale(6.75f * controller.distMulti);
            zoomSpeed = -(mainCamera.orthographicSize - targetZoom) / speed;
            buttonCoverImage.SetActive(true);
            playerCoverPage.SetActive(false);
            powerupCoverPage.SetActive(false);
            setMenuUI(false);
            statics.SetActive(true);
            staticTimer = 1f;
            inStatic = true;
            AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            updateCoinText();

            setBarConstraints();
        }
    }

    public void updateCoinText()
    {
        frontCoinText.text = controller.totalCoins.ToString();
        coinTextMain.text = controller.totalCoins.ToString();
        coinTextPlayer.text = controller.totalCoins.ToString();
        coinTextUI.text = controller.totalCoins.ToString();
        coinTextBoost.text = controller.totalCoins.ToString();
        coinTextPowerup.text = controller.totalCoins.ToString();
        coinTextModShop.text = controller.totalCoins.ToString();
        coinTextInventory.text = controller.totalCoins.ToString();
    }

    public void openMainStoreScreen()
    {
        mainCoverPage.SetActive(true);
        playerCoverPage.SetActive(false);
        powerupCoverPage.SetActive(false);
        uiCoverPage.SetActive(false);
        boostsCoverPage.SetActive(false);

    }

    public void openPlayerCarCustomazation()
    {
        mainCoverPage.SetActive(false);
        playerCoverPage.SetActive(true);
        powerupCoverPage.SetActive(false);
        uiCoverPage.SetActive(false);
        boostsCoverPage.SetActive(false);

        shopCategoryTransform = playerCategoryTransform;
        shopItemsTransform = playerItemsTransform;
        getPlayerItemButtons(0);
    }

    public void openPowerupUpgrades()
    {
        mainCoverPage.SetActive(false);
        playerCoverPage.SetActive(false);
        powerupCoverPage.SetActive(true);
        uiCoverPage.SetActive(false);
        boostsCoverPage.SetActive(false);

        shopCategoryTransform = powerupCategoryTransform;
        shopItemsTransform = powerupItemsTransform;

        createPowerupButtons();
    }

    public void openUICustomazation()
    {
        mainCoverPage.SetActive(false);
        playerCoverPage.SetActive(false);
        powerupCoverPage.SetActive(false);
        uiCoverPage.SetActive(true);
        boostsCoverPage.SetActive(false);

        shopCategoryTransform = uiCategoryTransform;
        shopItemsTransform = uiItemsTransform;
    }

    public void openBoosts()
    {
        mainCoverPage.SetActive(false);
        playerCoverPage.SetActive(false);
        powerupCoverPage.SetActive(false);
        uiCoverPage.SetActive(false);
        boostsCoverPage.SetActive(true);

        openModMenu();
    }

    public Vector3 setTargetSpeed(Vector3 target, float speed, Vector3 currPos)
    {
        return new Vector3((target.x - currPos.x) / speed, (target.y - currPos.y) / speed, (target.z - currPos.z) / speed);
    }

    public void exitShop(float speed)
    {
        AudioSource.PlayClipAtPoint(controller.clickSound, transform.position, controller.masterVol * controller.sfxVol);
        inStore = false;
        targetPos = new Vector3(0, cameraScale.cameraHight, -10);
        targetSpeed = setTargetSpeed(targetPos, speed, mainCamera.transform.position);
        inPos = false;
        targetZoom = cameraScale.cameraSize;
        zoomSpeed = -(mainCamera.orthographicSize - targetZoom) / speed;
        buttonCoverImage.SetActive(false);
        menuSigns.SetActive(true);

        statics.SetActive(true);
        staticTimer = 1f;
        inStatic = true;
        AudioSource.PlayClipAtPoint(staticSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    void startAnimation(float speed)
    {
        intro = false;
        inStore = false;
        targetPos = new Vector3(0, cameraScale.cameraHight, -10);
        targetSpeed = setTargetSpeed(targetPos, speed, mainCamera.transform.position);
        inPos = false;
        targetZoom = cameraScale.cameraSize;
        zoomSpeed = -(mainCamera.orthographicSize - targetZoom) / speed;
    }

    void startup(float scale)
    {
        intro = true;
        introTimer = 0;
        controller.scoreBlimp.transform.position = new Vector3(-7.75f + (1 * scale), 1.75f * (scale * 2.85f), 0);
        mainCamera.transform.position = new Vector3(-7.75f + (1.75f * scale), 1.85f * (scale * 2.85f), -10);
        mainCamera.orthographicSize = cameraScaler.getScale(6.5f * scale);
        setMenuUI(false);
    }

    public void equipPlayerItem()
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
                    if (buyItem(pManager.carPartsData.carTypes[itemID].cost, pManager.carTypeUnlocks, pManager.carIDs[itemID] + "type"))
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
                    if (buyItem(pManager.carPartsData.windowTints[itemID].cost, pManager.windowUnlocks, pManager.windowIDs[itemID] + "window"))
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
                    if(buyItem(pManager.carPartsData.carTypes[PlayerPrefs.GetInt("playerCarType", 0)].bodies[itemID].cost, pManager.bodyUnlocks[PlayerPrefs.GetInt("playerCarType", 0)], pManager.carIDs[PlayerPrefs.GetInt("playerCarType", 0)] + pManager.bodyIDs[PlayerPrefs.GetInt("playerCarType", 0)][itemID] + "body"))
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
                    if (buyItem(pManager.carPartsData.wheelTypes[itemID].cost, pManager.wheelUnlocks, pManager.wheelIds[itemID] + "wheel"))
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
                    if (buyItem(pManager.carPartsData.liveryTypes[itemID].cost, pManager.liveryUnlocks, pManager.liveryIDs[itemID] + "livery"))
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
                    if (buyItem(pManager.carPartsData.liveryColors[itemID].cost, pManager.liveryColorUnlocks, pManager.liveryColorIDs[itemID] + "color"))
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
            getDisplayCarStats();
            updatePlayerItemButtons(catergoryID);
            updateCoinText();

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
            controller.statManage.setCoins(controller.totalCoins);
            save[itemID] = true;
            pManager.unlockItem(saveName);
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

    public void selectPlayerCustomizationItem(int buttonID, Image toChange, Sprite spriteChange, int cost, bool unlocked)
    {
        selectPlayerItemButtonFunc(buttonID);

        showDisplayCarStatsChange(buttonID);

        toChange.sprite = spriteChange;

        selectItemEquipFunc(cost, unlocked);
    }

    public void selectPlayerCustomizationItem(int buttonID, Image toChange, Image toChange2, Sprite spriteChange, int cost, bool unlocked)
    {
        selectPlayerItemButtonFunc(buttonID);

        toChange.sprite = spriteChange;
        toChange2.sprite = spriteChange;

        selectItemEquipFunc(cost, unlocked);
    }

    public void selectPlayerCustomizationItem(int buttonID, Image toChange, Color colorChange, int cost, bool unlocked)
    {
        selectPlayerItemButtonFunc(buttonID);

        toChange.color = colorChange;

        selectItemEquipFunc(cost, unlocked);
    }

    public void selectPlayerCustomizationItem(int buttonID, int cost, bool unlocked)
    {
        selectPlayerItemButtonFunc(buttonID);

        setDisplayCarType(buttonID);

        selectItemEquipFunc(cost, unlocked);
    }

    void selectItemEquipFunc(int cost, bool unlocked)
    {
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

    void selectPlayerItemButtonFunc(int buttonID)
    {
        itemID = buttonID;
        activeCategoryButton.interactable = true;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.pressedColor;
        activeCategoryButton = itemButtonList[buttonID].GetComponent<Button>();
        activeCategoryButton.interactable = false;
        activeCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = activeCategoryButton.colors.highlightedColor;

        showDisplayCarStatsChange(buttonID);
    }

    public void getPlayerItemButtons(int buttonID)
    {
        selectPlayerCustomizationCategories(buttonID, true);
    }

    public void updatePlayerItemButtons(int buttonID)
    {
        selectPlayerCustomizationCategories(buttonID, false);
    }

    public void selectPlayerCustomizationCategories(int buttonID, bool resetItemBar)
    {
        catergoryID = buttonID;
        switch (buttonID)
        {
            case 0:
                activatePlayerButtons(pManager.carIcon, pManager.carNames, PlayerPrefs.GetInt("playerCarType", 0), pManager.carPartsData.carTypes, pManager.carTypeUnlocks);
                break;
            case 1:
                activatePlayerButtons(pManager.windowColors, pManager.windowNames, window, displayWindow, PlayerPrefs.GetInt("windowTint", 0), pManager.carPartsData.windowTints, pManager.windowUnlocks);
                break;
            case 2:
                activatePlayerButtons(pManager.bodies[PlayerPrefs.GetInt("playerCarType", 0)], pManager.bodyNames[PlayerPrefs.GetInt("playerCarType", 0)], displayBody, PlayerPrefs.GetInt("playerBody", 0), pManager.carPartsData.carTypes[PlayerPrefs.GetInt("playerCarType", 0)].bodies, pManager.bodyUnlocks[PlayerPrefs.GetInt("playerCarType", 0)]);
                break;
            case 3:
                activatePlayerButtons(pManager.wheels, pManager.wheelNames, displayWheelF, displayWheelB, PlayerPrefs.GetInt("wheelBody", 0), pManager.carPartsData.wheelTypes, pManager.wheelUnlocks);
                break;
            case 4:
                activatePlayerButtons(pManager.livery, pManager.liveryNames, displayLivery, PlayerPrefs.GetInt("liveryTint", 0), pManager.carPartsData.liveryTypes, pManager.liveryUnlocks);
                break;
            case 5:
                activatePlayerButtons(pManager.liveryColors, pManager.liveryColorNames, pManager.livery[PlayerPrefs.GetInt("liveryTint", 0)], displayLivery, PlayerPrefs.GetInt("liveryColorTint", 0), pManager.carPartsData.liveryColors, pManager.liveryColorUnlocks);
                break;
        }

        playerCategoryButton.interactable = true;
        playerCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 180, 255);
        playerCategoryButton = playerCategoryButtonList[buttonID];
        playerCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(0, 125, 255, 255);
        playerCategoryButton.interactable = false;
        getDisplayCarStats();
    }

    public void activatePlayerButtons(List<Sprite> iconList, List<string> nameList, Image toChange, int currSelect, carPart[] cost, List<bool> activeList)
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
        activatePlayerButtonsFunc(currSelect);
    }

    public void activatePlayerButtons(List<Sprite> iconList, List<string> nameList, Image toChange, Image toChange2, int currSelect, carPart[] cost, List<bool> activeList)
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
        activatePlayerButtonsFunc(currSelect);
    }

    public void activatePlayerButtons(List<Color> iconList, List<string> nameList, Sprite icon, Image toChange, int currSelect, carPart[] cost, List<bool> activeList)
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
        activatePlayerButtonsFunc(currSelect);
    }

    public void activatePlayerButtons(List<Sprite> iconList, List<string> nameList, int currSelect, carPart[] cost, List<bool> activeList)
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
        activatePlayerButtonsFunc(currSelect);
    }

    void activatePlayerButtonsFunc(int currSelect)
    {
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

    Button createButton(Sprite img, string text, int id, Image toChange, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectPlayerCustomizationItem(id, toChange, img, cost, unlocked));
        createButtonFunc(img, text, newButton, cost, unlocked);
        return newButton;
    }

    Button createButton(Sprite img, string text, int id, Image toChange, Image toChange2, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectPlayerCustomizationItem(id, toChange, toChange2, img, cost, unlocked));
        createButtonFunc(img, text, newButton, cost, unlocked);

        return newButton;
    }

    Button createButton(Sprite img, string text, int id, Image toChange, Color col, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectPlayerCustomizationItem(id, toChange, col, cost, unlocked));
        createButtonFunc(img, text, newButton, cost, unlocked);

        return newButton;
    }

    Button createButton(Sprite img, string text, int id, int cost, bool unlocked)
    {
        Button newButton = Instantiate(itemButton, shopItemsTransform).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectPlayerCustomizationItem(id, cost, unlocked));
        createButtonFunc(img, text, newButton, cost, unlocked);

        return newButton;
    }

    void createButtonFunc(Sprite img, string text, Button newButton, int cost, bool unlocked)
    {
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
        int bod = 0;
        if(typeID == PlayerPrefs.GetInt("playerCarType", 0))
        {
            bod = PlayerPrefs.GetInt("playerBody", 0);
        }
        displayBody.sprite = pManager.bodies[typeID][bod];
        displayWindow.sprite = pManager.windows[typeID];
        displayMask.sprite = pManager.liveryMask[typeID];

        displayWheelF.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[typeID].displayWheelX1, pManager.carPartsData.carTypes[typeID].displayWheelY, 0);
        displayWheelB.transform.localPosition = new Vector3(pManager.carPartsData.carTypes[typeID].displayWheelX2, pManager.carPartsData.carTypes[typeID].displayWheelY, 0);
        float scale = pManager.carPartsData.carTypes[typeID].displayWheelScale;
        displayWheelF.rectTransform.sizeDelta = new Vector2(10, scale);
        displayWheelB.rectTransform.sizeDelta = new Vector2(10, scale);
    }

    void getDisplayCarStats()
    {
        displayStartMPH = playerCar.startMph;
        displayUpMPH = playerCar.upMph;
        displayMoveTime = playerCar.moveTime;
        displayHealth = playerCar.hits;

        float startBarVal = getValueScale(displayStartMPH, displayStartMPHMin, displayStartMPHMax, 0.5f);
        float upBarVal = getValueScale(displayUpMPH, displayUpMPHMin, displayUpMPHMax, 0.5f);
        float moveBarVal = getValueScale(displayMoveTime, displayMoveTimeMin, displayMoveTimeMax, 0.5f);
        float healthBarVal = getValueScale(displayHealth, displayHealthMin, displayHealthMax, 0.5f);

        displayStartMPHBar.transform.localScale = new Vector3(startBarVal, 0.5f, 0.5f);
        displayStartMPHBarChange.transform.localScale = new Vector3(startBarVal, 0.5f, 0.5f);
        displayUpMPHBar.transform.localScale = new Vector3(upBarVal, 0.5f, 0.5f);
        displayUpMPHBarChange.transform.localScale = new Vector3(upBarVal, 0.5f, 0.5f);
        displayMoveTimeBar.transform.localScale = new Vector3(moveBarVal, 0.5f, 0.5f);
        displayMoveTimeBarChange.transform.localScale = new Vector3(moveBarVal, 0.5f, 0.5f);
        displayHealthBar.transform.localScale = new Vector3(healthBarVal, 0.5f, 0.5f);
        displayHealthBarChange.transform.localScale = new Vector3(healthBarVal, 0.5f, 0.5f);
    }

    void showDisplayCarStatsChange(int typeID)
    { 
        switch (catergoryID)
        {
            case 0:
                if (typeID != PlayerPrefs.GetInt("playerCarType", 0))
                {
                    if (playerCar.calcStartMPH(typeID) < playerCar.startMph)
                    {
                        displayStartMPHBar.transform.localScale = new Vector3(getValueScale(playerCar.calcStartMPH(typeID), displayStartMPHMin, displayStartMPHMax, 0.5f), 0.5f, 0.5f);
                        displayStartMPHBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayStartMPHBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcStartMPH(typeID), displayStartMPHMin, displayStartMPHMax, 0.5f), 0.5f, 0.5f);
                        displayStartMPHBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }

                    if (playerCar.calcUpMPH(typeID, PlayerPrefs.GetInt("wheelBody", 0)) < playerCar.upMph)
                    {
                        displayUpMPHBar.transform.localScale = new Vector3(getValueScale(playerCar.calcUpMPH(typeID, PlayerPrefs.GetInt("wheelBody", 0)), displayUpMPHMin, displayUpMPHMax, 0.5f), 0.5f, 0.5f);
                        displayUpMPHBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayUpMPHBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcUpMPH(typeID, PlayerPrefs.GetInt("wheelBody", 0)), displayUpMPHMin, displayUpMPHMax, 0.5f), 0.5f, 0.5f);
                        displayUpMPHBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }

                    if (playerCar.calcmoveTime(typeID, PlayerPrefs.GetInt("wheelBody", 0)) > playerCar.moveTime)
                    {
                        displayMoveTimeBar.transform.localScale = new Vector3(getValueScale(playerCar.calcmoveTime(typeID, PlayerPrefs.GetInt("wheelBody", 0)), displayMoveTimeMin, displayMoveTimeMax, 0.5f), 0.5f, 0.5f);
                        displayMoveTimeBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayMoveTimeBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcmoveTime(typeID, PlayerPrefs.GetInt("wheelBody", 0)), displayMoveTimeMin, displayMoveTimeMax, 0.5f), 0.5f, 0.5f);
                        displayMoveTimeBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }
                    if (playerCar.calcHealth(typeID) < playerCar.hits)
                    {
                        displayHealthBar.transform.localScale = new Vector3(getValueScale(playerCar.calcHealth(typeID), displayHealthMin, displayHealthMax, 0.5f), 0.5f, 0.5f);
                        displayHealthBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayHealthBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcHealth(typeID), displayHealthMin, displayHealthMax, 0.5f), 0.5f, 0.5f);
                        displayHealthBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }
                }
                else
                {
                    getDisplayCarStats();
                }
                break;
            case 3:
                if (typeID != PlayerPrefs.GetInt("wheelBody", 0))
                {
                    if (playerCar.calcUpMPH(PlayerPrefs.GetInt("playerCarType", 0), typeID) < playerCar.upMph)
                    {
                        displayUpMPHBar.transform.localScale = new Vector3(getValueScale(playerCar.calcUpMPH(PlayerPrefs.GetInt("playerCarType", 0), typeID), displayUpMPHMin, displayUpMPHMax, 0.5f), 0.5f, 0.5f);
                        displayUpMPHBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayUpMPHBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcUpMPH(PlayerPrefs.GetInt("playerCarType", 0), typeID), displayUpMPHMin, displayUpMPHMax, 0.5f), 0.5f, 0.5f);
                        displayUpMPHBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }

                    if (playerCar.calcmoveTime(PlayerPrefs.GetInt("playerCarType", 0), typeID) > playerCar.moveTime)
                    {
                        displayMoveTimeBar.transform.localScale = new Vector3(getValueScale(playerCar.calcmoveTime(PlayerPrefs.GetInt("playerCarType", 0), typeID), displayMoveTimeMin, displayMoveTimeMax, 0.5f), 0.5f, 0.5f);
                        displayMoveTimeBarChange.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        displayMoveTimeBarChange.transform.localScale = new Vector3(getValueScale(playerCar.calcmoveTime(PlayerPrefs.GetInt("playerCarType", 0), typeID), displayMoveTimeMin, displayMoveTimeMax, 0.5f), 0.5f, 0.5f);
                        displayMoveTimeBarChange.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    }
                }
                else
                {
                    getDisplayCarStats();
                }
                break;
        }
    }

    public void createPowerupButtons()
    {
        foreach (Button i in powerUpCategoryButtonList) { Destroy(i.transform.parent.gameObject); }
        powerUpCategoryButtonList.Clear();
        int pwButons = (pwManage.powerupIDs.Count + 1) / 2;

        for(int i = 0; i < pwButons; i++)
        {
            Transform newRow = new GameObject("Botton Row " + i, typeof(RectTransform)).transform;
            newRow.GetComponent<RectTransform>().sizeDelta = new Vector2(28, 13);
            newRow.SetParent(shopCategoryTransform, false);
            createPwChatButton(i * 2, -6.9f, newRow);
            createPwChatButton((i * 2) + 1, 6.9f, newRow);
        }
        powerUpCategoryButton = powerUpCategoryButtonList[0];
        selectPowerups(0);
    }

    Button createPwChatButton(int id, float placeX, Transform newParent)
    {
        Button newButton = Instantiate(categoryButton, newParent).GetComponent<Button>();
        powerUpCategoryButtonList.Add(newButton);
        newButton.onClick.AddListener(() => selectPowerups(id));

        newButton.transform.localPosition = new Vector3(placeX, 0, 0);

        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = pwManage.powerupNames[id];
        if (pwManage.unlocks[id])
        {
            newButton.transform.Find("Image").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[id]][pwManage.tiers[id]];
        }
        else
        {
            newButton.transform.Find("Image").GetComponent<Image>().sprite = pwManage.bubbles[pwManage.powerupIDs[id]][0];
            ColorBlock cb = newButton.colors;
            cb.normalColor = new Color32(100, 100, 140, 255);
            newButton.colors = cb;
        }

        return newButton;
    }

    public void selectPowerups(int buttonID)
    {
        catergoryID = buttonID;
        int stdLen = pwManage.pwReader.standard.Length;
        if (buttonID < stdLen) { activateStandardPowerupButtons(buttonID, pwManage.tiers[buttonID]); }
        else { activatePremiumPowerupButtons(buttonID, pwManage.tiers[buttonID]); }

        powerUpCategoryButton.interactable = true;
        powerUpCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 180, 255);
        powerUpCategoryButton = powerUpCategoryButtonList[buttonID];
        powerUpCategoryButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(0, 125, 255, 255);
        powerUpCategoryButton.interactable = false;
    }

    public void activateStandardPowerupButtons(int powID, int currTier)
    {
        int numButtons = 4;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = createPowerupUpgradeButtons(powID, i);
            int newInt = i;
            newButton.onClick.AddListener(() => makeBig(newInt));
            itemButtonList.Add(newButton);
        }
        makeBig(currTier);
    }

    public void activatePremiumPowerupButtons(int powID, int currTier)
    {
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = -1; i < 3; i++)
        {
            Button newButton = createPowerupUpgradeButtons(powID, i);
            int newInt = i + 1;
            newButton.onClick.AddListener(() => makeBig(newInt));
            itemButtonList.Add(newButton);
        }

        if (pwManage.unlocks[powID]) { makeBig(currTier + 1); }
        else { makeBig(0); }
    }

    public Button createPowerupUpgradeButtons(int powID, int tier)
    {
        Button newUpgrade = Instantiate(powerupButton, shopItemsTransform).GetComponent<Button>();
        newUpgrade.transform.Find("Upgrade Button").gameObject.SetActive(false);
        if (tier > -1)
        {
            newUpgrade.transform.Find("Icon").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[powID]][tier];
            newUpgrade.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = pwManage.tierNames[powID][tier];
            newUpgrade.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = pwManage.tierDescription[powID][tier];
            if (pwManage.unlocks[powID])
            {
                if (tier > pwManage.tiers[powID])
                {
                    newUpgrade.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = pwManage.tierCosts[powID][tier].ToString();
                    if (tier == pwManage.tiers[powID] + 1)
                    {
                        if (tier < pwManage.tierCosts[powID].Count)
                        {
                            newUpgrade.transform.Find("Upgrade Button").GetComponent<Button>().onClick.AddListener(() => upgradePowerup(powID, pwManage.tierCosts[powID][tier]));
                        }
                        if (pwManage.tierCosts[powID][tier] < controller.totalCoins)
                        {
                            newUpgrade.GetComponent<Image>().color = new Color32(150, 255, 150, 255);
                            newUpgrade.transform.Find("Unlock Text").GetComponent<TextMeshProUGUI>().text = "Upgrade";
                        }
                        else
                        {
                            newUpgrade.GetComponent<Image>().color = new Color32(65, 90, 65, 255);
                            newUpgrade.transform.Find("Unlock Backround").GetComponent<Image>().color = new Color32(65, 90, 65, 255);
                            newUpgrade.transform.Find("Unlock Text").GetComponent<TextMeshProUGUI>().text = "Not Enough";
                        }
                    }
                    else
                    {
                        newUpgrade.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                        newUpgrade.transform.Find("Unlock Text").gameObject.SetActive(false);
                        newUpgrade.transform.Find("Unlock Backround").gameObject.SetActive(false);
                    }
                }
                else
                {
                    newUpgrade.transform.Find("Cost").gameObject.SetActive(false);
                    newUpgrade.transform.Find("Coin").gameObject.SetActive(false);
                    newUpgrade.transform.Find("Unlock Text").gameObject.SetActive(false);
                    newUpgrade.transform.Find("Unlock Backround").gameObject.SetActive(false);
                    if (tier == pwManage.tiers[powID])
                    {
                        newUpgrade.GetComponent<Image>().color = new Color32(65, 65, 215, 255);
                    }
                    else
                    {
                        newUpgrade.GetComponent<Image>().color = new Color32(198, 202, 255, 255);
                    }
                }
            }
            else
            {
                if (tier == 0)
                {
                    newUpgrade.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = pwManage.unlockCosts[powID].ToString();
                    if (pwManage.tierCosts[powID][tier] < controller.totalCoins)
                    {
                        newUpgrade.GetComponent<Image>().color = new Color32(150, 255, 150, 255);
                        newUpgrade.transform.Find("Unlock Text").GetComponent<TextMeshProUGUI>().text = "Buy";
                        newUpgrade.transform.Find("Upgrade Button").GetComponent<Button>().onClick.AddListener(() => buyPowerup(powID, pwManage.unlockCosts[powID]));
                    }
                    else
                    {
                        newUpgrade.GetComponent<Image>().color = new Color32(65, 90, 65, 255);
                        newUpgrade.transform.Find("Unlock Backround").GetComponent<Image>().color = new Color32(65, 90, 65, 255);
                        newUpgrade.transform.Find("Unlock Text").GetComponent<TextMeshProUGUI>().text = "Not Enough";
                    }
                }
                else
                {
                    newUpgrade.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    newUpgrade.transform.Find("Unlock Text").gameObject.SetActive(false);
                    newUpgrade.transform.Find("Unlock Backround").gameObject.SetActive(false);
                    newUpgrade.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = pwManage.tierCosts[powID][tier].ToString();

                }
            }
        }
        else
        {
            newUpgrade.transform.Find("Icon").GetComponent<Image>().sprite = pwManage.bubbles[pwManage.powerupIDs[powID]][0];
            newUpgrade.transform.Find("Cost").gameObject.SetActive(false);
            newUpgrade.transform.Find("Coin").gameObject.SetActive(false);
            newUpgrade.transform.Find("Unlock Text").gameObject.SetActive(false);
            newUpgrade.transform.Find("Unlock Backround").gameObject.SetActive(false);
            if (pwManage.unlocks[powID])
            {
                newUpgrade.GetComponent<Image>().color = new Color32(198, 202, 255, 255);
                newUpgrade.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Unlocked";
                newUpgrade.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = "You already unlocked this powerup";
            }
            else
            {
                newUpgrade.GetComponent<Image>().color = new Color32(65, 65, 215, 255);
                newUpgrade.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Locked";
                newUpgrade.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = "You must buy this powerup to use it";
            }
        }

        return newUpgrade;
    }

    public void makeBig(int buttonID)
    {
        foreach (Button i in itemButtonList)
        {
            i.transform.localScale = new Vector3(1, 1, 1);
            i.interactable = true;
            i.transform.Find("Upgrade Button").gameObject.SetActive(false);
        }

        itemButtonList[buttonID].transform.localScale = new Vector3(1.5f, 1.5f, 1);
        itemButtonList[buttonID].interactable = false;
        itemButtonList[buttonID].transform.Find("Upgrade Button").gameObject.SetActive(true);
        itemID = buttonID;
    }

    public void buyPowerup(int powID, int cost)
    {
        if (cost < controller.totalCoins)
        {
            controller.totalCoins -= cost;
            controller.statManage.setCoins(controller.totalCoins);
            updateCoinText();
            pwManage.unlockPowerUp(pwManage.powerupIDs[powID]);
            pwManage.setPowerupOdds();
            selectPowerups(catergoryID);

            powerUpCategoryButton.transform.Find("Image").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[catergoryID]][0];
            ColorBlock cb = powerUpCategoryButton.colors;
            cb.normalColor = new Color32(200, 200, 255, 255);
            powerUpCategoryButton.colors = cb;
        }
        else
        {
            youSure areYouSure = Instantiate(areYouSureToCreate, sideTransform).GetComponent<youSure>();
            areYouSure.methodToCall = buyMoreCoins;
            areYouSure.message = "Not enough coins. Would you like to buy some more?";
        }
    }

    public void upgradePowerup(int powID, int cost)
    {
        if (cost < controller.totalCoins)
        {
            controller.totalCoins -= cost;
            controller.statManage.setCoins(controller.totalCoins);
            updateCoinText();
            pwManage.upgradePowerUp(pwManage.powerupIDs[powID]);
            selectPowerups(catergoryID);

            powerUpCategoryButton.transform.Find("Image").GetComponent<Image>().sprite = pwManage.icons[pwManage.powerupIDs[catergoryID]][pwManage.tiers[catergoryID]];
        }
        else
        {
            youSure areYouSure = Instantiate(areYouSureToCreate, sideTransform).GetComponent<youSure>();
            areYouSure.methodToCall = buyMoreCoins;
            areYouSure.message = "Not enough coins. Would you like to buy some more?";
        }
    }

    public void openModMenu()
    {
        modMenu.SetActive(true);
        modInventory.SetActive(false);
        modShop.SetActive(false);

        if(modIconCurr != null)
        {
            addAmmountElements(modIconCurr.GetComponent<modShopIcon>().id);
            Destroy(modIconCurr);
        }
    }

    public void openModShop()
    {
        modMenu.SetActive(false);
        modInventory.SetActive(false);
        modShop.SetActive(true);

        shopItemsTransform = modShopItemsTransform;
        activateModStoreButtons();
    }

    public void openModInventory()
    {
        modMenu.SetActive(false);
        modInventory.SetActive(true);
        modShop.SetActive(false);

        shopItemsTransform = inventoryItemsTransform;
        createInventoryButtons();
    }

    public void activateModStoreButtons()
    {
        int numButtons = modManage.inventory.Count;
        foreach (Button i in itemButtonList)
        {
            Destroy(i.gameObject);
        }
        itemButtonList.Clear();

        for (int i = 1; i < numButtons; i++)
        {
            Button newButton = createModStoreButton(i, shopItemsTransform);
            itemButtonList.Add(newButton);
        }
        selectMods(1);
    }

    Button createModStoreButton(int id, Transform newParent)
    {
        Button newButton = Instantiate(modButton, newParent).GetComponent<Button>();
        newButton.onClick.AddListener(() => selectMods(id));

        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = modManage.modNames[id];

        int cost = modManage.modCosts[id];

        setModButtonColor(newButton, cost, id);

        newButton.transform.Find("Image").GetComponent<Image>().sprite = modManage.icons[id];
        newButton.transform.Find("Image").GetComponent<Image>().preserveAspect = true;

        TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        costText.text = cost.ToString();
        newButton.transform.Find("Coin").localPosition = new Vector3(7.3f - (cost.ToString().Length * 2.25f), 6, 0);

        return newButton;
    }

    public void setModButtonColor(Button newButton, int cost, int id)
    {
        if (cost < controller.totalCoins)
        {
            switch (modManage.modRarity[id])
            {
                case 1:
                    newButton.GetComponent<Image>().color = new Color32(175, 255, 175, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 205, 125, 255);
                    break;
                case 2:
                    newButton.GetComponent<Image>().color = new Color32(175, 175, 255, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 205, 255);
                    break;
                case 3:
                    newButton.GetComponent<Image>().color = new Color32(175, 0, 175, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 50, 205, 255);
                    break;
            }
        }
        else
        {
            switch (modManage.modRarity[id])
            {
                case 1:
                    newButton.GetComponent<Image>().color = new Color32(175, 205, 175, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 155, 125, 255);
                    break;
                case 2:
                    newButton.GetComponent<Image>().color = new Color32(175, 175, 205, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 165, 255);
                    break;
                case 3:
                    newButton.GetComponent<Image>().color = new Color32(175, 125, 175, 255);
                    newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(175, 155, 175, 255);
                    break;
            }
        }
    }

    public void selectMods(int buttonID)
    {
        itemID = buttonID;
        int cost = modManage.modCosts[buttonID];

        switch (modManage.modRarity[buttonID])
        {
            case 1:
                modItem.GetComponent<Image>().color = new Color32(175, 255, 175, 255);
                modItem.transform.Find("rarity color").GetComponent<Image>().color = new Color32(175, 255, 175, 255);
                modItem.transform.Find("rarity text").GetComponent<TextMeshProUGUI>().text = "Basic";
                break;
            case 2:
                modItem.GetComponent<Image>().color = new Color32(175, 175, 255, 255);
                modItem.transform.Find("rarity color").GetComponent<Image>().color = new Color32(175, 175, 255, 255);
                modItem.transform.Find("rarity text").GetComponent<TextMeshProUGUI>().text = "Rare";
                break;
            case 3:
                modItem.GetComponent<Image>().color = new Color32(175, 0, 175, 255);
                modItem.transform.Find("rarity color").GetComponent<Image>().color = new Color32(175, 0, 175, 255);
                modItem.transform.Find("rarity text").GetComponent<TextMeshProUGUI>().text = "Super";
                break;
        }
        
        modItem.transform.Find("mod icon").GetComponent<Image>().sprite = modManage.icons[buttonID];
        modItem.transform.Find("name text").GetComponent<TextMeshProUGUI>().text = modManage.modNames[buttonID];
        modItem.transform.Find("description text").GetComponent<TextMeshProUGUI>().text = modManage.modDescription[buttonID];

        TextMeshProUGUI costText = modItem.transform.Find("coins text").GetComponent<TextMeshProUGUI>();
        costText.text = cost.ToString();
        modItem.transform.Find("coin image").localPosition = new Vector3(-0.7f - (cost.ToString().Length * 0.95f), -10.75f, 0);

        setAmmountElements(buttonID);
    }

    public void setAmmountElements(int id)
    {
        int cost = modManage.modCosts[id];
        int ammount = modManage.inventory[id];
        modBuyText.text = ammount.ToString();

        if (ammount >= 99)
        {
            modBuyBackround.color = new Color32(5, 5, 5, 255);
        }
        else
        {
            modBuyBackround.color = new Color32(100, 100, 100, 255);
        }

        if (cost < controller.totalCoins && ammount < 100)
        {
            modBuyButton.GetComponent<Image>().color = new Color32(0, 255, 150, 255);
        }
        else
        {
            modBuyButton.GetComponent<Image>().color = new Color32(115, 175, 150, 255);
        }
    }

    public void addAmmountElements(int id)
    {
        int cost = modManage.modCosts[id];
        modManage.addMod(id);
        int ammount = modManage.inventory[id];
        modBuyText.text = ammount.ToString();

        if (ammount >= 99)
        {
            modBuyBackround.color = new Color32(5, 5, 5, 255);
        }
        else
        {
            modBuyBackround.color = new Color32(100, 100, 100, 255);
        }

        if (cost < controller.totalCoins && ammount < 100)
        {
            modBuyButton.GetComponent<Image>().color = new Color32(0, 255, 150, 255);
        }
        else
        {
            modBuyButton.GetComponent<Image>().color = new Color32(115, 175, 150, 255);
        }
    }

    public void buyMod()
    {
        int cost = modManage.modCosts[itemID];
        int ammount = modManage.inventory[itemID];
        if (cost < controller.totalCoins && ammount < 100)
        {
            controller.totalCoins -= cost;
            controller.statManage.setCoins(controller.totalCoins);
            updateCoinText();

            modShopIcon mIcon = Instantiate(modIconOBJ, modItem.transform).GetComponent<modShopIcon>();
            mIcon.setIcon(modManage.icons[itemID], this, modBuyBackround.transform, itemID);
            modIconCurr = mIcon.gameObject;

            for(int i = 1; i < itemButtonList.Count; i++)
            {
                setModButtonColor(itemButtonList[i-1], modManage.modCosts[i], i);
            }
        }
        else
        {
            youSure areYouSure = Instantiate(areYouSureToCreate, sideTransform).GetComponent<youSure>();
            areYouSure.methodToCall = buyMoreCoins;
            areYouSure.message = "Not enough coins. Would you like to buy some more?";
        }
    }

    public void createInventoryButtons()
    {
        foreach (Button b in itemButtonList)
        {
            Destroy(b.gameObject);
        }
        itemButtonList.Clear();

        List<int> items = new List<int>();
        for (int j = 1; j < modManage.inventory.Count; j++)
        {
            for (int k = 0; k < modManage.inventory[j]; k++)
            {
                items.Add(j);
            }
        }

        int i = 0;
        while (i < items.Count)
        {
            Transform newRow = Instantiate(inventoryRow, shopItemsTransform).transform;
            for (int j = 0; j < 7; j++)
            {
                if (i < items.Count)
                {
                    Button newButton = createInventoryItem(items[i], newRow);
                    itemButtonList.Add(newButton);
                    i++;
                }
            }
        }
    }

    public Button createInventoryItem(int id, Transform paren)
    {
        Button newButton = Instantiate(inventoryItem, paren).GetComponent<Button>();

        switch (modManage.modRarity[id])
        {
            case 1:
                newButton.GetComponent<Image>().color = new Color32(175, 255, 175, 255);
                newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 205, 125, 255);
                break;
            case 2:
                newButton.GetComponent<Image>().color = new Color32(175, 175, 255, 255);
                newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 125, 205, 255);
                break;
            case 3:
                newButton.GetComponent<Image>().color = new Color32(175, 0, 175, 255);
                newButton.transform.Find("Text Backround").GetComponent<Image>().color = new Color32(125, 50, 205, 255);
                break;
        }
        newButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = modManage.modNames[id];
        newButton.transform.Find("mod icon").GetComponent<Image>().sprite = modManage.icons[id];

        return newButton;
    }

    public void buyMoreCoins()
    {
        buyCoinsCoverPage.SetActive(true);
    }

    public void closeBuyCoins()
    {
        buyCoinsCoverPage.SetActive(false);
    }


    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    float getValueRanged(float val, float min, float max)
    {
        float newVal = val;
        if (newVal > max) { newVal = max; } else if (val < min) { newVal = min; }
        return newVal;
    }

    void setBarConstraints()
    {
        displayStartMPHMin = 15;
        displayStartMPHMax = 60;
        displayUpMPHMin = 0.3f;
        displayUpMPHMax = 1.4f;
        displayMoveTimeMin = 1.2f;
        displayMoveTimeMax = 0.25f;
        displayHealthMin = 0.5f;
        displayHealthMax = 3.5f;
    }

    void addCoins(int ammount)
    {
        controller.totalCoins += ammount;
    }

    void debugCommands()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            addCoins(1000);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            pManager.resetUnlocks();
        }
    }
}