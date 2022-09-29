using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock2 : MonoBehaviour
{
    public Sprite lockedDoor;
    public Sprite unlockedDoor;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LockDoor()
    {
        spriteRenderer.sprite = lockedDoor;
    }

    public void UnlockDoor()
    {
        spriteRenderer.sprite = unlockedDoor;
    }
}
