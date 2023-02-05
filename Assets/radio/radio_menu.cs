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

    public List<Sprite> volIcons;
    public Image volIcon;
    public Slider vol;

    public List<GameObject> bars1;
    public List<GameObject> bars2;
    public List<GameObject> bars3;
    public List<GameObject> bars4;
    public List<GameObject> bars5;
    public float barTimer;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("radioManager").GetComponent<radio_manager>();

        vol.value = manager.musicVol;
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
            }else
            {
                stationText.text = radioNames[manager.radioID];
            }
        }
        else
        {
            songText.text = "";
            stationText.text = "Off";
        }

        barTimer += Time.unscaledDeltaTime;
        if (barTimer > 0.85f-(vol.value*0.75f))
        {
            updateBars();
        }
    }

    public void setStation(int station)
    {
        prevRadio = getRadio();
        manager.updateStation(station, 2.0f);
        radioChangeTimer = 0.75f;
        targetRadio = getRadio();
        songText.text = "";
    }

    public void nextStation()
    {
        prevRadio = getRadio();
        manager.updateStation(manager.radioID + 1, 2.0f);
        radioChangeTimer = 0.75f;
        targetRadio = getRadio();
        songText.text = "";
    }

    public void prevStation()
    {

        prevRadio = getRadio();
        manager.updateStation(manager.radioID - 1, 2.0f);
        radioChangeTimer = 0.75f;
        targetRadio = getRadio();
        songText.text = "";
    }

    public void volChange()
    {
        manager.musicVol = vol.value;

        volIcon.sprite = volIcons[(int)((1-vol.value)*4)];

        manager.updateVol(vol.value);
    }

    public string radioText(string text)
    {
        int split = text.IndexOf(" - ");
        string artist = text.Substring(0, split);
        string name = text.Substring(split+3);

        if (artist.Length >= 19)
        {
            artist = artist.Substring(0, 17) + "...";
        }
        if (name.Length >= 20)
        {
            name = name.Substring(0, 18) + "...";
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
            if (radioChangeTimer < -1) {
                int barLevel = (int)Random.Range(vol.value * 2, vol.value * 8);
                for (int i = 0; i < bars1.Count; i++)
                {
                    if (i <= barLevel)
                    {
                        bars1[i].SetActive(true);
                    }
                    else
                    {
                        bars1[i].SetActive(false);
                    }
                }

                int barLevel2 = (int)Random.Range(vol.value * 2, vol.value * 8);
                for (int i = 0; i < bars2.Count; i++)
                {
                    if (i <= barLevel2)
                    {
                        bars2[i].SetActive(true);
                    }
                    else
                    {
                        bars2[i].SetActive(false);
                    }
                }

                int barLevel3 = (int)Random.Range(vol.value * 2, vol.value * 8);
                for (int i = 0; i < bars3.Count; i++)
                {
                    if (i <= barLevel3)
                    {
                        bars3[i].SetActive(true);
                    }
                    else
                    {
                        bars3[i].SetActive(false);
                    }
                }

                int barLevel4 = (int)Random.Range(vol.value * 2, vol.value * 8);
                for (int i = 0; i < bars4.Count; i++)
                {
                    if (i <= barLevel4)
                    {
                        bars4[i].SetActive(true);
                    }
                    else
                    {
                        bars4[i].SetActive(false);
                    }
                }

                int barLevel5 = (int)Random.Range(vol.value * 2, vol.value * 8);
                for (int i = 0; i < bars5.Count; i++)
                {
                    if (i <= barLevel5)
                    {
                        bars5[i].SetActive(true);
                    }
                    else
                    {
                        bars5[i].SetActive(false);
                    }
                }
            }
            else
            {
                int barLevel = (int)Random.Range(((0 - radioChangeTimer)) * 2, (1 + (0 - radioChangeTimer)) * 8);
                for (int i = 0; i < bars1.Count; i++)
                {
                    if (i <= barLevel)
                    {
                        bars1[i].SetActive(true);
                    }
                    else
                    {
                        bars1[i].SetActive(false);
                    }
                }

                int barLevel2 = (int)Random.Range(((0 - radioChangeTimer)) * 2, (1 + (0 - radioChangeTimer)) * 8);
                for (int i = 0; i < bars2.Count; i++)
                {
                    if (i <= barLevel2)
                    {
                        bars2[i].SetActive(true);
                    }
                    else
                    {
                        bars2[i].SetActive(false);
                    }
                }

                int barLevel3 = (int)Random.Range(((0 - radioChangeTimer)) * 2, (1 + (0 - radioChangeTimer)) * 8);
                for (int i = 0; i < bars3.Count; i++)
                {
                    if (i <= barLevel3)
                    {
                        bars3[i].SetActive(true);
                    }
                    else
                    {
                        bars3[i].SetActive(false);
                    }
                }

                int barLevel4 = (int)Random.Range(((0 - radioChangeTimer)) * 2, (1 + (0 - radioChangeTimer)) * 8);
                for (int i = 0; i < bars4.Count; i++)
                {
                    if (i <= barLevel4)
                    {
                        bars4[i].SetActive(true);
                    }
                    else
                    {
                        bars4[i].SetActive(false);
                    }
                }

                int barLevel5 = (int)Random.Range(((0 - radioChangeTimer)) * 2, (1 + (0 - radioChangeTimer)) * 8);
                for (int i = 0; i < bars5.Count; i++)
                {
                    if (i <= barLevel5)
                    {
                        bars5[i].SetActive(true);
                    }
                    else
                    {
                        bars5[i].SetActive(false);
                    }
                }
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

        barTimer = 0;
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
}
