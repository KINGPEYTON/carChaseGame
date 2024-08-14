using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carIcon : MonoBehaviour
{
    public cars carAttached;
    public sense sen;

    public bool doFadeIn;
    public bool doFadeOut;
    public float fadeVal;
    public float fadeTimer;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = carAttached.warningIcon;
        fadeTime = 0.5f;
        startFade(true);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (carAttached.transform.position.x < transform.position.x + 2 && !doFadeOut)
            {
                startFade(false);
            }

            transform.position = new Vector3(transform.position.x, carAttached.transform.position.y, 1);

            if (doFadeIn) { visionFadeIn(); }
            if (doFadeOut) { visionFadeOut(); }
        }
        catch
        {
            fadeAni(0);
            sen.carIcons.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void visionFadeIn()
    {
        fadeVal = (getValueScale(fadeTimer, 0, fadeTime, 235));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(235);
            doFadeIn = false;
        }
    }

    void visionFadeOut()
    {
        fadeVal = (235 - getValueScale(fadeTimer, 0, fadeTime, 235));
        fadeAni(fadeVal);
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(0);
            sen.carIcons.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void startFade(bool fIn)
    {
        fadeTimer = 0;
        if (fIn)
        {
            doFadeIn = true;
        }
        else
        {
            doFadeOut = true;
        }
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }

    void fadeAni(float value)
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)value);
    }
}
