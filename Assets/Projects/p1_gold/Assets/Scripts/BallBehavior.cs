using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float WaitAtEndTime = 2.0f;
    [SerializeField] private float changeDirWaitTime = 1.0f;
    [SerializeField] private int changeDirNumTimes = 1;

    public bool moved;

    private Rigidbody ball_rb;
    private static Vector3[] directions;
    private int rand_dir;
    private Vector3 facing;
    private Vector3 curr_dir;
    public Vector3 final_pos;
    public bool can_Kick;
    private bool fetch;

    Coroutine moving;

    // Start is called before the first frame update
    void Start()
    {

        ball_rb = GetComponent<Rigidbody>();
        facing = player.GetComponent<ArrowKeyMovement>().destination;
        fetch = false;
        moved = false;
        can_Kick = false;

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        
    }

    private void Update()
    {

        moved = false;

        // currently can only kick if player is moving in that direction

        if (player.GetComponent<ArrowKeyMovement>().destination != Vector3.zero)
            facing = player.GetComponent<ArrowKeyMovement>().destination;

        if (can_Kick && Input.GetKeyDown(KeyCode.X))
        {
            //Input.GetKeyDown(KeyCode.X)
            moved = true;
            curr_dir = facing.normalized * 3;
            Debug.Log("standing on ball");
            StartCoroutine(MoveBall());
        }

    }


    public IEnumerator MoveBall()
    {
        //rand_dir = Random.Range(0, 8);
        //curr_dir = directions[rand_dir];
        

            //rand_dir = Random.Range(0, 4);
            

        Debug.Log(curr_dir);

        //transform.position *= 2;

        // if moving in x direction round y
        if (curr_dir.x != 0.0f && curr_dir.y == 0.0f)
            {

                transform.position = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                0);

            }
            // if moving in y direction round x
            else if (curr_dir.y != 0.0f && curr_dir.x == 0.0f)
            {
                transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                transform.position.y,
                0);
            }

            //transform.position /= 2;

            final_pos = transform.position + curr_dir;

            //if (Physics.Raycast(transform.position, curr_dir, 0.6f))
            //{
            //    final_pos = transform.position;
            //}

            moving = StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform,
                transform.position, final_pos, 0.2f));
            //Debug.Log("velocity: " + ball_rb.velocity.ToString()); ;

        ball_rb.velocity = Vector3.zero;

        //moved = false;

        yield return null;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("wall"))
        {

            StopCoroutine(moving);

            //Debug.Log("velocity1: " + keese_rb.velocity.ToString());
            ball_rb.velocity = Vector3.zero;
            //Debug.Log("velocity2: " + keese_rb.velocity.ToString());
            //curr_dir = -curr_dir.normalized;

            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            0);

            final_pos = transform.position;
        }
        
    }
}
