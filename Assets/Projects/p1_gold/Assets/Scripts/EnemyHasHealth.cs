using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHasHealth : MonoBehaviour
{
    public enum EnemyType
    {
        Stalfos,
        Keese,
        Gel,
        Goriya,
        Wallmaster,
        Aquamentus
    }

    public AudioClip stunSound;
    public AudioClip deathSound;
    public EnemyType type;

    public bool hasHealth = true;
    public float numLives;

    public float stunTime = 0.2f;
    public float knockbackTime = 1f;
    public float invulnerableTime = 0.3f;
    public float knockbackSpeed = -10;
    public float knockbackDist = 3;

    public bool spawnsItem;
    public SpawnKey spawner;

    [Tooltip("bomb, rupee, rupee(10), hp")]
    public List<GameObject> itemPrefabs;

    public Lock doorLock;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawner = GetComponent<SpawnKey>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseLife(Vector3 projectileDirection, float lives)
    {
        if (hasHealth)
        {
            numLives -= lives;

            if (type == EnemyType.Stalfos || type == EnemyType.Goriya)
                AudioSource.PlayClipAtPoint(stunSound, Camera.main.transform.position);

            if (numLives <= 0)
            {
                AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
                Destroy(gameObject);

                if (spawnsItem)
                {
                    spawner.Spawn();
                }

                if (type == EnemyType.Aquamentus)
                {
                    doorLock.UnlockDoor();
                }
                else
                {
                    RandomItemSpawner();
                }

                return;
            }

            //StartCoroutine(Knockback(projectileDirection));
            StartCoroutine(HitStun());
        }
    }

    private IEnumerator HitStun()
    {
        hasHealth = false;
        //if (numLives != 0)
        //    StartCoroutine("Knockback");

        yield return new WaitForSeconds(stunTime);
        hasHealth = true;
    }

    private IEnumerator Knockback(Vector3 direction)
    {
        Vector3 initalPos = transform.position;
        float initialTime = Time.time;

        if (rb != null)
        {
            Debug.Log("knockback started");
            while (Math.Abs(Vector3.Distance(transform.position, initalPos)) < knockbackDist
                && Time.time - initialTime < knockbackTime)
            {
                //Debug.Log(playerHealth.direction);
                rb.velocity = direction * knockbackSpeed;
                //Debug.Log(rb.velocity);
                yield return null;
            }

            rb.velocity = Vector3.zero;

            while (Time.time - initialTime < stunTime)
            {
                yield return null;
            }

            while (Time.time - initialTime < invulnerableTime)
            {
                yield return null;
            }

            if (!Invincibility.godMode)
                hasHealth = true;
            Debug.Log("knockback finished");
        }
    }

    private void RandomItemSpawner()
    {
        //int i = UnityEngine.Random.Range(0, 3);
        int i = 0;

        if (type == EnemyType.Keese && UnityEngine.Random.Range(1, 100) <= 5)
        {
            spawner.itemPrefab = itemPrefabs[i];
            spawner.Spawn();
        }
        else if (type == EnemyType.Gel && UnityEngine.Random.Range(1, 100) == 2)
        {
            spawner.itemPrefab = itemPrefabs[i];
            spawner.Spawn();
        }
        else if (type == EnemyType.Goriya && UnityEngine.Random.Range(1, 100) <= 10)
        {
            spawner.itemPrefab = itemPrefabs[i];
            spawner.Spawn();
        }
        else if (type == EnemyType.Stalfos && UnityEngine.Random.Range(1, 100) <= 8)
        {
            spawner.itemPrefab = itemPrefabs[i];
            spawner.Spawn();
        }
    }
}
