using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour
{

    public string EnemyType;

    // Animator boo
    [Header("Animator Parameters")]
    public string isRunningBool = "IsRunning";
    public string isHurtBool = "IsHurt";
    public string isAttackingBool = "IsAttacking";
    public string isIdleBool = "IsIdle";
    public string isDeadBool = "IsDead";

    // Patrol boundaries
    [Header("Patrol Settings")]
    public float minXPosition;
    public float maxXPosition;
    public float patrolSpeed = 2f;
    private Vector3 startingLocalPosition; // Store starting position for accurate visualization

    // Chase settings
    [Header("Chase Settings")]
    public float chaseSpeed = 4f;
    public float attackRange = 1f; // Horizontal distance at which the enemy stops and attacks
    public float attackDelay = 1f; // Delay before attacking

    // Hurt settings
    [Header("Hurt Settings")]

    public int health;
    public float hurtDuration = 0.5f; // Duration of the hurt state
    private bool isHurt = false; // Tracks if the boss is currently hurt
    private float hurtTimer = 0f; // Timer for hurt duration

    // Attack trigger
    [Header("Attack Settings")]
    public Collider2D attackTrigger; // Drag your attack trigger collider here
    
    [Header("Detection Settings")]
    [Tooltip("The enemy needs a trigger collider (separate from attackTrigger) to detect the player's 'Chase' tagged object (PlayerAttraction). This detection collider should be larger than the attack trigger.")]
    // Note: The detection collider is set up in Unity Inspector - it should be a trigger collider on the enemy GameObject or a child object

    // Private variables
    private Animator animator;
    private Rigidbody2D rb;
    private Transform playerTransform;
    public bool isChasing = false;
    public bool movingRight = true; // Tracks the direction of patrol movement
    public bool isAttacking = false; // Tracks if the boss is currently attacking
    private float attackTimer = 0f; // Timer for attack delay

    public int Damage;

    void Start()
    {
        Debug.Log($"[MobAI] *** START CALLED *** Enemy: {gameObject.name}");
        
        // Get required components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (animator == null)
        {
            Debug.LogError($"[MobAI] Animator component not found on {gameObject.name}!");
        }
        if (rb == null)
        {
            Debug.LogError($"[MobAI] Rigidbody2D component not found on {gameObject.name}!");
        }

        // Disable attack trigger initially
        if (attackTrigger != null)
        {
            attackTrigger.enabled = false;
        }
        else
        {
            Debug.LogWarning($"[MobAI] Attack trigger not assigned on {gameObject.name}!");
        }

        // Set initial animation state
        UpdateAnimationState();
        
        // Store starting local position for accurate visualization
        startingLocalPosition = transform.localPosition;
        
        // Debug.Log($"[MobAI] MobAI initialized for {gameObject.name}. AttackRange: {attackRange}, ChaseSpeed: {chaseSpeed}");
    }

    void Update()
    {
        // Handle hurt state
        if (isHurt)
        {
            hurtTimer += Time.deltaTime;
            if (health <= 0)
            {
                Die();
            }
        }

        if (hurtTimer >= hurtDuration)
        {
            isHurt = false;
            hurtTimer = 0f;

            // Reset animation after hurt
            animator.SetBool(isHurtBool, false);
            UpdateAnimationState();
        }

        // Ensure the enemy doesn't move while attacking
        if (isAttacking)
        {
            rb.velocity = Vector2.zero; // Stop all movement
            
            // Face the player while attacking
            if (playerTransform != null)
            {
                float direction = (playerTransform.position.x - transform.position.x);
                if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
                {
                    Flip();
                }
            }
            
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDelay)
            {
                Debug.Log($"[MobAI] Attack delay reached ({attackTimer} >= {attackDelay}), calling AttackPlayer()");
                AttackPlayer();
                attackTimer = 0f;
                // Keep isAttacking = true until ResetAttackAnimation is called
                // This prevents multiple calls to AttackPlayer() during the same attack
            }
        }
        else if (isChasing && !isHurt)
        {
            // CRITICAL: Only chase if we have a valid playerTransform
            // OnTriggerStay2D should maintain this, but if it's null, we can't chase
            if (playerTransform == null)
            {
                // If playerTransform is null but we're supposed to be chasing,
                // OnTriggerStay2D should fix this. Don't reset isChasing here.
                // Just skip this frame and wait for OnTriggerStay2D to restore it
                return;
            }
            
            // Only log occasionally to avoid spam
            if (Time.frameCount % 60 == 0)
            {
                Debug.Log($"[MobAI] Update: Calling ChasePlayer(). isChasing={isChasing}, playerTransform={playerTransform.name}, isAttacking={isAttacking}");
            }
            ChasePlayer();
        }
        else if (!isHurt)
        {
            // Debug why we're not chasing (only log occasionally to avoid spam)
            if (Time.frameCount % 120 == 0) // Log every 2 seconds
            {
                Debug.Log($"[MobAI] Update: Patrolling. isChasing={isChasing}, isHurt={isHurt}, isAttacking={isAttacking}, playerTransform={(playerTransform != null ? playerTransform.name : "null")}");
            }
            Patrol();
        }
    }

    void Patrol()
    {
        // Check if the enemy has reached the boundaries (using local position)
        if (movingRight && transform.localPosition.x >= maxXPosition)
        {
            Flip();
        }
        else if (!movingRight && transform.localPosition.x <= minXPosition)
        {
            Flip();
        }

        // Move the enemy in the current direction
        float horizontalMovement = movingRight ? patrolSpeed : -patrolSpeed;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        // Update animation state
        animator.SetBool(isRunningBool, true);
        animator.SetBool(isIdleBool, false);
    }

    void ChasePlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("[MobAI] ChasePlayer called but playerTransform is null!");
            isChasing = false;
            rb.velocity = Vector2.zero;
            UpdateAnimationState();
            return;
        }

        // Calculate horizontal distance to the player
        float horizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        
        // Only log occasionally to avoid spam
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"[MobAI] ChasePlayer: horizontalDistance={horizontalDistance}, attackRange={attackRange}, isChasing={isChasing}");
        }

        // If within attack range, stop and prepare to attack
        if (horizontalDistance <= attackRange)
        {
            // Face the player before attacking
            float direction = (playerTransform.position.x - transform.position.x);
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                Flip();
            }

            rb.velocity = Vector2.zero; // Stop moving
            
            // Only start attack sequence if not already attacking
            if (!isAttacking)
            {
                // Debug.Log($"[MobAI] Attack conditions met! Horizontal: {horizontalDistance}, AttackRange: {attackRange}");
                animator.SetBool(isRunningBool, false);
                animator.SetBool(isIdleBool, true);
                
                // Start attack sequence
                Debug.Log("[MobAI] Setting isAttacking to true - player is in attack range");
                isAttacking = true;
                attackTimer = 0f; // Reset timer when starting attack
            }
            // If already attacking, the attack state will handle it
        }
        else
        {
            // Player is not in attack range, continue chasing
            float direction = (playerTransform.position.x - transform.position.x);
            float horizontalMovement = direction > 0 ? chaseSpeed : -chaseSpeed;

            // Move towards the player
            rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

            // Flip the sprite based on direction
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                Flip();
            }

            // Update animation state
            animator.SetBool(isRunningBool, true);
            animator.SetBool(isIdleBool, false);
        }
    }

    void Flip()
    {
        // Flip the sprite by scaling the x-axis
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void UpdateAnimationState()
    {
        // Reset all animation states
        animator.SetBool(isRunningBool, false);
        animator.SetBool(isHurtBool, false);
        animator.SetBool(isAttackingBool, false);
        animator.SetBool(isIdleBool, true);
        animator.SetBool(isDeadBool, false);
    }

    void AttackPlayer()
    {
        Debug.Log("[MobAI] AttackPlayer() called!");
        
        // Ensure enemy is facing the player before attacking
        if (playerTransform != null)
        {
            float direction = (playerTransform.position.x - transform.position.x);
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                Flip();
            }
        }

        // Trigger the attack animation
        // Try setting the bool first
        if (!string.IsNullOrEmpty(isAttackingBool))
        {
            animator.SetBool(isAttackingBool, true);
            Debug.Log($"[MobAI] Set animator bool {isAttackingBool} to true");
        }
        
        // Also try common trigger names in case the animator uses triggers
        try
        {
            animator.SetTrigger("Attack");
            Debug.Log("[MobAI] Set Attack trigger");
        }
        catch { }
        
        try
        {
            animator.SetTrigger("IsAttacking");
            Debug.Log("[MobAI] Set IsAttacking trigger");
        }
        catch { }
        
        animator.SetBool(isIdleBool, false);
        
        Debug.Log("[MobAI] Attack animation parameters set! Check animator for correct parameter name.");

        // Optionally, you can add logic here to deal damage to the player
        // Debug.Log("Enemy is attacking the player!");

        // Reset the attack animation after it finishes
        Invoke("ResetAttackAnimation", 1f); // Adjust this delay to match your attack animation length
    }

    void ResetAttackAnimation()
    {
        Debug.Log($"[MobAI] ResetAttackAnimation() called - attack animation finished. isChasing={isChasing}, playerTransform={(playerTransform != null ? playerTransform.name : "null")}");
        
        // Reset the attack animation state
        animator.SetBool(isAttackingBool, false);
        
        // Ensure attack trigger is disabled
        if (attackTrigger != null)
        {
            attackTrigger.enabled = false;
        }
        
        // Reset isAttacking flag so we can attack again if player is still in range
        isAttacking = false;
        
        // CRITICAL: Don't reset isChasing here - OnTriggerStay2D maintains it
        // If player is still in range, OnTriggerStay2D will keep isChasing=true
        // and ChasePlayer() will immediately start a new attack in the next Update()
        
        // Don't set idle state here - let ChasePlayer() decide based on whether player is in attack range
        // If we're still chasing, ChasePlayer() will either attack again or continue chasing
        if (isChasing && playerTransform != null)
        {
            Debug.Log("[MobAI] ResetAttackAnimation: Still chasing! ChasePlayer() will handle next action");
            // Don't set any animation state - let ChasePlayer() handle it
        }
        else
        {
            Debug.Log("[MobAI] ResetAttackAnimation: Not chasing, setting idle state");
            animator.SetBool(isIdleBool, true);
            animator.SetBool(isRunningBool, false);
        }
    }

    // Animation Event Methods - Called by Animation Events
    public void ActivateAttackTrigger()
    {
        if (attackTrigger != null)
        {
            attackTrigger.enabled = true;
        }
    }

    public void DeactivateAttackTrigger()
    {
        if (attackTrigger != null)
        {
            attackTrigger.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[MobAI] *** OnTriggerEnter2D CALLED *** GameObject: {other.gameObject.name}, Tag: {other.gameObject.tag}");
        
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            other.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }  

        // Check if the colliding object is the Chase trigger (PlayerAttraction)
        if (other.CompareTag("Chase"))
        {
            Debug.Log($"[MobAI] *** CHASE TAG DETECTED! *** GameObject: {other.gameObject.name}");
            // Get the player transform (parent of the Chase object)
            Transform player = other.transform.parent;
            if (player != null)
            {
                Debug.Log($"[MobAI] *** PLAYER PARENT FOUND: {player.name} *** Setting isChasing = true");
                playerTransform = player;
                isChasing = true;
                
                // Face the player immediately when starting to chase
                float direction = (player.position.x - transform.position.x);
                if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
                {
                    Flip();
                }
                
                animator.SetBool(isRunningBool, true);
                animator.SetBool(isIdleBool, false);
                
                PolygonCollider2D polyCollider = gameObject.GetComponent<PolygonCollider2D>();
                if (polyCollider != null)
                {
                    polyCollider.enabled = true;
                }
            }
            else
            {
                Debug.LogWarning($"[MobAI] Chase object detected but player parent is null! Chase object: {other.gameObject.name}, Parent: {other.transform.parent}");
            }
            return; // Exit early to avoid checking other tags
        }

        // Legacy support: Check if the colliding object is the player directly
        if (other.CompareTag("Player") && other.gameObject.GetComponent<BoxCollider2D>() != null && other.gameObject.GetComponent<BoxCollider2D>().enabled)
        {
            playerTransform = other.transform;
            isChasing = true;
            animator.SetBool(isRunningBool, true);
            animator.SetBool(isIdleBool, false);
        }
        if ((other.gameObject.CompareTag("Lightning") || other.gameObject.CompareTag("FireBall") || other.gameObject.CompareTag("IceSpear") || other.gameObject.CompareTag("FighterQ") || other.gameObject.CompareTag("FighterR") || other.gameObject.CompareTag("Ranger Arrow") || other.gameObject.CompareTag("RangerE") || other.gameObject.CompareTag("FighterE")) && !isHurt)
        {
            if (other.gameObject.CompareTag("Lightning"))
            {
                health -= 45;
            }
            else if (other.gameObject.CompareTag("FireBall"))
            {
                health -= 25;
            }
            else if (other.gameObject.CompareTag("IceSpear"))
            {
                health -= 20;
            }
            else if (other.gameObject.CompareTag("FighterQ"))
            {
                health -= 25;   
            }
            else if (other.gameObject.CompareTag("FighterE"))
            {
                health -= 20;
            }
            else if (other.gameObject.CompareTag("FighterR"))
            {
                health -= 40;
            }
            else if (other.gameObject.CompareTag("Ranger Arrow"))
            {
                health -= 20;
            }
            else if (other.gameObject.CompareTag("RangerE"))
            {
                health -= 15;
            }

            GetHurt();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is the Chase trigger (PlayerAttraction)
        if (other.CompareTag("Chase"))
        {
            // Get the player transform (parent of the Chase object)
            Transform player = other.transform.parent;
            if (player != null)
            {
                // CRITICAL: This method is called EVERY FRAME while the trigger is active
                // Always maintain chase state and update player position
                // This is the AUTHORITATIVE source for chase state - it overrides everything else
                bool wasChasing = isChasing;
                bool hadPlayerTransform = (playerTransform != null);
                
                // Always update these - this ensures chase state persists as long as player is in trigger
                playerTransform = player;
                isChasing = true;
                
                // Log when chase state is restored (only occasionally to avoid spam)
                if ((!wasChasing || !hadPlayerTransform) && Time.frameCount % 60 == 0)
                {
                    Debug.Log($"[MobAI] *** OnTriggerStay2D: Maintaining chase for {player.name}. wasChasing={wasChasing}, hadPlayerTransform={hadPlayerTransform} ***");
                }
                
                // Don't interfere with attack animations - let ChasePlayer() handle movement and facing during attacks
                // Only update animation states if not currently attacking
                if (!isAttacking)
                {
                    // Face the player while chasing (but not during attack)
                    float direction = (player.position.x - transform.position.x);
                    if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
                    {
                        Flip();
                    }
                }
                
                PolygonCollider2D polyCollider = gameObject.GetComponent<PolygonCollider2D>();
                if (polyCollider != null)
                {
                    polyCollider.enabled = true;
                }
            }
            else
            {
                Debug.LogWarning("[MobAI] OnTriggerStay2D: Chase tag detected but player parent is null!");
            }
            return; // Exit early to avoid checking other tags
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            other.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the colliding object is a weapon (Melee or Range)

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is the Chase trigger (PlayerAttraction)
        if (other.CompareTag("Chase"))
        {
            Debug.Log("[MobAI] Chase object exited detection area - stopping chase");
            playerTransform = null;
            isChasing = false;
            isAttacking = false; // Stop any ongoing attacks
            animator.SetBool(isRunningBool, false);
            animator.SetBool(isIdleBool, true);
            animator.SetBool(isAttackingBool, false);
            return; // Exit early to avoid checking other tags
        }
        
        // Legacy support: Check if the exiting object is the player directly
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            isChasing = false;
            animator.SetBool(isRunningBool, false);
            animator.SetBool(isIdleBool, true);
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            other.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    void GetHurt()
    {
        // Stop movement and play the hurt animation
        rb.velocity = Vector2.zero;
        isHurt = true;

        // Play the hurt animation only once
        animator.SetBool(isHurtBool, true);
        animator.SetBool(isRunningBool, false);
        animator.SetBool(isIdleBool, false);

        // Optionally, you can add logic here to reduce the boss's health
        //Debug.Log("Boss is hurt!");
    }

    void OnDrawGizmos()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw patrol boundaries using local position
        // The boundaries minXPosition and maxXPosition are relative to the enemy's local space
        // We need to visualize them correctly in world space
        
        Vector3 minWorldPos, maxWorldPos;
        Vector3 currentWorldPos = transform.position;
        
        // Check if enemy has a parent
        if (transform.parent != null)
        {
            // Enemy is a child - boundaries are relative to parent's local space
            // Use the starting local Y and Z positions for consistent visualization
            Vector3 minLocalPos = new Vector3(minXPosition, startingLocalPosition.y, startingLocalPosition.z);
            Vector3 maxLocalPos = new Vector3(maxXPosition, startingLocalPosition.y, startingLocalPosition.z);
            
            // Convert from parent's local space to world space
            minWorldPos = transform.parent.TransformPoint(minLocalPos);
            maxWorldPos = transform.parent.TransformPoint(maxLocalPos);
        }
        else
        {
            // Enemy has no parent - boundaries are world coordinates
            // Use starting Y and Z for consistent visualization
            minWorldPos = new Vector3(minXPosition, startingLocalPosition.y, startingLocalPosition.z);
            maxWorldPos = new Vector3(maxXPosition, startingLocalPosition.y, startingLocalPosition.z);
        }
        
        // Draw the patrol line
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(minWorldPos, maxWorldPos);
        
        // Draw spheres at the boundaries (larger for visibility)
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(minWorldPos, 0.3f);
        Gizmos.DrawSphere(maxWorldPos, 0.3f);
        
        // Draw vertical lines at boundaries for better visibility
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(minWorldPos + Vector3.up * 0.5f, minWorldPos + Vector3.down * 0.5f);
        Gizmos.DrawLine(maxWorldPos + Vector3.up * 0.5f, maxWorldPos + Vector3.down * 0.5f);
        
        // Draw a visual indicator showing if current local position is within bounds
        Vector3 currentLocalPos = transform.localPosition;
        if (currentLocalPos.x >= minXPosition && currentLocalPos.x <= maxXPosition)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(currentWorldPos, currentWorldPos + Vector3.up * 0.8f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(currentWorldPos, currentWorldPos + Vector3.up * 0.8f);
        }
    }

    void Die()
    {
        animator.SetBool(isDeadBool, true);
        Destroy(gameObject);
    }
}