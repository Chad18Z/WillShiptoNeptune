using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enemy States for state machine
/// </summary>
public enum EnemyState
{
    WAIT,           // Waiting at a single patrol point
    PATROL,         // Patrolling from point to point
    PURSUE,         // Pursuing a target location
    ATTACK,         // Attacking the player
    FROZEN,         // Frozen from stasis grenade
    DEAD,           // Corpse lying on the ground to be eaten
    STAND           // Guarding a certain position
};

/// <summary>
/// Direction of the points in the patrol
/// </summary>
enum PatrolDirection { BACKWARD = -1, FORWARD = 1 };

/// <summary>
/// Enemy base class includes all behaviors common to all Enemies
/// </summary>
public class Enemy : MonoBehaviour
{
    #region Constants

    const float MAX_WAIT_TIME = 3.0f;
    const float MAX_FROZEN_TIME = 3.0f;
    protected const float WEAPON_DROP_CHANCE = 0.25f;
    protected const float MIN_TARGET_DISTANCE = 0.1f;
    protected float MAX_SPEED = 4.0f;

    #endregion

    #region Event Hooks

    EnemyMouseOverEvent mouseOverEvent;
    EnemyDeathEvent enemyDeathEvent;
    EnemyFoundEvent enemyPursueEvent;

    #endregion

    #region fields

    public GameObject[] patrolPositions;        // Points in the world used as patrol locations
    public GameObject FOV;                      // Field of view object
    public GameObject EnemyPistol;              // Droppable pistol
    public bool ReversingPath = false;          // Whether path is reversible or cyclical
    public bool canDropWeapon = true;           // Can the enemy drop a weapon or not

    protected float speed;                        // Speed of the agent
    protected int nextSpot;                       // Index of the next Patrol Point
    protected Animator animator;                  // Enemy Animator
    protected Vector2 targetPosition;             // World position the agent is moving toward
    private PatrolDirection patrolDirection;    // Direction of patrol position traversal (forward or backward)
    protected List<Vector3> path;                 // Sequence of world positions in agent's path
    public EnemyState currentState;            // Enemy's current state of being
    private float remainingWaitTime;            // Remaining time the agent is waiting

    #endregion

    #region Properties

    public virtual bool Alive { get; set; }             // Is the agent alive or not?

    #endregion

    #region Methods

    // Use this for initialization
    protected virtual void Start()
    {
        //Debug.Log("Called");
        Alive = true;

        // Initialize events
        mouseOverEvent = new EnemyMouseOverEvent();
        enemyDeathEvent = new EnemyDeathEvent();
        enemyPursueEvent = new EnemyFoundEvent();

        // Add this script to the list of invokers for the EnemyMouseOverEvent
        EventManager.AddEnemyMouseOverInvoker(this);
        EventManager.AddEnemyDeathInvoker(this);
        GetComponentInChildren<FOVScript>().AddEnemyFoundEventListener(OnEnemyAttackEvent);

        // Initialize object variables
        speed = MAX_SPEED;
        nextSpot = 0;
        patrolDirection = PatrolDirection.FORWARD;
        path = new List<Vector3>();
        animator = GetComponent<Animator>();

        // Start in the wait state
        TransitionToWaitState();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        //check if health is 0 and die if it is 
        if (currentState != EnemyState.DEAD && GetComponent<HealthScript>().Health <= 0)
        {
            currentState = EnemyState.DEAD;

            TransitionToDeadState();
        }

        switch (currentState)
        {
            case EnemyState.WAIT:
                UpdateWaitState();
                break;
            case EnemyState.PATROL:
                UpdatePatrolState();
                break;
            case EnemyState.PURSUE:
                UpdatePursueState();
                break;
            case EnemyState.ATTACK:
                UpdateAttackState();
                break;
            case EnemyState.FROZEN:
                UpdateFrozenState();
                break;
            case EnemyState.DEAD:
                UpdateDeadState();
                break;
            case EnemyState.STAND:
                UpdateStandState();
                break;
        }
    }

