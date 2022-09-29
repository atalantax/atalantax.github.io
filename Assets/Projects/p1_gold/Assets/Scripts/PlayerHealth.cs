using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HasHealth healthbar;
    //public WeaponAnimation weaponAnimation;
    public Vector3 direction;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<HasHealth>();
        //weaponAnimation = GetComponent<WeaponAnimation>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        HitDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Invincibility.godMode && healthbar != null && other.CompareTag("enemy"))
        {
            //Debug.Log("player hit by enemy");
            healthbar.LoseLife(0.5f);
            //Debug.Log(direction);
        }
    }

    private void HitDirection()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("run_down"))
            direction = Vector3.down;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("run_up"))
            direction = Vector3.up;
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("run_right"))
            direction = Vector3.right;
        else
            direction = Vector3.left;
    }
}
