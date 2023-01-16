using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pausePlane : MonoBehaviour
{
    public Button myButton;
    public main controller;
    public bool inPos;

    public Vector3 planeSpeed;
    public Vector3 newPlaneLocation;

    public float maxX;
    public float minX;

    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
        inPos = false;
        maxX = 7.0f;
        controller = GameObject.Find("contoller").GetComponent<main>();

        planeSpeed = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0)
        {
            myButton.interactable = true;
        } else
        {
            myButton.interactable = false;
        }

        if (controller.playing)
        {
            if (inPos)
            {
                if ((transform.position.x > maxX || transform.position.x < minX) && Mathf.Abs(transform.position.x - newPlaneLocation.x) > 1.0) //if blimp went out of bounce on the X
                {
                    updateX(); // find new target location for blimp
                }
                if ((transform.position.y > 4.68f || transform.position.y < 4.46f) && Mathf.Abs(transform.position.y - newPlaneLocation.y) > 0.25) //if blimp went out of bounce on the X
                {
                    updateY(); // find new target location for blimp
                }
            }
            else if (transform.position.x > maxX)
            {
                inPos = true;
                planeSpeed = new Vector3(0.3f, 0.075f, 0);
            }
            else
            {
                planeSpeed = new Vector3(1.35f, -0.5f, 0);
            }
        }
        else if (controller.isOver)
        {
            gameOver();
        }

        transform.position += planeSpeed * Time.deltaTime;
    }

    void updateX()
    {
        if (planeSpeed.x < 0)
        {
            planeSpeed = new Vector3(Random.Range(0.25f, 0.6f), planeSpeed.y, 0);
            maxX = Random.Range(6.69f, 8.85f);
            minX = 4.46f;
        } else
        {
            planeSpeed = new Vector3(Random.Range(-0.15f, -0.3f), planeSpeed.y, 0);
            minX = Random.Range(4.46f, 6.54f);
            maxX = 8.85f;
        }
        newPlaneLocation = new Vector3(transform.position.x, newPlaneLocation.y, 0);
    }

    void updateY()
    {
        planeSpeed = new Vector3(planeSpeed.x, planeSpeed.y * -1, 0);
        newPlaneLocation = new Vector3(newPlaneLocation.x, transform.position.y, 0);
    }

    public void gameOver()
    {
        inPos = false;
        planeSpeed = new Vector3(1.6f, 0.65f, 0);
    }
}
