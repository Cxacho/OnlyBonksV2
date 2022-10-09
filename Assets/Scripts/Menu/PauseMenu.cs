using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject settingsButtons;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject graphicsSettingsPanel;
    [SerializeField] private GameObject exitToMenuPrompt;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        mainPanel.SetActive(true);
        mainButtons.SetActive(true);
        settingsButtons.SetActive(false);
        audioSettingsPanel.SetActive(false);
        graphicsSettingsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitToMenuPrompt.SetActive(false);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("NewMainMenu");
    }
}