    /// <summary>
    /// Find a new path based on the target
    /// </summary>
    /// <param name="target"></param>
    protected virtual void FindNewPath(Vector3 target)
    {
        // if there's something wrong with the target, leave
        if (target == null)
        {
            return;
        }

        targetPosition = target;

        // Use the Grid to find a path from the current position to the movePosition
        path = GridManager.Instance.FindPath(transform.position, target);
    }

    /// <summary>
    /// Move along the path determined by the pathfinding algorithm
    /// </summary>
    protected virtual void MoveAlongPath()
    {
        // If there is no path, delete 
        if (path.Count == 0)
        {
            //Debug.Log("ERROR: AI has no path to follow");
            return;
        }

        // If he hasn't reached his destination 
        if (Vector2.Distance(transform.position, path[0]) > MIN_TARGET_DISTANCE)
        {
            // Make him face the direction he is moving
            transform.up = new Vector2(path[0].x - transform.position.x, path[0].y - transform.position.y).normalized;
            transform.Rotate(Vector3.forward * 90);

            // Move the enemy to his patrol spot
            transform.position = Vector2.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
        }
        // If he has reached his destination
        else
        {
            // Remove this destination
            path.RemoveAt(0);
        }
    }

    #region PATROL STATE

    /// <summary>
    /// Choose the next patrol location to move toward.  If we are
    /// in the reversible patrol, we go back over the same locations.
    /// If we are in the cyclical patrol, we wrap aroundl.  If we've
    /// been interrupted, pick the closest patrol point to restart.
    /// </summary>
    protected virtual void ChooseNextPatrolTarget()
    {
        Debug.Log("Patrolling");
        // If there are no patrol positions, return
        if (patrolPositions.Length == 0)
        {
            Debug.Log("ERROR: no patrol points");
            return;
        }
        

        nextSpot += (int)patrolDirection;

        // If we're on a reversible path
        if (ReversingPath)
        {
            // If we've reached the end, reverse the path
            if (nextSpot >= patrolPositions.Length || nextSpot < 0)
            {
                patrolDirection = (PatrolDirection)(-1 * (int)patrolDirection);
                nextSpot += (int)patrolDirection;
            }
        }
        // Otherwise, we're on a cyclical path
        else
        {
            // If we've reached the end, restart the path
            if (nextSpot >= patrolPositions.Length)
            {
                nextSpot = 0;
            }
        }

        // If somehow we're at a non-existent patrolPosition, choose the closest to resume patrol
        if (nextSpot >= patrolPositions.Length || nextSpot < 0)
        {
            nextSpot = 0;
            for (int i = 0; i < patrolPositions.Length; i++)
            {
                if (Vector2.Distance(transform.position, patrolPositions[i].transform.position)
                    < Vector2.Distance(transform.position, patrolPositions[nextSpot].transform.position))
                {
                    nextSpot = i;
                }
            }
        }

        FindNewPath(patrolPositions[nextSpot].transform.position);
    }

    /// <summary>
    /// Handle staying in the patrol state and deciding which state is next
    /// </summary>
    protected virtual void UpdatePatrolState()
    {
        // We've reached the end of this path, choose next patrol state
        if (path.Count == 0)
        {
            ChooseNextPatrolTarget();
        }
        MoveAlongPath();
    }

    /// <summary>
    /// Handle transitioning to the patrol state
    /// </summary>
    protected virtual void TransitionToPatrolState()
    {
        currentState = EnemyState.PATROL;
        //used to turn off stationary enemy foot moving if they only patrol one position
        if(patrolPositions.Length <= 1)
        {
            animator.SetBool("MoveInput", false);
        }
        else
        {
            animator.SetBool("MoveInput", true);
        }
        
        ChooseNextPatrolTarget();
    }
    #endregion

    #region PURSUE State

    /// <summary>
    /// Handle staying in the pursue state
    /// </summary>
    protected virtual void UpdatePursueState()
    {
        MoveAlongPath();

        // If we've reached the target's position, transition into wait state
        if (path.Count == 0)
        {
            TransitionToWaitState();
        }
    }

