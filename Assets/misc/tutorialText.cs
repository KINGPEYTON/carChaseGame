using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialText : MonoBehaviour
{
    public bool bounceUp;
    public float startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceUp)
        {
            transform.position += new Vector3(0, 1 * (Time.deltaTime), 0);
            if (transform.position.y > startPoint + 0.5f)
            {
                bounceUp = false;
            }
        }
        else
        {
            transform.position += new Vector3(0, 1 * (Time.deltaTime * -1), 0);
            if (transform.position.y < startPoint - 0.5f)
            {
                bounceUp = true;
            }
        }
    }
}
