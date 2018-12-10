using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolBelt : MonoBehaviour {

    DisableToolBeltUI disableEvent; //event used to disable the UI for toolbelt when all tools are disabled
    ToolbeltEvent changeSelectionEvent; //used to change the highlight to the enabled tool

    public Constants.Tools enabledTool; //the tool currently enabled to the player
    public bool allToolsDisabled; //determines if all tools are disabled after checking each count of each tool

    bool tripWireChecked = false; //determines if count of tripwire was checked
    bool stasisGrenadeChecked = false; //determines if count of grenade was checked
    bool pingDeviceChecked = false;

    
    private void Start()
    {
        //event for changing selected tool in the toolbelt UI
        changeSelectionEvent = new ToolbeltEvent();
        //event for disabling the UI for toolbelt when all tools are disabled
        disableEvent = new DisableToolBeltUI();
        //add this script as an invoker to the two above events
        EventManager.AddDisableAllToolsUIInvokers(this);
        EventManager.AddSelectionChangeInputInvokers(this);

        tripWireChecked = false;
        stasisGrenadeChecked = false;
        pingDeviceChecked = false;
        enabledTool = Constants.Tools.StasisGrenade; //by default set tripwire as enabled tool
        allToolsDisabled = true; //start of game disable all tools

        //event invoked to disable all tools in the UI 
        disableEvent.Invoke();
    }

    /// <summary>
    /// add a listener to the disable all tools event
    /// </summary>
    /// <param name="listener"></param>
    public void AddDisableAllToolsUIListener(UnityAction listener)
    {
        // disableEvent.AddL(listener);
        disableEvent.AddListener(listener);
    }

    /// <summary>
    /// add a listener to change selected tool event for toolbelt UI
    /// </summary>
    /// <param name="listener"></param>
    public void AddChangeToolSelectionListener(UnityAction<Constants.Tools> listener)
    {
        changeSelectionEvent.AddListener(listener);
    }
    // Update is called once per frame
    void Update () {

        //Debug.Log("Tools are disabled? " + allToolsDisabled);
       
        //if all the tools arent disabled
        if(!allToolsDisabled)
        {
            //if player presses one then try to switch to tripwire tool
            if (Input.GetKeyDown("1"))
            {
                //if count of tripwire is greater than 0, then switch to it
                if (gameObject.GetComponent<TripWire>().tripWireCount > 0)
                {
                    
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    //enabled tool is now TripWire
                    enabledTool = Constants.Tools.TripWire;
                    //invoke event to change the selection on the UI to the now enabled tool TripWire
                    changeSelectionEvent.Invoke(enabledTool);
                    //Debug.Log("Switched through num 1");
                }
                //if count is less than 1 then play sound for not enough materials
                else
                {
                    //Debug.Log("Not enough to switch for num 1 use");
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);
                }
            }
            //if player presses two then try to switch to grenade tool
            else if (Input.GetKeyDown("2"))
            {
                //if grenade count if greater than 0 then switch to this tool
                if (gameObject.GetComponent<StasisGrenade>().grenadeCount > 0)
                {
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    //set enabled tool as Stasis Grenade
                    enabledTool = Constants.Tools.StasisGrenade;
                    //invoke the event for changing the highlight in the toolbelt UI to the enabled tool, Stasis Grenade
                    changeSelectionEvent.Invoke(enabledTool);
                    //Debug.Log("Switched through num 2");
                }
                //if count is less than one then play sound for not enough materials
                else
                {
                    //Debug.Log("Not enough to switch for num 2 use ");
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                //if grenade count if greater than 0 then switch to this tool
                if (gameObject.GetComponent<PingDevice>().pingDeviceCount > 0)
                {
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    //set enabled tool as Stasis Grenade
                    enabledTool = Constants.Tools.PingDevice;
                    //invoke the event for changing the highlight in the toolbelt UI to the enabled tool, Stasis Grenade
                    changeSelectionEvent.Invoke(enabledTool);
                    //Debug.Log("Switched through num 2");
                }
                //if count is less than one then play sound for not enough materials
                else
                {
                    //Debug.Log("Not enough to switch for num 2 use ");
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);
                }
            }
            //Debug.Log("Tool enabled on " + enabledTool);
            //if player scrolls up then switch to the tool to the left of the current tool enabled
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {

                //used to set enabledTool to end of enums list
                //need to replace this with ping device once it's created
                if (enabledTool == Constants.Tools.TripWire)
                {
                    if(gameObject.GetComponent<PingDevice>().pingDeviceCount <= 0)
                    {
                        enableTool(Constants.Tools.StasisGrenade);
                    }
                    else
                    {
                        enableTool(Constants.Tools.PingDevice);
                    }
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    
                    
                }
                //if enabled tool isnt the beginning of the list then go back one
                //in the enums and try to enable that tool
                else if (enabledTool == Constants.Tools.StasisGrenade)
                {
                    if(gameObject.GetComponent<TripWire>().tripWireCount <= 0)
                    {
                        enableTool(Constants.Tools.PingDevice);
                    }
                    else
                    {
                        enableTool(Constants.Tools.TripWire);
                    }
                    
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                }
                else if(enabledTool == Constants.Tools.PingDevice)
                {
                    if(gameObject.GetComponent<StasisGrenade>().grenadeCount <= 0)
                    {
                        enableTool(Constants.Tools.TripWire);
                    }
                    else
                    {
                        enableTool(Constants.Tools.StasisGrenade);
                    }                   
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                }

                //Debug.Log("Scrolled up and tool is " + enabledTool);

            }
            //if player scrolls down to switch tools
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                

                if (enabledTool == Constants.Tools.TripWire)
                {
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    enableTool(Constants.Tools.StasisGrenade);
                   
                }
                else if (enabledTool == Constants.Tools.StasisGrenade)
                {
                    enableTool(Constants.Tools.PingDevice);
                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    
                }
                //used to reset the enabledTool to the beginning of enums
                //if we are at the end of enum list 
                else if (enabledTool == Constants.Tools.PingDevice)
                {

                    AudioManager.Instance.Play(AudioClipName.toolBelt_Search);
                    enableTool(Constants.Tools.TripWire);
                }

                //Debug.Log("Scrolled down and enabled tool is " + enabledTool);
            }
        }
        //when tools are all disabled and trying to change tools through num keys
        //play sound to show theres no materials
        else if(allToolsDisabled)
        {
            //for tripwire
            if (Input.GetKeyDown("1"))
            {
                //Debug.Log("cant move, all tools disabled");
                AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);

            }
            //for stasis grenade
            else if (Input.GetKeyDown("2"))
            {
                //Debug.Log("Cant move all tools disabled");
                AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);
            }
            else if(Input.GetKeyDown("3"))
            {
                AudioManager.Instance.Play(AudioClipName.toolBelt_Denied);
            }
        }
        
    }

    /// <summary>
    /// Used to disable the current tool passed as an argument
    /// </summary>
    /// <param name="toolToDisable"></param>
    public void disableTool(Constants.Tools toolToDisable)
    {
        if(toolToDisable == Constants.Tools.TripWire)
        {
            enableTool(Constants.Tools.StasisGrenade);
        }
        else if(toolToDisable == Constants.Tools.StasisGrenade) //or other item thats not to the right
        {
            enableTool(Constants.Tools.PingDevice);
        }
        else if (toolToDisable == Constants.Tools.PingDevice)
        {
            enableTool(Constants.Tools.TripWire);
        }
    }
    /// <summary>
    /// Tries to enable the tool 
    /// </summary>
    /// <param name="toolToEnable"></param>
    public void enableTool(Constants.Tools toolToEnable)
    {
        if (tripWireChecked && stasisGrenadeChecked && pingDeviceChecked)
        {
            //reset booleans to search again next time trying to switch
            tripWireChecked = false;
            stasisGrenadeChecked = false;
            pingDeviceChecked = false;
            //all tools were checked and were disabled, set bool to true
            allToolsDisabled = true;

            //invokes event that is a listener to disable all tools
            disableEvent.Invoke();
        }

        else if (toolToEnable == Constants.Tools.TripWire)
        {
            if (gameObject.GetComponent<TripWire>().tripWireCount > 0)
            {
                //AudioManager.Instance.Play(AudioClipName.tripwire_TTS);
                //Debug.Log("Enable the tripwire ");


                enabledTool = toolToEnable;
                changeSelectionEvent.Invoke(enabledTool);
                //reset checkings
                tripWireChecked = false;
                stasisGrenadeChecked = false;
                pingDeviceChecked = false;
            }
            else
            {
                //Debug.Log("Not enough tripwires");
                tripWireChecked = true;
                //next enum tool is stasis grenade
                enableTool(++toolToEnable);
            }

        }

        else if (toolToEnable == Constants.Tools.StasisGrenade)
        {
            if (gameObject.GetComponent<StasisGrenade>().grenadeCount > 0)
            {
                //AudioManager.Instance.Play(AudioClipName.stasis_Grenade_TTS);
                //Debug.Log("Enable grenades");
                enabledTool = toolToEnable;
                changeSelectionEvent.Invoke(enabledTool);
                //reset checking
                tripWireChecked = false;
                stasisGrenadeChecked = false;
                pingDeviceChecked = false;
            }
            else
            {
                //Debug.Log("not enough grenades");
                stasisGrenadeChecked = true;
                //next enum tool is ping device
                enableTool(++toolToEnable);
            }
        }

        else if (toolToEnable == Constants.Tools.PingDevice)
        {
            if (gameObject.GetComponent<PingDevice>().pingDeviceCount > 0)
            {
                //AudioManager.Instance.Play(AudioClipName.stasis_Grenade_TTS);
                Debug.Log("Enable ping device");
                enabledTool = toolToEnable;
                changeSelectionEvent.Invoke(enabledTool);
                tripWireChecked = false;
                stasisGrenadeChecked = false;
                pingDeviceChecked = false;
            }
            else
            {
                Debug.Log("not enough grenades");
                pingDeviceChecked = true;
                //goes to beginning of enum list to try and check for use
                enableTool(Constants.Tools.TripWire);
            }
        }

    }
}
