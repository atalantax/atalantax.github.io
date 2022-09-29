using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerang : MonoBehaviour
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
        //Debug.Log("boomerang collision");

        GameObject collisionObject = collision.gameObject;
        if (collisionObject.CompareTag("Player"))
        {
            Debug.Log("boomerang hit player!");
            HasHealth playerHealth = collisionObject.GetComponent<HasHealth>();
            playerHealth.LoseLife(0.5f);
        }
    }
}
