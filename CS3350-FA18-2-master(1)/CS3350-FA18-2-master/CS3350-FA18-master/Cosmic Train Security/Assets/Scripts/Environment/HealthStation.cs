using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthStation : MonoBehaviour {
    #region Fields

    int Stationhealth = 100;//The health remining in the station
    ChangePlayerHealth fillHealthEvent;//event for filling the players health
    [SerializeField]//sprites for changing what the health station looks like 
    Sprite full;
    [SerializeField]
    Sprite twoThirds;
    [SerializeField]
    Sprite OneThird;
    [SerializeField]
    Sprite Empty;
    SpriteRenderer SPR;//sprite renderer to change the sprite at runtime 
    int healthNeeded;

    #endregion;

    #region Methods

    /// <summary>
    /// start and initalize 
    /// </summary>
    void Start ()
    {
        fillHealthEvent = new ChangePlayerHealth(); //create a new change health event 
        EventManager.AddPlayerHealthChangeInvoker(this); //add this as a invoker 
        EventManager.AddGetPlayerHealthListeners(HealthNeeded); //add a listener for the get player health event 
        SPR = gameObject.GetComponent<SpriteRenderer>(); //get the sprite renderer 
	}
	
	// Update is called once per frame
	void Update ()
    {
        //change the sprite depending on health left in the station 
        if (Stationhealth > 60)
        {
            SPR.sprite = full;
        }
        else if (Stationhealth < 60 && Stationhealth > 30)
        {
            SPR.sprite = twoThirds;
        }
        else if (Stationhealth < 30 && Stationhealth>0)
        {
            SPR.sprite = OneThird;
        }
        else if (Stationhealth == 0)
        {
            SPR.sprite = Empty;
        }
	}
    /// <summary>
    /// add listnersers for the fill health event 
    /// </summary>
    /// <param name="listener"></param>
    public void AddPlayerHealthChangeListener(UnityAction<int> listener)
    {
        fillHealthEvent.AddListener(listener);
    }
    /// <summary>
    /// calculate the health needed and call the add health event 
    /// </summary>
    /// <param name="playerHealth"></param>
    void HealthNeeded(int playerHealth)
    {
        if (Stationhealth < 0) //exit the function if health left is 0
        {
            return;
        }
        healthNeeded = 100 - playerHealth; //calculate health needed from the satation 
        if (healthNeeded > Stationhealth) //if health needed is more than what is in the station give the player all the health that is left 
        {
            fillHealthEvent.Invoke(Stationhealth);
            if (playerHealth != 100)
            {
                AudioManager.Instance.Play(AudioClipName.healthStation_Charge);
            }
            Stationhealth = 0;
        }
        else
        {
            fillHealthEvent.Invoke(healthNeeded); //else fill the players health to full
            if (playerHealth != 100)
            {
                AudioManager.Instance.Play(AudioClipName.healthStation_Charge);
            }
            Stationhealth -= healthNeeded;
        }
    }
}

#endregion
