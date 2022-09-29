using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public Animator playerAnimator;
    public AudioClip secretSound;

    public float timeNeeded;
    public float pushDistance;

    public Lock doorLock;

    public bool bowRoom;

    private Vector3 prevDirection;

    [SerializeField] private Vector3 initialPos;
    private float initialTime;
    private float timePushed;

    private bool pushed;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        timePushed = 0;
        pushed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPosition()
    {
        transform.localPosition = initialPos;
        pushed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SetDirection();
        if (collision.gameObject.CompareTag("Player"))
            initialTime = Time.time;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("collision");
            if (timePushed < timeNeeded)
                timePushed = Time.time - initialTime;
            else if (!pushed)
            {
                pushed = true;
                StartCoroutine(PushBlock());
            }
        }
    }

    private IEnumerator PushBlock()
    {
        if ((bowRoom && prevDirection == Vector3.up) || !bowRoom)
        {
            Vector3 dest = transform.position + prevDirection * pushDistance;
            Vector3 initialPos = transform.position;
            yield return StartCoroutine(
                CoroutineUtilities.MoveObjectOverTime(transform, initialPos, dest, timeNeeded)
            );

            if (!bowRoom)
            {
                doorLock.UnlockDoor();
            }
            AudioSource.PlayClipAtPoint(secretSound, Camera.main.transform.position);
        }
    }

    // returns vector 3 in the direction that link is facing
    private void SetDirection()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo);

        if (stateInfo.IsName("run_down"))
        {
            prevDirection = Vector3.down;
        }
        else if (stateInfo.IsName("run_up"))
        {
            prevDirection = Vector3.up;
        }
        else if (stateInfo.IsName("run_right"))
        {
            prevDirection = Vector3.right;
        }
        else
        {
            prevDirection = Vector3.left;
        }
    }
}
