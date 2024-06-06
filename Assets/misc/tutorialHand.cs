using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialHand : MonoBehaviour
{
    public float bounceX;
    public float startPointX;
    public bool bounceDirX;
    public float speedX;

    public float bounceY;
    public float startPointY;
    public bool bounceDirY;
    public float speedY;

    // Start is called before the first frame update
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceDirX)
        {
            transform.position += new Vector3(bounceX * (Time.deltaTime * speedX), 0, 0);
            if (transform.position.x > startPointX + bounceX)
            {
                bounceDirX = false;
            }
        }
        else
        {
            transform.position += new Vector3(bounceX * (Time.deltaTime * (speedX * -2)), 0, 0);
            if (transform.position.x < startPointX - bounceX)
            {
                bounceDirX = true;
            }
        }

        if (bounceDirY)
        {
            transform.position += new Vector3(0, bounceY * (Time.deltaTime * speedY), 0);
            if (transform.position.y > startPointY + bounceY)
            {
                bounceDirY = false;
            }
        }
        else
        {
            transform.position += new Vector3(0, bounceY * (Time.deltaTime * (speedY * -2)), 0);
            if (transform.position.y < startPointY - bounceY)
            {
                bounceDirY = true;
            }
        }
    }

    public void setBounce(float x, float y, float rotation, float sX, float sY)
    {
        bounceX = x;
        bounceY = y;
        transform.eulerAngles = new Vector3(0, 0, rotation);
        speedX = sX;
        speedY = sY;
    }
}
