using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cone : MonoBehaviour
{
    public main controller;
    public float speed;

    public Sprite outline;
    public SpriteRenderer outlineOBJ;
    public construction construc;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        Update();
        if (controller.senseVision) { createOuline(controller.enhancedSense); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -13) //checks if its offscreen
        {
            construc.coneList.Remove(gameObject);
            if (controller.senseVision) { controller.enhancedSense.conesOutline.Remove(outlineOBJ); }
            Destroy(gameObject);
        }
    }

    public virtual void createOuline(sense sen)
    {
        SpriteRenderer newOutline = new GameObject("Sense Outline", typeof(SpriteRenderer), typeof(Animator)).GetComponent<SpriteRenderer>();
        outlineOBJ = newOutline;
        newOutline.sprite = outline;

        newOutline.transform.parent = transform;
        newOutline.sortingOrder = 152;

        newOutline.transform.parent = transform;
        newOutline.transform.localScale = new Vector3(1, 1, 1);
        newOutline.transform.localPosition = new Vector3(0, 0, 0);

        sen.conesOutline.Add(outlineOBJ);
        if (!(sen.doFadeIn || sen.doFadeOut))
        {
            newOutline.color = new Color32(200, 50, 50, 235);
        }
        else
        {
            newOutline.color = new Color32(200, 50, 50, 0);
        }
    }
}
