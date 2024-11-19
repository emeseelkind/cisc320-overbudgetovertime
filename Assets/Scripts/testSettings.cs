using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class testSettings : MonoBehaviour
{
    public Toggle toggleFullScreen;
    public TMP_Dropdown dropdownResolution;
    public TMP_Dropdown dropdownDifficulty;
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

    private int difficulty = 1; // 0: Easy, 1: Medium (default), 2: Hard

    void Start()
    {
        // Initialize resolutions first
        InitializeResolutionOptions();

        // Load settings
        LoadSettings();

        // Add listeners after initialization
        sliderMV.onValueChanged.AddListener(OnMVSliderChanged);
        sliderBGM.onValueChanged.AddListener(OnBGMSliderChanged);
        sliderSE.onValueChanged.AddListener(OnSESliderChanged);
        toggleMV.onValueChanged.AddListener(OnMVToggleChanged);
        toggleBGM.onValueChanged.AddListener(OnBGMToggleChanged);
        toggleSE.onValueChanged.AddListener(OnSEToggleChanged);
        dropdownDifficulty.onValueChanged.AddListener(OnDifficultyChanged);
        toggleFullScreen.onValueChanged.AddListener(OnToggleFullscreen);
        dropdownResolution.onValueChanged.AddListener(SetResolution);
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

        int currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex());

        // Boundary check
        if (currentResolutionIndex < 0 || currentResolutionIndex >= uniqueResolutions.Count)
        {
            currentResolutionIndex = GetCurrentResolutionIndex();
        }

        dropdownResolution.value = currentResolutionIndex;
        dropdownResolution.RefreshShownValue();
    }

    private void OnToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        // Save fullscreen choice
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
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

        // Save resolution choice
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();

        Debug.Log("Set resolution to " + selectedResolution.width + " x " + selectedResolution.height);
    }

    private void OnMVSliderChanged(float value)
    {
        mvVolume = value;

        // Save master volume choice
        PlayerPrefs.SetFloat("MVVolume", value);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void OnBGMSliderChanged(float value)
    {
        bgmVolume = value;

        // Save BGM volume
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void OnSESliderChanged(float value)
    {
        seVolume = value;

        // Save SE volume
        PlayerPrefs.SetFloat("SEVolume", value);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void OnMVToggleChanged(bool isOn)
    {
        sliderMV.interactable = isOn;

        // Save MV enabled
        PlayerPrefs.SetInt("MVEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void OnBGMToggleChanged(bool isOn)
    {
        sliderBGM.interactable = isOn;

        // Save BGM enabled
        PlayerPrefs.SetInt("BGMEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void OnSEToggleChanged(bool isOn)
    {
        sliderSE.interactable = isOn;

        // Save SE enabled
        PlayerPrefs.SetInt("SEEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateAudioVolumes();
    }

    private void UpdateAudioVolumes()
    {
        float masterVolume = toggleMV.isOn ? mvVolume : 0f;
        float bgmAdjustedVolume = toggleBGM.isOn ? bgmVolume * masterVolume : 0f;
        float seAdjustedVolume = toggleSE.isOn ? seVolume * masterVolume : 0f;

        // Apply the volumes to your audio sources
        // Example:
        // AudioListener.volume = masterVolume;
        // bgmAudioSource.volume = bgmAdjustedVolume;
        // seAudioSource.volume = seAdjustedVolume;

        Debug.Log($"MV: {masterVolume}, BGM: {bgmAdjustedVolume}, SE: {seAdjustedVolume}");
    }

    private void OnDifficultyChanged(int index)
    {
        difficulty = index;

        // Save difficulty
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();

        Debug.Log($"Difficulty set to: {GetDifficultyString(difficulty)}");
    }

    private void LoadSettings()
    {
        // Load sliders
        mvVolume = PlayerPrefs.GetFloat("MVVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1f);

        // Remove listeners to prevent unwanted calls during initialization
        sliderMV.onValueChanged.RemoveListener(OnMVSliderChanged);
        sliderBGM.onValueChanged.RemoveListener(OnBGMSliderChanged);
        sliderSE.onValueChanged.RemoveListener(OnSESliderChanged);

        sliderMV.value = mvVolume;
        sliderBGM.value = bgmVolume;
        sliderSE.value = seVolume;

        // Re-attach listeners
        sliderMV.onValueChanged.AddListener(OnMVSliderChanged);
        sliderBGM.onValueChanged.AddListener(OnBGMSliderChanged);
        sliderSE.onValueChanged.AddListener(OnSESliderChanged);

        // Load toggles
        toggleMV.onValueChanged.RemoveListener(OnMVToggleChanged);
        toggleBGM.onValueChanged.RemoveListener(OnBGMToggleChanged);
        toggleSE.onValueChanged.RemoveListener(OnSEToggleChanged);

        toggleMV.isOn = PlayerPrefs.GetInt("MVEnabled", 1) == 1;
        toggleBGM.isOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        toggleSE.isOn = PlayerPrefs.GetInt("SEEnabled", 1) == 1;

        toggleMV.onValueChanged.AddListener(OnMVToggleChanged);
        toggleBGM.onValueChanged.AddListener(OnBGMToggleChanged);
        toggleSE.onValueChanged.AddListener(OnSEToggleChanged);

        // Update slider interactability based on toggles
        sliderMV.interactable = toggleMV.isOn;
        sliderBGM.interactable = toggleBGM.isOn;
        sliderSE.interactable = toggleSE.isOn;

        // Load fullscreen
        toggleFullScreen.onValueChanged.RemoveListener(OnToggleFullscreen);

        toggleFullScreen.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = toggleFullScreen.isOn;

        toggleFullScreen.onValueChanged.AddListener(OnToggleFullscreen);

        // Apply saved resolution
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex());

        // Boundary check
        if (resolutionIndex < 0 || resolutionIndex >= uniqueResolutions.Count)
        {
            resolutionIndex = GetCurrentResolutionIndex();
        }

        dropdownResolution.value = resolutionIndex;
        dropdownResolution.RefreshShownValue();

        Resolution selectedResolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

        // Load difficulty
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 1);

        dropdownDifficulty.onValueChanged.RemoveListener(OnDifficultyChanged);
        dropdownDifficulty.value = savedDifficulty;
        dropdownDifficulty.RefreshShownValue();
        dropdownDifficulty.onValueChanged.AddListener(OnDifficultyChanged);

        // Update audio volumes after loading settings
        UpdateAudioVolumes();

        PlayerPrefs.Save();
    }

    private string GetDifficultyString(int index)
    {
        return index switch
        {
            0 => "Easy",
            1 => "Medium",
            2 => "Hard",
            _ => "Unknown"
        };
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
        dropdownDifficulty.onValueChanged.RemoveListener(OnDifficultyChanged);
    }
}
