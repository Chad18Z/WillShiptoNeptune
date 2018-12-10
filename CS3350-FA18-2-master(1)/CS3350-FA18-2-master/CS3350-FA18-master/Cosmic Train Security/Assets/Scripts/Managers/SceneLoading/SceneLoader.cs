using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script is ONLY for the loading screen, it checks what scene it should be moving to and begins loading it while animating
/// </summary>
public class SceneLoader : MonoBehaviour
{

    private bool isLoading = false;
    private int switchInt = 0;
    [SerializeField] Text loadingText;
    [SerializeField] Slider progressBar;

    // Update is called once per frame
    void Update()
    {
        if (!isLoading)
        {
            isLoading = true;
            switchInt++;
            StartCoroutine(LoadNewScene());
        }
        else
        {
            switch (switchInt)
            {
                case 0:
                    loadingText.text = "Loading";
                    break;
                case 1:
                    loadingText.text = "Loading.";
                    break;
                case 2:
                    loadingText.text = "Loading..";
                    break;
                case 3:
                    loadingText.text = "Loading...";
                    break;
            }

            if (switchInt < 3)
            {
                switchInt++;
            }
            else
            {
                switchInt = 0;
            }

            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
            progressBar.value++;
        }
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = null;

        switch (NextSceneHolder.Instance.CurrentScene)
        {
            case SceneHolderEnum.Level:
                async = SceneManager.LoadSceneAsync("Main");
                async.allowSceneActivation = false;
                break;
            case SceneHolderEnum.MainMenu:
                async = SceneManager.LoadSceneAsync("StartMenu");
                async.allowSceneActivation = false;
                break;
            case SceneHolderEnum.Tutorial:
                async = SceneManager.LoadSceneAsync("Tutorial");
                async.allowSceneActivation = false;
                break;
        }

        while (!async.isDone)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
