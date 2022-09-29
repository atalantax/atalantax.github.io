using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBowRoom : MonoBehaviour
{
    public BowRoomTransition transition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            StartCoroutine(LeaveRoom());
    }

    private IEnumerator LeaveRoom()
    {
        transition.player.can_player_input = false;
        yield return StartCoroutine(transition.DarkenScreen());

        transition.player.transform.position = new Vector3(-17, 51, 0);
        Camera.main.transform.position = BowRoomTransition.prevCameraPos;

        yield return StartCoroutine(transition.LightenScreen());
        PlayerController.inBowRoom = false;
        transition.player.can_player_input = true;
    }
}
