using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;
    private bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) UpdatePause();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdatePause()
    {
        if (paused) UnPause();
        else Pause();
        paused = !paused;
    }

    void Pause()
    {
        pauseCanvas.GetComponent<Canvas>().enabled = (true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        pauseCanvas.GetComponent<Canvas>().enabled = (false);
        Time.timeScale = 1;
    }


    public void Landed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void YouDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
