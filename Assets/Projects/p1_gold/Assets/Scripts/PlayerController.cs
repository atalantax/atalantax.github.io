using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public SpawnKey keyInstance;
    public static bool inOldManRoom;
    public static bool inBowRoom;

    public bool can_player_input;
    public Inventory inventory;

    public Animator animator;

    //public bool canKick = false;

    // Start is called before the first frame update
    void Start()
    {
        inBowRoom = false;
        inOldManRoom = false;
        can_player_input = true;
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        //doorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.RotateWeapons();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("custom level!!");
            SceneManager.LoadScene("CustomLevel");
        }

        animator.SetBool("canInput", can_player_input);
        //if (!can_player_input)
        //    animator.speed = 0.0f;
        //else
        //    animator.speed = 1.0f;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.tag);

        if (other.CompareTag("ball"))
        {
            //Debug.Log("standing on ball");
            other.GetComponent<BallBehavior>().can_Kick = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            other.GetComponent<BallBehavior>().can_Kick = false;
        }
    }
}
