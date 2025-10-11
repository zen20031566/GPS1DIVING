using UnityEngine;

public class UnderwaterEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float idleSpeed = 1f; //slow speed while idle
    public float chaseSpeed = 5f; //fast speed when chasing the player
    public float detectionRadius = 5f; //how far the enemy can see player

    [Header("Attack Settings")]
    public float attackCooldown = 2f; //time before it can attack again
    public int damage = 1;

    //for fish spawnpoint
    private Vector2 spawnPosition; //store where it spawned
    public float maxChaseDistance = 8f; //how far it can move away from spawn

    private Transform player; //reference to the player's position
    private Rigidbody2D rb; //rigidbody for physics-based movement
    private Vector2 idleDirection; //random direction during idle movement
    private bool canAttack = true; //for attack delay
    private bool playerInRange; //true if player is within detection radius

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //Find player

        //remember where it spawned
        spawnPosition = transform.position;

        PickNewIdleDirection();
        //pick new idle directions every 2 seconds
        InvokeRepeating(nameof(PickNewIdleDirection), 0, 2f);
    }

    void Update()
    {
        //check if the player is within range
        playerInRange = Vector2.Distance(transform.position, player.position) <= detectionRadius;

        //stop chasing if player goes too far
        if (Vector2.Distance(player.position, spawnPosition) > maxChaseDistance)
        {
            playerInRange = false; //stops chase
        }
    }

    void FixedUpdate()
    {
        float distanceFromSpawn = Vector2.Distance(transform.position, spawnPosition);

        //Only chase if player is close AND not too far from spawn
        if (playerInRange && canAttack && distanceFromSpawn < maxChaseDistance)
        {
            ChasePlayer();
        }
        else if (distanceFromSpawn >= maxChaseDistance)
        {
            //return toward spawn if it has gone too far
            ReturnToSpawn();
        }
        else
        {
            IdleMovement();
        }

        RotateToVelocity(); //fish face its movement
    }

    void IdleMovement()
    {
        //slowly drift in random direction
        Vector2 targetPos = rb.position + idleDirection * idleSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void RotateToVelocity()
    {
        //approximate movement direction from current to next position
        Vector2 velocity = (rb.position - spawnPosition).normalized; // temporary fallback if not moving much

        //use previous velocity if we can detect movement
        if (rb.linearVelocity != Vector2.zero)
        {
            velocity = rb.linearVelocity.normalized;
        }

        if (velocity.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            //subtract 90° if your fish sprite faces upward in its default orientation
            //angle -= 90f;
            rb.SetRotation(angle);
        }
    }

    void PickNewIdleDirection()
    {
        //Random.insideUnitCircle gives a random (x, y) direction within a circle
        idleDirection = Random.insideUnitCircle.normalized;
    }

    void ChasePlayer()
    {
        //find direction from enemy to player
        Vector2 dir = (player.position - transform.position).normalized;

        //move quickly toward player
        Vector2 newPos = rb.position + dir * chaseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void ReturnToSpawn()
    {
        Vector2 dir = (spawnPosition - (Vector2)transform.position).normalized;
        Vector2 newPos = rb.position + dir * idleSpeed * 1.5f * Time.fixedDeltaTime;
        rb.MovePosition(newPos); // swim back a bit faster
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if we hit the player and we’re allowed to attack
        if (collision.collider.CompareTag("Player") && canAttack)
        {
            //damage to player (you can connect to player health script)
            Debug.Log("Player hit for " + damage + " damage!");

            //stop attacking for a while
            canAttack = false;

            //stop moving for a moment after attack
            rb.linearVelocity = Vector2.zero;

            //after cooldown time, allow another attack
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        //allow enemy to attack again
        canAttack = true;
    }

    //draw detection radius in editor for easy tuning
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)spawnPosition : transform.position, maxChaseDistance);
    }

    //for animation when going left/right
    //if (rb.velocity.x > 0)
    //transform.localScale = new Vector3(1, 1, 1);
    //else if (rb.velocity.x< 0)
    //transform.localScale = new Vector3(-1, 1, 1);
}