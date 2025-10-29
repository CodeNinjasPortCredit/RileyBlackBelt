using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMovement : MonoBehaviour
{
    public float Icedestination;
    public float Icetime;
    public bool isFacingLeft;
    public float Speed;
    private Rigidbody2D rb;
    void Start()
    {
        if (gameObject.tag == "IceSpear")
        {
            Invoke("DestroyAfterSeconds", 1f);
        }
        rb = GetComponent<Rigidbody2D>();

        if (isFacingLeft == true)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rb.AddForce(new Vector2(-Speed, 0f));

        }
        else
        {
            rb.AddForce(new Vector2(Speed, 0f));
        }

    }
    void DestroyAfterSeconds()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}