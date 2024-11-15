using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class radio_menu : MonoBehaviour
{
    public List<string> radioNames;
    public radio_manager manager;

    public TextMeshProUGUI stationText;
    public TextMeshProUGUI songText;

    public float radioChangeTimer;
    public float prevRadio;
    public float targetRadio;

    public float artistDisplayTimer;
    public float songDisplayTimer;

    public List<Sprite> volIcons;
    public Image volIcon;
    public Slider vol;

    public List<GameObject> bars1;
    public List<GameObject> bars2;
    public List<GameObject> bars3;
    public List<GameObject> bars4;
    public List<GameObject> bars5;
    public float bars1Timer;
    public float bars2Timer;
    public float bars3Timer;
    public float bars4Timer;
    public float bars5Timer;
    public bool bars1Target;
    public bool bars2Target;
    public bool bars3Target;
    public bool bars4Target;
    public bool bars5Target;

    public AudioClip radioChangeUp;
    public AudioClip radioChangeDown;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("radioManager").GetComponent<radio_manager>();

        vol.value = manager.controller.radioVol;
        volChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.radioID > 0)
        {

            if (radioChangeTimer > 0)
            {
                radioChangeTimer -= Time.unscaledDeltaTime;

                stationText.text = ((int)((targetRadio + (((prevRadio - targetRadio) * radioChangeTimer))) * 10)) / 10.0f + "";
            }
            else if (radioChangeTimer > -1)
            {
                radioChangeTimer -= Time.unscaledDeltaTime;

                songText.text = radioText(manager.radioNames[manager.radioID][manager.radioList[manager.radioID]]);

                stationText.text = radioNames[manager.radioID].Substring(0, targetRadio.ToString().Length + (int)((radioNames[manager.radioID].Length - targetRadio.ToString().Length) * (0 - radioChangeTimer))) + (char)Random.Range(33, 64);
            }
            else
            {
                stationText.text = radioNames[manager.radioID];
                songText.text = radioText(manager.radioNames[manager.radioID][manager.radioList[manager.radioID]]);
            }
        }
        else
        {
            songText.text = "";
            stationText.text = "Off";
        }

        updateBars();
    }

    public void setStation(int station)
    {
        changeStation(manager.radioID, 2.0f);
    }

    public void nextStation()
    {
        changeStation(manager.radioID + 1, 2.0f);
        pauseMenu.playSound(radioChangeUp, new Vector3(0, 0, -10), manager.controller.masterVol * manager.controller.radioVol);
    }

    public void prevStation()
    {
        changeStation(manager.radioID - 1, 2.0f);
        pauseMenu.playSound(radioChangeDown, new Vector3(0, 0, -10), manager.controller.masterVol * manager.controller.radioVol);
    }

    public void changeStation(int station, float time)
    {
        prevRadio = getRadio();
        manager.updateStation(station, time);
        radioChangeTimer = time/2.5f;
        targetRadio = getRadio();
        songText.text = "";
        artistDisplayTimer = 0;
        songDisplayTimer = 0;
    }

    public void volChange()
    {
        //manager.controller.changeRadioVol(vol.value);

        volIcon.sprite = volIcons[(int)((1-vol.value)*3)];

        manager.updateVol(vol.value);
    }

    public string radioText(string text)
    {
        int split = text.IndexOf(" - ");
        string artist = text.Substring(0, split);
        string name = text.Substring(split+3);

        float extraLenArtist = (artist.Length - 19) * 0.25f;
        float extraLenSong = (name.Length - 19) * 0.25f;

        if (extraLenArtist > 0)
        {
            int startArtist = (int)getValueScale(getValueRanged(artistDisplayTimer - 2.5f, 0, extraLenArtist), 0, extraLenArtist, artist.Length - 19);
            artist = artist.Substring(startArtist, 19);
            artistDisplayTimer += Time.unscaledDeltaTime;
            if(artistDisplayTimer > 4 + extraLenArtist)
            {
                artistDisplayTimer = 0;
            }
        }
        if (extraLenSong > 0)
        {
            int startSong = (int)getValueScale(getValueRanged(songDisplayTimer - 2, 0, extraLenSong), 0, extraLenSong, name.Length - 19);
            name = name.Substring(startSong, 19);
            songDisplayTimer += Time.unscaledDeltaTime;
            if (songDisplayTimer > 5 + extraLenSong)
            {
                songDisplayTimer = 0;
            }
        }

        if (radioChangeTimer > -1)
        {
            if (20 * (0 - radioChangeTimer) < artist.Length)
            {
                artist = artist.Substring(0, (int)(20 * (0 - radioChangeTimer))) + (char)Random.Range(33, 64);
            }
            if (20 * (0 - radioChangeTimer) < name.Length)
            {
                name = name.Substring(0, (int)(20 * (0 - radioChangeTimer))) + (char)Random.Range(33, 64);
            }
        }

        return name + "\n" + artist;
    }

    private void updateBars()
    {
        if (manager.radioID > 0)
        {
            float barsTime = 0.35f;

            if (radioChangeTimer < -1)
            {
                setBar(ref bars1Timer, bars1, ref bars1Target, 4, barsTime);
                setBar(ref bars2Timer, bars2, ref bars2Target, 5, barsTime);
                setBar(ref bars3Timer, bars3, ref bars3Target, 6, barsTime);
                setBar(ref bars4Timer, bars4, ref bars4Target, 6, barsTime);
                setBar(ref bars5Timer, bars5, ref bars5Target, 2, barsTime);
            }
            else
            {
                setBar(ref bars1Timer, bars1, ref bars1Target, (int)((((2 - radioChangeTimer)-1)/2) * 4), barsTime);
                setBar(ref bars2Timer, bars2, ref bars2Target, (int)((((2 - radioChangeTimer) - 1) / 2) * 5), barsTime);
                setBar(ref bars3Timer, bars3, ref bars3Target, (int)((((2 - radioChangeTimer) - 1) / 2) * 6), barsTime);
                setBar(ref bars4Timer, bars4, ref bars4Target, (int)((((2 - radioChangeTimer) - 1) / 2) * 6), barsTime);
                setBar(ref bars5Timer, bars5, ref bars5Target, (int)((((2 - radioChangeTimer) - 1) / 2) * 2), barsTime);
            }
        }
        else
        {
            for (int i = 0; i < bars1.Count; i++)
            {
                bars1[i].SetActive(false);
            }
            for (int i = 0; i < bars2.Count; i++)
            {
                bars2[i].SetActive(false);
            }
            for (int i = 0; i < bars3.Count; i++)
            {
                bars3[i].SetActive(false);
            }
            for (int i = 0; i < bars4.Count; i++)
            {
                bars4[i].SetActive(false);
            }
            for (int i = 0; i < bars5.Count; i++)
            {
                bars5[i].SetActive(false);
            }
        }
    }

    public void setBar(ref float timer, List<GameObject> bars, ref bool targ, int numBars, float barsTime)
    {
        int barLevel = (int)getValueScale(timer, 0, barsTime, vol.value * numBars);
        if (targ)
        {
            timer += Time.unscaledDeltaTime * Random.Range(0.75f, 1.25f);
            if (timer > barsTime)
            {
                barLevel = (int)(vol.value * numBars);
                timer = barsTime;
                targ = false;
            }
        }
        else
        {
            timer -= Time.unscaledDeltaTime * Random.Range(0.75f, 1.25f);
            if (timer < 0)
            {
                barLevel = 0;
                timer = 0;
                targ = true;
            }
        }

        for (int i = 0; i < bars.Count; i++)
        {
            if (i <= barLevel)
            {
                bars[i].SetActive(true);
            }
            else
            {
                bars[i].SetActive(false);
            }
        }
    }

    public float getRadio()
    {
        if (manager.radioID > 0)
        {
            return float.Parse(radioNames[manager.radioID].Substring(0, radioNames[manager.radioID].IndexOf(".") + 2));
        }
        else
        {
            return 98.5f;
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
