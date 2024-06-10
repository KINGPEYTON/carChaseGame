using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetRay : MonoBehaviour
{
    public SpriteRenderer sr;
    public Transform tr;
    public byte grayColor;
    public byte colorAlpha;

    public float timer;
    public float lifetime;

    public float sizeScale;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tr = gameObject.transform;
        grayColor = 210;
        lifetime = 2.5f;

        tr.localScale = new Vector3(sizeScale, sizeScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime / sizeScale;
        if (timer < lifetime / 2)
        {
            fadeIn();
        }
        else
        {
            fadeOut();
        }
    }

    void fadeIn()
    {
        colorAlpha = (byte)getValueScale(timer, 0, lifetime/2, 255);
        sr.color = new Color32(grayColor, grayColor, grayColor, colorAlpha);
        float currScale = (1 * sizeScale) - getValueScale(timer, 0, lifetime / 2, 0.35f * sizeScale);
        tr.localScale = new Vector3(currScale, currScale, 1);
    }

    public void fadeOut()
    {
        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
        colorAlpha = (byte)(255 - getValueScale(timer, lifetime / 2, lifetime, 255));
        sr.color = new Color32(grayColor, grayColor, grayColor, colorAlpha);
        float currScale = (0.65f * sizeScale) - getValueScale(timer, lifetime/2, lifetime, 0.35f * sizeScale);
        tr.localScale = new Vector3(currScale, currScale, 1);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
