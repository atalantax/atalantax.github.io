using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoriyaBehavior : MonoBehaviour
{
    [SerializeField] private GameObject boomerang;
    [SerializeField] private float veloc = 4.0f;
    [SerializeField] private float WaitAtEndTime = 3.0f;
    [SerializeField] private float changeDirWaitTime = 1.0f;
    [SerializeField] private int changeDirNumTimes = 10;

    private Rigidbody goriya_rb;
    private Animator animator;
    private static Vector3[] directions;
    private int rand_dir;
    private Vector3 curr_dir;
    private bool can_collide = true;

    private Coroutine move;

    // Start is called before the first frame update
    void Start()
    {

        goriya_rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        move = StartCoroutine(MoveGoriya());
    }

    public IEnumerator MoveGoriya()
    {
        //rand_dir = Random.Range(0, 8);
        //curr_dir = directions[rand_dir];

        for (int i = 0; i < changeDirNumTimes; ++i)
        {

            rand_dir = UnityEngine.Random.Range(0, 4);
            curr_dir = directions[rand_dir];
            //Debug.Log(curr_dir);

            if (rand_dir == 0)
            {
                animator.SetTrigger("up");
                //StartCoroutine(Flip(GetComponent<SpriteRenderer>()));
            }
            else if (rand_dir == 1)
            {
                animator.SetTrigger("down");
                //StartCoroutine(Flip(GetComponent<SpriteRenderer>()));
            }
            else if (rand_dir == 2)
            {
                animator.SetTrigger("left");
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (rand_dir == 3)
            {
                animator.SetTrigger("right");
                GetComponent<SpriteRenderer>().flipX = false;
            }


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

            goriya_rb.velocity = curr_dir.normalized * veloc;
            //Debug.Log("velocity: " + goriya_rb.velocity.ToString()); ;

            yield return new WaitForSeconds(changeDirWaitTime);
        }

        goriya_rb.velocity = Vector3.zero;
        animator.SetTrigger("idle");

        StartCoroutine(ThrowBoomerang());

        yield return new WaitForSeconds(WaitAtEndTime);

        StartCoroutine(MoveGoriya());
    }

    IEnumerator Flip(SpriteRenderer sprite)
    {
        while (rand_dir == 0 || rand_dir == 1)
        {

            sprite.flipX = true;

            yield return new WaitForSeconds(0.3f);

            sprite.flipX = false;

            yield return new WaitForSeconds(0.3f);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("wall") && can_collide)
        {

            StartCoroutine(WaitToCollide(false));

            //Debug.Log("velocity1: " + keese_rb.velocity.ToString());
            goriya_rb.velocity = -goriya_rb.velocity;
            //Debug.Log("velocity2: " + keese_rb.velocity.ToString());
            curr_dir = -curr_dir.normalized;
        }
    }

    public IEnumerator WaitToCollide(bool stunned)
    {
        can_collide = false;
        if (stunned)
            StopCoroutine(move);

        for (int i = 0; i < 5; ++i)
        {
            yield return new WaitForFixedUpdate();
        }

        can_collide = true;
        if (stunned)
            move = StartCoroutine(MoveGoriya());
    }

    public IEnumerator ThrowBoomerang()
    {
        // Spawn boomerang prefab
        //GameObject new_boom = (GameObject) Instantiate(boomerang, transform.position + Vector3.down, Quaternion.identity);

        GameObject boom = Instantiate(
                    boomerang,
                    transform.position + curr_dir * 0.2f,
                    Quaternion.identity, transform);

        if (curr_dir == Vector3.up)
            boom.transform.rotation = Quaternion.Euler(0, 0, -90);
        else if (curr_dir == Vector3.down)
            boom.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (curr_dir == Vector3.right)
            boom.GetComponent<SpriteRenderer>().flipX = true;
        else
            boom.GetComponent<SpriteRenderer>().flipX = false;

        Vector3 initialPos = boom.transform.position;
        while (Math.Abs(Vector3.Distance(boom.transform.position, initialPos)) < 3.0f)
        {
            boom.GetComponent<Rigidbody>().AddForce(curr_dir.normalized * 12.0f);
            boom.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -10);

            yield return new WaitForFixedUpdate();
        }

        initialPos = boom.transform.position;
        while (Math.Abs(Vector3.Distance(boom.transform.position, initialPos)) < 3.0f)
        {
            boom.GetComponent<Rigidbody>().AddForce(-curr_dir.normalized * 12.0f);
            boom.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -10);

            yield return new WaitForFixedUpdate();
        }
        //playerAnimator.Play(prevState);


        //yield return new WaitForSeconds(2.0f);

        Destroy(boom);


    }

}