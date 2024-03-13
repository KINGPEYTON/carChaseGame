using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildings : MonoBehaviour
{
    public main controller;

    public bool isBillboard;
    public bool isBigBillboard;

    public float speed;

    // Start is called before the first frame update
    public virtual void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.mph, 0, 0); //moves building across the screen
        if (transform.position.x <= -14) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }

    public void setSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }
}
