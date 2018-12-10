using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object attached to this script is stunned for a certain time
/// when in contact of the stun grenade explosion
/// the object must also have the Timer.cs script attached
/// </summary>
public class StasisGrenadeStun : MonoBehaviour
{

    public bool timerStarted = false; //determines if timer started for this instance
    const float StunTime = 3f; //stun time for enemy to be stunned
    Timer stunTimer; //Timer for the enemy to be stunned
    public bool stunned = false; //determines if enemy is stunned

    /// <summary>
    /// Gets the Timer script and initially sets the bool values to false
    /// </summary>
    private void Start()
    {
        stunTimer = GetComponent<Timer>();
        timerStarted = false;
        stunned = false;
    }

    /// <summary>
    /// Enables enemy if the stun timer finished and the timer was started
    /// </summary>
    void Update()
    {

        if (stunTimer.Finished && timerStarted)
        {
            //timer is no longer running
            timerStarted = false;
            //enemy is no longer stunned
            stunned = false;

            //enable enemy scripts 
            if (gameObject.GetComponent<Enemy>())
            {
                gameObject.GetComponent<Enemy>().enabled = true;
            }
            if (gameObject.GetComponent<Berserker>() != null)
            {
                gameObject.GetComponent<Berserker>().enabled = true;
            }

            //if enemy is still alive after being stunned then set the FOV mesh to off
            if (gameObject.GetComponent<Enemy>())
            {
                if (gameObject.GetComponent<Enemy>().Alive)
                {
                    transform.Find("FOV").gameObject.SetActive(true);
                }
            }
            if (gameObject.GetComponent<Berserker>())
            {
                if (gameObject.GetComponent<Berserker>().Alive)
                {
                    transform.Find("FOV").gameObject.SetActive(true);
                }
            }

        }
    }


    /// <summary>
    /// Detects collision between enemy and stun grenade trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the enemy collided with stasis grenade and isnt stunned already
        if (collision.gameObject.tag == "Stasis Grenade" && !stunned)
        {
            //get the Timer script
            stunTimer = gameObject.AddComponent<Timer>();
            //set the Timer duration and start the Timer
            stunTimer.Duration = StunTime;
            stunTimer.Run();
            //set bools appropriately
            timerStarted = true;
            stunned = true;

            //play sound for enemy to be frozen
            AudioManager.Instance.Play(AudioClipName.enemy_Frozen);

            //disable the enemy
            gameObject.GetComponent<Enemy>().enabled = false;
            if (gameObject.GetComponent<Berserker>() != null)
            {
                gameObject.GetComponent<Berserker>().enabled = false;
            }
            transform.Find("FOV").gameObject.SetActive(false);
            gameObject.GetComponent<Animator>().SetBool("MoveInput", false);
        }
    }
}
