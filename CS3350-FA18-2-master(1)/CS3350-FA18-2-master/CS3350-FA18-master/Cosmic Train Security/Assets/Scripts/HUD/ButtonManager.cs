using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void OpenLevelScene()
    {
        NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.Level);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OpenTutorialScene()
    {
        NextSceneHolder.Instance.ChangeToNextScene(SceneHolderEnum.Tutorial);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void exitGameButton()
    {
        Application.Quit();
    }
}
