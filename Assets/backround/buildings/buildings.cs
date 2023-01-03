using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildings : MonoBehaviour
{
    public GameObject controller;

    public bool isBillboard;
    public bool isBigBillboard;
    public Sprite[] skins; //array of building skins
    public Sprite[] billboards; //array of billboard skins
    public Sprite[] bigBillboards; //array of big billboard skins

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller");
        if (isBillboard)
        {
            GetComponent<SpriteRenderer>().sprite = billboards[Random.Range(0, billboards.Length)]; //set the skin to a random one at spawn
        }
        else if (isBigBillboard)
        {
            GetComponent<SpriteRenderer>().sprite = bigBillboards[Random.Range(0, bigBillboards.Length)]; //set the skin to a random one at spawn
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)]; //set the skin to a random one at spawn
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime / 12 * controller.GetComponent<main>().mph, 0, 0); //moves building across the screen
        //Debug.Log("Hello: ");
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
