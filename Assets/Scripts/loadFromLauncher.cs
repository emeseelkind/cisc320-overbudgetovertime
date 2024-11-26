using System;
using UnityEngine;

public class CommandLineArgsHandler : MonoBehaviour
{
    void Start()
    {
        // Get command-line arguments
        string[] args = Environment.GetCommandLineArgs();

        // Check if the game is started as a new game or as a load game
        bool isNewGame = false;
        bool isLoadGame = false;

        // Parse arguments
        foreach (string arg in args)
        {
            if (arg == "--newgame")
            {
                isNewGame = true;
            }
            else if (arg == "--load")
            {
                isLoadGame = true;
            }
        }

        // Handle new game
        if (isNewGame)
        {
            Debug.Log("New Game: Clearing all PlayerPrefs.");
            PlayerPrefs.DeleteAll(); // Clear all saved data
        }

        // Update PlayerPrefs with launcher arguments
        foreach (string arg in args)
        {
            if (arg.StartsWith("--difficulty="))
            {
                int difficulty = int.Parse(arg.Substring("--difficulty=".Length));
                PlayerPrefs.SetInt("Difficulty", difficulty);
                Debug.Log($"Difficulty set to: {difficulty}");
            }
            else if (arg.StartsWith("--masterVolume="))
            {
                bool masterVolume = arg.Substring("--masterVolume=".Length) == "1";
                PlayerPrefs.SetInt("MVEnabled", masterVolume ? 1 : 0);
                Debug.Log($"Master Volume: {(masterVolume ? "ON" : "OFF")}");
            }
            else if (arg.StartsWith("--music="))
            {
                bool music = arg.Substring("--music=".Length) == "1";
                PlayerPrefs.SetInt("BGMEnabled", music ? 1 : 0);
                Debug.Log($"Music: {(music ? "ON" : "OFF")}");
            }
            else if (arg.StartsWith("--sound="))
            {
                bool sound = arg.Substring("--sound=".Length) == "1";
                PlayerPrefs.SetInt("SEEnabled", sound ? 1 : 0);
                Debug.Log($"Sound: {(sound ? "ON" : "OFF")}");
            }
        }

        // Handle load game
        if (isLoadGame)
        {
            PlayerPrefs.SetInt("LoadGame", 1);
            Debug.Log("Load Game flag set.");
        }

        // Save PlayerPrefs to persist changes
        PlayerPrefs.Save();
    }
}
