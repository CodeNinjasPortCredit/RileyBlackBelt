using UnityEngine;

public class ForcePosition : MonoBehaviour
{
    [SerializeField] private bool useGlobalSpace = false; // Toggle between global and local space
    [SerializeField] private Vector3 targetPosition; // Position to force the object to

    void Update()
    {
        if (useGlobalSpace)
        {
            // Set the global position
            transform.position = targetPosition;
        }
        else
        {
            // Set the local position
            transform.localPosition = targetPosition;
        }
    }
}
