using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterBehavior : MonoBehaviour
{
    [SerializeField] private int direction_num;
    private static Vector3[] directions;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject player;
    private Vector3 direction;
    private Vector3 original;
    private float prev_time;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.enabled = false;

        directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        direction = directions[direction_num];
        prev_time = Time.time;
        original = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitWall;
        bool is_something = Physics.Raycast(
            transform.position, direction,
            out hitWall, 1.5f);

        if (is_something)
        {
            sprite.enabled = true;
            transform.position = Vector3.MoveTowards(
                transform.position, transform.position + direction,
                0.5f * (Time.time - prev_time));
        }
        else if (transform.position == original)
        {
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;
            transform.position = Vector3.MoveTowards(
                transform.position, original,
                0.5f * (Time.time - prev_time));
        }
            


        prev_time = Time.time;
    }

    private void OnTriggerEnter(Collider collision)
    {
        sprite.enabled = true;

        if (collision.gameObject.CompareTag("Player"))
        {

            player = collision.gameObject;
            collision.gameObject.transform.position = transform.position;
            animator.SetTrigger("playerCollision");
            StartCoroutine(Wait());
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(ResettoBeginning());
        
    }

    public IEnumerator ResettoBeginning()
    {
        //Camera.main.enabled = false;
        //Camera.main.transform.position = new Vector3(0.5f, 0f, 10.0f);
        Camera.main.transform.position = new Vector3(0.5f, 0f, 10.0f);
        
        player.transform.position = new Vector3(0.5f, -5.0f, 0.0f);
        yield return new WaitForSeconds(2.0f);
        Camera.main.transform.position = new Vector3(0.5f, 0f, -10.0f);
        //Camera.main.enabled = true;

    }
}
