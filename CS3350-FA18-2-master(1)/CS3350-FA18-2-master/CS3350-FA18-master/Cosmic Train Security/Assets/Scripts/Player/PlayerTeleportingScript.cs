using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportingScript : MonoBehaviour {
    #region fields
    //bool to check if player is on a vent 
    bool onvent; //true if player is on vent 
    Vector2 target; //the location of the target vent 
    #endregion
    #region Methods

    WallDestroyer wallDestroyer;

    // Use this for initialization
    void Start () {

        wallDestroyer = GameObject.FindGameObjectWithTag("Grid").GetComponent<WallDestroyer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (onvent)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position = new Vector2(target.x,target.y);
                AudioManager.Instance.Play(AudioClipName.vent_Open);

            }
        }
    }

    /// <summary>
    /// check if the player is on a vent 
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "vent")
        {
            onvent = true;

            // check if this vent is a part of exterior wall
            Vector3 ventPosition = coll.gameObject.transform.position;
            ExteriorEdgeEnum edgePostion = wallDestroyer.ExteriorWall(ventPosition);

            target = coll.gameObject.GetComponent<VentScript>().Target;

            if (target != Vector2.zero)
            {
                edgePostion = ExteriorEdgeEnum.NOEDGE;
            }

            switch (edgePostion)
            {
                case ExteriorEdgeEnum.TOPEDGE:
                    ventPosition.y += 3f;
                    target = ventPosition;
                    break;

                case ExteriorEdgeEnum.RIGHTEDGE:
                    ventPosition.x += 3f;
                    target = ventPosition;
                    break;

                case ExteriorEdgeEnum.LEFTEDGE:
                    ventPosition.x -= 3f;
                    target = ventPosition;
                    break;

                case ExteriorEdgeEnum.BOTTOMEDGE:
                    ventPosition.y -= 3f;
                    target = ventPosition;
                    break;

                case ExteriorEdgeEnum.NOEDGE:
                    target = coll.gameObject.GetComponent<VentScript>().Target;
                    break;

                default:
                    target = coll.gameObject.GetComponent<VentScript>().Target;
                    break;
            }

        }
    }

    /// <summary>
    /// check when player exits the vent 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "vent")
        {
            onvent = false;
            target = Vector2.zero;
        }
    }
}
#endregion
