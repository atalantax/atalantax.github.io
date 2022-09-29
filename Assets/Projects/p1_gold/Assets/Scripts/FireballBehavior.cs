using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{

    //public bool hitPlayer;
    public bool destroyMe;

    // Start is called before the first frame update
    void Start()
    {
        destroyMe = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        { 
            HasHealth player_health = other.GetComponent<HasHealth>();
            player_health.LoseLife(0.5f);
            //hitPlayer = true;
        }
        if (other.CompareTag("wall") || other.CompareTag("Player"))
        {
            destroyMe = true;
        }
    }
}
