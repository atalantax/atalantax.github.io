using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lock : MonoBehaviour
{
    public bool puzzleLock;
    public List<GameObject> enemies;

    public Sprite lockedDoor;
    public Sprite unlockedDoor;

    public RoomController.Direction direction;
    public GameObject door2;
    //[Tooltip("for puzzle/trap doors: 1 for left/top half and 2 for right/bottom.")]
    //public int doorNum;

    //[Tooltip("The direction the room transition will go toward.")]
    //public Direction roomDirection;

    [SerializeField]
    private Inventory inventory;
    //private PlayerController player;

    private bool isLocked;
    [SerializeField]
    private AudioClip lockSound;
    private SpriteRenderer spriteRenderer;

    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //enemies = GetComponent<SpawnKey>().enemies;
        inventory = GetComponent<RoomController>().inventory;

        lockSound = GetComponent<RoomController>().lockSound;
        isLocked = GetComponent<RoomController>().isLocked;

        direction = GetComponent<RoomController>().roomDirection;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
            }
        }
        //enemies = GetComponent<SpawnKey>().enemies;

        isLocked = GetComponent<RoomController>().isLocked;
        if (GetComponent<RoomController>().trapActivated
            && enemies.Count == 0 && isLocked)
        {
            UnlockDoor();
        }
    }

    public void CollidedWithDoor(Collider other)
    {
        if (direction == RoomController.Direction.Down)
            return;

        GameObject objectCollidedWith = other.gameObject;
        //Debug.Log(objectCollidedWith.name);

        //Debug.Log(isLocked);
        if (objectCollidedWith.CompareTag("Player") && isLocked)
        {
            if (!puzzleLock)
            {
                if (!Invincibility.godMode && inventory.GetKeys() >= 1)
                {
                    isLocked = false;
                    inventory.AddKeys(-1);
                }
                else if (Invincibility.godMode)
                    isLocked = false;

                if (!isLocked)
                {
                    UnlockDoor();
                }
            }
            else if (GetComponent<RoomController>().trapLock && enemies.Count == 0)
            {
                UnlockDoor();
            }
        }

        //StartCoroutine(DoorUnlockAnimation());
    }

    public void LockDoor()
    {
        AudioSource.PlayClipAtPoint(lockSound, Camera.main.transform.position);

        spriteRenderer.sprite = lockedDoor;
        GetComponent<RoomController>().isLocked = true;

        if (direction == RoomController.Direction.Up)
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, 0, collider.center.z);
            door2.GetComponent<Lock2>().LockDoor();
        }
    }

    public void UnlockDoor()
    {
        Debug.Log("unlocking door");

        if (!isPlaying)
        {
            AudioSource.PlayClipAtPoint(lockSound, Camera.main.transform.position);
            isPlaying = true;
        }

        spriteRenderer.sprite = unlockedDoor;
        GetComponent<RoomController>().isLocked = false;

        if (direction == RoomController.Direction.Up)
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, 0.5f, 0f);
            Debug.Log(collider.center);
            door2.GetComponent<Lock2>().UnlockDoor();
        }
        else if (direction == RoomController.Direction.Left && puzzleLock)
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(-0.75f, collider.center.y, 0f);
        }
    }
}
