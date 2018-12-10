using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = transform.position + new Vector3(0,0.1f,0);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the enemy collides with a bullet
        if (collision.gameObject.tag == "Wall")
        {
            //Destroy(gameObject);
            transform.position += new Vector3(0, -2, 0);
            transform.position += new Vector3(2, 0, 0);
            
        }
    }


}
