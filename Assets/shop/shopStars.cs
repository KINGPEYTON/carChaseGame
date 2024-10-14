using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopStars : MonoBehaviour
{
    public float lifeTime;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(255 - getValueScale(getValueRanged(Mathf.Abs(lifeTime - 0.5f), 0, 0.5f), 0, 0.5f, 255)));
        float newScale = getValueScale(lifeTime, 0, 2, 3.5f);
        transform.localScale = new Vector3(newScale, newScale, 1);
        lifeTime += Time.deltaTime;
        if(lifeTime > 1) { Destroy(gameObject); }
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
