using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FiringScript : MonoBehaviour
{
    #region Fields
    float cooldownTime;
    [SerializeField]
    float defaultGunCooldownTime = .5f;
    [SerializeField]
    float enemyGunCooldownTime = .3f;
    [SerializeField]
    int EnergyPerShot = 20;
    [SerializeField]
    GameObject prefabPlayerBullet;
    [SerializeField]
    GameObject prefabEnemyBullet;
    //the bullet the player will shoot
    GameObject bullet;
    [SerializeField]
    public Animator animator;
    bool HasShotThisFrame = false;  // a bool to keep the player from going full auto
    int energy = 100;
    float cooldown = 1;             // time for cooldown when energy is low
    bool onCooldown = false;
    bool gameIsFrozen;

    //variable to reference the empty event class
    EnergyEvent maxEnergyEvent;
    PlayerDeathEvent playerDeathEvent;
    #endregion

    #region Properties

    /// <summary>
    /// Property for getting and setting the current energy
    /// </summary>
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
        }

    }

    #endregion

    #region Methods

    private void Start()
    {
        //Initializes the EnergyEvent instance
        //then sets the object attached to this script as 
        //an invoker in the invoker list in the Event Manager
        maxEnergyEvent = new EnergyEvent();
        EventManager.SetChargeToMaxAddEventInvoker(this);
        EventManager.AddPlayerDeathListener(HandleSetGameIsFrozen);
    }
    /// <summary>
    /// Adds the listener to the Event Manager
    /// </summary>
    /// <param name="listener"></param>
    public void MaxEnergyListener(UnityAction listener)
    {
        maxEnergyEvent.AddListener(listener);
    }
    /// <summary>
    /// check to see if the user is trying to fire
    /// </summary>
    void Update()
    {
        if (animator.GetBool("EnemyPistol") == true)
        {
            cooldownTime = enemyGunCooldownTime;
            bullet = prefabEnemyBullet;
        }
        else if (animator.GetBool("EnemyPistol") == false)
        {
            cooldownTime = defaultGunCooldownTime;
            bullet = prefabPlayerBullet;
        }
        //check if input is being provided and keep the player from rapid fire
        if (Input.GetAxisRaw("Fire1") != 0 && HasShotThisFrame != true && onCooldown == false && PauseMenu.GamePaused == false && gameIsFrozen == false)
        {
            //Empty firing sound setup
            if (energy <= 0)
            {
                AudioManager.Instance.Play(AudioClipName.player_WeaponEmpty);
            }
            //Standard firing sound setup
            else
            {
                AudioManager.Instance.Play(AudioClipName.Fire);
            }

            // grabs the vector position of the mouse cursor
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // creates a new direction value by using the coordinate differences between the cursor and player
            Vector3 direction = (new Vector3(cursorPosition.x - transform.position.x, cursorPosition.y - transform.position.y, 0).normalized);

            // Move the bullet out to the edge of the sprite where the gun is (assume it is centered and at the edge of the sprite)
            // Also assuming that a unit square in Unity is 128x128 
            float spriteWidth = GetComponentInParent<SpriteRenderer>().sprite.rect.width / 128;

            GameObject bulletInstance = Instantiate(bullet, transform.position + direction * spriteWidth, transform.rotation);
            Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            //GameObject bulletInstance = Instantiate(prefabBullit, transform.position, transform.rotation);
            energy -= EnergyPerShot;
            //turn on the cooldown 
            onCooldown = true;

            //use for enabling the cooldown and slowing the bullits
            if (energy < 15) 
            {

                //used to fix a null reference exception when 
                //player has enemy pistol equipped and energy goes below 15
                //it tries to grab the bullet script of the player but should
                //be grabbing the enemy bullet script when bullet comes from enemy pistol
                if (bulletInstance.GetComponent<BulletScript>() != null)
                {
                    bulletInstance.GetComponent<BulletScript>().BulletSpeed = .3f;
                }
                else if (bulletInstance.GetComponent<EnemyBulletScript>() != null)
                {
                    bulletInstance.GetComponent<EnemyBulletScript>().BulletSpeed = .3f;
                }
                else
                {
                    Debug.LogError("Null references to all bullet scripts");
                }
                

            }

            HasShotThisFrame = true;
        }

        //change has shot this frame to false after the player stops holding the button
        if (Input.GetAxisRaw("Fire1") == 0) 
        {
            HasShotThisFrame = false;
        }

        //count down through the cool down and alow firing after it compleats
        if (onCooldown)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
            {
               
                onCooldown = false;
                //set the cooldown to cooldown timer or a longer timer based on energy remaining
                if (energy > 15)
                {
                    cooldown = cooldownTime;
                }
                else
                {
                    cooldown = cooldownTime*2;
                }
            }
        }

        if ((energy <= 0) && (animator.GetBool("EnemyPistol") == true))
        {
            animator.SetBool("EnemyPistol", false);
        }
    }

    #endregion

    /// <summary>
    /// Used to detect collisions with the player such as
    /// colliding with the charge station
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Charge Station") || (collision.gameObject.tag == "EnemyPistol"))
        {
            //invokes event to maximize the energy value
            AudioManager.Instance.Play(AudioClipName.gun_Charge);
            maxEnergyEvent.Invoke();
        }
    }

    void HandleSetGameIsFrozen()
    {
        gameIsFrozen = true;
    }

}