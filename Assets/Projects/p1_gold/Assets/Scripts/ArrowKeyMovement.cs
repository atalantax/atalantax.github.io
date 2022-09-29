using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    public float movementSpeed;
    public int roomWidth = 36;
    public int roomHeight = 21;
    public LayerMask wallLayer;
    public float wallRayLength = 2f;
    public PlayerController playerController;

    private Rigidbody rb;
    public Vector3 destination;
    private float prev_time;

    private RigidbodyConstraints rbConstraints;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rbConstraints = rb.constraints;
        destination = transform.position;
        playerController = GetComponent<PlayerController>();
        prev_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(
        //    AlignToGrid(transform.position.x),
        //    AlignToGrid(transform.position.y),
        //    0.0f);

        //if (!playerController.can_player_input)
        //    return;

        bool is_player = TryGetComponent(out PlayerController player);
        //Debug.Log("player!?" + is_player.ToString());
        //Debug.Log(GetComponent<PlayerController>());
        if (is_player && player.can_player_input)
        {
            rb.constraints = rbConstraints;
            Vector2 currentInput = GetInput(player.can_player_input);
            destination = new Vector3(currentInput.x, currentInput.y, 0f);
        }
        else
        {
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            prev_time = Time.time;
            //rb.velocity = Vector3.zero;
            //RigidbodyConstraints.FreezeRotationZ;
            return;
        }

        //Vector2 currentInput = GetInput();
        //destination = new Vector3(currentInput.x, currentInput.y, 0f);
        //destination.x = AlignToGrid(destination.x);
        //destination.y = AlignToGrid(destination.y);

        if (destination.x > 0 && walkingIntoWall(Vector3.right)) { }
        else if (destination.x < 0 && walkingIntoWall(Vector3.left)) { }
        else if (destination.y > 0 && walkingIntoWall(Vector3.up)) { }
        else if (destination.y < 0 && walkingIntoWall(Vector3.down)) { }
        else if (Mathf.Abs(destination.x) > 0 || Mathf.Abs(destination.y) > 0)
        {
            if (!PlayerController.inBowRoom)
            {
                transform.position *= 2;

                // if moving in x direction round y
                if (destination.x != 0.0f && destination.y == 0.0f)
                {
                    transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Round(transform.position.y),
                    0);
                }
                // if moving in y direction round x
                else if (destination.y != 0.0f && destination.x == 0.0f)
                {
                    transform.position = new Vector3(
                    Mathf.Round(transform.position.x),
                    transform.position.y,
                    0);
                }

                transform.position /= 2;
            }

            transform.position = Vector3.MoveTowards(
                transform.position, transform.position + destination,
                movementSpeed * (Time.time - prev_time));
        }

        prev_time = Time.time;

    }

    private Vector2 GetInput(bool canInput)
    {
        if (!canInput)
            return Vector2.zero;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontalInput) > 0.0f)
            verticalInput = 0.0f;
        else
            horizontalInput = 0.0f;

        return new Vector2(horizontalInput, verticalInput);
    }

    private bool walkingIntoWall(Vector3 direction)
    {
        //Vector3 rayStart = transform.position + direction * wallRayLength;

        RaycastHit hitWall;
        Physics.Raycast(
            transform.position, direction,
            out hitWall, wallRayLength, wallLayer);
        Debug.DrawLine(
            transform.position,
            transform.position + direction * wallRayLength,
            Color.red);

        if (hitWall.collider != null && !hitWall.collider.tag.Contains("door"))
            return true;
        if (Physics.Raycast(
            transform.position, direction,
            out hitWall, wallRayLength, wallLayer, QueryTriggerInteraction.Collide))
        {
            //Debug.Log(hitWall);
            if (hitWall.collider.tag == "Water") return true;
            else if (hitWall.collider.tag == "2D Wall") return true;
        }

        return false;
    }
}
