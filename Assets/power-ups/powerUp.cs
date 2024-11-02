using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public string powerUpId;
    public int typeID;

    public main controller;
    public powerUpManager pwManager;
    public Animator popAni;
    public float speed;

    public int tier;
    public Sprite icon;
    public GameObject iconOBJ;

    //animation stuff
    public bool bounceUp;
    public float startPoint;
    public AudioClip popSound;

    public bool popped;
    public float popTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.y;
        controller = GameObject.Find("contoller").GetComponent<main>();
        popAni = GetComponent<Animator>();
        popAni.enabled = false;
        speed = 25.5f;

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
                transform.position -= new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves guard across the screen
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

    public void createPowerUp(string id, int tpId, Sprite newIcon, Sprite bubble, int level, powerUpManager pman, GameObject iconO)
    {
        powerUpId = id;
        typeID = tpId;
        tier = level;
        icon = newIcon;
        GetComponent<SpriteRenderer>().sprite = bubble;
        pwManager = pman;
        iconOBJ = iconO;
    }

    public void collect()
    {
        popped = true;
        powerUpIcon iconThing = Instantiate(iconOBJ, transform.position, Quaternion.identity).GetComponent<powerUpIcon>();
        iconThing.setIcon(icon, powerUpId, tier);
        setPopAnimation();
    }

    void bounceAnimation()
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

    void setPopAnimation()
    {
        AudioSource.PlayClipAtPoint(popSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
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
        if(powerUpId == "shield" && tier == 2)
        {
            typeID = 3;
        }
        else if(powerUpId == "teleport" && tier == 2)
        {
            typeID = 2;
        }
    }
}
