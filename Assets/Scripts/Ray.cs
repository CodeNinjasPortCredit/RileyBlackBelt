using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Vector2 raycastOriginOffset = new Vector2(0, 0);
    public Vector2 raycastDirection = Vector2.down;
    public float raycastDistance = 1f;
    public Color raycastColor = Color.red;
    public LayerMask groundLayerMask = 3; // Add layer mask for ground detection

    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = RaycastHitsGround((Vector2)transform.position + raycastOriginOffset, raycastDirection, raycastDistance, raycastColor);
        Debug.Log(isGrounded);
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
    }

    private bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
    }
    /// <summary>
    /// Casts a ray from the given origin in the given direction and distance, and returns true if it hits an object with the tag "Ground". Draws the ray for visualization.
    /// </summary>
    /// <param name="origin">The starting point of the ray.</param>
    /// <param name="direction">The direction of the ray.</param>
    /// <param name="distance">The length of the ray.</param>
    /// <param name="color">The color to draw the ray in the Scene view.</param>
    /// <returns>True if the ray hits an object with the tag "Ground"; otherwise, false.</returns>
    public bool RaycastHitsGround(Vector2 origin, Vector2 direction, float distance, Color color)
    {
        Debug.DrawRay(origin, direction.normalized * distance, color);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            return true;
        }
        return false;
    }
}
