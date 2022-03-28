using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuController : MonoBehaviour
{


    [SerializeField] public GameObject newGamePanel;
    [SerializeField] public GameObject settingsChoosePanel;
    [SerializeField] public GameObject buttonsGroup;
    [SerializeField] public GameObject collectionsChoosePanel;
    [SerializeField] public TextMeshProUGUI newGameTxt;
    [SerializeField] public TextMeshProUGUI collectionsChoosePanelBackButtonTxt;
    [SerializeField] public TextMeshProUGUI settingsChoosePanelBackButtonTxt;
    [SerializeField] public TextMeshProUGUI newGamePanelNoButtonTxt;
    RectTransform buttonsGroupRect;
    private float buttonsGroupStartPozX;
    private float buttonsGroupStartPozY;
    Tweener yoyo;

    [Header("Volume Settings")]

    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
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
    private Resolution[] resultions;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    public void NewGamePanelOpen()
    {
        Sequence newGamePanelSeq = DOTween.Sequence()
        .Append
         (
            buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX - 570f, buttonsGroupStartPozY), 0.5f)
         )
        .OnComplete(() =>
        {
            buttonsGroup.SetActive(false);
            newGamePanel.SetActive(true);
            
            yoyo.Play();
        }
        );

        
    }

    public void NewGamePanelClose()
    {
        yoyo.Pause();
        newGameTxt.color = new Color32(255, 255, 255, 255);
        newGamePanelNoButtonTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(newGamePanelNoButtonTxt.GetComponent<RectTransform>().anchoredPosition.x-50f, newGamePanelNoButtonTxt.GetComponent<RectTransform>().anchoredPosition.y);
        newGamePanelNoButtonTxt.GetComponent<TextMeshProUGUI>().enableVertexGradient = false;
        newGamePanel.SetActive(false);
        buttonsGroup.SetActive(true);
        buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX, buttonsGroupStartPozY), 0.5f);
            

    }

    public void CollectionsChoosePanelOpen()
    {
        Sequence collectionsChoosePanelSeq = DOTween.Sequence()
        .Append
         (
            buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX - 570f, buttonsGroupStartPozY), 0.5f)
         )
        .OnComplete(() =>
        {
            buttonsGroup.SetActive(false);
            collectionsChoosePanel.SetActive(true);

        }
        );
    }

    public void CollectionsChoosePanelClose()
    {

        collectionsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(collectionsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition.x - 50f, collectionsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition.y);
        collectionsChoosePanelBackButtonTxt.GetComponent<TextMeshProUGUI>().enableVertexGradient = false;
        collectionsChoosePanel.SetActive(false);
        buttonsGroup.SetActive(true);
        buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX, buttonsGroupStartPozY), 0.5f);


    }

    public void SettingsChoosePanelOpen()
    {
        Sequence optionsChoosePanelSeq = DOTween.Sequence()
        .Append
         (
            buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX - 570f, buttonsGroupStartPozY), 0.5f)
         )
        .OnComplete(() =>
        {
            buttonsGroup.SetActive(false);
            settingsChoosePanel.SetActive(true);
            
        }
        );
    }

    public void SettingsChoosePanelClose()
    {

        settingsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(settingsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition.x - 50f, settingsChoosePanelBackButtonTxt.GetComponent<RectTransform>().anchoredPosition.y);
        settingsChoosePanelBackButtonTxt.GetComponent<TextMeshProUGUI>().enableVertexGradient = false;
        settingsChoosePanel.SetActive(false);
        buttonsGroup.SetActive(true);
        buttonsGroupRect.DOAnchorPos(new Vector2(buttonsGroupStartPozX, buttonsGroupStartPozY), 0.5f);


    }

    private void Start()
    {

        buttonsGroupRect = buttonsGroup.GetComponent<RectTransform>();
        buttonsGroupStartPozX = buttonsGroupRect.anchoredPosition.x;
        buttonsGroupStartPozY = buttonsGroupRect.anchoredPosition.y;

        yoyo = newGameTxt.DOFade(0f, 2f).SetLoops(-1, LoopType.Yoyo);

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
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {

        }
    }
     
    public void ExitButton()
    {
        Application.Quit();
    }


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);

    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0");
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
