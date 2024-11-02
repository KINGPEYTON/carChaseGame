using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public delegate void methodType();

public class youSure : MonoBehaviour {

	public main controller;

	public methodType methodToCall;
	public Button prevButton;

	public string message;
	public TextMeshProUGUI displayMessage;

	public Button yesButton;
	public Button noButton;
	public Image cover;
	public GameObject sign;

	public bool inPos;
	public float targetPos;
	public float curPos;
	public float posTime;

	public float speedTimer;

	public AudioClip clickSound;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("contoller").GetComponent<main>();

		inPos = false;
		posTime = 0.45f;
		targetPos = 500f;
		curPos = -800.0f;

		makeInteractable(false);
	}

	// Update is called once per frame
	void Update () {
		displayMessage.text = message;

		if (!inPos)
		{
			getInPos();
		}
		else
		{
			exitPos();
		}
	}

	void getInPos()
	{
		if (speedTimer < posTime)
		{
			cover.color = new Color32(36, 36, 36, (byte)getValueScale(speedTimer, 0, posTime, 175));
			sign.transform.position = new Vector3(1389, curPos - getValueScale(speedTimer, 0, posTime, curPos - targetPos), 0);
			speedTimer += Time.unscaledDeltaTime;
			if (speedTimer > posTime)
			{
				yesButton.interactable = true;
				noButton.interactable = true;
				cover.color = new Color32(36, 36, 36, 175);
				sign.transform.position = new Vector3(1389, targetPos, 0);
			}
		}
	}

	void exitPos()
	{

		speedTimer += Time.unscaledDeltaTime;
		cover.color = new Color32(36, 36, 36, (byte)(175 - getValueScale(getValueRanged(speedTimer, 0, posTime), 0, posTime, 175)));
		sign.transform.position = new Vector3(1389, targetPos + getValueScale(getValueRanged(speedTimer, 0, posTime), 0, posTime, curPos - targetPos), 0);
		if (speedTimer > posTime)
		{
			Destroy(gameObject);
		}
	}

	private void SimpleMethod(methodType method){
		method();
	}

	public void yes(){
		SimpleMethod(methodToCall);
		goAway();
	}

	public void goAway(){
		inPos = true;
		speedTimer = 0;
		yesButton.interactable = false;
		noButton.interactable = false;
		pauseMenu.playSound(clickSound, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
		makeInteractable(true);
	}

	void makeInteractable(bool side)
    {
		if(prevButton != null)
        {
			prevButton.interactable = side;
		}
	}

	float getValueScale(float val, float min, float max, float scale)
	{
		return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
	}

	float getValueRanged(float val, float min, float max)
	{
		float newVal = val;
		if (newVal > max) { newVal = max; } else if (val < min) { newVal = min; }
		return newVal;
	}
}
