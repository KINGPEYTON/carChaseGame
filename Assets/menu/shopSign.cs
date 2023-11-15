using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shopSign : MonoBehaviour
{
    public main controller;
    public TextMeshProUGUI coinText;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = controller.totalCoins.ToString();
        transform.position = transform.position - new Vector3(Time.deltaTime / speed * controller.GetComponent<main>().mph, 0, 0); //moves guard across the screen
        if (transform.position.x <= -13) //checks if its offscreen
        {
            Destroy(gameObject);
        }
    }
}
