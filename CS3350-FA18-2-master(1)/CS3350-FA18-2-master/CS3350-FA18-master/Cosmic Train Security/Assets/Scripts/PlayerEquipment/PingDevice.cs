using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PingDevice : MonoBehaviour {

    #region Fields
    public int pingDeviceCount = 0;
    [SerializeField]
    GameObject prefabTransparentPing;
    GameObject transparentPingInstance;

    ToolbeltEvent changeSelectionEvent;
    ToolbeltCountUpdateUI updateCountEvent;

    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        pingDeviceCount = 0;

        //event for changing the selection on the UI
        changeSelectionEvent = new ToolbeltEvent();
        //event for updating count for tool in UI
        updateCountEvent = new ToolbeltCountUpdateUI();

        //adding this script as Invokers to the two UI events
        EventManager.AddSelectionChangePingDeviceInvokers(this);
        EventManager.AddUpdatePingDeviceCountInvokers(this);
    }
	
    /// <summary>
    /// Adding a listener for event for changing enabled tool in UI
    /// </summary>
    /// <param name="listener"></param>
    public void AddChangeSelectionPingDeviceListener(UnityAction<Constants.Tools> listener)
    {
        changeSelectionEvent.AddListener(listener);
    }

    /// <summary>
    /// Adding listener for updating count on UI event
    /// </summary>
    /// <param name="listener"></param>
    public void AddUpdateCountPingDeviceListener(UnityAction<Constants.Tools, int> listener)
    {
        updateCountEvent.AddListener(listener);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void setTransparentPingRadius()
    {
        if (pingDeviceCount > 0)
        {
            Debug.Log("set transparent ping");
            transparentPingInstance = Instantiate(prefabTransparentPing, transform.position, transform.rotation);
        }

    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ping Materials")
        {
            if(gameObject.GetComponent<ToolBelt>().allToolsDisabled == true)
            {
                // AudioManager.Instance.Play(AudioClipName.tripwire_TTS);
                gameObject.GetComponent<ToolBelt>().enabledTool = Constants.Tools.PingDevice;
                gameObject.GetComponent<ToolBelt>().allToolsDisabled = false;

                //invoke the event to change the UI to make the tripwire the enabled tool
                changeSelectionEvent.Invoke(Constants.Tools.PingDevice);
            }
            AudioManager.Instance.Play(AudioClipName.item_Pickup);
            pingDeviceCount += 2;
            //TEMPORARY CODE FOR TUTORIAL TO TESTERS
            if (pingDeviceCount > 5)
            {
                pingDeviceCount = 5;
            }
            updateCountEvent.Invoke(Constants.Tools.PingDevice, pingDeviceCount);
            Destroy(collision.gameObject);
        }
    }
    #endregion
}
