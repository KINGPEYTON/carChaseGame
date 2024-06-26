using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carRam : MonoBehaviour
{
    public playerCar pCar;
    public main controller;
    public speedometer speedo;

    public float targX;
    public float targY;
    public Vector3 startPos;
    public float startTimer;
    public bool inPos;

    public int uses;
    public bool justCars;
    public bool headOn;

    public bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        pCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            Vector3 dis = new Vector3(targX - startPos.x, targY - startPos.y, 1);
            transform.localPosition = calcPos(dis, startPos, startTimer, 1);
            startTimer += Time.deltaTime;
            if (startTimer > 0.5f)
            {
                pCar.ramOn = true;
                GetComponent<SpriteRenderer>().sortingOrder = pCar.livery.sortingOrder;
            }
            if(startTimer > 1)
            {
                inPos = true;
                transform.localPosition = new Vector3(targX, targY, 1);
            }
        } else if (destroyed)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.mph, 0, 0); //moves guard across the screen
            if (transform.position.x <= -13) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
    }

    public void setTargetPos(float x, float y)
    {
        targX = x;
        targY = y;
    }

    public void ramHit()
    {
        uses--;
        speedo.usePowerUp(1);
        if(uses == 0)
        {
            destroyed = true;
            inPos = true;
            pCar.ramOn = false;
            transform.parent = null;
        }
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y) + ((targetTime * 2) - Mathf.Abs((targetTimer * 4) - (targetTime * 2)));
        return new Vector3(xVal, yVal, 0) + startScale;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
