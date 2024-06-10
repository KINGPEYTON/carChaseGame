using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetField : MonoBehaviour
{
    public float lifetime;
    public Transform carPoint;
    public float sizeScale;

    public Transform tr;
    public float rayTimer;
    public float rayTime;
    public GameObject ray;

    // Start is called before the first frame update
    void Start()
    {
        rayTime = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }

        rayTimer += Time.deltaTime;
        if(rayTimer > rayTime && lifetime > rayTime)
        {
            magnetRay newRay = Instantiate(ray, carPoint.position, Quaternion.identity, transform).GetComponent<magnetRay>();
            newRay.sizeScale = tr.localScale.x;
            rayTimer = 0;
        }
    }

    public void setSize(float newSize){
        sizeScale = newSize;
        tr = gameObject.transform;
        tr.localScale = new Vector3(sizeScale, sizeScale, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            coins hitCoin = collision.GetComponent<coins>();
            hitCoin.attract(carPoint);
        }
    }
}
