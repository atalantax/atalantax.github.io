using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeTrap : MonoBehaviour
{
    public enum Loc
    {
        topLeft,
        topRight,
        bottomLeft,
        bottomRight
    }

    public enum Status
    {
        idle,
        attacking,
        returning
    }

    public Loc loc;

    public float rayVerticalLength = 1.0f;
    public float rayHorizontalLength = 4.0f;

    public float attackSpeed = 8.0f;
    public float returnSpeed = 6.0f;

    [SerializeField] private float AttackDuration = 1.0f;
    [SerializeField] private float ReturnDuration = 0.5f;

    private RaycastHit hitVertical;
    private RaycastHit hitHorizontal;

    [SerializeField] private Status status;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 verticalDir;
    [SerializeField] private Vector3 horizontalDir;

    private Vector3 currPos;

    private Rigidbody rb;

    private Coroutine attack;
    private Coroutine returnHome;
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        status = Status.idle;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (verticalDir == Vector3.zero && horizontalDir == Vector3.zero)
        {
            switch (loc)
            {
                case Loc.topLeft:
                    verticalDir = Vector3.down;
                    horizontalDir = Vector3.right;
                    break;
                case Loc.topRight:
                    verticalDir = Vector3.down;
                    horizontalDir = Vector3.left;
                    break;
                case Loc.bottomLeft:
                    verticalDir = Vector3.up;
                    horizontalDir = Vector3.right;
                    break;
                case Loc.bottomRight:
                    verticalDir = Vector3.up;
                    horizontalDir = Vector3.left;
                    break;
            }
        }

        //Debug.DrawLine(transform.po, Vector3.down * 100f, Color.red);

        //Debug.Log(transform.position.y + " " + transform.position.y * verticalDir.y);
        //Debug.Log(transform.position.x + " " + transform.position.x * horizontalDir.x);

        Debug.DrawLine(transform.position,
            new Vector3(
                transform.position.x,
                transform.position.y + verticalDir.y * rayVerticalLength,
                0),
            Color.red);
        Debug.DrawLine(transform.position,
            new Vector3(
                transform.position.x + horizontalDir.x * rayHorizontalLength,
                transform.position.y, 0),
            Color.red);

        if (status == Status.idle && transform.localPosition == initialPos)
        {
            //rb.isKinematic = true;
            GetComponent<BoxCollider>().enabled = true;
            // check vertical raycast
            Physics.Raycast(
                transform.position, verticalDir,
                out hitVertical, rayVerticalLength
            );

            if (hitVertical.collider != null && hitVertical.collider.CompareTag("Player"))
            {
                //rb.isKinematic = false;
                Debug.Log("vertical raycast hit");
                rb.constraints =
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                attack = StartCoroutine(Attack(verticalDir));
                return;
            }

            // check horizontal raycast
            Physics.Raycast(
                transform.position, horizontalDir,
                out hitHorizontal, rayHorizontalLength
            );

            if (hitHorizontal.collider != null && hitHorizontal.collider.CompareTag("Player"))
            {
                //rb.isKinematic = false;
                Debug.Log("horizontal raycast hit");
                rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                //rb.constraints = RigidbodyConstraints.FreezePositionZ;
                //rb.constraints = RigidbodyConstraints.FreezeRotation;
                attack = StartCoroutine(Attack(horizontalDir));
                return;
            }
        }
        else if (status == Status.idle)
        {
            transform.localPosition = initialPos;
        }
    }

    private IEnumerator Attack(Vector3 direction)
    {
        moveDir = direction;
        status = Status.attacking;
        yield return new WaitForSeconds(0.1f);

        //if (direction == Vector3.left || direction == Vector3.right)
        //{
        //    yield return CoroutineUtilities.MoveObjectOverTime(
        //        transform, transform.localPosition,
        //        transform.localPosition + direction * 3.5f, 1.5f);
        //}
        //else
        //{
        //    yield return CoroutineUtilities.MoveObjectOverTime(
        //        transform, transform.localPosition,
        //        transform.localPosition + direction * 1.5f, 3.5f);
        //}

        if (direction == Vector3.left || direction == Vector3.right)
        {
            while (status == Status.attacking && Math.Abs(transform.localPosition.x - initialPos.x) < 4.5f)
            {
                rb.velocity = direction * attackSpeed;
                currPos = transform.localPosition;
                yield return null;
            }
        }
        else
        {
            while (status == Status.attacking && Math.Abs(transform.localPosition.y - initialPos.y) < 2.5f)
            {
                rb.velocity = direction * attackSpeed;
                currPos = transform.localPosition;
                yield return null;
            }
        }

        //yield return CoroutineUtilities.MoveObjectOverTime(
        //    transform, initialPos, initialPos + direction * 4.0f, 1.0f
        //);

        Debug.Log("finished attacking");

        rb.velocity = Vector3.zero;
        returnHome = StartCoroutine(Return(-direction));
    }

    private IEnumerator Return(Vector3 direction)
    {
        Debug.Log("returning");
        moveDir = direction;
        status = Status.returning;
        yield return new WaitForSeconds(0.2f);

        //yield return CoroutineUtilities.MoveObjectOverTime(
        //    transform, transform.localPosition,
        //    initialPos, 4.0f);

        if (direction == Vector3.left || direction == Vector3.right)
        {
            while (status == Status.returning && Math.Abs(transform.localPosition.x - initialPos.x) > 0.1f)
            {
                rb.velocity = direction * returnSpeed;
                currPos = transform.localPosition;
                yield return null;
            }
        }
        else
        {
            while (status == Status.returning && Math.Abs(transform.localPosition.y - initialPos.y) > 0.1f)
            {
                rb.velocity = direction * returnSpeed;
                currPos = transform.localPosition;
                yield return null;
            }
        }

        rb.velocity = Vector3.zero;
        transform.localPosition = initialPos;
        yield return new WaitForSeconds(0.4f);

        if (direction == Vector3.left || direction == Vector3.right)
        {
            rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints =
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
        }

        status = Status.idle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (attack != null)
                StopCoroutine(attack);
            if (returnHome != null)
                StopCoroutine(returnHome);

            GetComponent<BoxCollider>().enabled = false;
            rb.velocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.localPosition = initialPos;
            Debug.Log(transform.localPosition);
            status = Status.idle;
            //GetComponent<BoxCollider>().enabled = false;
            //rb.isKinematic = true;
            //StopAllCoroutines();
            //ResetPos();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!Invincibility.godMode && player.GetComponent<HasHealth>().hasHealth)
            {
                Debug.Log(loc + " spike hit player");
                //player.GetComponent<HasHealth>().LoseLife(1.0f);
            }

            //StartCoroutine(Stun());
        }
        else if (other.gameObject.CompareTag("wall"))
        {
            rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator Stun()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (rb.velocity != Vector3.zero)
        {
            player.GetComponent<PlayerController>().can_player_input = false;
            yield return null;
        }

        player.GetComponent<PlayerController>().can_player_input = true;
    }

    private void ResetPos()
    {
        Vector3 vel = rb.velocity;
        Debug.Log(vel);
        rb.velocity = Vector3.zero;
        if (vel.x != 0)
        {
            if (loc == Loc.bottomLeft || loc == Loc.bottomRight)
            {
                transform.localPosition =
                    new Vector3(transform.localPosition.x, 2f, 0f);
            }
            else
            {
                transform.localPosition =
                    new Vector3(transform.localPosition.x, 8f, 0f);
            }
        }

        if (vel.y != 0)
        {
            if (loc == Loc.bottomLeft || loc == Loc.topLeft)
            {
                transform.localPosition =
                    new Vector3(2f, transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition =
                    new Vector3(13f, transform.localPosition.y, 0);
            }
        }

        Debug.Log(transform.localPosition);
        //rb.velocity = vel;

        attack = StartCoroutine(Attack(moveDir));
    }
}
