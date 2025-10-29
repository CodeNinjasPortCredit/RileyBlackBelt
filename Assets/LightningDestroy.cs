using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyAfterSeconds", 0.55f);
    }

    void DestroyAfterSeconds()
    {
        Destroy(gameObject);
    }
}
