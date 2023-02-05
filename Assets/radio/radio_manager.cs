using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class radio_manager : MonoBehaviour
{
    public List<AudioClip> twoAMTurnUp;
    public List<string> twoAMTurnUpNames;
    public List<AudioClip> FUM;
    public List<string> FUMNames;
    public List<AudioClip> moldieOldies;
    public List<string> moldieOldiesNames;
    public List<AudioClip> rockHard;
    public List<string> rockHardNames;
    public List<AudioClip> alternativeAlt;
    public List<string> alternativeAltNames;
    public List<AudioClip> HOD;
    public List<string> HODNames;
    public List<AudioClip> myPOP;
    public List<string> myPOPNames;
    public List<AudioClip> hopBop;
    public List<string> hopBopNames;
    public List<AudioClip> feelingsFM;
    public List<string> feelingsFMNames;
    public List<AudioClip> cityVibes;
    public List<string> cityVibesNames;
    public List<AudioClip> countrysideRadio;
    public List<string> countrysideRadioNames;
    public List<AudioClip> foreignMusic;
    public List<string> foreignMusicNames;
    public List<AudioClip> themeHQ;
    public List<string> themeHQNames;

    public List<float> radioTotalTimes;
    public List<float> radioTimes;
    public List<int> radioList;

    public List<List<AudioClip>> radios = new List<List<AudioClip>>();
    public List<List<string>> radioNames = new List<List<string>>();
    public int radioID;
    public float totalTime;
    public float musicVol;
    public AudioSource music;
    public AudioLowPassFilter musicStatic;
    public AudioDistortionFilter musicStatic2;
    public float staticTimer;

    void Awake()
    {
        GameObject obj = GameObject.Find("radioManager");

        if (obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        music = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        musicStatic = music.GetComponent<AudioLowPassFilter>();
        musicStatic2 = music.GetComponent<AudioDistortionFilter>();

        radioID = PlayerPrefs.GetInt("radio", radioID); //sets high score to the one saved
        musicVol = PlayerPrefs.GetFloat("musicVol", musicVol); //sets high score to the one saved

        makeRadio(new List<AudioClip>(), new List<string>(), "???");
        makeRadio(twoAMTurnUp, twoAMTurnUpNames, "radio/89.3 2am Turn-up");
        makeRadio(FUM, FUMNames, "radio/91.5 FUM Fun Unaltered Music");
        makeRadio(moldieOldies, moldieOldiesNames, "radio/92.7 The Moldie Oldies");
        makeRadio(rockHard, rockHardNames, "radio/94.5 Rock Hard");
        makeRadio(alternativeAlt, alternativeAltNames, "radio/97.7 Alternative Ault");
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.unscaledDeltaTime;

        if (music == null)
        {
            music = GameObject.Find("Main Camera").GetComponent<AudioSource>();
            musicStatic = music.GetComponent<AudioLowPassFilter>();
            musicStatic2 = music.GetComponent<AudioDistortionFilter>();
            updateStation(radioID, 0);
        }

        if (!music.isPlaying)
        {
            updateStation(radioID, 0);
        }

        if(staticTimer > 0)
        {
            staticTimer -= Time.unscaledDeltaTime;

            if(staticTimer < 2.1f)
            {
                musicStatic.cutoffFrequency = 22000 - (staticTimer*10000 + 10);
                musicStatic2.distortionLevel = staticTimer * 0.25f;
            }
            else
            {
                musicStatic.cutoffFrequency = 100f;
                musicStatic2.distortionLevel = 0.5f;
            }

            if (staticTimer < 0)
            {
                staticTimer = 0;
                musicStatic.cutoffFrequency = 22000;
                musicStatic2.distortionLevel = 0;
            }
        }
    }

    void ShuffleStations<T>(IList<T> list, IList<string> list2)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int idx = Random.Range(0, i);
            var temp = list[i];
            string temp2 = list2[i];
            list[i] = list[idx];
            list2[i] = list2[idx];
            list[idx] = temp;
            list2[idx] = temp2;
        }
    }

    public void updateStation(int station, float staticTime)
    {
        if (station < 0)
        {
            station = radios.Count - 1;
        }
        else if (station > radios.Count - 1)
        {
            station = 0;
        }
        radioID = station;

        if (radioID > 0)
        {
            while ((radioTimes[station] + (float)radios[station][radioList[station]].length) < totalTime % radioTotalTimes[station])
            {
                radioTimes[station] += (float)radios[station][radioList[station]].length;
                radioList[station]++;
            }

            if (totalTime % radioTotalTimes[station] < radioTimes[station])
            {
                radioTimes[station] = 0;
                radioList[station] = 0;
            }

            music.clip = radios[station][radioList[station]];
            music.time = totalTime % radioTotalTimes[station] - radioTimes[station];
            music.Play();
        }
        else
        {
            music.Stop();
        }

        music.volume = musicVol * GameObject.Find("contoller").GetComponent<main>().masterVol;
        staticTimer = staticTime;

        PlayerPrefs.SetInt("radio", radioID); //saves the new high score
        PlayerPrefs.SetFloat("musicVol", musicVol); //saves the new high score
    }

    public void updateVol(float newVol)
    {
        musicVol = newVol;
        music.volume = musicVol * GameObject.Find("contoller").GetComponent<main>().masterVol;
        PlayerPrefs.SetFloat("musicVol", musicVol); //saves the new high score
    }

    private void makeRadio(List<AudioClip> station, List<string> stationNames, string path)
    {
        float stationTime = 0;
        var stationList = Resources.LoadAll(path);

        for (int i = 0; i < stationList.Length; i++)
        {
            //if (stationList[i].name.IndexOf(".mp3") > -1)
            {
                AudioClip song = stationList[i] as AudioClip;
                string name = stationList[i].name;
                station.Add(song);
                stationNames.Add(name);
                stationTime += song.length;
            }
        }

        radios.Add(station);
        radioNames.Add(stationNames);
        radioTotalTimes.Add(stationTime);

        radioTimes.Add(0);
        radioList.Add(0);

        ShuffleStations(station, stationNames);
    }
}
