using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("hit enemy!");
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
