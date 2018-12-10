using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckIntoSpace : MonoBehaviour {
    #region Fields

    List<GameObject> suckOut = new List<GameObject> ();
    List<GameObject> outside = new List<GameObject>();
    List<Vector2> direction = new List<Vector2>();
    BoxCollider2D box;
    [SerializeField]
    GameObject reinforcedWall;

    float time=5;
    #endregion
    #region Mehtods
    // Use this for initialization
    void Start () {
        box = gameObject.AddComponent<BoxCollider2D>();
        box.size =new Vector2 (10,10);
        box.isTrigger = true;
        //play sucking sound
        AudioManager.Instance.Play(AudioClipName.wall_Alarm);
	}
	
	// Update is called once per frame
	void Update () {
		for(int i =0; i<suckOut.Count; i++)
        {
            GameObject go = suckOut[i];
            Rigidbody2D rb2 = go.GetComponent<Rigidbody2D>();
            rb2.AddForce(direction[i]);
            go.transform.Rotate(Vector3.back*3);
            
           

        }
        time -= Time.deltaTime;
        if (time < 0)
        {
            foreach(GameObject go in suckOut)
            {
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);//if the player is still inside let them move around normaly 
            }
            //kill everything outside
            foreach(GameObject go in outside)
            {
                go.GetComponent<HealthScript>().ChangeHealth(-100);
            }
            //replace the sprites with reinfoced walls 
         
            AudioManager.Instance.Play(AudioClipName.wall_CloseSeal);
            Destroy(gameObject);
        }
        //after you leave the ship take damage 
        foreach(GameObject go in outside)
        {
            go.GetComponent<HealthScript>().ChangeHealth(-1);
           
        }

	}
    /// <summary>
    /// add anything in rainge to the list of things to be pulled out 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"||collision.gameObject.tag=="Enemy")
        {
     
                suckOut.Add(collision.gameObject);
                // collision.gameObject.GetComponent<Rigidbody2D>().drag = 0;
                Vector2 D = transform.position - collision.gameObject.transform.position;
                direction.Add(D.normalized);
            if (collision.gameObject.GetComponent<Rigidbody2D>() == null)
            {
                collision.gameObject.AddComponent<Rigidbody2D>();
            }
            //play a screaming sound 
            AudioManager.Instance.Play(AudioClipName.player_Scream);
            
        }

    }
    /// <summary>
    /// after the plater passes through the hole kill them slowly 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            outside.Add(collision.gameObject);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            Destroy(collision.gameObject.GetComponent<Collider2D>());
        }
    }

}
#endregion