using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelBehavior : MonoBehaviour
{

    //[SerializeField] private float accel;
    //[SerializeField] private float veloc;
    [SerializeField] private float WaitAtEndTime = 2.0f;
    [SerializeField] private float changeDirWaitTime = 1.0f;
    [SerializeField] private int changeDirNumTimes = 3;

    private Rigidbody gel_rb;
    private static Vector3[] directions;
    private int rand_dir;
    private Vector3 curr_dir;
    private Vector3 final_pos;
    private bool can_collide = true;

    // Start is called before the first frame update
    void Start()
    {

        gel_rb = GetComponent<Rigidbody>();

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        StartCoroutine(MoveStalfos());
    }


    public IEnumerator MoveStalfos()
    {
        //rand_dir = Random.Range(0, 8);
        //curr_dir = directions[rand_dir];

        for (int i = 0; i < changeDirNumTimes; ++i)
        {

            rand_dir = Random.Range(0, 4);
            curr_dir = directions[rand_dir];

            //transform.position *= 2;

            // if moving in x direction round y
            if (curr_dir.x != 0.0f && curr_dir.y == 0.0f)
            {

                transform.position = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                0);

            }
            // if moving in y direction round x
            else if (curr_dir.y != 0.0f && curr_dir.x == 0.0f)
            {
                transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                transform.position.y,
                0);
            }

            //transform.position /= 2;

            final_pos = transform.position + curr_dir;

            if(Physics.Raycast(transform.position, curr_dir, 0.6f))
            {
                --i;
                continue;
            }

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform, 
                transform.position, final_pos, 0.2f));
            //Debug.Log("velocity: " + gel_rb.velocity.ToString()); ;

            yield return new WaitForSeconds(Random.Range(0, changeDirWaitTime+1));
        }

        gel_rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(Random.Range(0, WaitAtEndTime+1));

        StartCoroutine(MoveStalfos());
    }

}
