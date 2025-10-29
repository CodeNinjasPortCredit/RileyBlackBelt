using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    [Header("Spells")]
    public float Firespeed;
    public float Icespeed;

    [Header("Arrows")]
    public float Arrowspeed;

    [Header("Waves")]
    public float Speed;


    private void Start()
    {
        if (gameObject.tag == "FireBall")
        {
            Invoke("DestroyAfterSeconds", 2.5f);
        }
        if (gameObject.tag == "IceSpear")
        {
            Invoke("DestroyAfterSeconds", 1.0f);
        }
        if (gameObject.tag == "Lightning")
        {
            Invoke("DestroyAfterSeconds", 0.65f);
        }
        if (gameObject.tag == "Ranger Arrow")
        {
            Invoke("DestroyAfterSeconds", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "FireBall")
        {
            transform.position += new Vector3(Time.deltaTime * Firespeed, 0f, 0f);
        }
        else if (gameObject.tag == "IceSpear")
        {
            transform.position += new Vector3(Time.deltaTime * Icespeed, 0f, 0f);
        }
        else if (gameObject.tag == "Ranger Arrow")
        {
            transform.position += new Vector3(Time.deltaTime * Arrowspeed, 0f, 0f);
        }
    }

    void DestroyAfterSeconds()
    {
        Destroy(gameObject);
    }
}
