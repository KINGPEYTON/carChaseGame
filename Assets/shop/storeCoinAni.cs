using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeCoinAni : MonoBehaviour
{
    public Sprite holo;

    void Start()
    {
        int odds = Random.Range(0, 8);
        if(odds == 7)
        {
            Image img = GetComponent<Image>();
            img.sprite = holo;
            img.color = new Color32(255, 200, 200, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speedVar = 3;
        transform.localPosition += new Vector3(speedVar * Time.deltaTime, -speedVar * Time.deltaTime, 1);

        if(transform.localPosition.x > 50 || transform.localPosition.y < -30)
        {
            Destroy(gameObject);
        }
    }
}
