using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUDDamage : MonoBehaviour {

    #region fields 
    GameObject player;              //get the player object
    CanvasGroup overlayGroup;       //reference to canvas group which overlay resides in
    int previousHealth;             //the health of the player before taking damage
    int currentHealth;              //the current health of the player 
    public float maxAlpha = 1f;     //the max aplha that the overlay can reach
    public float minAlpha = 0f;     //the min aplha that the overlay can reach
    public float currentAlpha;      //the current alpha of the overlay
    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        previousHealth = player.GetComponent<HealthScript>().health;
        // retrieve canvas group component of overlay and set its alpha to max
        overlayGroup = GetComponentInChildren<CanvasGroup>();
        overlayGroup.alpha = minAlpha;
    }
	
	// Update is called once per frame
	void Update () {
        
        currentHealth = player.GetComponent<HealthScript>().health;
        //if the player has taken damage
        if (currentHealth < previousHealth)
        {
            //flash the damage taken HUD
            currentAlpha = maxAlpha;
            //set previous health to current health
            previousHealth = currentHealth;
        }
        //if the damage overlay is being displayed, fade it out.
        if (overlayGroup.alpha > 0)
        {
            currentAlpha -= .05f;
        }
        overlayGroup.alpha = currentAlpha;
    }
}
#endregion
