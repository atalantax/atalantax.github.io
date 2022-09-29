using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusBehavior : MonoBehaviour
{

    [SerializeField] private GameObject fireball;
    [SerializeField] private AudioClip screamSound;
    [SerializeField] private float screamWait;
    //private GameObject[] fireballList = new GameObject[3];
    private List<GameObject> fireballList = new List<GameObject>();

    [SerializeField] private float accel;
    [SerializeField] private float veloc = 3;
    [SerializeField] private float fire_veloc = 5;
    [SerializeField] private float WaitAtEndTime = 3.0f;
    [SerializeField] private float changeDirWaitTime = 4.0f;
    [SerializeField] private int changeDirNumTimes = 10;

    private Rigidbody aqua_rb;
    private static Vector3[] directions;
    private static Vector3[] fire_dirs = new Vector3[3];
    private int rand_dir;
    private Vector3 curr_dir;
    private bool can_collide = true;

    // Start is called before the first frame update
    void Start()
    {
        fire_dirs[0] = new Vector3(-1, 0.5f, 0);
        fire_dirs[1] = new Vector3(-1, 0, 0);
        fire_dirs[2] = new Vector3(-1, -0.5f, 0);

        aqua_rb = GetComponent<Rigidbody>();

        directions = new Vector3[]
        {
            Vector3.left,
            Vector3.right,
        };

        StartCoroutine(MoveAquamentus());
        //StartCoroutine(Fireball());
    }

    // Update is called once per frame
    void Update()
    {

        if (fireballList.Count > 0)
        {
            //Debug.Log("fireball list not empty");

            // keep shooting fireballs
        }
        else if (fireballList.Count <= 0)
        {
            //Debug.Log("fireball list empty");
            

            for (int i = 0; i < 3; ++i)
            {
                fireballList.Add(Instantiate(fireball, 
                    transform.position + new Vector3(-1, 0.5f, 0), 
                    Quaternion.identity));
                StartCoroutine( Fireball(fireballList[i], 
                    fireballList[i].GetComponent<Rigidbody>(),
                    fireballList[i].GetComponent<FireballBehavior>(),
                    fire_dirs[i]));
            }
        }
    }

    private IEnumerator MoveAquamentus()
    {

        for (int i = 0; i < changeDirNumTimes; ++i)
        {

            rand_dir = Random.Range(0, 2);
            curr_dir = directions[rand_dir];

            if (transform.position.x < 33)
            {
                curr_dir = directions[1];
            }
            else if (transform.position.x > 36.5)
            {
                curr_dir = directions[0];
            }

            //transform.position = new Vector3(
            //    transform.position.x,
            //    Mathf.Round(transform.position.y),
            //    0);

            aqua_rb.velocity = curr_dir.normalized * veloc;

            yield return new WaitForSeconds(changeDirWaitTime);
        }

        aqua_rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(WaitAtEndTime);

        Debug.Log("aquamentus moving");

        StartCoroutine(MoveAquamentus());
    }

    private IEnumerator Fireball(GameObject fireball, Rigidbody fire_rb, 
        FireballBehavior fire, Vector3 direction)
    {
        Debug.Log("fireball start");
        //AudioSource.PlayClipAtPoint(screamSound, Camera.main.transform.position);
        fire_rb.velocity = direction.normalized * fire_veloc;

        while (!fire.destroyMe)
        {
            Debug.Log("fireball out");

            yield return null;
        }


        fireballList.Remove(fireball);
        Destroy(fireball);
        Debug.Log("fireball destroyed");

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("right_door"))
    //    {
    //        curr_dir = -curr_dir.normalized;
    //        aqua_rb.velocity = -aqua_rb.velocity;
    //    }
    //}
}
