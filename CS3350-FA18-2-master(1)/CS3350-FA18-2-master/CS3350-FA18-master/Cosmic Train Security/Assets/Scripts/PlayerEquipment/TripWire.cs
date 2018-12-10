using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TripWire : MonoBehaviour
{
    #region Fields

    public int tripWireCount = 0; //count for tripwire materials player has
    public bool initialTripWirePlaced = false;
    GameObject tripWireInitialPlacement;
    [SerializeField]
    GameObject prefabInitialTripWire;
    public float placementRadius; //radius the player can place the second piece of trap
    public float explosionRadius; //radius the explosion reaches

    ToolbeltEvent changeSelectionEvent;
    ToolbeltCountUpdateUI updateCountEvent;
    #endregion

    #region Methods

   
    /// <summary>
    /// Used to initialize variables
    /// </summary>
    void Start()
    {
        //event for changing the selection on the UI
        changeSelectionEvent = new ToolbeltEvent();
        //event for updating count for tool in UI
        updateCountEvent = new ToolbeltCountUpdateUI();

        //adding this script as Invokers to the two UI events
        EventManager.AddSelectionChangeTripWireInvokers(this);
        EventManager.AddUpdateTripWireCountInvokers(this);
        //since a Unity tile unit is 1 for height and width
        //we want the explosion radius to be within 1.5 files
        explosionRadius = 2.121f;

        //distance of 2 tiles to use for placement
        //this is used for determining how far the 
        //second part of trip wire can be placed before it destroys itself
        placementRadius = 2.828f;
    }

    /// <summary>
    /// Adding listener for event for changing enabled tool in UI 
    /// </summary>
    /// <param name="listener"></param>
    public void AddChangeSelectionTripWireListener(UnityAction<Constants.Tools> listener)
    {
        changeSelectionEvent.AddListener(listener);
    }

    /// <summary>
    /// Adding listener for updating count on UI event
    /// </summary>
    /// <param name="listener"></param>
    public void AddUpdateCountTripWireListener(UnityAction<Constants.Tools,int> listener)
    {
        updateCountEvent.AddListener(listener);
    }
    /// <summary>
    /// Used to detect tripwire material pick ups as this is attached to the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trip Wire Materials")
        {
            //this should probably be an event instead
            if(gameObject.GetComponent<ToolBelt>().allToolsDisabled == true)
            {
               // AudioManager.Instance.Play(AudioClipName.tripwire_TTS);
                gameObject.GetComponent<ToolBelt>().enabledTool = Constants.Tools.TripWire;
                gameObject.GetComponent<ToolBelt>().allToolsDisabled = false;

                //invoke the event to change the UI to make the tripwire the enabled tool
                changeSelectionEvent.Invoke(Constants.Tools.TripWire);
            }

            

            tripWireCount++;
            //TEMPORARY CODE FOR TUTORIAL TO TESTERS
            if(tripWireCount > 5)
            {
                tripWireCount = 5;
            }
            updateCountEvent.Invoke(Constants.Tools.TripWire, tripWireCount);
            Destroy(collision.gameObject);
            AudioManager.Instance.Play(AudioClipName.item_Pickup);
        }
    }

    /// <summary>
    /// Used to place the initial tripwire plug in front of the player
    /// </summary>
    public void placeInitialWirePlug()
    {
        //trip wire is looking for input from player to place down second part of wire every frame
        //if the wire is placed, the trip wire should no longer care about player input
        //there's no way to set placedWire back to false through this class
        //therefore once the tripwire is placed, it cant be placed again
        // grabs the vector position of the mouse cursor
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // creates a new direction value by using the coordinate differences between the cursor and player
        Vector3 direction = (new Vector3(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, 0).normalized);

        // place the initial wire in front of the player instead of on the player, similar to the bullet instantiation
        // Also assuming that a unit square in Unity is 128x128
        float spriteWidth = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width / 128;

        if (tripWireCount > 0 && !initialTripWirePlaced)
        {
            //play sound for placing trap
            AudioManager.Instance.Play(AudioClipName.tripWire_Placement1);

            //instantiates the final piece of the tripwire in front of the player
            //where the player pressed Q
            tripWireInitialPlacement = Instantiate(prefabInitialTripWire, transform.position + direction * spriteWidth * placementRadius * .85f, Quaternion.identity);

            //This makes sure we dont instantiate another tripwire
            //before the first one is set
            initialTripWirePlaced = true;
        }

    }
    #endregion
}
