using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour {

	#region Fields

	Animator animator;
	bool open;
    bool canMove = true;
    int counter = 60;
    float animatorSpeed = 5.0f;

	#endregion

	#region Methods
	/// <summary>
	/// sets open to be false initially and gets the animator component.
	/// </summary>
	void Start()
	{
		open = false;
		animator = GetComponent<Animator>();
        animator.speed = animatorSpeed;
	}

    // <summary>
    // counts down each update cycle
    // when counter <= 0 then the doors can move again
    void Update()
    {
        if(canMove == false)
        {
            counter--;
        }
        if (counter <= 0)
        {
            canMove = true;
        }
    }

    /// <summary>
    /// Triggers when the player enters the box collider to open the door.
    /// </summary>
    /// <param name="collider">the player</param>
    void OnTriggerEnter2D(Collider2D collider)
	{
		// Checks if gameobject that is colliding is the player or not
		if((collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy") && canMove == true)
		{
			// Sets open to true and activates the animator to open the doors
			open = true;
            AudioManager.Instance.Play(AudioClipName.door_Open);
            ActivateDoors("Open");
            canMove = false;
            counter = 60;
		}
	}

	/// <summary>
	/// Triggers when the player leaves the box collider to close the door
	/// </summary>
	/// <param name="collider">the player</param>
	/*void OnTriggerExit2D(Collider2D collider)
	{
		// If the door is open and player leaves collision area, close the door.
		if ((collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy") && canMove == true)
		{
			open = false;
            AudioManager.Instance.Play(AudioClipName.door_Close);
            ActivateDoors("Close");
            canMove = false;
            counter = 60;
        }
	}*/

	/// <summary>
	/// Activates the door animator
	/// </summary>
	/// <param name="state">The state of the door (only options are Open and Close)</param>
	void ActivateDoors(string state)
	{
		animator.SetTrigger(state);
	}
	#endregion
}
