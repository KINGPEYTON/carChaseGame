using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpIcon : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 startSize;
    public bool sizeU = false;

    public Transform speedometer;
    public float targetTimer;

    public float targetTime;
    public float targetSize;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        startSize = transform.localScale;
        targetTime = 1.25f;
        targetSize = 0.35f;
        speedometer = GameObject.Find("Speedometer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetTimer += Time.deltaTime;

        if (!sizeU)
        {
            Vector3 size = new Vector3(targetSize - startSize.x, targetSize - startSize.y, 1);
            transform.localScale = calcPos(size, startSize) ;
            if (targetTimer > targetTime) { startSize = transform.localScale; sizeU = true; targetTimer = 0; }
        }
        else
        {
            runAwayIcon();
        }
    }

    void runAwayIcon()
    {
        Vector3 dis = new Vector3(speedometer.position.x - startPoint.x, speedometer.position.y - startPoint.y, 0);
        transform.position = calcPos(dis, startPoint);
        Vector3 size = new Vector3(startSize.x / 1.5f, startSize.y / 1.5f, 1);
        transform.localScale = new Vector3(startSize.x, startSize.y, 1) - calcPos(size, new Vector3(0, 0, 0));
        if (targetTimer > targetTime) { Destroy(gameObject); }
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
