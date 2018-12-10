using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {
    #region Fields
    //a refrence to the slider 
    Slider healthbar;
    //the image used to fill the healthbar 
    [SerializeField]
    Image fill;
    //the image used for the backround of the healthbar 
    [SerializeField]
    Image backround;

    // Lighter Blue Color 
    Color32 colorGreen = new Color32(4, 112, 63, 255);
    Color32 colorRed = new Color32(150, 25, 30, 255);

    #endregion

    #region methods
    // Use this for initialization
    void Start () {

        healthbar = gameObject.GetComponent<Slider>();

        //add this method as a listener for the change health event. 
        EventManager.AddHealthChangeListeners(ChangeHealth);
        backround.color=Color.white;//set backround to gray
        healthbar.value = 100;// make sure the health bar starts full 
        fill.color = colorGreen;//set the fill collor to blue 
	}
	

    /// <summary>
    /// use to change the value of the health bar 
    /// </summary>
    /// <param name="health"></param>
    void ChangeHealth(int health)
    {
        healthbar.value = health;//set the slider value = to health 

        //change color based off of the health value
        if (healthbar.value < 50)
        {
            fill.color = colorRed;//change health color to red 
        }
        else
        {
            fill.color = colorGreen;//set health color to green
        }
    }
}
#endregion
