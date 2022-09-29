using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public static bool godMode;
    public bool godModePublic;
    public HasHealth health;

    // Start is called before the first frame update
    void Start()
    {
        godMode = false;
        health = GetComponent<HasHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //godModePublic = godMode;
        //if (!godMode && Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    godMode = true;
        //    health.hasHealth = false;
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    godMode = false;
        //    health.hasHealth = true;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            godMode = !godMode;

            if (godMode)
                health.hasHealth = true;
            else
                health.hasHealth = false;
        }
    }
}
