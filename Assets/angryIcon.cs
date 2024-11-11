using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angryIcon : MonoBehaviour
{
    public float lifeTimer;
    public float targetScale;
    public float fadeTime;
    public bool inFade;
    public bool outFade;
    public float existTime;

    // Start is called before the first frame update
    void Start()
    {
        fadeTime = 0.25f;
        existTime = 0.85f;
        inFade = true;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        bounceAnimation();
    }

    void bounceAnimation()
    {
        if (inFade)
        {
            if (lifeTimer < fadeTime)
            {
                float newScale = getValueScale(lifeTimer, 0, fadeTime, targetScale);
                transform.localScale = new Vector3(newScale, newScale, 1);
            }
            else
            {
                inFade = false;
                transform.localScale = new Vector3(targetScale, targetScale, 1);
                lifeTimer = 0;
                GetComponent<Animator>().Play("angry ani");
            }
        }
        else if (outFade)
        {
            if (lifeTimer < fadeTime)
            {
                float newScale = targetScale - getValueScale(lifeTimer, 0, fadeTime, targetScale);
                transform.localScale = new Vector3(newScale, newScale, 1);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (lifeTimer > existTime)
            {
                outFade = true;
                lifeTimer = 0;
            }
        }

    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
