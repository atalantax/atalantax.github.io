using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    public WeaponType type;
    public bool hit;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        type = WeaponType.Boomerang;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("boomerang trigger");
        hit = true;

        GameObject collisionObject = collision.gameObject;
        Debug.Log(collisionObject.name);
        if (collisionObject.CompareTag("enemy"))
        {
            Debug.Log("boomerang hit enemy!");
            EnemyHasHealth enemyHealth = collisionObject.GetComponent<EnemyHasHealth>();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            switch(enemyHealth.type)
            {
                case EnemyHasHealth.EnemyType.Stalfos:
                    StalfosBehavior stalfos = collisionObject.GetComponent<StalfosBehavior>();
                    stalfos.StartCoroutine(stalfos.WaitToCollide(true));
                    break;
                case EnemyHasHealth.EnemyType.Gel:
                    enemyHealth.LoseLife(player.GetComponent<WeaponAnimation>().prevDirection, 1.0f);
                    break;
                case EnemyHasHealth.EnemyType.Keese:
                    enemyHealth.LoseLife(player.GetComponent<WeaponAnimation>().prevDirection, 1.0f);
                    break;
                case EnemyHasHealth.EnemyType.Goriya:
                    GoriyaBehavior goriya = collisionObject.GetComponent<GoriyaBehavior>();
                    goriya.StartCoroutine(goriya.WaitToCollide(true));
                    //enemyHealth.LoseLife(GetComponent<WeaponAnimation>().prevDirection, 1.0f);
                    break;
            }
            //EnemyHasHealth enemyHealth = collisionObject.GetComponent<EnemyHasHealth>();
            //enemyHealth.LoseLife(1.0f);
        }
        //else if (collisionObject.CompareTag("Player"))
        //{
        //    Debug.Log("boomerang hit player!");
        //    HasHealth playerHealth = collisionObject.GetComponent<HasHealth>();
        //    playerHealth.LoseLife(0.5f);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("boomerang hit wall");
            hit = true;
        }
    }
}
