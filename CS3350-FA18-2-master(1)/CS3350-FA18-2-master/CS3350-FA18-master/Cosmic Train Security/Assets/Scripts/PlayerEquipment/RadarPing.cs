using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RadarPing : MonoBehaviour
{
    #region Fields
    public float scaleRate = 0.015f;    // rate that ping grows in scale
    public float fadeRate = 0.02f;      // rate that ping fades away
    private float startScale = 0.05f;
    private Transform scale;
    private SpriteRenderer render;

    public Sprite radarRingPing;
    GameObject playerForPosition;
    public bool pinging = false;
    public bool setStartOfPing = false;

    ToolbeltCountUpdateUI updateCountEvent;

    #endregion

    #region Methods
    // Use this for initialization
    void Start ()
    {
        setStartOfPing = false;
        pinging = false;
        playerForPosition = GameObject.FindGameObjectWithTag("Player");
        render = gameObject.GetComponent<SpriteRenderer>();
        //this way ping doesnt start pinging until user sets the ping
        scale = gameObject.GetComponent<Transform>();

        //event to update the tool count in UI for tripwire
        updateCountEvent = new ToolbeltCountUpdateUI();
        //adding this script as an invoker for the update count UI event
        EventManager.AddUpdateRadarPingCountInvokers(this);

    }


    /// <summary>
    /// Add a listener to the update count event
    /// </summary>
    /// <param name="listener"></param>
    public void AddUpdateCountRadarPingDeviceListener(UnityAction<Constants.Tools, int> listener)
    {
        updateCountEvent.AddListener(listener);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), playerForPosition.GetComponent<Collider2D>());
        
            if (Input.GetKeyUp(KeyCode.Mouse1) && !pinging)
            {
                //Debug.Log("Pinging");
                gameObject.transform.position = playerForPosition.transform.position;
                AudioManager.Instance.Play(AudioClipName.transponder_RadarBleeps);
                playerForPosition.GetComponent<PingDevice>().pingDeviceCount--;
                //invoke event to update count for UI
                updateCountEvent.Invoke(Constants.Tools.PingDevice, playerForPosition.GetComponent<PingDevice>().pingDeviceCount);
                if (playerForPosition.GetComponent<PingDevice>().pingDeviceCount <= 0)
                {
                    playerForPosition.GetComponent<ToolBelt>().disableTool(Constants.Tools.PingDevice);
                }
                pinging = true;
            }
            if (pinging)
            {

                if (!setStartOfPing)
                {
                    render.sprite = radarRingPing;
                    scale.localScale = new Vector3(startScale, startScale, 1);
                    setStartOfPing = true;
                }


                if (scale.localScale.x < 1.7)
                {
                    scale.localScale += new Vector3(scaleRate, scaleRate, 0);
                }
                else
                {

                    render.color -= new Color(0, 0, 0, fadeRate);

                    if (render.color.a < 0.01)
                    {
                        //Debug.Log("Disable here??");
                        scale.localScale = new Vector3(startScale, startScale, 0);
                        render.color += new Color(0, 0, 0, 1);
                    Destroy(gameObject);
                    }
                }
            }
            else if (!pinging)
            {
                gameObject.transform.position = playerForPosition.transform.position;
            }
        
            
		    
	}

    #endregion
}
