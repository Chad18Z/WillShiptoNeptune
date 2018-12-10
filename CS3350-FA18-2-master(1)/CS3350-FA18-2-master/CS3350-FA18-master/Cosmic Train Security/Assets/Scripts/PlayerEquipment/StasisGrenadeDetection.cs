using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisGrenadeDetection : MonoBehaviour {

    [SerializeField]
    GameObject enemyBullet; //used to ignore collision between grenade and enemy bullet
    public Rigidbody2D grenadeBody; //rigidbody to grenade instance
    public bool landed = false; //bool for whether grenade landed
    public bool timerStarted = false; //determines if timer started
    public float maxThrowDistance; //max throw distance player can throw grenade

    const float DetonateTime = .5f; //time before grenade detonates

    Timer detonatorTimer; //Timer to time the detonater time

    Vector3 cursorPos; //position of the mouse cursor

    Vector3 newestPosition; //new position for the grenade to be at each frame

    /// <summary>
    /// Initializes all values to default as soon as an instance of the grenade appears in scene
    /// </summary>
    void Start () {

        timerStarted = false;
        detonatorTimer = GetComponent<Timer>();
        maxThrowDistance = 8.485f;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        grenadeBody = gameObject.GetComponent<Rigidbody2D>();

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = transform.position.z;
        landed = false;
	}
	
	/// <summary>
    /// Determines if grenade landed and if the timer finished for grenade to explode
    /// </summary>
	void FixedUpdate () {

        //if grenade hasnt landed move the grenade towards the landing position 
        //where the mouse cursor was when this object was instantiated
        if (!landed)
        {
            //move grenade towards stored mouse cursor position when this object was first instantiated
            transform.position = Vector3.MoveTowards(transform.position, cursorPos, maxThrowDistance * Time.deltaTime);
            newestPosition = cursorPos;

            //if position of grenade reached the cursor position then grenade landed
            if (transform.position == newestPosition)
            {
                //this way grenade doesnt move with other collisions
                grenadeBody.isKinematic = true;
                //grenade now landed is true
                landed = true;
                //play grenade landed sound
                AudioManager.Instance.Play(AudioClipName.grenade_Land);
            }
        }
        //if grenade landed
        else if (landed)
        {

            //if the timer isnt started for the detonation then start it
            if(!timerStarted)
            {
                //set duration and start timer
                detonatorTimer = gameObject.AddComponent<Timer>();
                detonatorTimer.Duration = DetonateTime;
                detonatorTimer.Run();
                //set variable to true so we dont set the timer again before its finished
                timerStarted = true;
            }
            
            //if detonate timer is finished and the timer was started then detonate the grenade
            if(detonatorTimer.Finished && timerStarted)
            {
                //play grenade explosion sound
                AudioManager.Instance.Play(AudioClipName.grenade_Explode);
                //timer is done then enable ci//need to do this after timer
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                //destroy the grenade after as well
                Destroy(gameObject,.1f);
            }

        }
        
	}

    /// <summary>
    /// Detects collisions with other objects so grenade stops at the position it collided with the object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Locker" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Canister")
        {
            //set newest position to the position the grenade was on contact with object
            newestPosition = transform.position;
            //play grenade landed sound
            AudioManager.Instance.Play(AudioClipName.grenade_Land);
            //landed is now true
            landed = true;
            //this way grenade doesnt move from other collisions
            grenadeBody.isKinematic = true;
        }
    }
}
