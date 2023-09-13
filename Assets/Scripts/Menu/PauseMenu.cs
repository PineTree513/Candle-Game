using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    public void Resume()
    {
        playerMovement.isPaused = false;
    }

    public void SaveAndQuit()
    {
        playerMovement.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
