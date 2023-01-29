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
        setStation(manager.radioID);
        volChange();
    }

    // Update is called once per frame
    void Update()
    {
        stationText.text = radioNames[manager.radioID];

        if (manager.radioID > 0)
        {
            songText.text = radioText(manager.radioNames[manager.radioID][manager.radioList[manager.radioID]]);
        }
        else
        {
            songText.text = "";
        }

        barTimer += Time.unscaledDeltaTime;
        if (barTimer > 0.85f-(vol.value*0.75f))
        {
            updateBars();
        }
    }

    public void setStation(int station)
    {
        manager.updateStation(station);
    }

    public void nextStation()
    {
        manager.updateStation(manager.radioID + 1);
    }

    public void prevStation()
    {
        manager.updateStation(manager.radioID - 1);
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
        return name + "\n" + artist;
    }

    private void updateBars()
    {
        if (manager.radioID > 0)
        {
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
}
