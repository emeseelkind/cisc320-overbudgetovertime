
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public VerticalLayoutGroup contentLayout;
    public LevelButton levelButtonPrefab;
    private RectTransform contentRect; 
    void Start() 
    {
        contentRect = contentLayout.GetComponent<RectTransform>();
        for (int i = 1; i <= 10; i++)
        {
            addLevelButton(i, $"Level {i}");
        }
    }

    private void addLevelButton(int level, string text) 
    {
        LevelButton newButton = Instantiate(levelButtonPrefab, contentLayout.transform);
        newButton.Initialize(level, text, () =>
        {
            Debug.Log($"Level {level} selected!");
            SceneManager.LoadScene($"Scenes/Level {level}");
        });

        UpdateContentHeight(newButton.GetComponent<RectTransform>());
    }

    private void UpdateContentHeight(RectTransform newButtonRect)
    {
        if (contentRect == null || newButtonRect == null)
            return;
        float newHeight = contentRect.rect.height + newButtonRect.rect.height + contentLayout.spacing;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
    }

    public void ReturnButtonEvent()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }
}
