using UnityEngine;

public class MaintainGlobalScale : MonoBehaviour
{
    public GameObject parentReference;
    private Transform currentChild;
    private float lastParentScaleX;

    private void Start()
    {
        currentChild = transform;
        // Initialize with the parent's current scale sign (positive or negative)
        lastParentScaleX = Mathf.Sign(parentReference.transform.lossyScale.x);
    }

    // Update is called once per frame
    void Update()
    {
        float currentParentScaleSign = Mathf.Sign(parentReference.transform.lossyScale.x);
        // Check if the sign of the parent's scale has changed
        if (currentParentScaleSign != lastParentScaleX)
        {
            currentChild.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            lastParentScaleX = currentParentScaleSign;
            print("Flip detected!");
        }
    }
}
