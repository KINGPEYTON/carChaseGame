using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildings : MonoBehaviour
{
    public main controller;

    public bool isBillboard;
    public bool isBigBillboard;
    public Sprite[] skins; //array of building skins

    // Start is called before the first frame update
    public virtual void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
    }

    // Update is called once per frame
    public virtual void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / 12 * controller.mph, 0, 0); //moves building across the screen
        //Debug.Log("Hello: ");
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
