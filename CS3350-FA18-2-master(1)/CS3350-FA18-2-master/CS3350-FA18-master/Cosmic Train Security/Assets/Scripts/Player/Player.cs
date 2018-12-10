using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Fields

    //new vectors used to reposition towards direction of mouse cursor and moving with WASD
    Vector2 direction;
    Vector2 newPlayerPosition;
    Rigidbody2D playerRigidBody;

    //used to determine to move the players position or not
    bool moveInput = false;

    //modifiable variable to adjust speed to game liking
    public float adjustSpeed = 5.5f;

    // variable for animation purposes
    public Animator animator;

    // variable for walk sounds
    int walkTimer = 0;

    Constants.Tools toolEnabled; //determines which tool is enabled to the player

    #endregion

    #region Methods

    /// <summary>
    /// Used for initializing variables
    /// </summary>
    void Start()
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Looking for input from player for tool use
    /// </summary>
    private void Update()
    {
        //if player presses right mouse button and all tools arent disabled
        if (Input.GetKeyDown(KeyCode.Mouse1) && !gameObject.GetComponent<ToolBelt>().allToolsDisabled && PauseMenu.GamePaused == false)
        {
            Debug.Log("Receiving input while a tool is enabled");
            //set the toolenabled for the player to what the toolbelt has the enabled tool set as
            toolEnabled = gameObject.GetComponent<ToolBelt>().enabledTool;
            //if enabled tool is tripwire
            if (toolEnabled == Constants.Tools.TripWire)
            {
                Debug.Log("Tripwire is enabled and trying to be used");
                //call function to place tripwire
                gameObject.GetComponent<TripWire>().placeInitialWirePlug();
            }
            //if enabled tool is stasis grenade
            else if(toolEnabled == Constants.Tools.StasisGrenade)
            {
                Debug.Log("Stasis grenade is enabled and trying to be used");
                //call function to throw grenade
                gameObject.GetComponent<StasisGrenade>().throwStasisGrenade();
            }
            else if(toolEnabled == Constants.Tools.PingDevice)
            {
                Debug.Log("Ping Device is enabled and trying to be used");
                gameObject.GetComponent<PingDevice>().setTransparentPingRadius();
            }
            
        }
        

    }


    /// <summary>
    /// Used for handling player movement
    /// </summary>
    private void FixedUpdate()
    {
        // values grabbed by the players input from WASD
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // checks if the coordinates of the mouse cursor are not 0
        // if it isnt zero, then redirect the player towards the position of the mouse cursor
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            // grabs the vector position of the mouse cursor
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // creates a new direction value by using the coordinate differences between the cursor and player
            direction = (new Vector2(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y).normalized);
            // the Y-Axis of the player is changes based on the direction value
            transform.up = direction;
            transform.Rotate(Vector3.forward * 90);
           
        }

        // used to move the player if A or D is used to move horizontally
        if (horizontalInput != 0)
        {
            // modifies the players x coordinate position only based on the horizonalInput of 1 (D) or -1 (A)
            // Time.deltaTime makes the frame rate indpedent
            newPlayerPosition.x = transform.position.x + adjustSpeed * horizontalInput * Time.deltaTime;
            newPlayerPosition.y = transform.position.y;
            moveInput = true;
        }

        
        // used to move the player if W or S is used to move vertically
        if(verticalInput != 0)
        {
            // modifies the players Y coordinate position only based on verticalInput of 1 (W) or -1 (S)
            newPlayerPosition.x = transform.position.x;
            newPlayerPosition.y = transform.position.y + adjustSpeed * verticalInput * Time.deltaTime;
            moveInput = true;
        }

        // when vertical and horizontal input is given, we want diagonal movement
        if (horizontalInput != 0 && verticalInput != 0)
        {
            // Changing both X and Y positions but with a 0.75f value to adjust the speed to be similar to horizontal 
            // and vertical movement
            newPlayerPosition.x = transform.position.x + (0.75f * adjustSpeed) * horizontalInput * Time.deltaTime;
            newPlayerPosition.y = transform.position.y + (0.75f * adjustSpeed) * verticalInput * Time.deltaTime;
            moveInput = true;
        }

        //when it is true there's player movement input, the players position is changed
        if (moveInput)
        {
            // set animation parameter to true
            animator.SetBool("MoveInput", true);

            playerRigidBody.MovePosition(newPlayerPosition);
            //bool is set back to false so that the player stops moving on the next frame without any input
            moveInput = false;

            // play the movement sound
            if (walkTimer <= 0)
            {
                AudioManager.Instance.Play(AudioClipName.walk);
                walkTimer = 15;
            }
            else
            {
                walkTimer -= 1;
            }
        }
        // if no movement check to see if the animator parameter needs to be changed to false
        else if (animator.GetBool("MoveInput"))
        {
            walkTimer = 0;
            animator.SetBool("MoveInput", false);
        }
    }

    /// <summary>
    /// This method handles collisions with objects
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Play wall impact sound on collission with wall.
        if (collision.gameObject.tag == "Wall")
        {
            AudioManager.Instance.Play(AudioClipName.wall_Impact);
        }

        if (collision.gameObject.tag == "EnemyPistol")
        {
            Destroy(collision.gameObject);
            animator.SetBool("EnemyPistol", true);
            AudioManager.Instance.Play(AudioClipName.gun_Pickup);
        }

        if (collision.gameObject.tag == "ExitObject")
        {
            MenuManager.GoToMenu(MenuNames.StartMenu);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BerserkerSword")
        {
            Debug.Log("Ow");
            //disable the sword so it doesnt do damage again if it already hit player
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;       
            gameObject.GetComponent<HealthScript>().ChangeHealth(-collision.GetComponentInParent<HealthScript>().Damage);
            
        }    
    }


    #endregion
}
