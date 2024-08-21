using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinhuna : MonoBehaviour
{
    public main controller;

    public float lifetime;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            controller.endCoinhuna();
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
    }
}
