using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane_ad : MonoBehaviour
{
    public main controller;

    public Vector3 planeSpeed;
    public Vector3 newPlaneLocation;

    public GameObject adOBJ;
    public Sprite[] ads; //array of ads to appear

    // Start is called before the first frame update
    void Start()
    {

        controller = GameObject.Find("contoller").GetComponent<main>();

        planeSpeed = new Vector3(Random.Range(-0.95f, -1.85f), 0.075f, 0);

        adOBJ.GetComponent<SpriteRenderer>().sprite = ads[Random.Range(0, ads.Length)]; //set the ad to a random one at spawn
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.y > 4.68f || transform.position.y < 4.46f) && Mathf.Abs(transform.position.y - newPlaneLocation.y) > 0.25) //if plane went out of bounce on the Y
        {
            updateY(); // find new target location for blimp
        }

        transform.position += planeSpeed * Time.deltaTime;
    }

    void updateY()
    {
        planeSpeed = new Vector3(planeSpeed.x, planeSpeed.y * -1, 0);
        newPlaneLocation = new Vector3(newPlaneLocation.x, transform.position.y, 0);
    }
}
