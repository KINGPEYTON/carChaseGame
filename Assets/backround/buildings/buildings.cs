using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildings : MonoBehaviour
{
    public main controller;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        moreStart();
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves building across the screen
        if (transform.position.x <= -14) //checks if its offscreen
        {
            Destroy(gameObject);
        }
        moreUpdate();
    }

    public virtual void setSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }

    public virtual void moreStart()
    {

    }

    public virtual void moreUpdate()
    {

    }
}
