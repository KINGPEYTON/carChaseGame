using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pauseMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //fixedDeltaTime
    }

    public void resume()
    {
        Time.timeScale = 1;
        Destroy(gameObject);

        GameObject.Find("playerCar").GetComponent<playerCar>().tapped = true;
    }

    public void menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }

    public void settings()
    {
        
    }
}
