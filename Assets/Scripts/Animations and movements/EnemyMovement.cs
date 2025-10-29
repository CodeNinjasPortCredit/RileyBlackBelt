using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maximumXPosition;
    public float minimumXPosition;
    public float speed;
    public int maxSpeed;

    [Header("Health Settings")]
    public int currentHealth = 35;
    public HP_Bar hpBar;

    [Header("Animator")]
    public Animator animator;

    private bool isFacingLeft = true;
    private Rigidbody2D rb;

    private void Start()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing from the enemy object.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is missing from the enemy object.");
        }

        hpBar = GetComponent<HP_Bar>();
        if (hpBar == null)
        {
            Debug.LogError("HP_Bar script is missing from the enemy object.");
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // Check if the enemy should change direction
        if ((transform.localPosition.x <= minimumXPosition && isFacingLeft) ||
            (transform.localPosition.x >= maximumXPosition && !isFacingLeft))
        {
            FlipDirection();
        }

        // Move enemy
        float moveDirection = isFacingLeft ? -1 : 1;
        rb.AddForce(new Vector2(speed * moveDirection, 0));

        // Limit the velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void FlipDirection()
    {
        isFacingLeft = !isFacingLeft;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Update health bar
        if (hpBar != null)
        {
            hpBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            ByeBye();
        }
    }

    private void ByeBye()
    {
        if (animator != null)
        {
            animator.SetBool("Dead", true);
        }
        Invoke("DestroyEnemy", 1f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fetch manager and wizard stats only if collision is relevant
        if (collision.CompareTag("FireBall") || collision.CompareTag("IceSpear") || collision.CompareTag("Lightning"))
        {
            GameObject managerObject = GameObject.Find("Manager");
            if (managerObject == null) return;

            Manager manager = managerObject.GetComponent<Manager>();
            if (manager == null || manager.wizard_stats == null) return;

            int damage = 0;
            if (collision.CompareTag("FireBall"))
            {
                damage = manager.wizard_stats.Q_dmg;
            }
            else if (collision.CompareTag("IceSpear"))
            {
                damage = manager.wizard_stats.E_dmg;
            }
            else if (collision.CompareTag("Lightning"))
            {
                damage = manager.wizard_stats.R_dmg;
            }
            else
            {
                // add the rest of the attacks for the fighter and ranger.:/
            }

            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("FighterQ")){
            Debug.Log("ah");
        }
    }
}
