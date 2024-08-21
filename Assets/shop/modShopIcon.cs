using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modShopIcon : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 startSize;

    public Transform ammountBackround;
    public float targetTimer;

    public float targetTime;

    public int id;
    public shopBillboard sBoard;
    public boostManager modMang;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        startSize = transform.localScale;
        targetTime = 0.75f;
        modMang = GameObject.Find("modsManager").GetComponent<boostManager>();
    }

    // Update is called once per frame
    void Update()
    {
        targetTimer += Time.deltaTime;

        runAwayIcon();
    }

    public void setIcon(Sprite icon, shopBillboard shop, Transform target, int newid)
    {
        GetComponent<SpriteRenderer>().sprite = icon;
        sBoard = shop;
        ammountBackround = target;
        id = newid;
    }

    void runAwayIcon()
    {
        Vector3 dis = new Vector3(ammountBackround.position.x - startPoint.x, ammountBackround.position.y - startPoint.y, 0);
        transform.position = calcPos(dis, startPoint);
        Vector3 size = new Vector3(startSize.x / 1.15f, startSize.y / 1.15f, 1);
        transform.localScale = new Vector3(startSize.x, startSize.y, 1) - calcPos(size, new Vector3(0, 0, 0));
        if (targetTimer > targetTime)
        {
            sBoard.addAmmountElements(id);
            Destroy(gameObject);
        }
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale)
    {
        float scaledTimer = getValueRanged(targetTimer, 0, targetTime);
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y);
        return new Vector3(xVal, yVal, 0) + startScale;
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
}
