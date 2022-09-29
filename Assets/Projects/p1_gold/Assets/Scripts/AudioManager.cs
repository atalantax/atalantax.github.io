using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip playerLowHealth;
    public AudioClip playerHit;
    public AudioClip playerDeath;

    public float loopDelay;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHitSound()
    {
        AudioSource.PlayClipAtPoint(playerHit, Camera.main.transform.position);
    }

    public void PlayerDeathSound()
    {
        AudioSource.PlayClipAtPoint(playerDeath, Camera.main.transform.position);
    }

    public void PlayLowHealthSound()
    {
        StartCoroutine(LoopAudio(playerLowHealth));
    }

    private IEnumerator LoopAudio(AudioClip clip)
    {
        audio.clip = clip;
        //float length = audio.clip.length;

        while (true)
        {
            yield return new WaitForSeconds(loopDelay);
            audio.Play();
        }
    }
}
