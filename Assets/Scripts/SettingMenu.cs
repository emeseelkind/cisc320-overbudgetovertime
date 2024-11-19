using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class SettingMenu : MonoBehaviour
{
    public Toggle toggleFullScreen;
    public TMP_Dropdown dropdownResolution;
    private List<Resolution> uniqueResolutions;
    public Slider sliderMV;
    public Slider sliderBGM;
    public Slider sliderSE;
    public Toggle toggleMV;
    public Toggle toggleBGM;
    public Toggle toggleSE;
    private float mvVolume = 1f;
    private float bgmVolume = 1f;
    private float seVolume = 1f;
    
    void Start()
    {
        sliderMV.value = mvVolume;
        sliderBGM.value = bgmVolume;
        sliderSE.value = seVolume;
        toggleMV.isOn = true;
        toggleBGM.isOn = true;
        toggleSE.isOn = true;

        sliderMV.onValueChanged.AddListener(OnMVSliderChanged);
        sliderBGM.onValueChanged.AddListener(OnBGMSliderChanged);
        sliderSE.onValueChanged.AddListener(OnSESliderChanged);
        toggleMV.onValueChanged.AddListener(OnMVToggleChanged);
        toggleBGM.onValueChanged.AddListener(OnBGMToggleChanged);
        toggleSE.onValueChanged.AddListener(OnSEToggleChanged);

        toggleFullScreen.isOn = Screen.fullScreen;
        toggleFullScreen.onValueChanged.AddListener(OnToggleFullscreen);
        InitializeResolutionOptions();
    }

    private void InitializeResolutionOptions()
    {
        uniqueResolutions = new List<Resolution>();
        dropdownResolution.ClearOptions();
        var options = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            if (!options.Contains(option))
            {
                options.Add(option);
                uniqueResolutions.Add(resolution);
            }
        }
        dropdownResolution.AddOptions(options);
        int currentResolutionIndex = GetCurrentResolutionIndex();
        dropdownResolution.value = currentResolutionIndex;
        dropdownResolution.RefreshShownValue();
        dropdownResolution.onValueChanged.AddListener(SetResolution);
    }

    private void OnToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        Debug.Log("Set resolution to " + selectedResolution.width + " x " + selectedResolution.height);
    }

    private void OnMVSliderChanged(float value)
    {
        mvVolume = value;
        UpdateAudioVolumes();
    }

    private void OnBGMSliderChanged(float value)
    {
        bgmVolume = value;
        UpdateAudioVolumes();
    }

    private void OnSESliderChanged(float value)
    {
        seVolume = value;
        UpdateAudioVolumes();
    }

    private void OnMVToggleChanged(bool isOn)
    {
        sliderMV.interactable = isOn; // 使滑动条可以被互动
        UpdateAudioVolumes();
    }

    private void OnBGMToggleChanged(bool isOn)
    {
        sliderBGM.interactable = isOn;
        UpdateAudioVolumes();
    }

    private void OnSEToggleChanged(bool isOn)
    {
        sliderSE.interactable = isOn;
        UpdateAudioVolumes();
    }

    private void UpdateAudioVolumes()
    {
        // TODO: voice control
        
        // float masterVolume = toggleMV.isOn ? mvVolume : 0f;
        // float bgmAdjustedVolume = toggleBGM.isOn ? bgmVolume * masterVolume : 0f;
        // float seAdjustedVolume = toggleSE.isOn ? seVolume * masterVolume : 0f;
        // AudioListener.volume = masterVolume;
        // Debug.Log($"MV: {masterVolume}, BGM: {bgmAdjustedVolume}, SE: {seAdjustedVolume}");
    }
    
    public void SwitchSettingPanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void HomeButtonEvent()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }

    void OnDestroy()
    {
        sliderMV.onValueChanged.RemoveListener(OnMVSliderChanged);
        sliderBGM.onValueChanged.RemoveListener(OnBGMSliderChanged);
        sliderSE.onValueChanged.RemoveListener(OnSESliderChanged);

        toggleMV.onValueChanged.RemoveListener(OnMVToggleChanged);
        toggleBGM.onValueChanged.RemoveListener(OnBGMToggleChanged);
        toggleSE.onValueChanged.RemoveListener(OnSEToggleChanged);

        toggleFullScreen.onValueChanged.RemoveListener(OnToggleFullscreen);
        dropdownResolution.onValueChanged.RemoveListener(SetResolution);
    }
}