    /// <summary>
    /// Handle transitioning to the pursue state
    /// </summary>
    /// <param name="target"></param>
    protected virtual void TransitionToPursueState(Vector3 target)
    {

        currentState = EnemyState.PURSUE;
        animator.SetBool("MoveInput", true);
        FindNewPath(target);
    }

    #endregion

    #region WAIT State

    /// <summary>
    /// Handle staying in the wait state and deciding
    /// which state is next
    /// </summary>
    protected void UpdateWaitState()
    {
        remainingWaitTime -= Time.deltaTime;

        // If our waiting time is exhausted, transition to Patrol state
        if (remainingWaitTime <= 0)
        {
            TransitionToPatrolState();
        }
    }

    /// <summary>
    /// Handle transitioning to the wait state
    /// </summary>
    protected void TransitionToWaitState()
    {
        currentState = EnemyState.WAIT;
        animator.SetBool("MoveInput", false);
        remainingWaitTime = MAX_WAIT_TIME;
    }

    #endregion

    #region ATTACK State

    /// <summary>
    /// Handle staying in the Attack state and deciding what other
    /// states to transition to next
    /// </summary>
    public virtual void UpdateAttackState()
    {
        // If we're within range of our target position, we must have lost
        // the player, so wait and then return to patrol
        if (path.Count == 0)
        {
            TransitionToWaitState();
            return;
        }

        gameObject.GetComponent<EnemyFiring>().tryShooting();

        MoveAlongPath();
    }

    /// <summary>
    /// Handle transitioning to the Attack state
    /// </summary>
    /// <param name="target"></param>
    public virtual void TransitionToAttackState(Vector3 target)
    {
        FindNewPath(target);
        animator.SetBool("MoveInput", true);
        currentState = EnemyState.ATTACK;
        //gameObject.GetComponent<EnemyFiring>().shootCoolDown = 0;
    }

    #endregion

    #region FROZEN State

    /// <summary>
    /// Handle staying in the Frozen state and deciding what state to 
    /// transition to next
    /// </summary>
    protected virtual void UpdateFrozenState()
    {
        remainingWaitTime -= Time.deltaTime;

        // If our waiting time is exhausted, transition to Patrol state
        if (remainingWaitTime <= 0)
        {
            TransitionToPatrolState();
        }
    }

    /// <summary>
    /// Handle the transition to the Frozen state from the statis grenade
    /// </summary>
    protected virtual void TransitionToFrozenState()
    {
        currentState = EnemyState.FROZEN;
        animator.SetBool("MoveInput", false);
        remainingWaitTime = MAX_FROZEN_TIME;
    }

    #endregion

    #region DEAD State

    protected virtual void UpdateDeadState()
    {
        // TODO: put some code in here to handle removing the corpse after some time?
    }

    public virtual void TransitionToDeadState()
    {
        //DeathRotation();
        GetComponentInChildren<HUDEnemyQuotes>().DisplayQuote();        // Display a death quote
        gameObject.GetComponent<BoxCollider2D>().enabled = false;       // disable box collider
        currentState = EnemyState.DEAD;                                 // set the state to dead
        animator.SetBool("Alive", false);                               // set the animator boolian alive to false
        Alive = false;                                                  // set alive to false
        transform.Find("FOV").gameObject.SetActive(false);
        AudioManager.Instance.Play(AudioClipName.spacePirate_Death1);   // play the death sound

        if (canDropWeapon)
        {
            canDropWeapon = false;
            if (Random.Range(0f, 1.0f) < WEAPON_DROP_CHANCE)
            {
                GameObject enemyPistolInstance = Instantiate(EnemyPistol, transform.position + new Vector3(0, 2), transform.rotation);
                AudioManager.Instance.Play(AudioClipName.gun_Drop);
            }
        }
    }

    #endregion

    #region Stand State

    /// <summary>
    /// Hand Look out paterns if implemented 
    /// this would be rotating left to right to check for intruders
    /// </summary>
    protected void UpdateStandState()
    {
        // currently empty
    }

