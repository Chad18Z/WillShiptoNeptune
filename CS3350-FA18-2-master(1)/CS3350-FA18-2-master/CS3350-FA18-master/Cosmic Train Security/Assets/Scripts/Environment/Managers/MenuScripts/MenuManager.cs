using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// manages moving between scenes / menus
/// </summary>
public static class MenuManager
{
    #region Methods
    /// <summary>
    /// Goes to menu with given name
    /// </summary>
    /// <param name="name">name of menu to go to</param>
    public static void GoToMenu(MenuNames name)
    {
        // switch to load scences based on desired case
        switch (name)
        {
            case MenuNames.TestRoom:
                SceneManager.LoadScene("TestRoom");
                break;
            case MenuNames.Main:
                SceneManager.LoadScene("Main");
                break;
            case MenuNames.StartMenu:
                SceneManager.LoadScene("StartMenu");
                break;
        }
    }
    #endregion
}
