using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resume()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    public void menu()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single); //resets the game
    }
}
