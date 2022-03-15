using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]


    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;


    [Header("Volume Settings")]


    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;


    [Header("Fullscreen Settings")]
    
    [SerializeField] private Toggle fullScreenToggle;


    [Header("Quality Settings")]


    [SerializeField] private TMP_Dropdown qualityDropdown;


    private void Awake()
    {
        if(canUse)
        {
            if(PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if(PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }


            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterFullscreen");

                if(localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }
        }
    }
}
