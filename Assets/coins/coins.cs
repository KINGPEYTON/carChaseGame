using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour
{
    public main controller;
    public Transform speedometer;

    public int value;

    public bool collected;
    public Vector3 collectedpos;
    public float collectedTimer;

    public AudioClip[] pickupsSFX;
    public AudioClip[] collectsSFX;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedometer = GameObject.Find("Speedometer").transform;

        setLane();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collected)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.GetComponent<main>().mph, 0, 0); //moves coin across the screen
            if (transform.position.x <= -12) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collectedTimer > 1)
            {
                collect();
            }
            else
            {
                transform.position = calcCollectPos();
                transform.localScale = calcCollectScale();
                collectedTimer += Time.unscaledDeltaTime;
            }
        }
    }

    void setLane()
    {
        int lane = Mathf.Abs((int)((transform.position.y / 1.25f) - 0.65f));
        GetComponent<SpriteRenderer>().sortingOrder = 2 + lane;
    }

    void collect()
    {
        controller.collectCoin(value);
        AudioSource.PlayClipAtPoint(collectsSFX[Random.Range(0, collectsSFX.Length - 1)], new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        Destroy(gameObject);
    }

    Vector3 calcCollectPos()
    {
        return new Vector3(speedometer.position.x + ((collectedpos.x - speedometer.position.x) * (1 - collectedTimer)), speedometer.position.y + ((collectedpos.y - speedometer.position.y) * (1 - collectedTimer)), 0);
    }

    Vector3 calcCollectScale()
    {
        return new Vector3((1.5f - (collectedTimer * 1.5f)) + 0.5f, (1.5f - (collectedTimer * 1.5f)) + 0.5f, 0);
    }

    public void pickup()
    {
        collected = true;
        collectedpos = transform.position;
        AudioSource.PlayClipAtPoint(pickupsSFX[Random.Range(0, pickupsSFX.Length - 1)], new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        GetComponent<SpriteRenderer>().sortingOrder = 55;
    }
}
