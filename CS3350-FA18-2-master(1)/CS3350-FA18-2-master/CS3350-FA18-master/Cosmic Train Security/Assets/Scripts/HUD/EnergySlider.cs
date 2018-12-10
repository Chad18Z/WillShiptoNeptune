using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergySlider : MonoBehaviour {
    #region Fields

    Slider energySlider;
    GameObject player;
    [SerializeField]
    Image fill;
    [SerializeField]
    Image background;
    float flashTimer=.25f;
    float timer;
    
    // Dark Blue Color
    Color32 colorDB = new Color32(0, 252, 255, 255);

    #endregion

    #region Methods
    /// <summary>
    /// use for initialization set value on slider to full
    /// </summary>
    void Start ()
    {
        energySlider = gameObject.GetComponent<Slider>();
		player = GameObject.FindGameObjectWithTag("Player");
        energySlider.value = 20;

        //adds the maxEnergy function as a listener to the listener list created in the Event Manager
        EventManager.SetChargeToMaxAddEventListener(maxEnergy);

    }
	
	/// <summary>
    /// Keeps the value of energy up to date as the player shoots
    /// </summary>
	void Update ()
    {
        energySlider.value = player.GetComponent<FiringScript>().Energy;

        // Change the color of the slider based on how much energy the player has used.
        if (energySlider.value < 50)
        {
            fill.color = Color.yellow;
        }
        
        if (energySlider.value > 50)
        {
            fill.color = colorDB;
        }
        if (energySlider.value < 20)
        {
            fill.color = Color.red;
            
            timer += Time.deltaTime;
        }
        if (energySlider.value == 0)
        {
            if (timer >= flashTimer)
            {
				// Reset the timer and flash the background
                timer = 0;
                BackroundFlash(background.color);

            }
        }
    }
    /// <summary>
    /// Flash the background sprite when energy is low
    /// </summary>
    void BackroundFlash(Color color)
    {
        if (color == Color.red)
        {
            background.color = Color.black;
        }
        else
        {
            background.color = Color.red;

        }

    }


    #endregion
    //maximizes energy value 
    public void maxEnergy()
    {
        int currentEnergy = player.GetComponent<FiringScript>().Energy;

        //100 is the currentMax energy for the player
        //set the energy to max if it isnt already the max value
        if (currentEnergy != 100)
        {
            //setting energy of player to the max
            player.GetComponent<FiringScript>().Energy = 100;
            background.color = Color.black;
            //playing charge sound
            AudioManager.Instance.Play(AudioClipName.player_WeaponCharge);
        }
       
        
    }
}