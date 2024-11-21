using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialText : MonoBehaviour
{
    public float bounceTimer;
    public float startPoint;

    // Update is called once per frame
    void Update()
    {
        bounceTimer += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startPoint + getValueScale(Mathf.Abs((bounceTimer % 2) - 1), 0, 2, 1f), 1);
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
