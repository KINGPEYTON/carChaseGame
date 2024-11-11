using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class police_icon : MonoBehaviour
{
    public float bounceTimer;
    public float bounceTime;

    // Start is called before the first frame update
    void Start()
    {
        bounceTime = 1.75f;
    }

    // Update is called once per frame
    void Update()
    {
        bounceTimer += Time.deltaTime;
        bounceAnimation();
    }

    void bounceAnimation()
    {
        transform.localPosition = new Vector3(0, 1.65f + getValueScale(Mathf.Abs((bounceTimer % bounceTime) - (bounceTime / 2)), 0, (bounceTime / 2), 0.35f), 0);

    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}
