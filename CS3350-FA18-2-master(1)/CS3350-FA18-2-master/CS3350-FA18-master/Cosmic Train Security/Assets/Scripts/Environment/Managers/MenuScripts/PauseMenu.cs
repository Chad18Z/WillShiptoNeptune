using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Fields
	
    // field for checking if game is paused
    public static bool GamePaused = false;
    // obj to hold menu
    public GameObject pauseMenuObj;

    #endregion

    #region Methods

    /// <summary>
    /// Update Method
    /// </summary>
    void Update()
    {
		// checks if key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			// checks bool and sends to appropriate methods
            if (GamePaused)
            {
                OnResumePressed();
            }
            else
            {
                OnPausePressed();
            }
        }
    }

    /// <summary>
    /// Pauses Game
    /// </summary>
    void OnPausePressed()
    {
		// sets the object to true (make the pause panel appear)
        pauseMenuObj.SetActive(true);
		// pauses game
        Time.timeScale = 0f;
        GamePaused = true;
    }

    /// <summary>
    /// Resumes Game
    /// </summary>
    public void OnResumePressed()
    {
        // sets the object to true (make the pause panel disappear)
        AudioManager.Instance.Play(AudioClipName.button_Select);
        pauseMenuObj.SetActive(false);
		// unpauses game
        Time.timeScale = 1f;
        GamePaused = false;
    }

    /// <summary>
    /// Quits Game
    /// </summary>
    public void OnQuitPressed()
    {
        Time.timeScale = 1f;
        GamePaused = false;
        AudioManager.Instance.Play(AudioClipName.button_Select);
        // Debug Log as App Quit only works in builds
        NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.MainMenu);
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}
