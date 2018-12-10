using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VentScript : MonoBehaviour {
    #region Fields
    MouseOverVent mouseOverVent;
    MouseOffVent mouseOffVent;
    SpriteRenderer spr;

    [SerializeField]
    GameObject target;
    #endregion
    #region  properties

    public Vector2 Target
    {
        get
        {
            if (target != null)
            {
                Vector3 parentrotation = target.transform.parent.localEulerAngles;
                Vector3 offset=Vector3.zero;
                float tangle = target.transform.localEulerAngles.z;
                Debug.Log(tangle);
            
                
                    if (tangle == 0)//up or down
                    {
                        offset.y = -2;

                    }
                    else if (tangle == 180)//up or down 
                    {
                        offset.y = 2;

                    }
                    else if (tangle == 90)//right or left
                    {
                        offset.x = 2;

                    }

                    else if (tangle == 270)//RIGHT OR LEFT
                    {
                        offset.x = -2;



                    }
                if (parentrotation != Vector3.zero)
                {
                    offset.y *= -1;
                }
               

                Debug.Log(offset);
                return target.transform.position+offset;             
            }
            else
            {
                return Vector2.zero;
            }
            
        }
    }

    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        //initallize a new mouseoverVent event 
        mouseOverVent = new MouseOverVent();
        mouseOffVent = new MouseOffVent();
        //add this script to be an ionvoker for  that event 
        EventManager.AddMouseOverVentInvoker(this);
        EventManager.AddMouseOffVentInvoker(this);
        spr = target.GetComponent<SpriteRenderer>();

	}
	


    /// <summary>
    /// add listeners to the mouse over vent event 
    /// </summary>
    /// <param name="listener"></param>
    public void AddVentMouseOverListener(UnityAction<Vector2,Vector2> listener)
    {
        mouseOverVent.AddListener(listener);
    }

    /// <summary>
    /// add listeners to the mouse off vent event
    /// </summary>
    public void AddMouseOffVentLIstener( UnityAction listener )
    {
        mouseOffVent.AddListener(listener);
    }

    /// <summary>
    /// invoke the mouse off vent event
    /// </summary>
    private void OnMouseExit()
    {
        mouseOffVent.Invoke();
    }

    /// <summary>
    /// invoke the on mouse over event
    /// </summary>
    private void OnMouseEnter()
    {
        if (target != null)
        {
            mouseOverVent.Invoke(transform.position, target.transform.position);
        }
    
    }
    #endregion
}
