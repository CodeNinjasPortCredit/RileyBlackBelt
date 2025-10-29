using UnityEngine;

/// <summary>
/// Controls the boss AI behavior, including patrolling, chasing, attacking, and reacting to damage.
/// </summary>
public class BossAI : MonoBehaviour
{
    public enum BossType { Wizard, Fighter }
    public enum BossState { Patrolling, Chasing, Attacking, RangedAttacking, Hurt, Idle }

    [Header("Boss Settings")]
    public BossType bossType;
    public string enemyType;

    [Header("Animator Parameters")]
    [SerializeField] private string isRunningParam = "IsRunning";
    [SerializeField] private string isHurtParam = "IsHurt";
    [SerializeField] private string isAttackingParam = "IsAttacking";
    [SerializeField] private string isRangedAttackingParam = "IsRangedAttacking";
    [SerializeField] private string isIdleParam = "IsIdle";
    [SerializeField] private string isDeadParam = "IsDead";

    [Header("Patrol Settings")]
    public float minXPosition;
    public float maxXPosition;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float chaseSpeed = 4f;
    public float attackRange = 1f;
    public float rangedAttackRange = 3f;
    [Range(0, 1)] public float rangedAttackChance = 0.75f;
    public float attackDelay = 1f;
    public float rangedAttackDelay = 1f;

    [Header("Hurt Settings")]
    public float hurtDuration = 0.5f;

    // Private fields
    private Animator animator;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private BossState currentState = BossState.Patrolling;
    private bool movingRight = true;
    private float stateTimer = 0f;
    private float rangedAttackAnimDuration = 0.2f; // or whatever matches your animation

    public int Damage;

    public Collider2D attackTrigger;

    // Animator hashes
    private int isRunningHash, isHurtHash, isAttackingHash, isIdleHash, isDeadHash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (!animator || !rb)
        {
            Debug.LogError("Missing required components on Boss!");
            enabled = false;
            return;
        }

        // Cache animator hashes
        isRunningHash = Animator.StringToHash(isRunningParam);
        isHurtHash = Animator.StringToHash(isHurtParam);
        isAttackingHash = Animator.StringToHash(isAttackingParam);
        isIdleHash = Animator.StringToHash(isIdleParam);
        isDeadHash = Animator.StringToHash(isDeadParam);
    }

    void Start()
    {
        SetAllAnimationBoolsFalse();
        animator.SetBool(isIdleHash, true);
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.Patrolling:
                Patrol();
                break;
            case BossState.Chasing:
                ChasePlayer();
                break;
            case BossState.Attacking:
                HandleAttack(attackDelay, BossState.Chasing);
                break;
            case BossState.RangedAttacking:
                HandleAttack(rangedAttackDelay, BossState.Chasing);
                break;
            case BossState.Hurt:
                HandleHurt();
                break;
            case BossState.Idle:
                animator.SetBool(isIdleHash, true);
                break;
        }
    }

    private void Patrol()
    {
        // Check if the boss has reached the boundaries
        if (movingRight && transform.localPosition.x >= maxXPosition)
        {
            Flip();
        }
        else if (!movingRight && transform.localPosition.x <= minXPosition)
        {
            Flip();
        }

        // Move the boss in the current direction
        float horizontalMovement = movingRight ? patrolSpeed : -patrolSpeed;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        SetAllAnimationBoolsFalse();
        animator.SetBool(isRunningHash, true);
        animator.SetBool(isIdleHash, false);
    }

    private void ChasePlayer()
    {
        if (!playerTransform)
        {
            currentState = BossState.Patrolling;
            SetAllAnimationBoolsFalse();
            animator.SetBool(isIdleHash, true);
            return;
        }

        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // If within melee attack range, always do melee attack (even for Wizard)
        if (distance <= attackRange)
        {
            currentState = BossState.Attacking;
            stateTimer = 0f;
            SetAllAnimationBoolsFalse();
            animator.SetBool(isAttackingHash, true);
            rb.velocity = Vector2.zero;
        }
        // If within ranged attack range (but not melee), only Wizard can do ranged attack
        else if (bossType == BossType.Wizard && distance <= rangedAttackRange && currentState != BossState.RangedAttacking && Random.value < rangedAttackChance)
        {
            currentState = BossState.RangedAttacking;
            stateTimer = 0f;
            SetAllAnimationBoolsFalse();
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Otherwise, continue chasing
            float direction = (playerTransform.position.x - transform.position.x);
            float horizontalMovement = direction > 0 ? chaseSpeed : -chaseSpeed;

            // Move towards the player
            rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

            // Flip the sprite based on direction
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                Flip();
            }

            SetAllAnimationBoolsFalse();
            animator.SetBool(isRunningHash, true);
            animator.SetBool(isIdleHash, false);
        }
    }

    private void HandleAttack(float delay, BossState nextState)
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= delay)
        {
            // If the player is still in melee range, immediately perform another melee attack
            if (playerTransform)
            {
                float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

                // Chain melee attacks while the player remains in melee range
                if (distanceToPlayer <= attackRange)
                {
                    stateTimer = 0f;
                    SetAllAnimationBoolsFalse();
                    animator.SetBool(isAttackingHash, true);
                    rb.velocity = Vector2.zero;
                    currentState = BossState.Attacking;
                    return;
                }

                // Optionally chain a ranged attack for Wizard if player is in ranged range (but not melee)
                if (bossType == BossType.Wizard && distanceToPlayer <= rangedAttackRange && Random.value < rangedAttackChance)
                {
                    stateTimer = 0f;
                    SetAllAnimationBoolsFalse();
                    rb.velocity = Vector2.zero;
                    currentState = BossState.RangedAttacking;
                    return;
                }
            }

            // Otherwise, go to the intended next state
            SetAllAnimationBoolsFalse();
            animator.SetBool(isIdleHash, true);
            currentState = nextState;
        }
    }

    private void HandleHurt()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= hurtDuration)
        {
            SetAllAnimationBoolsFalse();
            animator.SetBool(isIdleHash, true);
            currentState = BossState.Patrolling;
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void SetAllAnimationBoolsFalse()
    {
        if (bossType == BossType.Wizard)
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isHurtHash, false);
            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isIdleHash, false);
            animator.SetBool(isDeadHash, false);
        }
        else
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isHurtHash, false);
            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isIdleHash, false);
            animator.SetBool(isDeadHash, false);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            currentState = BossState.Chasing;
            SetAllAnimationBoolsFalse();
            animator.SetBool(isRunningHash, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            currentState = BossState.Patrolling;
            SetAllAnimationBoolsFalse();
            animator.SetBool(isIdleHash, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Melee") || other.gameObject.CompareTag("Range")) && currentState != BossState.Hurt)
        {
            GetHurt();
        }
    }

    private void GetHurt()
    {
        rb.velocity = Vector2.zero;
        currentState = BossState.Hurt;
        stateTimer = 0f;
        SetAllAnimationBoolsFalse();
        animator.SetBool(isHurtHash, true);
        // Optionally, add logic here to reduce the boss's health
        Debug.Log("Boss is hurt!");
    }

    void OnDrawGizmos()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        // Draw patrol boundaries
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minXPosition, transform.position.y, 0), new Vector3(maxXPosition, transform.position.y, 0));
        Gizmos.DrawSphere(new Vector3(minXPosition, transform.position.y, 0), 0.2f);
        Gizmos.DrawSphere(new Vector3(maxXPosition, transform.position.y, 0), 0.2f);
    }
    
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
}