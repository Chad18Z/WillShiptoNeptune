using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Berserker : Enemy {
    public float berserkerSpeed = 0f;
    Timer swordTriggerEnabledTimer;
    Timer cooldownBetweenSlashesTimer;
    public const float cooldownSlash = 1.5f;
    public const float enabledswordTime = .25f;
    bool attacking = false;
    float slashingDistance = .933f;

    // bool for players initial detection
    bool playerDetected;

    // beserker trigger event
    BeserkerTriggerEvent besekerTriggerEvent;

    // timer for beserker and variable
    Timer beginChaseTimer;
    public float beserkerWaitTime = 30.0f;

    GameObject player;
    // Use this for initialization
	protected override void  Start () {
        // adds beserker transition state to the listeners. 
        EventManager.AddBeserkerTriggerListeners(PlayerDetectedable);

        base.Start();
        berserkerSpeed = MAX_SPEED;

        swordTriggerEnabledTimer = GetComponent<Timer>();
        cooldownBetweenSlashesTimer = GetComponent<Timer>();
        player = GameObject.FindGameObjectWithTag("Player");

        // TransitionToPursueState(player.transform.position);
        // starts beserker at waiting state until Triggered by invoker of Beserker Trigger event
        TransitionToStandState();

        // prepares timer and ensure player isn't accedentally detected
        playerDetected = false;
        beginChaseTimer = GetComponent<Timer>();
        beginChaseTimer.Duration = beserkerWaitTime;
        beginChaseTimer.Run();

        //at start, disable the berserker sword trigger
        transform.Find("BerserkerSword").gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    protected override void Update()
    {
        // checks if it is time to chase player
        if (beginChaseTimer.Finished && !playerDetected)
        {
            // passes player position and prevents looping by setting detection to true.
            TransitionToPursueState(player.transform.position);
            playerDetected = true;
        }
        
            //Make sure sword is always in front of Berserkers rotation
            transform.Find("BerserkerSword").gameObject.transform.rotation = gameObject.transform.rotation;
            if (path.Count == 0 && playerDetected)
            {
                TransitionToPursueState(player.transform.position);
            }

            //if we are attacking and the timer for the sword to be enabled if done
            if (swordTriggerEnabledTimer.Finished && attacking)
            {
                //we are no longer attacking
                attacking = false;

            gameObject.GetComponent<Animator>().SetBool("MoveInput", false);
            //stop attacking animation
            //gameObject.GetComponent<Animator>().SetBool("Attacking", false);

            // disable sword trigger and start timer for cooldown between slashes
            transform.Find("BerserkerSword").gameObject.GetComponent<BoxCollider2D>().enabled = false;
                cooldownBetweenSlashesTimer.Duration = cooldownSlash;
                cooldownBetweenSlashesTimer.Run();
                TransitionToPursueState(player.transform.position);
            }
            


            //base.Update();
            //check if health is 0 and die if it is 
            if (currentState != EnemyState.DEAD && GetComponent<HealthScript>().Health <= 0)
            {
                TransitionToDeadState();
            }

            switch (currentState)
            {
                case EnemyState.WAIT:
                    UpdateWaitState();
                    break;
                //case EnemyState.PATROL:
                //    UpdatePatrolState();
                //    break;
                case EnemyState.PURSUE:
                    UpdatePursueState();
                    break;
                case EnemyState.ATTACK:
                    UpdateAttackState();
                    break;
                //case EnemyState.FROZEN:
                //    UpdateFrozenState();
                //    break;
                case EnemyState.DEAD:
                    UpdateDeadState();
                    break;
                case EnemyState.STAND:
                    UpdateStandState();
                    break;
            }
        
        


    }

    protected override void FindNewPath(Vector3 target)
    {
        //base.FindNewPath(target);
        // if there's something wrong with the target, leave
        if (target == null)
        {
            return;
        }

        targetPosition = target;

        // Use the Grid to find a path from the current position to the movePosition
        if(GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            path = GridManager.Instance.FindPath(transform.position, target);
        }
        else
        {
            TransitionToWaitState();
        }
        
    }

    protected override void MoveAlongPath()
    {
        // If there is no path, delete 
        if (path.Count == 0)
        {
            Debug.Log("ERROR: AI has no path to follow");
            return;
        }

        // If he hasn't reached his destination 
        if (Vector2.Distance(transform.position, path[0]) > MIN_TARGET_DISTANCE)
        {
            // Make him face the direction he is moving
            transform.up = new Vector2(path[0].x - transform.position.x, path[0].y - transform.position.y).normalized;
            transform.Rotate(Vector3.forward * 90);

            // Move the enemy to his patrol spot
            transform.position = Vector2.MoveTowards(transform.position, path[0], berserkerSpeed * Time.deltaTime);
        }
        // If he has reached his destination
        else
        {
            // Remove this destination
            path.RemoveAt(0);
        }
    }

    protected override void ChooseNextPatrolTarget()
    {
        base.ChooseNextPatrolTarget();
    }

    protected override void UpdatePatrolState()
    {
        base.UpdatePatrolState();
    }

    protected override void TransitionToPatrolState()
    {
        base.TransitionToPatrolState();
    }

    protected override void UpdatePursueState()
    {
        base.UpdatePursueState();
    }
    protected override void TransitionToPursueState(Vector3 target)
    {
        base.TransitionToPursueState(target);
    }

    public override void TransitionToAttackState(Vector3 target)
    {
        //base.TransitionToAttackState(target);
        FindNewPath(target);
        animator.SetBool("MoveInput", true);

        currentState = EnemyState.ATTACK;
    }
    public override void UpdateAttackState()
    {
        //base.UpdateAttackState();
        // If we're within range of our target position, we must have lost
        // the player, so wait and then return to patrol
        if (path.Count == 0)
        {
            //TransitionToWaitState();

            //instead pursue the player again if we lost the player
            //TransitionToAttackState(player.transform.position);
            TransitionToPursueState(player.transform.position);
            return;
        }

        //try to attack the player with the sword while in attack state

        trySlashing();
        MoveAlongPath();
    }

    public override void TransitionToDeadState()
    {
        //base.TransitionToDeadState();
        //GetComponentInChildren<HUDEnemyQuotes>().DisplayQuote();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;  // disable box collider
        currentState = EnemyState.DEAD;
        //animator.SetBool("Alive", false);
        Alive = false;
        //transform.Find("FOV").gameObject.SetActive(false);
        if (canDropWeapon)
        {
            canDropWeapon = false;
            if (Random.Range(0f, 1.0f) < WEAPON_DROP_CHANCE)
            {
                GameObject enemyPistolInstance = Instantiate(EnemyPistol, transform.position + new Vector3(0, 2), transform.rotation);
                AudioManager.Instance.Play(AudioClipName.gun_Drop);
            }
        }
        //death sound
        AudioManager.Instance.Play(AudioClipName.spacePirate_Death2);
        Destroy(gameObject);
    }
   

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EnemyBullet" &&  Alive == true)
        {
            //Debug.Log(Alive);
            // change health 
            GetComponent<HealthScript>().ChangeHealth(-10);
            Destroy(collision.gameObject);
            //destroy the enemy
            if (GetComponent<HealthScript>().Health <= 0)
            {
                TransitionToDeadState();
            }

        }
    }

    private void trySlashing()
    {
        //if Berserker is within the slashing distance then try to slash at the player
        if(Vector2.Distance(gameObject.transform.position, player.transform.position) <= slashingDistance )
        {
            //Debug.Log("Within distance");
            //can slash if not already attacking and the cooldown timer between slashes isnt running
            if (!attacking && !cooldownBetweenSlashesTimer.Running)
            {
                Debug.Log("ATTACKINGGG!");
                float soundDistribution = Random.Range(0f, 1.0f);

                //attacking sound for sword when slashing
                if (soundDistribution <= 0.33)
                {                   
                    AudioManager.Instance.Play(AudioClipName.berserker_Attack1);
                }
                else if(soundDistribution <= 0.67)
                {
                    AudioManager.Instance.Play(AudioClipName.berserker_Attack2);
                }
                else
                {
                    AudioManager.Instance.Play(AudioClipName.berserker_Attack3);
                }
                //Debug.Log("Attacking now");
                //run timer for how long trigger on sword is enabled
                swordTriggerEnabledTimer.Duration = enabledswordTime;
                swordTriggerEnabledTimer.Run();
                //gameObject.GetComponent<Animation>().
                    //["Berserker_Attack"].wrapMode = WrapMode.Once;
            
                //animation for berserker
                gameObject.GetComponent<Animator>().SetTrigger("Attacking");
                gameObject.GetComponent<Animator>().SetBool("MoveInput", false);



                //while attacking, enable the berserker sword trigger collider
                transform.Find("BerserkerSword").gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        
    }        

    private void PlayerDetectedable(Vector3 detectedLocation)
    {
        // allows beserker to stalk player
        playerDetected = true;

        // initiates transition to pursue state.
        TransitionToPursueState(detectedLocation);
    }
}
