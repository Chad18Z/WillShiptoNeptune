using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TripWireDetection : MonoBehaviour {

    #region Fields
    //need to implement event system to grab player positions
    public GameObject player;

    public bool fullTrapSet = false; //determines if actions after trap is set need to be done
    float explosionRadius; //explosion radius from bomb piece
    public float placementRadius; //placement radius for tripwire
    float plugSpriteWidth; //sprite width of intial tripwire plug sprite

    [SerializeField]
    GameObject finalPlacementTripWire; //contains bomb piece of tripwire
    [SerializeField]
    GameObject explosionAnimation; //contains explosion animation

    //used to detect placement of tripwire plug
    //adjust distance when raycast is in contact of any walls
    [SerializeField]
    LayerMask wallMask;
    [SerializeField]
    LayerMask waistHighMask;

    //used to raycast the wire to only care about the enemy
    [SerializeField]
    LayerMask enemyOnly;

    ToolbeltCountUpdateUI updateCountEvent;

    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        //event to update the tool count in UI for tripwire
        updateCountEvent = new ToolbeltCountUpdateUI();
        //adding this script as an invoker for the update count UI event
        EventManager.AddUpdateTripWireDetectionCountInvokers(this);

        player = GameObject.FindGameObjectWithTag("Player");
        //since a Unity tile unit is 1 for height and width
        //we want the explosion radius to be within 1.5 files
        explosionRadius = 2.121f;

        //distance of 2 tiles to use for placement
        //this is used for determining how far the 
        //second part of trip wire can be placed before it destroys itself
        placementRadius = 2.828f;

        //every instance sets this to false at instantiation
        //this way object doesnt think it's fully set if another instance was set and variable is true
        fullTrapSet = false;
    }

    /// <summary>
    /// Add a listener to the update count event
    /// </summary>
    /// <param name="listener"></param>
    public void AddUpdateTripWireDetectionCountListener(UnityAction<Constants.Tools,int> listener)
    {
        updateCountEvent.AddListener(listener);
    }

    /// <summary>
    /// Used to check for player input to place the tripwire fully
    /// </summary>
    void Update () {
        
        //trip wire is looking for input from player to place down second part of wire every frame
        //if the wire is placed, the trip wire should no longer care about player input
        //there's no way to set fullTrapSet back to false for this instance once its set to true
        //therefore once this instance of the tripwire is placed, it cant be placed again

        // grabs the vector position of the mouse cursor
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // creates a new direction value by using the coordinate differences between the cursor and player
        Vector3 direction = (new Vector3(cursorPosition.x - player.transform.position.x, cursorPosition.y - player.transform.position.y, 0).normalized);

        // Also assuming that a unit square in Unity is 128x128
        float spriteWidth = player.GetComponent<SpriteRenderer>().sprite.rect.width / 128;

        //waiting for the key Q to be lifted up to place the wire completely
        if (Input.GetKeyUp(KeyCode.Mouse1) && !fullTrapSet)
        {

            //play sound for placing trap
            AudioManager.Instance.Play(AudioClipName.tripWire_Placement1);

            //instantiates the final trip wire piece in front of the player
            finalPlacementTripWire = Instantiate(finalPlacementTripWire, player.transform.position + direction * .85f, player.transform.rotation);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), finalPlacementTripWire.GetComponent<Collider2D>());

            //decreasing one material from players inventory
            player.GetComponent<TripWire>().tripWireCount--;
            //invoke event
            updateCountEvent.Invoke(Constants.Tools.TripWire, player.GetComponent<TripWire>().tripWireCount);
            if (player.GetComponent<TripWire>().tripWireCount <= 0)
            {
                player.GetComponent<ToolBelt>().disableTool(Constants.Tools.TripWire);
            }

            //this way this instance of the trap cant be set anymore to a new position
            //and trap can start casting rays to look for an enemy
            fullTrapSet = true;

            //this way player can place another initial plug
            player.GetComponent<TripWire>().initialTripWirePlaced = false;
        }

    }

    /// <summary>
    /// Used to adjust the plug when placing the trap and casting rays to check
    /// for an enemy to trip over the wire and perform actions to characters caught in blast
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // creates a new direction value by using the coordinate differences between the cursor and player
        Vector3 direction = new Vector3(cursorPosition.x - player.transform.position.x, cursorPosition.y - player.transform.position.y, 0).normalized;

        // Also assuming that a unit square in Unity is 128x128
        float spriteWidth = player.GetComponent<SpriteRenderer>().sprite.rect.width / 128;

        //adjust position of tripwire plug as it is not full placed
        if (!fullTrapSet)
        {
            //changes position of plug to infront of player with distance of two tiles or less
            transform.position = player.transform.position + direction * spriteWidth * placementRadius;

            plugSpriteWidth = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width / 100;

            // draw a raycast to search for detecting a collision with the wall
            Vector2 rayDirection = new Vector2(transform.position.x - player.transform.position.x,
            transform.position.y - player.transform.position.y);
            float tripWireDistance = rayDirection.magnitude + plugSpriteWidth;
            RaycastHit2D wallHit = Physics2D.Raycast(player.transform.position, direction, tripWireDistance, wallMask);

            // if the raycast detects a collider then shorten the trip wires distance from the player
            if (wallHit.collider != null)
            {
                // distance between the object and the player
                Vector2 hitPoint = wallHit.point;
                float distanceBetweenObjAndPlayer = new Vector2(hitPoint.x - player.transform.position.x,
                hitPoint.y - player.transform.position.y).magnitude;

                // resetting the initial trip wire to the new radius
                transform.position = player.transform.position + direction * (wallHit.distance - 0.5f * plugSpriteWidth);
            }

            // draw a raycast to search for detecting a collision with the wall
            RaycastHit2D waistHighHit = Physics2D.Raycast(player.transform.position, direction, tripWireDistance, waistHighMask);

            // if the raycast detects a collider then shorten the trip wires distance from the player
            if (waistHighHit.collider != null)
            {
                // distance between the object and the player
                Vector2 hitPoint = waistHighHit.point;
                float distanceBetweenObjAndPlayer = new Vector2(hitPoint.x - player.transform.position.x,
                hitPoint.y - player.transform.position.y).magnitude;

                // resetting the initial trip wire to the new radius
                transform.position = player.transform.position + direction * (waistHighHit.distance - 0.5f * plugSpriteWidth);
            }
        }

        // full trap is set including the bomb piece
        if (fullTrapSet)
        {
            //casts a ray from the final trip wire position to the initial trip wire placement position
            Debug.DrawRay(finalPlacementTripWire.transform.position, transform.position - finalPlacementTripWire.transform.position);
           
            RaycastHit2D hit = Physics2D.Raycast(finalPlacementTripWire.transform.position, transform.position - finalPlacementTripWire.transform.position, Vector2.Distance(transform.position, finalPlacementTripWire.transform.position), enemyOnly);

            //if the ray casts over a collider proceed into the block
            if (hit.collider != null)
            {

                //if that collider has the tag enemy, then proceed with explosion actions
                if (hit.collider.gameObject.tag == "Enemy")
                {

                    //grabs all enemies within the radius from the intial plug and bomb piece in the scene into an array
                    Collider2D[] allEnemiesInScene = Physics2D.OverlapCircleAll(finalPlacementTripWire.transform.position, Vector2.Distance(transform.position, finalPlacementTripWire.transform.position));

                    //iterate through each character in the scene that was stored in the array
                    foreach (Collider2D sceneEnemy in allEnemiesInScene)
                    {
                        // Get the transform and direction of the character
                        Transform playerCaught = sceneEnemy.transform;
                        Vector2 dirPlayer = new Vector2(playerCaught.position.x - finalPlacementTripWire.transform.position.x, playerCaught.position.y - finalPlacementTripWire.transform.position.y);

                        //if the angle between the character caught within the radius and the angle of the finalPlacementTripWire is less than 40 degrees
                        if (Vector2.Angle(dirPlayer, finalPlacementTripWire.transform.right) < 80 / 2)
                        {
                            float distancePlayer = Vector2.Distance(finalPlacementTripWire.transform.position, playerCaught.position);

                            // cast a ray from the finalPlacementTripWire and the initial tripwire plug
                            //if the tag is enemy or player thats caught in the ray then perform actions as needed 
                            if (!Physics2D.Raycast(finalPlacementTripWire.transform.position, transform.position, distancePlayer) && playerCaught.tag == "Player" || playerCaught.tag == "Enemy")
                            {
                                //if the player caught in the blast is enemy
                                //disable the enemy and play the respective sounds
                                if (playerCaught.tag == "Enemy")
                                {
                                    //kill enemy
                                    //death sound
                                    //AudioManager.Instance.Play(AudioClipName.spacePirate_Death1);
                                    if (playerCaught.name == "Enemy")
                                    {
                                        playerCaught.GetComponent<Enemy>().TransitionToDeadState();
                                    }
                                    //might need to replace this death state with other code
                                    if (playerCaught.name == "Berserker")
                                    {
                                        playerCaught.GetComponent<Berserker>().TransitionToDeadState();
                                    }
                                    //playerCaught.GetComponentInChildren<HUDEnemyQuotes>().DisplayQuote();
                                    //playerCaught.gameObject.GetComponent<Animator>().SetBool("Alive", false);
                                    //playerCaught.gameObject.GetComponent<Enemy>().Alive = false;
                                    //playerCaught.gameObject.transform.Find("FOV").gameObject.SetActive(false);
                                    //playerCaught.gameObject.GetComponent<Enemy>().enabled = false;
                                    //playerCaught.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                                    //needed for berserker class
                                    if (playerCaught.gameObject.GetComponent<Berserker>() != null)
                                    {
                                        playerCaught.GetComponent<Berserker>().TransitionToDeadState();
                                    }
                                }
                                //if the character caught in the blast is the player
                                //then reduce health and play respective sounds
                                else if (playerCaught.tag == "Player")
                                {
                                    //hurt player if in explosion
                                    playerCaught.gameObject.GetComponent<HealthScript>().ChangeHealth(-40);
                                    //player hurt sound played
                                    AudioManager.Instance.Play(AudioClipName.player_Hurt);

                                }
                            }
                        }
                    }

                    //explosion sounds
                    AudioManager.Instance.Play(AudioClipName.tripWire_Explosion);

                    //instantiate the object containing the explosion animation at the position of the bomb piece
                    //make sure rotation matches the bomb piece so explosion goes the right way
                    explosionAnimation = Instantiate(explosionAnimation, finalPlacementTripWire.transform.position, finalPlacementTripWire.transform.rotation);
                    explosionAnimation.GetComponent<Animator>().SetBool("Exploded", true);

                    //destroy the animationobject after .5 seconds, otherwise animation sprite stays in the scene and animation lasts only .5 seconds
                    Destroy(explosionAnimation, .5f);

                    //destroy the initial and final parts of the tripwire trap
                    Destroy(finalPlacementTripWire);
                    Destroy(gameObject);

                }


            }
        }

    }

    #endregion
}
