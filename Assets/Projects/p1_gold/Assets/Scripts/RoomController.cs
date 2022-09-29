using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public AudioClip lockSound;

    [Tooltip("Whether the door is a locked door at the start.")]
    public bool isLocked;

    [Tooltip("Whether the door locks after entering the room.")]
    public bool trapLock;
    public bool previouslyTrapped;
    public bool trapActivated;

    public bool bossRoom;
    public bool oldManRoom;
    public bool bowRoom;
    public bool inRoom;

    public UITypewriter message;

    public int keysToUnlock;
    //public SpawnKey itemSpawner;
    //public bool hasItem;
    //public bool hasCompass;

    [Tooltip("The direction the room transition will go toward.")]
    public Direction roomDirection;
    public RoomController nextRoom;

    public PushableBlock block;

    public Inventory inventory;
    public PlayerController player;

    private BoxCollider playerCollider;
    [SerializeField]
    private bool inTransition;
    private Lock doorLock;
    //private bool inRoom;

    private Vector3 move_room_right = new Vector3(16, 0, -10);
    private Vector3 move_room_left = new Vector3(-16, 0, -10);
    private Vector3 move_room_up = new Vector3(0, 11, -10);
    private Vector3 move_room_down = new Vector3(0, -11, -10);

    // Start is called before the first frame update
    void Start()
    {
        previouslyTrapped = false;
        trapActivated = false;
        inRoom = false;
        //inRoom = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inTransition = false;
        playerCollider = player.GetComponent<BoxCollider>();
        inventory = player.GetComponent<Inventory>();
        //itemSpawner = GetComponent<SpawnKey>();

        doorLock = GetComponent<Lock>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (inventory.GetKeys() == keysToUnlock)
        //{
        //    isLocked = false;
        //    inventory.RemoveKeys(keysToUnlock);
        //}

        //if (hasItem && !itemSpawner.spawned)
        //{
        //    //Debug.Log(gameObject);
        //    itemSpawner.Spawn();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject objectCollidedWith = other.gameObject;
        if (isLocked && !bossRoom)
        {
            doorLock.CollidedWithDoor(other);
            //return;
        }

        //Debug.Log("player hit door");

        //if (objectCollidedWith.CompareTag("Player") && !inTransition)
        if (objectCollidedWith.CompareTag("Player") && !isLocked && !inTransition)
        {
            switch (roomDirection)
            {
                case Direction.Left:
                    StartCoroutine(RoomTransition(move_room_left));
                    break;
                case Direction.Right:
                    StartCoroutine(RoomTransition(move_room_right));
                    break;
                case Direction.Up:
                    StartCoroutine(RoomTransition(move_room_up));
                    break;
                case Direction.Down:
                    StartCoroutine(RoomTransition(move_room_down));
                    break;
            }
        }
    }

    private IEnumerator RoomTransition(Vector3 move_direction)
    {
        playerCollider.enabled = false;
        player.can_player_input = false;
        inTransition = true;

        Vector3 newPlayerPosition = player.transform.position;
        float leftFactor = 3.5f;
        if (nextRoom != null && nextRoom.oldManRoom)
            leftFactor = 4f;

        switch (roomDirection)
        {
            case Direction.Left:
                newPlayerPosition = player.transform.position + Vector3.left * leftFactor;
                //newPlayerPosition = player.transform.position + Vector3.left * 3.5f;
                break;
            case Direction.Right:
                newPlayerPosition = player.transform.position + Vector3.right * 3.5f;
                break;
            case Direction.Up:
                newPlayerPosition = player.transform.position + Vector3.up * 4.5f;
                break;
            case Direction.Down:
                newPlayerPosition = player.transform.position + Vector3.down * 4.5f;
                break;
        }

        // hide text when leaving old man room
        if (oldManRoom && inRoom)
        {
            StartCoroutine(HideTextOverTime(message));

            if (nextRoom.block != null)
                nextRoom.block.ResetPosition();
        }

        StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(player.transform, player.transform.position, newPlayerPosition, 2.5f)
        );

        Vector3 initial_position = Camera.main.transform.position;
        Vector3 final_position = initial_position + move_direction;


        /* Transition to new "room" */
        yield return StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(Camera.main.transform, initial_position, final_position, 2.3f)
        );


        //playerRenderer.enabled = true;
        yield return new WaitForSeconds(0.3f);

        if (nextRoom != null)
        {
            if (nextRoom.trapLock && !nextRoom.previouslyTrapped)
            {
                nextRoom.GetComponent<Lock>().LockDoor();
                nextRoom.previouslyTrapped = true;
                nextRoom.trapActivated = true;
            }
            else if (nextRoom.oldManRoom)
            {
                if (!nextRoom.inRoom)
                {
                    block.transform.position = new Vector3(7, 5, 0);
                    nextRoom.inRoom = true;
                    GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(nextRoom.message.PlayText());
                    yield return new WaitForSeconds(4.0f);
                }
            }
            else if (oldManRoom && nextRoom.block != null)
            {
                nextRoom.doorLock.LockDoor();
            }
        }

        player.can_player_input = true;
        playerCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider>().enabled = true;
        inTransition = false;
    }

    private IEnumerator HideTextOverTime(UITypewriter message)
    {
        while (message.txt.alpha > 0)
        {
            message.txt.alpha -= 10;
            yield return new WaitForSeconds(0.1f);
        }

        message.txt.text = "";
    }
}
