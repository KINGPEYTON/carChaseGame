using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public int powerUpId;
    public int typeID;

    public main controller;
    public powerUpManager pwManager;
    public Animator popAni;
    public float speed;

    public int tier;
    public List<Sprite> tierSkins;
    public Sprite icon;
    public GameObject iconOBJ;

    //animation stuff
    public bool bounceUp;
    public float startPoint;

    public bool popped;
    public float popTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.y;
        controller = GameObject.Find("contoller").GetComponent<main>();
        pwManager = GameObject.Find("powerUpManager").GetComponent<powerUpManager>();
        popAni = GetComponent<Animator>();
        popAni.enabled = false;
        speed = 7.5f;

        tier = pwManager.tiers[powerUpId];
        icon = pwManager.icons[powerUpId][tier];
        GetComponent<SpriteRenderer>().sprite = tierSkins[tier];

        checkChanges();
    }

    // Update is called once per frame
    void Update()
    {
        if (!popped)
        {
            bounceAnimation();

            if (controller.playing)
            {
                transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves guard across the screen
                if (transform.position.x <= -13) //checks if its offscreen
                {
                    Destroy(gameObject);
                }
            }
        } else
        {
            popTimer += Time.deltaTime;
            if (popTimer > 0.5) { Destroy(gameObject); }
        }
    }

    public void bounceAnimation()
    {
        if (bounceUp)
        {
            transform.position += new Vector3(0, 0.35f * (Time.deltaTime), 0);
            if (transform.position.y > startPoint + 0.1f)
            {
                bounceUp = false;
            }
        }
        else
        {
            transform.position += new Vector3(0, 0.35f * (Time.deltaTime * -1), 0);
            if (transform.position.y < startPoint - 0.1f)
            {
                bounceUp = true;
            }
        }
    }

    public void collect()
    {
        popped = true;
        GameObject iconThing = Instantiate(iconOBJ, transform.position, Quaternion.identity);
        iconThing.GetComponent<powerUpIcon>().id = powerUpId;
        iconThing.GetComponent<SpriteRenderer>().sprite = icon;
        setPopAnimation();
    }

    void setPopAnimation()
    {
        popAni.enabled = true;
        switch (typeID)
        {
            case 0:
                popAni.Play("std blue");
                break;
            case 1:
                popAni.Play("std red");
                break;
            case 2:
                popAni.Play("prem blue");
                break;
            case 3:
                popAni.Play("prem red");
                break;
        }
    }

    void checkChanges()
    {
        if(powerUpId == 6 && tier == 2)
        {
            typeID = 3;
        }
        else if(powerUpId == 10 && tier == 2)
        {
            typeID = 2;
        }
    }
}
