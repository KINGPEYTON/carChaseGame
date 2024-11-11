using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tinyCars : MonoBehaviour
{
    public main controller;

    public float lifetime;

    public AudioClip tinySound;
    public AudioClip bigSound;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            controller.endTinyCars();
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(bigSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        }
    }

    public void setTinyCars(int time, bool hitBigCars)
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        lifetime = time;
        controller.inTinyCars = true;
        controller.startTinyCars(hitBigCars);
        AudioSource.PlayClipAtPoint(tinySound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }
}
