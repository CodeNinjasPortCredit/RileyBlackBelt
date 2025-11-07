using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float Arrowdestination;
    public float Arrowtime;
    public bool isFacingLeft;
    public float SpeedX;

    public float SpeedY;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (isFacingLeft == true)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);

            rb.AddForce(new Vector2(-SpeedX, SpeedY));

        }
        else
        {
            rb.AddForce(new Vector2(SpeedX, SpeedY));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //LeanTween.moveX(gameObject, Arrowdestination, Arrowtime).setEase(LeanTweenType.linear);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
