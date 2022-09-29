using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Sword,
        Arrow,
        Boomerang,
        Bomb,
        Null
    }

    public float weaponLag = 0.3f;
    public float weaponOffset = 0.8f;

    [Tooltip("MUST be in order: vertical, horizontal")]
    public GameObject[] prefabs;
    public GameObject weapon;

    public AudioClip sound;

    [Tooltip("MUST be in order: down, up, right, left")]
    public Sprite[] sprites;

    public float weaponSpeed;
    public bool projectile;

    private WeaponType type;
    private IEnumerator projectileCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        projectile = false;
        //Debug.Log("weapon instantiated");
        //inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (type == WeaponType.Sword)
        //{
        //    weapon = gameObject.GetComponent<Sword>().weapon;
        //}
    }

    public void SetWeaponType(WeaponType typeIn)
    {
        type = typeIn;
    }

    public WeaponType GetWeaponType()
    {
        return type;
    }

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
    }

    //public bool GetInUse()
    //{
    //    return inUse;
    //}

    //public void SetInUse()
    //{
    //    inUse = !inUse;
    //}

    public GameObject Use(Vector3 direction, Vector3 pos, Quaternion rot)
    {
        if (direction == Vector3.down || direction == Vector3.up)
            weapon = Instantiate(prefabs[0], pos + direction * weaponOffset, rot);
        else
            weapon = Instantiate(prefabs[1], pos + direction * weaponOffset, rot);

        if (direction == Vector3.down)
        {
            weapon.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if (direction == Vector3.up)
        {
            weapon.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else if (direction == Vector3.right)
        {
            weapon.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }

        weapon.SetActive(true);
        //SetInUse();

        //Debug.Log("using weapon");
        //StartCoroutine(WaitForWeapon());
        return weapon;
    }

    //public IEnumerator Projectile(Vector3 prevDirection)
    //{
    //    Debug.Log("started projectile coroutine");
    //    Debug.Log(weapon);
    //    projectile = true;
    //    //explosion = true;
    //    while (gameObject != null && weapon != null)
    //    {
    //        yield return null;
    //        //Debug.Log("weapon moving: " + projectile);
    //        weapon.transform.position = Vector3.MoveTowards(
    //            weapon.transform.position,
    //            weapon.transform.position + prevDirection,
    //            weaponSpeed * Time.deltaTime);
    //    }

    //    Destroy(gameObject);
    //    Debug.Log("projectile destroyed");

    //    //yield return null;
    //    //projectileCoroutine = ProjectileAnimator(prevDirection);
    //    //StartCoroutine(projectileCoroutine);
    //}

    private IEnumerator ProjectileAnimator(Vector3 prevDirection)
    {
        Debug.Log("started projectile coroutine");
        projectile = true;
        //explosion = true;
        while (gameObject != null && projectile && weapon != null)
        {
            yield return null;
            //Debug.Log("weapon moving: " + projectile);
            weapon.transform.position = Vector3.MoveTowards(
                weapon.transform.position,
                weapon.transform.position + prevDirection,
                weaponSpeed * Time.deltaTime);
        }

        //Destroy(weapon);
        Debug.Log("projectile destroyed");
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;
        if (collisionObject.CompareTag("enemy"))
        {
            Debug.Log("hit enemy!");
            EnemyHasHealth enemyHealth = collisionObject.GetComponent<EnemyHasHealth>();
            if (enemyHealth != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                enemyHealth.LoseLife(player.GetComponent<WeaponAnimation>().prevDirection, 1.0f);
            }
            Destroy(gameObject);
            //EnemyHasHealth enemyHealth = collisionObject.GetComponent<EnemyHasHealth>();
            //enemyHealth.LoseLife(1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("weapon collision");
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.CompareTag("wall") && projectile)
            Destroy(gameObject);
    }
}
