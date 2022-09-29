using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class HasHealth : MonoBehaviour
{
    public bool hasHealth = true;
    public float maxHealth;
    public float numLives;
    public PlayerHealth playerHealth;

    public PlayerController playerController;
    public Rigidbody rb;
    //public AudioClip lowHealthSound;

    public AudioManager audioManager;
    public AudioClip deathSound;
    //public WeaponAnimation weaponAnimation;

    public float stunTime;
    public float knockbackTime;
    public float invulnerableTime;
    public float knockbackSpeed;
    public float knockbackDist;

    private bool lowHealthIsPlaying;
    private bool deathIsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody>(out rb);
        lowHealthIsPlaying = false;
        deathIsPlaying = false;
        Debug.Log("set lives in start to " + numLives);
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numLives <= 0.0f && hasHealth)
        {
            if (CompareTag("Player") && !deathIsPlaying)
            {
                Debug.Log("player has died");
                playerController.can_player_input = false;
                Destroy(gameObject.GetComponent<PlayerController>());
                Destroy(gameObject.GetComponent<BoxCollider>());
                StartCoroutine(WaitForDeath());
            }
            else if (CompareTag("enemy"))
            {
                Debug.Log("enemy killed");
                Destroy(gameObject);
            }
        }
        else if (numLives <= 1.0f && hasHealth && !lowHealthIsPlaying)
        {
            lowHealthIsPlaying = true;
            audioManager.PlayLowHealthSound();
            //StartCoroutine(PlayLowHealthSound());
        }
        else if (numLives > 1.0f)
        {
            lowHealthIsPlaying = false;
        }
    }

    public void LoseLife(float lives)
    {
        if (hasHealth)
        {
            audioManager.PlayerHitSound();
            numLives -= lives;

            if (CompareTag("Player"))
            {
                Debug.Log("player lost health");
                hasHealth = false;
                playerController.can_player_input = false;
                StartCoroutine(Knockback());
            }
        }
    }

    public void GainLife(float lives)
    {
        if (hasHealth)
            numLives += lives;
    }

    public void IncreaseMaxLife(float lives)
    {
        maxHealth += lives;
        numLives = maxHealth;
    }

    public float GetNumLives()
    {
        return numLives;
    }

    private IEnumerator WaitForDeath()
    {
        //audioManager.PlayerDeathSound();
        deathIsPlaying = true;
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        yield return new WaitForSeconds(audioManager.playerDeath.length - 0.1f);
        Debug.Log("player died, restarting scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Knockback()
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
                rb.velocity = playerHealth.direction * knockbackSpeed;
                //Debug.Log(rb.velocity);
                yield return null;
            }

            rb.velocity = Vector3.zero;

            while (Time.time - initialTime < stunTime)
            {
                yield return null;
            }
            playerController.can_player_input = true;

            while (Time.time - initialTime < invulnerableTime)
            {
                yield return null;
            }

            if (!Invincibility.godMode)
                hasHealth = true;
            Debug.Log("knockback finished");
        }
    }
}
