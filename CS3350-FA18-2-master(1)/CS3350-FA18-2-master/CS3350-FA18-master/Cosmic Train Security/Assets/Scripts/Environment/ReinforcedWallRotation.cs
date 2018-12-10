using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcedWallRotation : MonoBehaviour {
    #region Fields
    Collider2D coll;
    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        //cast rays to find what direction it should face 
        int layerMask = 1 << 10;
       // layerMask = ~layerMask;
        if ( Physics2D.Raycast(transform.position, Vector2.right, 1,layerMask))
        {
            Debug.DrawRay(transform.position, Vector2.right,Color.blue,5);
            transform.Rotate(new Vector3(0, 0, 0));
        }
        if (Physics2D.Raycast(transform.position, Vector2.up, 1,layerMask))
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.red, 5);
            transform.Rotate(new Vector3(0, 0, 90));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Walls")
        {
            coll = collision.collider;
           
        }
    }
}


    #endregion
