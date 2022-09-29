using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public WeaponType type;
    public float timer;
    public float range;

    public AudioClip sound;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        type = WeaponType.Bomb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
