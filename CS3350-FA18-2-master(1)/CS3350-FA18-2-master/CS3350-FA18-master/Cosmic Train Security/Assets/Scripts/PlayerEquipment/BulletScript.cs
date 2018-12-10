using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletScript : MonoBehaviour
{
    #region Fields

    float x;                // x and y coordinates for the bullet movement 
    float y;
    float bulletSpeed = 1;  //speed of the bullet
    float removetime = 2;   //time until the bullet is removed 
    Rigidbody2D rb2d;
    [SerializeField]
    public ParticleSystem splashEffect;


    #endregion
    #region Properties 

    /// <summary>
    /// Allow for changing of bullit speed due to low energy
    /// </summary> 
    public float BulletSpeed
    {
        set
        {
            bulletSpeed = value;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// do the math for determinig the bullet direction
    /// </summary>
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); //get the rigid body 
        x = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z) * bulletSpeed; //determine x and y components
        y = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z) * bulletSpeed;

      
        

    }

    /// <summary>
    /// move the bullet the appropriate amount every frame
    /// </summary>
    void Update()
    {
        //move the bullet appropriately
        rb2d.MovePosition(new Vector2(transform.position.x + x, transform.position.y + y));

        removetime -= Time.deltaTime;

        //delete the bullet after a certain time 
        if (removetime <= 0)
        {
            Destroy(gameObject);
        }

    }

   
    /// <summary>
    /// called when the bullet collides with something
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
		// Destory on impact with Wall.
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door")
        {
            ParticleSystem splashEffectParticleSystem = Instantiate(splashEffect, transform.position, transform.rotation);
            Destroy(splashEffectParticleSystem, 1);
        }
		if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Locker" || 
            collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Charge Station" ||
            collision.gameObject.tag == "Large Crate" || collision.gameObject.tag == "Door")
		{
			Destroy(gameObject);
		}
        // If the bullet comes into contact with a canister
      
	}

	#endregion
}
