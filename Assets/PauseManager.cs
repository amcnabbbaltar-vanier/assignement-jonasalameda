using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    GameObject pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.Find("Pause");
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0;

        characterMovement.enabled = false;
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
        
        characterMovement.enabled = true;
    }
}
