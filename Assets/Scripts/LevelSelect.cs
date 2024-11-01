
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void ReturnButtonEvent()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void TestButtonEvent()
    {
        SceneManager.LoadScene("Scenes/GameScene");
    }
}
