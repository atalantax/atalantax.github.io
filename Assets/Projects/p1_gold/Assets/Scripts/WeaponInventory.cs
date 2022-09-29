using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public Sword sword;
    public Arrow arrow;
    public Boomerang boomerang;
    public Bomb bomb;

    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        //sword = GetComponent<Sword>();
        //arrow = GetComponent<Arrow>();
        //boomerang = GetComponent<Boomerang>();
        //bomb = GetComponent<Bomb>();

        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
