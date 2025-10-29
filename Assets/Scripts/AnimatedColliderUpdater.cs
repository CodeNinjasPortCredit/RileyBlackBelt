using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedColliderUpdater : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;
    private Sprite lastSprite;

    void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSprite = null;
    }

    void LateUpdate()
    {
        // Only update if the sprite has changed
        if (spriteRenderer.sprite != lastSprite)
        {
            lastSprite = spriteRenderer.sprite;
            UpdateColliderWithCurrentSprite();
        }
    }

    void UpdateColliderWithCurrentSprite()
    {
        // Get the physics shape from the current sprite
        polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();

        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            List<Vector2> path = new List<Vector2>();
            spriteRenderer.sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path.ToArray());
        }
    }
}