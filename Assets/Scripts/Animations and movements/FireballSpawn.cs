using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawn : MonoBehaviour
{
    public GameObject fireprefab;
    public GameObject Firespawn;
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

    IEnumerator SpawnFireballs()
    {
        while (true)
        {
            if (fireprefab.GetComponent<FireBallMovement>().isFacingLeft)
            {
                GameObject spawnedFire = Instantiate(fireprefab, Firespawn.transform);
                spawnedFire.transform.SetParent(null);
                spawnedFire.GetComponent<FireBallMovement>().Firetime = 18f;
                spawnedFire.GetComponent<FireBallMovement>().Firedestination = 0;
            }
            else
            {
                GameObject spawnedFire = Instantiate(fireprefab, Firespawn.transform);
                spawnedFire.transform.localScale = new Vector3(spawnedFire.transform.localScale.x * -1f, spawnedFire.transform.localScale.y, spawnedFire.transform.localScale.z);
                spawnedFire.transform.SetParent(null);

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
