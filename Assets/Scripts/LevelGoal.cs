using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") +1);
        SceneManager.LoadScene("LevelSelectScene");
        Debug.Log($"Trigger entered by: {collision.gameObject.name}");
    }
}
