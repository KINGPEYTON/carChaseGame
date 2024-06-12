using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour
{
    public main controller;
    public Transform speedometer;
    public Animator ani;

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


    public bool makingHolo;
    public bool makingNormal;
    public float transitionTimer;
    public float startSize;
    public float targetBigSize;

    public AudioClip[] pickupsSFX;
    public AudioClip[] collectsSFX;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedometer = GameObject.Find("Speedometer").transform;
        ani = GetComponent<Animator>();

        controller.coinList.Add(gameObject);
        setLane();
        collectedTime = 1.25f;
        collectedSize = 0.35f;
        attractTime = 0.75f;

        startSize = 2;
        targetBigSize = 3.25f;

        if (isHolo)
        {
            ani.Play("holoCoin");
        } else if (controller.isBigCoinhuna)
        {
            makeHolo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!collected)
        {
            if (attractTarget == null)
            {
                transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.GetComponent<main>().mph, 0, 0); //moves coin across the screen
            }
            else
            {
                getAttracted();
            }
            if (transform.position.x <= -12) //checks if its offscreen
            {
                controller.coinList.Remove(gameObject);
                Destroy(gameObject);
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
        controller.coinList.Remove(gameObject);
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

    }

    public void makeNormal()
    {
        ani.Play("coin", 0, ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        value = 1;
    }

    void transitionAnimation() {
        if (transitionTimer < 0.35f)
        {
            Vector3 size = new Vector3(targetBigSize - startSize, targetBigSize - startSize, 1);
            transform.localScale = calcPos(size, new Vector3(startSize, startSize, 1), transitionTimer, 1);
            transitionTimer += Time.deltaTime;
            if(transitionTimer > 0.35f)
            {
                if (makingHolo)
                {
                    makeHolo();
                }
                else
                {
                    makeNormal();
                }
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
}
