using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    public TMP_Text buttonText;
    private int level;

    public void Initialize(int index, string displayText, UnityEngine.Events.UnityAction onClickAction)
    {
        level = index;
        if (buttonText != null)
        {
            buttonText.text = displayText;
        }
        Button button = GetComponent<Button>();
        if (button != null && onClickAction != null)
        {
            button.onClick.AddListener(onClickAction);
        }
    }
}
