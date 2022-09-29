using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalfosBehavior : MonoBehaviour
{

    [SerializeField] private float accel;
    [SerializeField] private float veloc;
    [SerializeField] private float WaitAtEndTime = 3.0f;
    [SerializeField] private float changeDirWaitTime = 4.0f;
    [SerializeField] private int changeDirNumTimes = 10;

    private Rigidbody stalfos_rb;
    private static Vector3[] directions;
    private int rand_dir;
    private Vector3 curr_dir;
    private bool can_collide = true;

    private Coroutine move;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flip(GetComponent<SpriteRenderer>()));

        stalfos_rb = GetComponent<Rigidbody>();

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        move = StartCoroutine(MoveStalfos());
    }

    IEnumerator Flip(SpriteRenderer sprite)
    {
        while (true)
        {

            sprite.flipX = true;

            yield return new WaitForSeconds(0.3f);

            sprite.flipX = false;

            yield return new WaitForSeconds(0.3f);

            yield return null;
        }
    }

    public IEnumerator MoveStalfos()
    {
        //rand_dir = Random.Range(0, 8);
        //curr_dir = directions[rand_dir];

        for (int i = 0; i < changeDirNumTimes; ++i)
        {
            
            rand_dir = Random.Range(0, 4);
            curr_dir = directions[rand_dir];

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
                Mathf.Round(transform.position.x * 2) / 2,
                transform.position.y,
                0);
            }

            //transform.position /= 2;

            stalfos_rb.velocity = curr_dir.normalized * veloc;
            //Debug.Log("velocity: " + stalfos_rb.velocity.ToString()); ;

            yield return new WaitForSeconds(changeDirWaitTime);
        }

        stalfos_rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(WaitAtEndTime);

        StartCoroutine(MoveStalfos());
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("wall") && can_collide)
        {

            StartCoroutine(WaitToCollide(false));

            //Debug.Log("velocity1: " + keese_rb.velocity.ToString());
            stalfos_rb.velocity = -stalfos_rb.velocity;
            //Debug.Log("velocity2: " + keese_rb.velocity.ToString());
            curr_dir = -curr_dir.normalized;
        }
    }

    public IEnumerator WaitToCollide(bool stunned)
    {
        can_collide = false;
        if (stunned)
            StopCoroutine(move);

        for (int i = 0; i < 3; ++i)
        {
            yield return new WaitForFixedUpdate();
        }

        can_collide = true;
        if (stunned)
            move = StartCoroutine(MoveStalfos());
    }
}
