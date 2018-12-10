using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StasisGrenade : MonoBehaviour {

    ToolbeltEvent selectionChangeEvent;
    ToolbeltCountUpdateUI updateCountEvent;
    public int grenadeCount = 0; //keep count for grenades player has
    [SerializeField]
    GameObject stasisGrenadePrefab;
    GameObject stasisGrenade;

    Timer grenadeCoolDown; //cool down timer for player to be able to throw grenade again
    const float CoolDownTime = .25f; //time it takes for the cooldown

	// Use this for initialization
	void Start () {
        grenadeCoolDown = gameObject.GetComponent<Timer>();
        //event for changing enabled tool in UI
        selectionChangeEvent = new ToolbeltEvent();
        //adding this script as an invoker for changing enabled tool in UI
        EventManager.AddSelectionChangeStasisGrenadeInvokers(this);
        //event for updating the count for the Grenade tool in the UI
        updateCountEvent = new ToolbeltCountUpdateUI();
        //adding this script as an invoker to the update count UI event
        EventManager.AddUpdateStasisGrenadeInvokers(this);
	}
    private void Update()
    {
        //Debug.Log("grenade count " + grenadeCount);
    }

    /// <summary>
    /// Add listener to event for changing enabled tool in UI
    /// </summary>
    /// <param name="listener"></param>
    public void AddChangeSelectionStasisGrenadeListener(UnityAction<Constants.Tools> listener)
    {
        selectionChangeEvent.AddListener(listener);
    }

    /// <summary>
    /// Add listener for event for updating the count for the grenade tool in the UI
    /// </summary>
    /// <param name="listener"></param>
    public void AddUpdateStasisGrenadeCountListener(UnityAction<Constants.Tools,int> listener)
    {
        updateCountEvent.AddListener(listener);
    }


    /// <summary>
    /// Player interacts with grenade materials and increments count
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grenade Material")
        {
            //This should probably be an event instead
            if (gameObject.GetComponent<ToolBelt>().allToolsDisabled == true)
            {
                //AudioManager.Instance.Play(AudioClipName.stasis_Grenade_TTS);
                gameObject.GetComponent<ToolBelt>().enabledTool = Constants.Tools.StasisGrenade;
                gameObject.GetComponent<ToolBelt>().allToolsDisabled = false;

                //invoke event to change highlight to grenade
                selectionChangeEvent.Invoke(Constants.Tools.StasisGrenade);
            }
            Destroy(collision.gameObject);
            grenadeCount++;
            //TEMPORARY CODE FOR TUTORIAL TO TESTERS
            if (grenadeCount > 5)
            {
                grenadeCount = 5;
            }
            //update the count in the UI for the stasis grenade
            updateCountEvent.Invoke(Constants.Tools.StasisGrenade, grenadeCount);
            AudioManager.Instance.Play(AudioClipName.item_Pickup);
        }
    }

    public void throwStasisGrenade()
    {
        if (grenadeCount > 0 && !grenadeCoolDown.Running)
        {
            // grabs the vector position of the mouse cursor
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // creates a new direction value by using the coordinate differences between the cursor and player
            Vector3 direction = (new Vector3(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, 0).normalized);

            float spriteWidth = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width / 100;

            //place the grenade initially in front of the player
            stasisGrenade = Instantiate(stasisGrenadePrefab, transform.position + direction * spriteWidth, Quaternion.identity);

            //decrement grenade count
            --grenadeCount;

            //invoker event for updating count for grenade tool in UI 
            updateCountEvent.Invoke(Constants.Tools.StasisGrenade, grenadeCount);
            if(grenadeCount <= 0)
            {
                gameObject.GetComponent<ToolBelt>().disableTool(Constants.Tools.StasisGrenade);
            }

            //set the Timer duration
            grenadeCoolDown.Duration = CoolDownTime;
            //start the Timer
            grenadeCoolDown.Run();

            //Play grenade throw sound
            AudioManager.Instance.Play(AudioClipName.grenade_Throw);

        }
    }
}
