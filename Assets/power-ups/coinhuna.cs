using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinhuna : MonoBehaviour
{
    public main controller;

    public float lifetime;

    public AudioClip coinStart;
    public AudioClip coinRift;
    public AudioClip coinEnd;
    public AudioSource sndSource;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            controller.endCoinhuna();
            sndSource.clip = null;
            AudioSource.PlayClipAtPoint(coinEnd, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            Destroy(gameObject);
        }
    }

    public void setCoinhuna(float time, float spawnRate, bool makeCoinsHolo)
    {
        controller = GameObject.Find("contoller").GetComponent<main>();

        lifetime = time;
        controller.coinSpawnMultiplier *= spawnRate;
        controller.isBigCoinhuna = true;
        if (makeCoinsHolo)
        {
            controller.makeCoinsHolo();
        }

        AudioSource.PlayClipAtPoint(coinStart, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        sndSource = GameObject.Find("secondAudio").GetComponent<AudioSource>();
        sndSource.clip = coinRift;
        sndSource.volume = controller.sfxVol * controller.masterVol;
        sndSource.Play();
    }
}
