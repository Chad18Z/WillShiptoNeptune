using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeserkerTrigger : MonoBehaviour {

    #region field

    // beserker trigger event
    BeserkerTriggerEvent besekerTriggerEvent;

    // allow multiple allerts
    public bool multipleAlerts = false;

    // initial Beserker Trigger
    bool playerArrived;

    #endregion

    #region methods

    /// <summary>
    /// Use this to initialize the invoker for the Beserker Trigger system
    /// </summary>
    void Start ()
    {
        // creates new event
        besekerTriggerEvent = new BeserkerTriggerEvent();

        // adds Trigger to invokers
        EventManager.AddBeserkerTriggerInvokers(this);

        // sets first interaction to false
        playerArrived = false;

    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update () {
		
	}

    /// <summary>
    /// Add a listener to the canisterExplosionEvent invoked by this script
    /// </summary>
    /// <param name="call"></param>
    public void AddBeserkerTriggerListener(UnityAction<Vector3> call)
    {
        besekerTriggerEvent.AddListener(call);
    }

    /// <summary>
    /// when box collider enters a collision, check to see if it is the player
    /// only if it is then player, then Trigger (invoke) the Beserker into action. 
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if first collsion
        if (!playerArrived)
        {
            if (collision.gameObject.tag == "Player")
            {
                // invokes the beserker of locaiton.
                besekerTriggerEvent.Invoke(collision.gameObject.transform.position);

                // checks if multiple beserker alerts are being used
                // if not, closes the use of the the invoker. 
                if (!multipleAlerts)
                {
                    playerArrived = false;
                }
            }
        }
    }

    #endregion
}
