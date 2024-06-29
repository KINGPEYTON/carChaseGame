using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpAni : MonoBehaviour
{
    public main controller;
    public float speed;
    public bool movingOut;
    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingOut)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves guard across the screen
            
            if (transform.position.x <= -15) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
    }

    public void makeMove()
    {
        movingOut = true;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void kill()
    {
        Destroy(gameObject);
    }
}
