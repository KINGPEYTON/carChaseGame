using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modNewTitle : MonoBehaviour
{
    public Image mainBar;
    public float barTime;
    public float barTimer;

    // Update is called once per frame
    void Update()
    {
        if (barTimer < barTime)
        {
            GetComponent<Image>().fillAmount = getValueScale(barTimer, 0, barTime, 1);
            barTimer += Time.deltaTime;
            if(barTimer > barTime)
            {
                GetComponent<Image>().fillAmount = 1;
                mainBar.sprite = GetComponent<Image>().sprite;
            }
        }
        else
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(255 - getValueScale(barTimer, barTime, barTime * 2, 255)));
            barTimer += Time.deltaTime;
            if (barTimer > barTime * 2)
            {
                Destroy(gameObject);
            }
        }
    }

    public static void newModTitleOutline(Sprite newSprite, float newTime, Image mainOBJ)
    {
        GameObject nm = new GameObject("newOutline", typeof(modNewTitle), typeof(Image));
        nm.transform.parent = mainOBJ.gameObject.transform;
        nm.transform.localPosition = new Vector3(0, 0, 0);
        nm.transform.localScale = new Vector3(1, 1, 1);
        nm.GetComponent<RectTransform>().sizeDelta = mainOBJ.gameObject.GetComponent<RectTransform>().sizeDelta;
        nm.GetComponent<Image>().sprite = newSprite;
        nm.GetComponent<Image>().type = Image.Type.Filled;
        nm.GetComponent<Image>().fillMethod = Image.FillMethod.Radial90;
        nm.GetComponent<Image>().fillOrigin = (int)Image.Origin90.BottomRight;
        nm.GetComponent<modNewTitle>().mainBar = mainOBJ;
        nm.GetComponent<modNewTitle>().barTime = newTime;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
