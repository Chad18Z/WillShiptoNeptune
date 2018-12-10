using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlastDoor : MonoBehaviour
{

    #region Fields

    Animator animator;
    bool open;
    bool canMove = true;
    int counter = 60;
    float animatorSpeed = 2.0f;
    InitNewTraincar initNewTraincar;
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

        // Add player as invoker to move to next train car
        initNewTraincar = new InitNewTraincar();
        EventManager.AddInitNewTraincarInvoker(this);
    }

    // <summary>
    // counts down each update cycle
    // when counter <= 0 then the doors can move again
    void Update()
    {
        if (canMove == false)
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
        if (collider.gameObject.tag == "Player" && canMove == true)
        {
            // Sets open to true and activates the animator to open the doors
            open = true;
            AudioManager.Instance.Play(AudioClipName.blast_Door_Open);
            ActivateDoors("Open");
            canMove = false;
            counter = 60;

        }
    }

    /// <summary>
    /// Triggers when the player leaves the box collider to close the door
    /// </summary>
    /// <param name="collider">the player</param>
    void OnTriggerExit2D(Collider2D collider)
    {
        // If the door is open and player leaves collision area, close the door.
        if (open && collider.gameObject.tag == "Player" && canMove == true)
        {
            open = false;
            AudioManager.Instance.Play(AudioClipName.blast_Door_Close);
            ActivateDoors("Close");
            canMove = false;
            counter = 60;

            initNewTraincar.Invoke();
        }
    }

    /// <summary>
    /// Activates the door animator
    /// </summary>
    /// <param name="state">The state of the door (only options are Open and Close)</param>
    void ActivateDoors(string state)
    {
        animator.SetTrigger(state);
    }
    #endregion

    // Adds listener to the enter new train car event
    public void AddInitNewTraincarListener(UnityAction listener)
    {
        initNewTraincar.AddListener(listener);
    }
}


