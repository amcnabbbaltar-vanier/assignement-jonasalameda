using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PauseManager pauseManager;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
        pauseManager = GetComponent<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pauseManager.PauseGame();
        }
    }
}
