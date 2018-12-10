using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class HealthScript : MonoBehaviour {
    #region fields 
    //the starting health 
    [SerializeField]
    public int health;
    //damage that a bullit dose 
    [SerializeField]
    int damage=33;
    //creat the event
    ChangeHealthBarEvent healthChangeEvent;
    //make the playeer emune to damage for a short time
    int InvunlerabilityTIme = 1;
    float timer = 0;//timer suport 
    bool invunlnerabll=false;//bool to check if inul is on 
    GetPlayerHealthEvent GetHealth;
    PlayerDeathEvent playerDeathEvent;  // Event for player death

    #endregion

    #region properties
    //allow for geting of the health 
    public int Health
    {
        get { return health; }
    }
    #endregion

    #region Methods  

    public int Damage
    {
        get { return damage; }
    }

   /// <summary>
   /// used for initalization 
   /// </summary>
    void Start () {

        // player death event
        playerDeathEvent = new PlayerDeathEvent();

        //create a new changeHEalthEvent
        healthChangeEvent = new ChangeHealthBarEvent();
        GetHealth = new GetPlayerHealthEvent();
        //add this script as an invoker
        EventManager.AddHealthChangeInvokers(this);
        EventManager.AddGetPlayerHealthInvoker(this);
       
        //add as a listener for canister explosions 
          EventManager.AddCanisterProjectileCollisionListener(HandleExplosions);

        //if on the player add the player health change event
        if (gameObject.tag == "Player")
        {
            EventManager.AddPlayerHealthChangeListener(ChangeHealth);
          
        }

        // Add to the list of invokers for player death
        EventManager.AddPlayerDeathInvoker(this);
	}
    /// <summary>
    /// used for player invunriblility 
    /// </summary>
    private void Update()
    {
        //timer for invurnrability
        if (invunlnerabll)
        {
            timer += Time.deltaTime;
            if(timer>InvunlerabilityTIme)
            {
                invunlnerabll = false;
            }
        }

        //if this script is on a wall that is destroyed 
        if(health<=0 && gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (health <= 0 && gameObject.tag == "Player")
        {
            //AudioManager.Instance.Play(AudioClipName.player_Death);
            playerDeathEvent.Invoke();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// add listeners to he health changed event
    /// </summary>
    /// <param name="listener"></param>
    public void AddHealthChangeListeners(UnityAction<int> listener)
    {
        healthChangeEvent.AddListener(listener);
    }

    public void AddGetPlayerHealthListener(UnityAction<int> listener)
    {
        GetHealth.AddListener(listener);
    }

    /// <summary>
    /// add listeners to the player death event
    /// </summary>
    /// <param name="listener"></param>
    public void AddPlayerDeathListener(UnityAction listener)
    {
        playerDeathEvent.AddListener(listener);
    }

    /// <summary>
    /// a method to allow other scripts to change the health 
    /// </summary>
    /// <param name="change"></param the amount you want the health to change>
    public void ChangeHealth(int change)
    {

        health += change;
        
        //if on the player update the healthbar 
        if (gameObject.tag == "Player")
        {
            if (health > 100)
            {
                health = 100;
            }
            healthChangeEvent.Invoke(health);
        }
    }

    /// <summary>
    /// detect colisions 
    /// </summary>
    /// <param name="collision"></param>
     void OnCollisionEnter2D(Collision2D collision)
    {
       
            if (collision.gameObject.tag == "HealthStation")
            {
                GetHealth.Invoke(health);
            }
        
        if (invunlnerabll == false)
        {
            //check what you are colliding with 
            if (collision.gameObject.tag == "EnemyBullet")
            {
               
                if (gameObject.tag == "Player")//if the player is hit call the health changed event to change the health bar 
                {
                    health -= damage;
                    AudioManager.Instance.Play(AudioClipName.player_Hurt);
                    timer = 0;
                    //only make invulrable and set healthbar for player 
                    invunlnerabll = true;
                    healthChangeEvent.Invoke(health);

                   
                }

            }

            /*
            //for testing pourposes only take damage when running into eanamys 
            // if(collision.gameObject.tag=="Enemy")
            // {
            //     if (gameObject.GetComponent<Enemy>().alive)
            //     {
            //         //deal damage               
            //         health -= 20;
            //         //update health bar 
            //         healthChangeEvent.Invoke(health);
            //         //set invunrable 
            //         invunlnerabll = true;
            //         //play the sound 
            //         AudioManager.Instance.Play(AudioClipName.player_Hurt);
            //     }
            // }
            //end of testing code 
            */

          
            //if the player loses all his health play the death sound and reload the scene  
            
        }
    }


    /// <summary>
    /// used for taking damage when there is an explosiuon
    /// </summary>
    /// <param name="location"></param>
    void HandleExplosions ( Vector2 location)
    {
      
        //determan distance from the explosion using pythagream formula 
        float distance =  Mathf.Abs(Mathf.Sqrt(Mathf.Pow(transform.position.x - location.x,2) + Mathf.Pow(transform.position.y - location.y , 2)));
        //check distance and apply damage to every thing inside of that distance.
        if (distance < 3)
        {

            health -= 50;
            //if on the player update the health bar
            if (gameObject.tag == "Player")
            {
                healthChangeEvent.Invoke(health);
            }
        }
        // plays death audio clip here to prevent it from stacking up and playing multiple times
        if (health <= 0 && gameObject.tag == "Player")
        {
            AudioManager.Instance.Play(AudioClipName.player_Death2);
        }
    }

  
    #endregion
}
