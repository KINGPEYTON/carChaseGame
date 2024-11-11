using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetField : MonoBehaviour
{
    public float lifetime;
    public bool isPerm;
    public Transform carPoint;
    public float sizeScale;
    public bool getHolo;

    public Transform tr;
    public float rayTimer;
    public float rayTime;
    public GameObject ray;

    public AudioClip getAttracted;
    public AudioClip fieldSound;
    public AudioSource sndSource;

    // Start is called before the first frame update
    void Start()
    {
        rayTime = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPerm)
        {
            lifetime -= Time.deltaTime;
        }
        if (lifetime < 0)
        {
            sndSource.clip = null;
            Destroy(gameObject);
        }

        rayTimer += Time.deltaTime;
        if(rayTimer > rayTime && lifetime > rayTime)
        {
            magnetRay newRay = Instantiate(ray, carPoint.position, Quaternion.identity, carPoint).GetComponent<magnetRay>();
            newRay.sizeScale = tr.localScale.x;
            rayTimer = 0;
        }
    }

    public void setSize(float newSize){
        sizeScale = newSize;
        tr = gameObject.transform;
        tr.localScale = new Vector3(sizeScale, sizeScale, 1);

        sndSource = GameObject.Find("secondAudio").GetComponent<AudioSource>();
        main controller = GameObject.Find("contoller").GetComponent<main>();
        sndSource.clip = fieldSound;
        sndSource.volume = controller.sfxVol * controller.masterVol;
        sndSource.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            coins hitCoin = collision.GetComponent<coins>();
            if (getHolo || !hitCoin.isHolo)
            {
                hitCoin.attract(carPoint);
                main controller = GameObject.Find("contoller").GetComponent<main>();
                AudioSource.PlayClipAtPoint(getAttracted, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            }
        }
    }
}
