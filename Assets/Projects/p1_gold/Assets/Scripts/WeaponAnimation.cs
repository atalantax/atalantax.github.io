using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public HasHealth hasHealth;
    public PlayerController playerController;

    public WeaponInventory weaponInventory;
    public Inventory inventory;

    public Vector3 prevDirection;

    public Weapon.WeaponType primary;
    public Weapon.WeaponType secondary;

    static bool primaryInUse;
    static bool secondaryInUse;

    private Animator playerAnimator;
    private string prevState;

    private bool projectile;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        hasHealth = GetComponent<HasHealth>();
        playerController = GetComponent<PlayerController>();
        inventory = GetComponent<Inventory>();

        primaryInUse = false;
        secondaryInUse = false;

        projectile = false;
    }

    // Update is called once per frame
    void Update()
    {
        //secondary = inventory.GetSecondaryWeapon();

        if (Input.GetKeyDown(KeyCode.X) && !primaryInUse)
        {
            Debug.Log("using primary weapon");
            primary = inventory.GetPrimaryWeapon();
            if (primary == Weapon.WeaponType.Null)
                return;

            Debug.Log("player input disabled");
            playerController.can_player_input = false;
            playerAnimator.SetBool("weapon", true);
            primaryInUse = true;

            SetDirection(primary);
            UseWeapon(primary);

            //playerController.can_player_input = false;
            //primary.Use();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !secondaryInUse)
        {
            secondary = inventory.GetSecondaryWeapon();
            if (secondary == Weapon.WeaponType.Null)
                return;

            playerController.can_player_input = false;
            playerAnimator.SetBool("weapon", true);
            secondaryInUse = true;

            SetDirection(secondary);
            UseWeapon(secondary);
        }
    }

    private void UseWeapon(Weapon.WeaponType weaponType)
    {
        GameObject weapon = null;
        //GameObject weaponObject = null;
        int prefabIndex = 0;

        if (prevDirection == Vector3.right || prevDirection == Vector3.left)
            prefabIndex = 1;

        switch (weaponType)
        {
            case Weapon.WeaponType.Sword:
                weapon = Instantiate(
                    weaponInventory.sword.prefabs[prefabIndex],
                    transform.position + prevDirection * weaponInventory.sword.weaponOffset,
                    Quaternion.identity);
                weapon.GetComponent<Weapon>().projectile = false;
                weapon.GetComponent<Weapon>().PlaySound();
                break;

            case Weapon.WeaponType.Arrow:
                if (!Invincibility.godMode)
                {
                    if (inventory.GetRupees() <= 0)
                    {
                        FinishAnimation();

                        if (weaponType == primary)
                            primaryInUse = false;
                        else if (weaponType == secondary)
                            secondaryInUse = false;
                        return;
                    }
                    else
                        inventory.AddRupees(-1);
                }

                weapon = Instantiate(
                    weaponInventory.arrow.prefabs[prefabIndex],
                    transform.position + prevDirection * weaponInventory.sword.weaponOffset,
                    Quaternion.identity);
                weapon.GetComponent<Weapon>().projectile = true;
                weapon.GetComponent<Weapon>().PlaySound();
                break;

            case Weapon.WeaponType.Boomerang:
                weapon = Instantiate(
                    weaponInventory.boomerang.prefabs[0],
                    transform.position + prevDirection * weaponInventory.sword.weaponOffset,
                    Quaternion.identity);

                if (prevDirection == Vector3.up)
                    weapon.transform.rotation = Quaternion.Euler(0, 0, -90);
                else if (prevDirection == Vector3.down)
                    weapon.transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (prevDirection == Vector3.right)
                    weapon.GetComponent<SpriteRenderer>().flipX = true;
                else
                    weapon.GetComponent<SpriteRenderer>().flipX = false;

                Debug.Log("instantiated boomerang" + weapon);
                weapon.GetComponent<Weapon>().PlaySound();
                break;

            case Weapon.WeaponType.Bomb:
                if (!Invincibility.godMode)
                {
                    if (inventory.GetBombs() <= 0)
                    {
                        FinishAnimation();

                        if (weaponType == primary)
                            primaryInUse = false;
                        else if (weaponType == secondary)
                            secondaryInUse = false;
                        return;
                    }

                    inventory.AddBombs(-1);
                }

                weapon = Instantiate(
                    weaponInventory.bomb.prefabs[0], transform.position, Quaternion.identity);
                //Gizmos.DrawSphere(weapon.transform.position, weaponInventory.bomb.range);

                Destroy(weapon.GetComponent<Collider>());

                StartCoroutine(Bomb(weapon, weaponInventory.bomb));

                
                return;
        }

        if (prevDirection == Vector3.left)
        {
            if (weaponType == Weapon.WeaponType.Boomerang)
                weapon.GetComponent<SpriteRenderer>().flipX = false;
            else
                weapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (prevDirection == Vector3.up)
        {
            if (weaponType == Weapon.WeaponType.Boomerang)
                weapon.GetComponent<SpriteRenderer>().flipX = false;
            else
                weapon.GetComponent<SpriteRenderer>().flipY = true;
        }

        StartCoroutine(WaitForWeapon(weapon, weaponType));
    }

    private IEnumerator WaitForWeapon(GameObject weaponObject, Weapon.WeaponType type)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();
        //if (weapon.GetWeaponType() == primary)
        //    primaryInUse = false;
        //else if (weapon.GetWeaponType() == secondary)
        //    secondaryInUse = false;

        bool fullHealth = hasHealth.GetNumLives() == hasHealth.maxHealth;
        if ((fullHealth && type == Weapon.WeaponType.Sword && !projectile)
                || type == Weapon.WeaponType.Arrow)
        {
            //yield return new WaitForSeconds(weapon.weaponLag / 4);
            //Debug.Log("weapon.weapon " + weapon.weapon);
            //Debug.Log("weaponObject " + weaponObject);

            projectile = true;
            weaponObject.SetActive(true);
            weaponObject.GetComponent<Weapon>().projectile = true;
            //playerAnimator.Play(prevState);

            yield return new WaitForSeconds(weapon.weaponLag);
            if (type == primary)
                primaryInUse = false;
            else if (type == secondary)
                secondaryInUse = false;

            StartCoroutine(Projectile(weaponObject, weapon.weaponSpeed, type));
            FinishAnimation();

            while (weaponObject != null)
            {
                yield return null;
            }

            projectile = false;
        }
        else if (type == Weapon.WeaponType.Boomerang)
        {
            weaponObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -10);
            yield return new WaitForSeconds(weapon.weaponLag / 4);
            Debug.Log("weapon.weapon " + weapon.weapon);
            Debug.Log("weaponObject " + weaponObject);
            weaponObject.SetActive(true);
            StartCoroutine(BoomerangProjectile(weaponObject, weapon.weaponSpeed));
            //playerAnimator.Play(prevState);

            //yield return new WaitForSeconds(weapon.weaponLag);
            FinishAnimation();

            while (weaponObject != null)
            {
                yield return null;
            }

            if (type == primary)
                primaryInUse = false;
            else if (type == secondary)
                secondaryInUse = false;
        }
        else
        {
            yield return new WaitForSeconds(weapon.weaponLag);

            if (type == primary)
                primaryInUse = false;
            else if (type == secondary)
                secondaryInUse = false;

            try
            {
                Destroy(weaponObject);
            }
            catch (Exception e)
            {
                
            }

            FinishAnimation();

            //playerAnimator.Play(prevState);
        }

        //if (weaponObject != null)
        //    Destroy(weaponObject);

        //FinishAnimation(type);
    }

    private IEnumerator Projectile(GameObject weapon, float weaponSpeed, Weapon.WeaponType type)
    {
        Debug.Log("started projectile coroutine");
        Debug.Log(weapon);

        Vector3 direction = prevDirection;

        AudioSource.PlayClipAtPoint(
            weaponInventory.sword.projectileSound, Camera.main.transform.position);

        //explosion = true;
        while (weapon != null && weapon.gameObject != null)
        {
            yield return null;
            //Debug.Log("weapon moving: " + projectile);
            try
            {
                weapon.transform.position = Vector3.MoveTowards(
                    weapon.transform.position,
                    weapon.transform.position + direction,
                    weaponSpeed * Time.deltaTime);
            }
            catch (Exception e)
            {
                break;
            }
        }

        //Destroy(weapon);
        Debug.Log("projectile destroyed");
    }

    private IEnumerator BoomerangProjectile(GameObject weapon, float weaponSpeed)
    {
        Debug.Log("started boomerang projectile coroutine");
        Debug.Log(weapon);

        IEnumerator loop = LoopSound();
        StartCoroutine(loop);

        bool isBoom = weapon.TryGetComponent<Boomerang>(out Boomerang boomerang);

        float distanceTravelled = 0.0f;
        Vector3 initialPos = weapon.transform.position;
        Vector3 direction = prevDirection;

        while (weapon != null && weapon.gameObject != null && isBoom &&
               !boomerang.hit && distanceTravelled < boomerang.range)
        {
            yield return null;
            //Debug.Log("weapon moving: " + projectile);
            try
            {
                weapon.transform.position = Vector3.MoveTowards(
                    weapon.transform.position,
                    weapon.transform.position + direction,
                    weaponSpeed * Time.deltaTime);

                distanceTravelled = Math.Abs(
                    Vector3.Distance(initialPos, weapon.transform.position));
            }
            catch (Exception e)
            {
                break;
            }
        }

        while (true)
        {
            yield return null;
            try
            {
                weapon.transform.position = Vector3.MoveTowards(
                    weapon.transform.position,
                    transform.position,
                    weaponSpeed * Time.deltaTime
                );

                if (weapon.transform.position == transform.position)
                    break;
            }
            catch (Exception e)
            {
                break;
            }
        }

        if (weapon != null && weapon.transform.position == transform.position)
        {
            try
            {
                Destroy(weapon);
            }
            catch (Exception e) { }
        }

        StopCoroutine(loop);

        //Destroy(weapon);
        Debug.Log("projectile destroyed");
    }

    private IEnumerator LoopSound()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = weaponInventory.boomerang.sound;
        //float length = audio.clip.length;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            audio.Play();
        }
    }

    private IEnumerator Bomb(GameObject weapon, Bomb bomb)
    {
        AudioSource.PlayClipAtPoint(bomb.sound, Camera.main.transform.position);

        yield return new WaitForSeconds(bomb.weaponLag);
        FinishAnimation();
        if (Weapon.WeaponType.Bomb == primary)
            primaryInUse = false;
        else if (Weapon.WeaponType.Bomb == secondary)
            secondaryInUse = false;

        List<GameObject> enemiesInRange = GetEnemiesInRange(bomb.range, weapon.transform.position);
        yield return new WaitForSeconds(bomb.timer);
        AudioSource.PlayClipAtPoint(bomb.explosionSound, Camera.main.transform.position);
        foreach (GameObject enemy in enemiesInRange)
        {
            Destroy(enemy);
        }

        weapon.GetComponent<Weapon>().PlaySound();
        Destroy(weapon);
    }

    private List<GameObject> GetEnemiesInRange(float range, Vector3 loc)
    {
        List<GameObject> enemiesInRange = new();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            if (Math.Abs(Vector3.Distance(enemy.transform.position, loc)) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    // returns vector 3 in the direction that link is facing
    // sets bools for animation states & sword sprite
    private void SetDirection(Weapon.WeaponType type)
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo);

        if (stateInfo.IsName("run_down"))
        {
            prevState = "run_down";
            prevDirection = Vector3.down;
            SetWeaponAnimationDirection(type, "Down");
        }
        else if (stateInfo.IsName("run_up"))
        {
            prevState = "run_up";
            prevDirection = Vector3.up;
            SetWeaponAnimationDirection(type, "Up");
        }
        else if (stateInfo.IsName("run_right"))
        {
            prevState = "run_right";
            prevDirection = Vector3.right;
            SetWeaponAnimationDirection(type, "Right");
        }
        else
        {
            prevState = "run_left";
            prevDirection = Vector3.left;
            SetWeaponAnimationDirection(type, "Left");
        }
    }

    private void SetWeaponAnimationDirection(Weapon.WeaponType type, string direction)
    {
        switch (type)
        {
            case Weapon.WeaponType.Sword:
                playerAnimator.SetBool("sword" + direction, true);
                break;
            case Weapon.WeaponType.Arrow:
                playerAnimator.SetBool("arrow" + direction, true);
                break;
            case Weapon.WeaponType.Boomerang:
                //playerAnimator.SetBool("boomerang" + direction, true);
                break;
        }
    }

    private void FinishAnimation()
    {
        ResetDirection();
        playerAnimator.SetBool("weapon", false);
        playerAnimator.Play(prevState);

        Debug.Log("player input re-enabled");
        playerController.can_player_input = true;
    }

    private void ResetDirection()
    {
        playerAnimator.SetBool("swordDown", false);
        playerAnimator.SetBool("swordUp", false);
        playerAnimator.SetBool("swordLeft", false);
        playerAnimator.SetBool("swordRight", false);

        playerAnimator.SetBool("arrowDown", false);
        playerAnimator.SetBool("arrowUp", false);
        playerAnimator.SetBool("arrowLeft", false);
        playerAnimator.SetBool("arrowRight", false);
    }
}
