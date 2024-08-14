using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour
{
    public main controller;
    public Transform speedometer;
    public Animator ani;
    public SpriteRenderer sr;

    public int value;
    public bool isHolo;

    public bool collected;
    public Vector3 collectedpos;

    public Vector3 collectedStartSize;
    public float collectedSize;
    public float collectedTimer;
    public float collectedTime;

    public Transform attractTarget;
    public Vector3 attractStart;
    public float attractTime;

    public GameObject outlineOBJ;

    public bool makingHolo;
    public bool makingNormal;
    public float transitionTimer;
    public float startSize;
    public float targetBigSize;

    public AudioClip[] pickupsSFX;
    public AudioClip[] collectsSFX;

    public float hitboxSizeX;
    public float hitboxSizeY;
    public BoxCollider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedometer = GameObject.Find("Speedometer").transform;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();

        hitboxSizeX = hitbox.size.x;
        hitboxSizeY = hitbox.size.y;

        controller.coinList.Add(gameObject);
        setLane();
        collectedTime = 1.25f;
        collectedSize = 0.001f;
        attractTime = 0.75f;

        startSize = 0.04f;
        targetBigSize = 0.065f;

        if (isHolo)
        {
            ani.Play("holoCoin");
            sr.color = new Color32(255, 200, 200, 255);

        }
        else if (controller.isBigCoinhuna)
        {
            makeHolo();
        }

        if (controller.senseVision) { createOuline(controller.enhancedSense); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!collected)
        {
            if (attractTarget == null)
            {
                transform.position -= new Vector3(Time.deltaTime / 6 * controller.GetComponent<main>().mph, 0, 0); //moves coin across the screen
            }
            else
            {
                getAttracted();
            }
            if (transform.position.x <= -12) //checks if its offscreen
            {
                destroyCoin();
            }

            if(makingHolo || makingNormal) { transitionAnimation(); }
        }
        else
        {
            if (collectedTimer > collectedTime)
            {
                collect();
            }
            else
            {
                getCollected();
            }
        }
    }

    void setLane()
    {
        int lane = Mathf.Abs((int)((transform.position.y / 1.25f) - 0.65f));
        GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
    }

    void getAttracted()
    {
        Vector3 dis = new Vector3(attractTarget.position.x - attractStart.x, attractTarget.position.y - attractStart.y, 0);
        transform.position = calcPos(dis, attractStart, collectedTimer, attractTime);
        collectedTimer += Time.deltaTime;
    }

    void getCollected()
    {
        Vector3 dis = new Vector3(speedometer.position.x - collectedpos.x, speedometer.position.y - collectedpos.y, 0);
        transform.position = calcPos(dis, collectedpos, collectedTimer, collectedTime);
        Vector3 size = new Vector3(collectedSize - collectedStartSize.x, collectedSize - collectedStartSize.y, 1);
        transform.localScale = calcPos(size, collectedStartSize, collectedTimer, collectedTime);
        collectedTimer += Time.deltaTime;
    }

    void collect()
    {
        controller.collectCoin(value);
        AudioSource.PlayClipAtPoint(collectsSFX[Random.Range(0, collectsSFX.Length - 1)], new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        destroyCoin();
    }

    void destroyCoin()
    {
        controller.coinList.Remove(gameObject);
        if (controller.senseVision) { controller.enhancedSense.coinsOutline.Remove(outlineOBJ); }
        Destroy(gameObject);
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
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

    public void pickup()
    {
        collected = true;
        collectedpos = transform.position;
        collectedStartSize = transform.localScale;
        collectedTimer = 0;
        AudioSource.PlayClipAtPoint(pickupsSFX[Random.Range(0, pickupsSFX.Length - 1)], new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        GetComponent<SpriteRenderer>().sortingOrder = 55;
    }

    public void attract(Transform attTarget)
    {
        attractTarget = attTarget;
        attractStart = transform.position;
    }

    public void makeHolo()
    {
        ani.Play("holoCoin", 0, ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        value = 2;
        sr.color = new Color32(255, 200, 200, 255);

    }

    public void makeNormal()
    {
        ani.Play("coin", 0, ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        value = 1;
        sr.color = new Color32(255, 255, 255, 255);
    }

    void transitionAnimation() {
        if (transitionTimer < 0.35f)
        {
            Vector3 size = new Vector3(targetBigSize - startSize, targetBigSize - startSize, 1);
            transform.localScale = calcPos(size, new Vector3(startSize, startSize, 1), transitionTimer, 1);
            transitionTimer += Time.deltaTime;
            if(transitionTimer > 0.35f)
            {
                if (makingHolo) { makeHolo(); } else { makeNormal(); }
            }
        }
        else
        {
            Vector3 size = new Vector3(startSize - targetBigSize, startSize - targetBigSize, 1);
            transform.localScale = calcPos(size, new Vector3(targetBigSize, targetBigSize, 1), transitionTimer, 1);
            transitionTimer += Time.deltaTime;
            if(transitionTimer > 0.7f)
            {
                transform.localScale = new Vector3(startSize, startSize, 1);
                makingHolo = false;
                makingNormal = false;
            }
        }
    }

    public virtual void createOuline(sense sen)
    {
        GameObject newOutline = new GameObject("Sense Outline", typeof(SpriteRenderer), typeof(Animator));
        outlineOBJ = newOutline;
        newOutline.GetComponent<Animator>().runtimeAnimatorController = ani.runtimeAnimatorController;
        newOutline.GetComponent<Animator>().Play("outline", 0, ani.GetCurrentAnimatorStateInfo(0).normalizedTime);

        newOutline.transform.parent = transform;
        newOutline.GetComponent<SpriteRenderer>().sortingOrder = 151;

        newOutline.transform.parent = transform;
        newOutline.transform.localScale = new Vector3(1, 1, 1);
        newOutline.transform.localPosition = new Vector3(0, 0, 0);

        sen.coinsOutline.Add(newOutline);
        if (!(sen.doFadeIn || sen.doFadeOut))
        {
            if (isHolo)
            {
                newOutline.GetComponent<SpriteRenderer>().color = new Color32(0, 200, 0, 235);
            }
            else
            {
                newOutline.GetComponent<SpriteRenderer>().color = new Color32(180, 200, 0, 235);
            }
        }
        else
        {
            newOutline.GetComponent<SpriteRenderer>().color = new Color32(180, 200, 0, 0);
        }

        changeHitbox(1 / (sen.hitBoxSize));
        //changeHitbox(sen.pCar.hitboxSizeX * sen.hitBoxSize, sen.pCar.hitboxSizeY * sen.hitBoxSize);
    }

    public void changeHitbox(float multi)
    {
        hitbox.size = new Vector2(hitboxSizeX * multi, hitboxSizeY * multi);
    }
}
