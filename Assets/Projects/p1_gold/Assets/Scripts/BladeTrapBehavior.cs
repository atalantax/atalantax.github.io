using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrapBehavior : MonoBehaviour
{

    [SerializeField] private bool upDir;
    [SerializeField] private bool downDir;
    [SerializeField] private bool leftDir;
    [SerializeField] private bool rightDir;

    [SerializeField] private float AttackSpeed = 3.0f;
    [SerializeField] private float ReturnSpeed = 6.0f;

    private Rigidbody bt_rb;
    private static Vector3 original_pos;
    private static Vector3[] directions;
    private bool can_collide = true;
    private RaycastHit upHit;
    private RaycastHit downHit;
    private RaycastHit leftHit;
    private RaycastHit rightHit;

    private bool upCast = false;
    private bool downCast = false;
    private bool leftCast = false;
    private bool rightCast = false;

    private bool oldupCast = true;
    private bool olddownCast = true;
    private bool oldleftCast = true;
    private bool oldrightCast = true;

    private Coroutine returning;
    private Coroutine attacking;
    private Coroutine moving;
    private Vector3 boxSize = new Vector3(0.5f, 0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(
        Mathf.Round(transform.position.x),
        Mathf.Round(transform.position.y),
        0);

        bt_rb = GetComponent<Rigidbody>();
        original_pos = transform.position;

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };
    }

    private void FixedUpdate()
    {

        if (upCast || downCast)
        {

            transform.position = new Vector3(
            transform.position.x,
            Mathf.Round(transform.position.y),
            0);

        }
        // if moving in y direction round x
        else if (leftCast || rightCast)
        {
            transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            0);
        }

        if (transform.position == original_pos && returning != null)
        {
            //Debug.Log("home");
            returning = null;
            oldupCast = true;
            olddownCast = true;
            oldleftCast = true;
            oldrightCast = true;
        }

        // boxcasts
        //upCast = Physics.BoxCast(transform.position, boxSize,
        //    directions[0], out upHit, Quaternion.identity, 12.0f);
        //downCast = Physics.BoxCast(transform.position, boxSize,
        //    directions[1], out downHit, Quaternion.identity, 12.0f);
        //leftCast = Physics.BoxCast(transform.position, boxSize,
        //    directions[2], out leftHit, Quaternion.identity, 12.0f);
        //rightCast = Physics.BoxCast(transform.position, boxSize,
        //    directions[3], out rightHit, Quaternion.identity, 12.0f);

        if (oldupCast)
            upCast = Physics.Raycast(transform.position,
            directions[0], out upHit,  12.0f);
        if (olddownCast)
            downCast = Physics.Raycast(transform.position,
            directions[1], out downHit, 12.0f);
        if (oldleftCast)
            leftCast = Physics.Raycast(transform.position,
            directions[2], out leftHit, 12.0f);
        if (oldrightCast)
            rightCast = Physics.Raycast(transform.position,
            directions[3], out rightHit, 12.0f);

        //Debug.DrawLine(transform.position, transform.position + directions[0] * 12, Color.red);

        if (upDir && upCast && upHit.transform.CompareTag("Player"))
        {
            if (returning != null)
            {
                StopCoroutine(returning);
                returning = null;
                oldupCast = true;
                olddownCast = true;
                oldleftCast = true;
                oldrightCast = true;
            }
            if (attacking == null)
            {
                oldupCast = true;
                olddownCast = false;
                oldleftCast = false;
                oldrightCast = false;
                attacking = StartCoroutine(BladeTrapAttack(directions[0] * 12.0f));
            }
                
            
        }
        else if (downDir && downCast && downHit.transform.CompareTag("Player"))
        {
            if (returning != null)
            {
                StopCoroutine(returning);
                returning = null;
                oldupCast = true;
                olddownCast = true;
                oldleftCast = true;
                oldrightCast = true;
            }
            if (attacking == null)
            {
                oldupCast = false;
                olddownCast = true;
                oldleftCast = false;
                oldrightCast = false;
                attacking = StartCoroutine(BladeTrapAttack(directions[1] * 12.0f));
            }
                
        }
        else if (leftDir && leftCast && leftHit.transform.CompareTag("Player"))
        {
            //Debug.Log("left");
            if (returning != null)
            {
                //Debug.Log("stopped returning");
                StopCoroutine(returning);
                returning = null;
                oldupCast = true;
                olddownCast = true;
                oldleftCast = true;
                oldrightCast = true;
            }
            if (attacking == null)
            {
                oldupCast = false;
                olddownCast = false;
                oldleftCast = true;
                oldrightCast = false;
                attacking = StartCoroutine(BladeTrapAttack(directions[2] * 12.0f));
            }
                
        }
        else if (rightDir && rightCast && rightHit.transform.CompareTag("Player"))
        {
            if (returning != null)
            {
                StopCoroutine(returning);
                returning = null;
                oldupCast = true;
                olddownCast = true;
                oldleftCast = true;
                oldrightCast = true;
            }
            if (attacking == null)
            {
                oldupCast = false;
                olddownCast = false;
                oldleftCast = false;
                oldrightCast = true;
                attacking = StartCoroutine(BladeTrapAttack(directions[3] * 12.0f));
            }
                
        }

        
        

        //if (attacking == null)
        //    returning = StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform,
        //    transform.position, original_pos, ReturnSpeed));
    }


    public IEnumerator BladeTrapAttack(Vector3 direction)
    {
        //Debug.Log(transform.position);
        //Debug.Log(original_pos + direction);
        moving = StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform,
            transform.position, original_pos + direction, AttackSpeed));

        //returning = StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform,
        //    transform.position, original_pos, ReturnSpeed));

        yield return new WaitForSeconds(AttackSpeed);

        StopCoroutine(attacking);
        attacking = null;

        

    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (can_collide)
        {

            StartCoroutine(WaitToCollide());
            if (attacking != null)
            {
                StopCoroutine(moving);
                StopCoroutine(attacking);
                attacking = null;
            }

            returning = StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform,
            transform.position, original_pos, ReturnSpeed));


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

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    //Check if there has been a hit yet
    //    if (leftCast)
    //    {
    //        //Draw a Ray forward from GameObject toward the hit
    //        Gizmos.DrawRay(transform.position, Vector3.left * leftHit.distance);
    //        //Draw a cube that extends to where the hit exists
    //        Gizmos.DrawWireCube(transform.position + Vector3.left * leftHit.distance, transform.localScale);
    //    }
    //    //If there hasn't been a hit yet, draw the ray at the maximum distance
    //    else
    //    {
    //        //Draw a Ray forward from GameObject toward the maximum distance
    //        Gizmos.DrawRay(transform.position, transform.forward * 12);
    //        //Draw a cube at the maximum distance
    //        Gizmos.DrawWireCube(transform.position + transform.forward * 12, transform.localScale);
    //    }
    //}
}
