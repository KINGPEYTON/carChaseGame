using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void methodType();

public class youSure : MonoBehaviour {

	public methodType methodToCall;

	public string message;
	public TextMeshProUGUI displayMessage;

	public Image cover;
	public GameObject sign;

	public bool inPos;
	public float targetPos;
	public float curPos;
	public float coverColor;

	public float speedTime;

	public AudioClip clickSound;

	// Use this for initialization
	void Start () {
		inPos = false;
		coverColor = 0;
		targetPos = 642f;
		curPos = -400.0f;

		speedTime = 5000f;
	}

	// Update is called once per frame
	void Update () {
		displayMessage.text = "Are you sure " + message;

        if (!inPos)
        {
			if(transform.position.y < targetPos)
            {
				transform.position = new Vector3(1389, curPos, 0);
				curPos += (Time.unscaledDeltaTime * speedTime);
			}
            else
            {
				transform.position = new Vector3(1389, targetPos, 0);
			}

			if (coverColor < 175)
            {
				cover.color = new Color32(36, 36, 36, (byte)coverColor);
				coverColor += (Time.unscaledDeltaTime * (speedTime/6));
			}
            else
            {
				cover.color = new Color32(36, 36, 36, 175);
			}
		}
        else
        {
			if (transform.position.y > targetPos)
			{
				transform.position = new Vector3(1389, curPos, 0);
				curPos -= (Time.unscaledDeltaTime * speedTime);
			}
            else
            {
				Destroy(gameObject);
			}
			if (coverColor > 0)
			{
				cover.color = new Color32(36, 36, 36, (byte)coverColor);
				coverColor -= (Time.unscaledDeltaTime * (speedTime/6));
			}
		}
	}

	private void SimpleMethod(methodType method){
		method();
	}

	public void yes(){
		SimpleMethod(methodToCall);
		AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), GameObject.Find("contoller").GetComponent<main>().masterVol);

		Destroy(gameObject);
	}

	public void no(){
		inPos = true;
		targetPos = -600;
		AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, -10), GameObject.Find("contoller").GetComponent<main>().masterVol);
	}
}
