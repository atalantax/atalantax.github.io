using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehavior : MonoBehaviour
{

    public GameObject[] balls;

    [SerializeField] private float veloc = 4.0f;
    [SerializeField] private float WaitAtEndTime = 3.0f;
    [SerializeField] private float changeDirWaitTime = 1.0f;

    private Rigidbody dog_rb;
    private Animator animator;
    [SerializeField] private GameObject player;
    private Inventory playerInventory;
    [SerializeField] private AudioClip keyCollectionSound;

    //private static Vector3[] directions;
    //private int rand_dir;
    public Vector3 curr_dir;
    public Vector3 final_pos;
    public bool reached_dest = true;
    private bool can_collide = true;

    private bool overWater;
    private Queue<Vector3> ballPosQ;
    private Queue<int> ballIDQ;

    // Start is called before the first frame update
    void Start()
    {
        dog_rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInventory = player.GetComponent<Inventory>();
        final_pos = transform.position;
        ballPosQ = new Queue<Vector3> ();
        ballIDQ = new Queue<int> ();

        overWater = false;

        //directions = new Vector3[]
        //    {
        //    Vector3.up,
        //    Vector3.down,
        //    Vector3.left,
        //    Vector3.right,
        //    };

        StartCoroutine(MoveDog());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < balls.Length; ++i)
        {
            if (balls[i].GetComponent<BallBehavior>().moved &&
                !ballIDQ.Contains(i))
            {
                ballPosQ.Enqueue(balls[i].GetComponent<BallBehavior>().final_pos);
                ballIDQ.Enqueue(i);
                Debug.Log("qing position " + i.ToString());
            }
            else if (balls[i].GetComponent<BallBehavior>().moved &&
                ballIDQ.Contains(i))
            {

                Queue<Vector3> temp = new Queue<Vector3>();
                Queue<int> tempID = new Queue<int>();

                while (ballPosQ.Count != 0)
                {
                    if (ballIDQ.Peek() == i)
                    {
                        temp.Enqueue(balls[i].GetComponent<BallBehavior>().final_pos);
                        tempID.Enqueue(i);
                    }
                    else
                    {
                        temp.Enqueue(ballPosQ.Peek());
                        tempID.Enqueue(ballIDQ.Peek());
                    }

                    ballPosQ.Dequeue();
                    ballIDQ.Dequeue();
                }

                ballPosQ = new Queue<Vector3>(temp);
                ballIDQ = new Queue<int>(tempID);
            }
        }

        // if theyre on the same y axis
        if (ballPosQ.Count > 0)
        {
            if (Mathf.Abs(ballPosQ.Peek().x - transform.position.x) < 0.01 && reached_dest)
            {
                final_pos = ballPosQ.Dequeue(); ballIDQ.Dequeue();
                reached_dest = false;
                StartCoroutine(MoveDog());
            }
            // if theyre on the same x axis
            else if (Mathf.Abs(ballPosQ.Peek().y - transform.position.y) < 0.01 && reached_dest)
            {
                final_pos = ballPosQ.Dequeue(); ballIDQ.Dequeue();
                reached_dest = false;
                StartCoroutine(MoveDog());
            }
            //else if (!reached_dest)
            //{
            //    final_pos = final_pos;
            //}
        }
        else if (ballPosQ.Count == 0 && reached_dest)
        {
            animator.SetTrigger("dogNotWalking");
        }

        //Debug.Log(ballPosQ);
    }

    public IEnumerator MoveDog()
    {
        //rand_dir = Random.Range(0, 8);
        //curr_dir = directions[rand_dir];
        //Debug.Log((final_pos - transform.position).magnitude);

        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            0);

        

        while ((final_pos - transform.position).magnitude > 0.23)
        //while (final_pos != transform.position)
        {
            animator.SetTrigger("dogWalking");

            //rand_dir = Random.Range(0, 4);
            reached_dest = false;
            curr_dir = (final_pos - transform.position).normalized;
            Debug.Log(transform.localPosition);
            Debug.Log(transform.position);
            //Debug.Log((final_pos - transform.position).magnitude);

            //if (rand_dir == 0)
            //{
            //    animator.SetTrigger("up");
            //    //StartCoroutine(Flip(GetComponent<SpriteRenderer>()));
            //}
            //else if (rand_dir == 1)
            //{
            //    animator.SetTrigger("down");
            //    //StartCoroutine(Flip(GetComponent<SpriteRenderer>()));
            //}
            //else
            if (curr_dir.x < 0)
            {
                //animator.SetTrigger("left");
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (curr_dir.x > 0)
            {
                //animator.SetTrigger("right");
                GetComponent<SpriteRenderer>().flipX = false;
            }


            //transform.position *= 2;

            // if moving in x direction round y
            if (curr_dir.x != 0.0f ) // && curr_dir.y == 0.0f)
            {

                transform.position = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                0);

            }
            // if moving in y direction round x
            else if (curr_dir.y != 0.0f ) // && curr_dir.x == 0.0f)
            {
                transform.position = new Vector3(
                Mathf.Round(transform.position.x * 2) / 2,
                transform.position.y,
                0);
            }

            //transform.position /= 2;

            dog_rb.velocity = curr_dir.normalized * veloc;
            //Debug.Log("velocity: " + goriya_rb.velocity.ToString()); ;

            yield return new WaitForSeconds(0.20f);
            //yield return null;
        }

        // if moving in x direction round y
        transform.position = new Vector3(
        Mathf.Round(transform.position.x),
        Mathf.Round(transform.position.y),
        0);

        dog_rb.velocity = Vector3.zero;

        animator.SetTrigger("dogNotWalking");

        reached_dest = true;

        //StartCoroutine(MoveDog());
    }
    

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("wall") && can_collide)
        {

            StartCoroutine(WaitToCollide());

            //Debug.Log("velocity1: " + keese_rb.velocity.ToString());
            dog_rb.velocity = -dog_rb.velocity;
            //Debug.Log("velocity2: " + keese_rb.velocity.ToString());
            curr_dir = -curr_dir.normalized;
            reached_dest = true;
        }
        else if (collision.gameObject.CompareTag("water"))
        {
            //water_dir = curr_dir;
            overWater = true;
            animator.SetTrigger("dogJumping");
            //StartCoroutine(JumpWater());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("key") && reached_dest)
        {
            if (playerInventory != null)
                playerInventory.AddKeys(1);
            
            Destroy(other.gameObject);
            
            AudioSource.PlayClipAtPoint(keyCollectionSound, Camera.main.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            overWater = false;
            animator.SetTrigger("dogWalking");
        }
    }

    public IEnumerator WaitToCollide()
    {
        can_collide = false;

        for (int i = 0; i < 5; ++i)
        {
            yield return new WaitForFixedUpdate();
        }

        can_collide = true;
    }

    //public IEnumerator JumpWater()
    //{
    //    while(overWater)
    //    {
    //        curr_dir = water_dir;
    //    }
    //    yield return null;
    //}
}
