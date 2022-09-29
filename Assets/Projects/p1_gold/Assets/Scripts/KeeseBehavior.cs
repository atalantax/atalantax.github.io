using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseBehavior : MonoBehaviour
{
    [SerializeField] private float accel;
    [SerializeField] private float veloc;
    [SerializeField] private float WaitAtEndTime = 3.0f;
    [SerializeField] private float change_dir_wait_time = 1.0f;
    [SerializeField] private int change_dir_num_times = 10;

    private Rigidbody keese_rb;
    private static Vector3[] directions;
    private int rand_dir;
    private Vector3 curr_dir;
    private bool can_collide = true;

    // Start is called before the first frame update
    void Start()
    {

        keese_rb = GetComponent<Rigidbody>();

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            new Vector3(1, 1, 0), // upright
            new Vector3(-1, -1, 0), // downleft
            new Vector3(-1, 1, 0), // upleft
            new Vector3(1, -1, 0) // downright
        };

        StartCoroutine(MoveKeese());
    }

    public IEnumerator MoveKeese()
    {
        rand_dir = Random.Range(0, 8);
        curr_dir = directions[rand_dir];

        while (keese_rb.velocity.magnitude < veloc)
        {
            keese_rb.AddForce(curr_dir.normalized * accel, ForceMode.Acceleration);

            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < change_dir_num_times; ++i)
        {
            rand_dir = Random.Range(0, 8);
            curr_dir = directions[rand_dir];

            keese_rb.velocity = curr_dir.normalized * veloc;

            yield return new WaitForSeconds(Random.Range(0, change_dir_wait_time+1));
        }

        
        //Quaternion rotation = Quaternion.LookRotation(keese_rb.velocity, Vector3.up);

        while (keese_rb.velocity.magnitude >= 0.01)
        {
            //Quaternion curr_rotation = Quaternion.LookRotation(keese_rb.velocity, Vector3.up);

            //if (rotation != curr_rotation)
            //    keese_rb.velocity = Vector3.zero;
            //else
            //{
                curr_dir = -keese_rb.velocity;
                keese_rb.AddForce(curr_dir.normalized * accel,
                ForceMode.Acceleration);
            //}

            yield return new WaitForFixedUpdate();
        }

        keese_rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(Random.Range(0, WaitAtEndTime+1));

        StartCoroutine(MoveKeese());
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("wall") && can_collide)
        {

            StartCoroutine(WaitToCollide());

            //Debug.Log("velocity1: " + keese_rb.velocity.ToString());
            keese_rb.velocity = -keese_rb.velocity;
            //Debug.Log("velocity2: " + keese_rb.velocity.ToString());
            curr_dir = -curr_dir.normalized;
        }
    }

    public IEnumerator WaitToCollide()
    {
        can_collide = false;

        for (int i = 0; i < 3; ++i)
        {
            yield return new WaitForFixedUpdate();
        }

        can_collide = true;
    }
}
