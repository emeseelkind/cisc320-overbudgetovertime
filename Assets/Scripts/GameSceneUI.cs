using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    public GameObject settingPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchSettingPanel();
        }
    }

    public void SwitchSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

}
