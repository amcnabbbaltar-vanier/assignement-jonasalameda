using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Jawn");
        SceneManager.LoadScene("OverworldLevel1");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
