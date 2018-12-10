using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class HUDEnemyQuotes : MonoBehaviour {

    bool display;
    Timer displayTimer;
    Text enemyQuote;
    Quaternion rotation;
    GameObject panel;



	// Use this for initialization
	void Start () {

        //text component
        GetComponent<CanvasGroup>().alpha = 0;
        rotation = Quaternion.Euler(0, 0, 0);
        enemyQuote = gameObject.GetComponentInChildren<Text>();
        //enemyQuote.transform.SetParent(null);
        //enemyQuote.rectTransform.position = GetComponent<Transform>().position;
        enemyQuote.text = "";
        enemyQuote.color = Color.red;
       // enemyQuote.fontSize = 1;

        // Panel component
        //gameObject.GetComponentInChildren<panel>
        // Init 
        display = false;
        displayTimer = gameObject.GetComponent<Timer>();

        // Add listener
       // EventManager.AddEnemyDeathListener(DisplayQuote);
	}
	
    /// <summary>
    /// Plays random space pirate gibberish noise upon display of quote
    /// </summary>
    void PlayGibberishSound()
    {
        // pick random sound to play
        int gibberishNum = Random.Range(0, 10);

        // based off random selection, play appropriate sound
        switch (gibberishNum)
        {
            case 0:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish1);
                break;
            case 1:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish2);
                break;
            case 2:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish3);
                break;
            case 3:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish4);
                break;
            case 4:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish5);
                break;
            case 5:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish6);
                break;
            case 6:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish7);
                break;
            case 7:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish8);
                break;
            case 8:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish9);
                break;
            case 9:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish10);
                break;
            case 10:
                AudioManager.Instance.Play(AudioClipName.pirateGibberish11);
                break;
            default:
                break;
        }
    }

   public void DisplayQuote()
    {
        display = true;
        GetComponent<CanvasGroup>().alpha = 1;
        displayTimer.Duration = 4;
        displayTimer.Run();
        transform.rotation = rotation;
        enemyQuote.text = LoadCSVFiles.GetEnemyQuote;

        // play random space pirate gibberish noise
        PlayGibberishSound();
    }


    // Update is called once per frame
    void Update ()
    {
        if (displayTimer.Finished && display)
        {
            GetComponent<CanvasGroup>().alpha = 0;
            enemyQuote.text = "";
            Debug.Log("stopped displaying quote");
            display = false;
        }
    }

}