    /// <summary>
    /// Handle transitioning to the Stand state
    /// </summary>
    protected void TransitionToStandState()
    {
        currentState = EnemyState.STAND;
        animator.SetBool("MoveInput", false);
    }

    #endregion

    /// <summary>
    /// Adds the given listener for the EnemyMouseOverEvent
    /// </summary>
    /// <param name="listener"></param>
    public void AddEnemyMouseOverListener(UnityAction listener)
    {
        mouseOverEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the EnemyMouseOverEvent
    /// </summary>
    /// <param name="listener"></param>
    public void AddEnemyDeathListener(UnityAction listener)
    {
        enemyDeathEvent.AddListener(listener);
    }

    /// <summary>
    /// Handle the Pursue Event
    /// </summary>
    /// <param name="target"></param>
    private void OnEnemyAttackEvent(Vector3 target)
    {
        //Debug.Log("Transition to Attack event called");
        if (currentState != EnemyState.DEAD)
        {
            TransitionToAttackState(target);
        }
    }

    /// <summary>
    /// Invokes the mouseOverEvent when the mouse is hovering over the object to which this script is attached
    /// </summary>
    private void OnMouseOver()
    {
        mouseOverEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ping" && !collision.gameObject.GetComponent<RadarPing>().pinging)
        {
            gameObject.GetComponent<HighlightFromTransponder>().highlightEnemy();
            //Debug.Log("Caught in highlight");
        }
        else if (collision.gameObject.tag == "Ping" && collision.gameObject.GetComponent<RadarPing>().pinging)
        {
            if (gameObject.GetComponent<HighlightFromTransponder>().highlighted)
            {
                gameObject.GetComponent<HighlightFromTransponder>().disableHighlight();
            }

            //CALL THE METHOD OF THE ENEMY WITH THE POSITION OF THE PING OBJECT AS THE PURSUE POINT
            TransitionToPursueState(collision.gameObject.transform.position);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ping")
        {
            if (gameObject.GetComponent<HighlightFromTransponder>().highlighted)
            {
                gameObject.GetComponent<HighlightFromTransponder>().disableHighlight();
            }


        }
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //if Enemy collides with the player, transition to Attack state on player
        if(collision.gameObject.tag == "Player" && Alive)
        {
            TransitionToAttackState(collision.gameObject.transform.position);
        }
        //Debug.Log("Enemy on collision");
        //if the enemy collides with a bullet
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EnemyBullet" && Alive == true)
        {
            //Debug.Log(Alive);
            //Alert the enemy
            TransitionToPursueState(collision.transform.position);
            // Destroy the bullet
            Destroy(collision.gameObject);
            // change health 
            GetComponent<HealthScript>().ChangeHealth(-10);

            //When hit by bullet sound
            AudioManager.Instance.Play(AudioClipName.enemy_BulletHit);

            //destroy the enemy
            if (GetComponent<HealthScript>().Health <= 0)
            {
                TransitionToDeadState();
            }

        }
    }

    /// <summary>
    /// Sets how to draw the path 
    /// </summary>
    public void OnDrawGizmos()
    {
        if (path == null || path.Count == 0)
        {
            return;
        }

        foreach (Vector3 n in path)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawCube(n, Vector3.one * 0.75f);
        }

        if (targetPosition != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(targetPosition, Vector3.one);
        }
    }

    /// <summary>
    /// Rotates the object to face the x or y axis; currently doesn't work
    /// </summary>
    private void DeathRotation()
    {
        float rotationDeterminant = Random.Range(0, 4);

        if (rotationDeterminant == 0)
            gameObject.transform.Rotate(0, 0, 90 - gameObject.transform.rotation.z);
        else if (rotationDeterminant == 1)
            gameObject.transform.Rotate(0, 0, 180 - gameObject.transform.rotation.z);
        else if (rotationDeterminant == 2)
            gameObject.transform.Rotate(0, 0, 270 - gameObject.transform.rotation.z);
        else if (rotationDeterminant == 3)
            gameObject.transform.Rotate(0, 0, 360 - gameObject.transform.rotation.z);
            
    }

    #endregion
}