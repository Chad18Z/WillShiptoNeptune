using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    #region Fields

    bool GameIsFrozen = false;

    // objs to hold menu
    public GameObject deathMenuObject;
    public GameObject EnergyBarAndHealth;
    public Image DeathPanel;

    #endregion

    #region Methods
    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial"))
        {
           NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.Tutorial);
        }
        else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main"))
        {
            NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.Level);
        }

        // get needed comps
        DeathPanel.GetComponent<CanvasGroup>().alpha = 0;
        DeathPanel.GetComponent<CanvasGroup>().interactable = false;

        // set death menu to be active (but alpha is 0), 
        // this is just so its not in peoples face in the scene view before start
        deathMenuObject.SetActive(true);

        // Add this as a listener for the enemy death event
        EventManager.AddPlayerDeathListener(OnDeath);
    }

    /// <summary>
    /// Update Method
    /// </summary>
    void Update()
    {
        // checks bool and sends to appropriate methods
        if (!GameIsFrozen && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Opens Death Menu
    /// </summary>
    void OnDeath()
    {
        // opens up death menu
        GameIsFrozen = true;
        Time.timeScale = 0f;
        DeathPanel.GetComponent<CanvasGroup>().alpha = 1;
        DeathPanel.GetComponent<CanvasGroup>().interactable = true;
        EnergyBarAndHealth.SetActive(false);
    }

    /// <summary>
    /// Goes to checkpoint
    /// </summary>
    public void OnCheckpointPressed()
    {
        // sets the object to false (make the pause panel disappear)
        GameIsFrozen = false;
        Time.timeScale = 1f;
        AudioManager.Instance.Play(AudioClipName.button_Select);
        DeathPanel.GetComponent<CanvasGroup>().interactable = false;
        EnergyBarAndHealth.SetActive(true);
        // unpauses game
        SceneManager.LoadScene("LoadingScreen");
    }

    /// <summary>
    /// Goes to Menu
    /// </summary>
    public void OnMenuPressed()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.Play(AudioClipName.button_Select);
        GameIsFrozen = false;
        DeathPanel.GetComponent<CanvasGroup>().interactable = false;
        EnergyBarAndHealth.SetActive(true);
        NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.MainMenu);
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}
