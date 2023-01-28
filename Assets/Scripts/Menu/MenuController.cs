using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject settingsButtons;

    [Header("Volume Settings")]

    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] public Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 100f;

    [Header("Levels To Load")]

    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject continueButton;

    [Header("Graphics Settings")]

    private int _qualityLevel;
    private bool _isFullScreen;


    [Header("Resolution Dropdown")]

    public TMP_Dropdown resoultionDropdown;
    [SerializeField] private Resolution[] resultions;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;



    private void Start()
    {
        //if (PlayerPrefs.HasKey("Map")) continueButton.SetActive(true);

        resultions = Screen.resolutions;
        resoultionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resultions.Length; i++)
        {
            string option = resultions[i].width + " x " + resultions[i].height;
            options.Add(option);

            if(resultions[i].width == Screen.width && resultions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resoultionDropdown.AddOptions(options);
        resoultionDropdown.value = currentResolutionIndex;
        resoultionDropdown.RefreshShownValue();

    }

    public void audioSettings()
    {

        contentPanel.SetActive(true);
        audioPanel.SetActive(true);
        graphicsPanel.SetActive(false);
        contentPanel.GetComponent<Image>().DOFade(1f, 0.3f);
        audioPanel.GetComponent<Image>().DOFade(1f, 0.3f);

    }

    public void GraphicsSettings()
    {

        contentPanel.SetActive(true);
        graphicsPanel.SetActive(true);
        audioPanel.SetActive(false);
        contentPanel.GetComponent<Image>().DOFade(1f, 0.3f);
        graphicsPanel.GetComponent<Image>().DOFade(1f, 0.3f);

    }

    public void ExitPanel()
    {
        if (contentPanel.activeSelf == true)
        {
            contentPanel.GetComponent<Image>().DOFade(0f, 0.3f).OnComplete(PanelCallback);
            audioPanel.GetComponent<Image>().DOFade(0f, 0.3f);
            graphicsPanel.GetComponent<Image>().DOFade(0f, 0.3f);
        }
        else
        {
            settingsButtons.SetActive(false);
            mainButtons.SetActive(true);
        }
    }

    public void PanelCallback()
    {
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        contentPanel.SetActive(false);
        

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resultions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void Awake()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            continueButton.SetActive(true);
        }
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
            volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
            volumeTextValue.text = volumeSlider.value.ToString();
        }
        
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey("tutorial")
        PlayerPrefs.DeleteKey("Map");
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("Map"))
        {
            
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {

        }
    }
     
    public void ExitGame()
    {
        Application.Quit();
    }


    public void SetVolume()
    {
        
        AudioListener.volume = volumeSlider.value;
        volumeTextValue.text = volumeSlider.value.ToString();
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        Debug.Log("AudioListener volume : " + AudioListener.volume);
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString();
            VolumeApply();
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
    }
}
