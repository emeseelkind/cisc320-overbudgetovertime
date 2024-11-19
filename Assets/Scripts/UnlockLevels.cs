using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    public List<Button> lvlButtons = new List<Button>();

    void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 0);

        for (int i = 0; i < lvlButtons.Count; i++) 
        {
            if (i > currentLevel) 
            {
                lvlButtons[i].interactable = false;
            }
        }
    }

    public void AddLevelButton(Button button)
    {
        if (!lvlButtons.Contains(button))
        {
            lvlButtons.Add(button);
        }
    }
}
