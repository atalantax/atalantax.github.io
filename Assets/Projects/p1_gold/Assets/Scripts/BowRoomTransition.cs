using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowRoomTransition : MonoBehaviour
{
    public Image darkPanel;
    public Vector3 playerPos;
    public Vector3 cameraPos;

    public PushableBlock block;
    public PlayerController player;

    public static Vector3 prevCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        darkPanel.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.inBowRoom = true;
            player.GetComponent<HasHealth>().hasHealth = false;
            prevCameraPos = Camera.main.transform.position;
            StartCoroutine(RoomTransition());
        }
    }

    public IEnumerator LightenScreen()
    {

        int opacity = 100;
        while (darkPanel.color.a > 0)
        {
            opacity -= 10;
            darkPanel.color = new Color(0, 0, 0, opacity);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator DarkenScreen()
    {
        Debug.Log("darkening screen");
        int opacity = 0;
        while (darkPanel.color.a < 100)
        {
            opacity += 10;
            darkPanel.color = new Color(0, 0, 0, opacity);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RoomTransition()
    {
        yield return StartCoroutine(DarkenScreen());
        //player.GetComponent<ArrowKeyMovement>().enabled = false;

        // reset room state
        block.ResetPosition();

        player.transform.position = playerPos;
        player.can_player_input = false;
        Camera.main.transform.position = cameraPos;

        yield return StartCoroutine(LightenScreen());

        if (!Invincibility.godMode)
            player.GetComponent<HasHealth>().hasHealth = true;
        player.can_player_input = true;
    }
}
