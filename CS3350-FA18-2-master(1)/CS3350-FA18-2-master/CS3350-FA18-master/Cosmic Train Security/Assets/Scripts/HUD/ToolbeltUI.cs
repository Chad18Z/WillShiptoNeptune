using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToolbeltUI : MonoBehaviour {


    public Sprite select1;
    public Sprite select2;
    public Sprite select3;
    public Color transparentColor = Color.white;
    public Color clearColor = Color.black;
    public Color solidColor = Color.black;

    //Holds the UI elements
    [SerializeField]
    Image selection;
    [SerializeField]
    Image frame;
    [SerializeField]
    Image icon1;
    [SerializeField]
    Image icon2;
    [SerializeField]
    Image icon3;
    [SerializeField]
    Text icon1text;
    [SerializeField]
    Text icon2text;
    [SerializeField]
    Text icon3text;

    private SpriteRenderer spriteRenderer;

    //// Use this for initialization
    //void Start()
    //{
    //    Debug.Log("Called Start");
    //    transparentColor.a = 0.2f;
    //    clearColor.a = 0f;
    //    solidColor.a = 1.0f;

    //    //Add methods as the listeners to each Event for toolbelt UI
    //    EventManager.AddDisableAllToolsUIListeners(disableAllToolImages);
    //    EventManager.AddSelectionChangeListeners(ChangeSelection);
    //    EventManager.AddUpdateToolCountListeners(checkCountOfTool);
    //}
    private void Awake()
    {
       
        transparentColor.a = 0.2f;
        clearColor.a = 0f;
        solidColor.a = 1.0f;

        //Add methods as the listeners to each Event for toolbelt UI
        EventManager.AddDisableAllToolsUIListeners(disableAllToolImages);
        EventManager.AddSelectionChangeListeners(ChangeSelection);
        EventManager.AddUpdateToolCountListeners(checkCountOfTool);
    }

    /// <summary>
    /// Used to disable all tool icons in the toolbelt UI
    /// </summary>
    void disableAllToolImages()
    {
        icon1.color = transparentColor;
        icon1text.color = clearColor;
        icon2.color = transparentColor;
        icon2text.color = clearColor;
        icon3.color = transparentColor;
        icon3text.color = clearColor;
        selection.enabled = false;

    }

    /// <summary>
    /// Changes the highlight for which tool is enabled to show the user this tool is enabled
    /// </summary>
    /// <param name="tool"></param>
    void ChangeSelection(Constants.Tools tool)
    {
        //ensures the selection is reenabled if it was disabled from disableAllToolImages function
        if(!selection.enabled )
        {
            selection.enabled = true;
        }
        //Determines which tool will now contain the highlight
        switch (tool)
        {
            case Constants.Tools.TripWire:
                selection.sprite = select1;
                icon1.color = solidColor;
                icon1text.color = solidColor;
                //Debug.Log("Swap to 1");
                break;
            case Constants.Tools.StasisGrenade:
                selection.sprite = select2;
                icon2.color = solidColor;
                icon2text.color = solidColor;
                //Debug.Log("Swap to 2");
                break;
            case Constants.Tools.PingDevice:
                selection.sprite = select3;
                icon3.color = solidColor;
                icon3text.color = solidColor;
                //Debug.Log("Swap to 3");
                break;
        }
    }

    /// <summary>
    /// Checks the count of the tool and displays the count or disables the icon and text completely
    /// </summary>
    /// <param name="tool"></param>
    /// <param name="count"></param>
    void checkCountOfTool(Constants.Tools tool, int count)
    {
        if(tool == Constants.Tools.TripWire)
        {
            //Converts passed value to a string to display in the UI
            string display = count.ToString();
            icon1text.text = display;

            //If the count is 0 is sets the icon image to near transparent
            if (count == 0)
            {
                icon1.color = transparentColor;
                icon1text.color = clearColor;
            }
            //Otherwise its full color
            else
            {
                icon1.color = solidColor;
                icon1text.color = solidColor;
            }
        }
        else if(tool == Constants.Tools.StasisGrenade)
        {
            //Converts passed value to a string to display in the UI
            string display = count.ToString();
            icon2text.text = display;

            //If the count is 0 is sets the icon image to near transparent
            if (count == 0)
            {
                icon2.color = transparentColor;
                icon2text.color = clearColor;
            }
            //Otherwise its full color
            else
            {
                icon2.color = solidColor;
                icon2text.color = solidColor;
            }
        }
        else if(tool == Constants.Tools.PingDevice)
        {
            //Converts passed value to a string to display in the UI
            string display = count.ToString();
            icon3text.text = display;

            //If the count is 0 is sets the icon image to near transparent
            if (count == 0)
            {
                icon3.color = transparentColor;
                icon3text.color = clearColor;
            }
            //Otherwise its full color
            else
            {
                icon3.color = solidColor;
                icon3text.color = solidColor;
            }
        }

    }
}