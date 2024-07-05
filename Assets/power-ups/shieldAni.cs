using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldAni : MonoBehaviour
{
    public Animator ani;

    public float lifeTimer;
    public float sizeTime;

    public bool doFadeIn;
    public bool doFadeOut;
    public float fadeTimer;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        sizeTime = 1.2f;
        fadeTime = 1.5f;

        startFade(true);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        float tScale = 0.95f + getValueScale(Mathf.Abs((lifeTimer % 2) - 1), 0, sizeTime, 0.15f);
        transform.localScale = new Vector3(tScale, tScale, 1);

        if (doFadeIn) { fadeIn(); }
        if (doFadeOut) { fadeOut(); }
    }

    public void startAni()
    {
        ani = GetComponent<Animator>();
        ani.Play("shield glow");
    }

    void fadeIn()
    {
        fadeAni(getValueScale(fadeTimer, 0, fadeTime, 200));
        fadeTimer += Time.deltaTime;
        if(fadeTimer > fadeTime)
        {
            fadeAni(200);
            doFadeIn = false;
        }
    }

    void fadeOut()
    {
        fadeAni(200 - getValueScale(fadeTimer, 0, fadeTime, 200));
        fadeTimer += Time.deltaTime;
        if (fadeTimer > fadeTime)
        {
            fadeAni(200);
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
