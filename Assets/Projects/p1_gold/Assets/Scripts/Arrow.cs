using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Weapon
{
    //public float arrowSpeed = 8.0f;

    public WeaponType type;

    // Start is called before the first frame update
    void Start()
    {
        type = WeaponType.Arrow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public IEnumerator ArrowProjectile(Vector3 prevDirection)
    //{
    //    Debug.Log("started explosion coroutine");
    //    //explosion = true;
    //    while (true)
    //    {
    //        yield return null;
    //        transform.position = Vector3.MoveTowards(
    //            transform.position,
    //            transform.position + prevDirection,
    //            arrowSpeed * Time.deltaTime);
    //    }

    //    Destroy(gameObject);
    //    Debug.Log("arrow destroyed");
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    string collisionTag = collision.gameObject.tag;

    //    if (collision.gameObject.CompareTag("enemy"))
    //    {
    //        //Debug.Log("hit enemy!");
    //        Destroy(collision.gameObject);
    //        //hit = true;
    //        Destroy(gameObject);
    //    }
    //    else if (collisionTag == "wall")
    //    {
    //        Debug.Log("beam hit wall");
    //        //hit = true;
    //        Destroy(gameObject);
    //        //SwordExplosion(center);
    //    }
    //}
}
