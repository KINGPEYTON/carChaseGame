using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane_big : MonoBehaviour
{
    public main controller;

    public Vector3 planeSpeed;
    public int direction;

    public AudioClip planeSound;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("contoller").GetComponent<main>();
        direction = Random.Range(0, 2);

        transform.localScale = new Vector3((-1 + (direction) * 2), 1, 1);
        transform.position = new Vector3(Random.Range(14.75f, 16.25f) * (-1 + (direction) * 2), Random.Range(4.65f, 5.5f), 0);
        planeSpeed = new Vector3(Random.Range(5.75f, 10.2f) * (1 - (direction) * 2), Random.Range(-0.15f, -0.4f), 0);
        AudioSource.PlayClipAtPoint(planeSound, new Vector3(0, 0, -8), controller.masterVol * controller.sfxVol);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += planeSpeed * Time.deltaTime;
        if(Mathf.Abs(transform.position.x) > 20)
        {
            Destroy(gameObject);
        }
    }
}
