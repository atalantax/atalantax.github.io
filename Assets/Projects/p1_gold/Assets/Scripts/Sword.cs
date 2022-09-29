using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    //public float swordSpeed = 8.0f;

    public WeaponType type;
    new public AudioClip sound;
    public AudioClip projectileSound;

    //private bool hit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        type = WeaponType.Sword;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public IEnumerator SwordProjectile(Vector3 prevDirection)
    //{
    //    Debug.Log("started projectile coroutine");
    //    //explosion = true;
    //    while (gameObject)
    //    {
    //        yield return null;
    //        transform.position = Vector3.MoveTowards(
    //            transform.position,
    //            transform.position + prevDirection,
    //            weaponSpeed * Time.deltaTime);
    //    }

    //    Debug.Log("sword destroyed");
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    string collisionTag  = collision.gameObject.tag;

    //    if (collisionTag == "enemy")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //    else if (collisionTag == "wall")
    //    {
    //        Debug.Log("beam hit wall");
    //    }

    //    Destroy(gameObject);
    //}
}
