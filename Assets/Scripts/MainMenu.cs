using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject settingPanel;

    void Start() 
    {
        Debug.Log("MainMenu::Start");
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchSettingPanel();
        }
    }

    public void PlayGame()
    {
        Debug.Log("MainMenu::PlayGame");
        SceneManager.LoadScene("Scenes/LevelSelectScene");
    }

    public void SwitchSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void SettingButtonEvent()
    {
        Debug.Log("MainMenu::SettingButtonEvent");
        SwitchSettingPanel();
    }

    public void QuitGame()
    {
        Debug.Log("MainMenu::QuitGame");
        Application.Quit();
    }
}
