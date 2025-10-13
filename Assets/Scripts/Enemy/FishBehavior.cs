using UnityEngine;
using System.Collections;

public class FishBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float idleSpeed = 1f; //slow speed while idle
    public float chaseSpeed = 5f; //fast speed when chasing the player
    public float detectionRadius = 8f; //how far the enemy can see player

    [Header("Attack Settings")]
    public float attackCooldown = 2f; //time before it can attack again
    public int damage = 1;

    //for fish spawnpoint
    private Vector2 spawnPosition; //store where it spawned
    private Vector2 previousPosition;
    public float maxChaseDistance = 14f; //how far it can move away from spawn
    public float returnThreshold = 0.5f; //how close before idle again

    private Transform player; //reference to the player's position
    private Rigidbody2D rb;
    private Vector2 idleDirection; //random direction during idle movement
    private bool canAttack = true; //for attack delay
    private bool playerInRange; //true if player is within detection radius
    private bool returningToSpawn; //fish goes back to its spawn

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; //'?' in case no player

        //remember where it spawned
        spawnPosition = transform.position;
        previousPosition = transform.position;

        StartCoroutine(ChangeIdleDirectionRoutine());
    }

    IEnumerator ChangeIdleDirectionRoutine()
    {
        while (true)
        {
            // only change direction if not chasing or returning
            if (!playerInRange && !returningToSpawn)
            {
                PickNewIdleDirection();
            }

            // hold the same direction for 6–12 seconds
            yield return new WaitForSeconds(Random.Range(6f, 12f));
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceFromSpawn = Vector2.Distance(transform.position, spawnPosition);

        //detect player if within detection range
        playerInRange = distanceToPlayer <= detectionRadius;

        /////PRIORITY: if too far from spawn, stop chasing immediately
        if (distanceFromSpawn >= maxChaseDistance)
        {
            playerInRange = false;
            returningToSpawn = true;
        }
        //only start returning if player is out of range AND fish is actually away from spawn
        else if (!playerInRange && !returningToSpawn && distanceFromSpawn > returnThreshold)
        {
            returningToSpawn = true;
        }

        //once it's home, resume idle (set only once when reached)
        if (returningToSpawn && distanceFromSpawn <= returnThreshold)
        {
            returningToSpawn = false;
            idleDirection = Random.insideUnitCircle.normalized;
        }
    }

    void FixedUpdate()
    {
        float distanceFromSpawn = Vector2.Distance(transform.position, spawnPosition);

        //only chase if player is close AND not too far from spawn
        if (!returningToSpawn && playerInRange && canAttack && distanceFromSpawn < maxChaseDistance)
        {
            ChasePlayer();
        }
        else if (returningToSpawn || distanceFromSpawn >= maxChaseDistance)
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
        if (idleDirection == Vector2.zero)
            PickNewIdleDirection();

        Vector2 targetPos = rb.position + idleDirection * idleSpeed * Time.fixedDeltaTime;
        //smoother motion with Lerp but keep movement sufficient to avoid tiny numerical jitter (lower is smoother)
        rb.MovePosition(Vector2.Lerp(rb.position, targetPos, 0.4f));
    }

    void RotateToVelocity()
    {
        //compute raw movement (don't normalize until after magnitude check)
        Vector2 movement = rb.position - previousPosition;
        previousPosition = rb.position;

        //only rotate when movement is noticeably larger than tiny numerical noise
        if (movement.sqrMagnitude > 0.0004f) // 0.02^2 = 0.0004
        {
            Vector2 dir = movement.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        //move quickly toward player
        Vector2 newPos = rb.position + dir * chaseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void ReturnToSpawn()
    {
        Vector2 toSpawn = (spawnPosition - rb.position);
        float dist = toSpawn.magnitude;

        if (dist <= 0.0001f)
            return;

        Vector2 dir = toSpawn / dist;
        Vector2 newPos = rb.position + dir * (idleSpeed * 3.0f) * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        //if near spawn, stop returning and pick an idle direction (do this only once)
        if (dist <= returnThreshold)
        {
            returningToSpawn = false;
            idleDirection = Random.insideUnitCircle.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if we hit the player and we’re allowed to attack
        if (collision.collider.CompareTag("Player") && canAttack)
        {
            //damage to player (you can connect to player health script)
            Debug.Log("Player receives " + damage + " damage!");

            //stop attacking for a while
            canAttack = false;

            //stop moving for a moment after attack
            rb.linearVelocity = Vector2.zero; // ok to use linearVelocity in newer Unity

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

