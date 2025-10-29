using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrowprefab;
    public GameObject Arrowspawn;
    public bool facingLeft;
    public float SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
       // StartCoroutine("SpawnArrows");
        LeanTween.init(80000);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnArrows()
    {
        while (true)
        {
            if (arrowprefab.GetComponent<ArrowMovement>().isFacingLeft)
            {
                GameObject spawnedArrow = Instantiate(arrowprefab, Arrowspawn.transform);
                spawnedArrow.transform.SetParent(null);
                spawnedArrow.GetComponent<ArrowMovement>().Arrowtime = 18f;
                spawnedArrow.GetComponent<ArrowMovement>().Arrowdestination = 0;
            }
            else
            {
                GameObject spawnedArrow = Instantiate(arrowprefab, Arrowspawn.transform);
                spawnedArrow.transform.localScale = new Vector3(spawnedArrow.transform.localScale.x * -1f, spawnedArrow.transform.localScale.y, spawnedArrow.transform.localScale.z);
                spawnedArrow.transform.SetParent(null);

            }
            yield return new WaitForSeconds(SpawnTime);
        }
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
    }

}
