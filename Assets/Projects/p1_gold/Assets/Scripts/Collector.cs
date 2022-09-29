using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public AudioClip rupeeCollectionSound;
    public AudioClip keyCollectionSound;
    public AudioClip bombCollectionSound;
    public AudioClip heartCollectionSound;
    public AudioClip itemCollectionSound;

    public Inventory inventory;
    public HasHealth hasHealth;

    public Sprite getBowSprite;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        hasHealth = GetComponent<HasHealth>();
        // try to grab a ref to the inventory component on this gameobject
        //inventory = GetComponent<inventory>();
        //if (inventory == null)
        //{
        //    Debug.LogWarning("WARNING: GameObject with a collector has no inventory to store things in!");

        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject object_collided_with = other.gameObject;

        if (other.gameObject.CompareTag("rupee"))
        {
            // check to see if our inventory exists before we add a rupee to it
            if (inventory != null)
            {
                int num = other.gameObject.GetComponent<RupeeValue>().value;
                inventory.AddRupees(num);
            }
            Destroy(other.gameObject);

            // play sound effect
            AudioSource.PlayClipAtPoint(rupeeCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("key"))
        {
            if (inventory != null)
                inventory.AddKeys(1);
            //Debug.Log("num keys" + inventory.GetKeys());
            Destroy(other.gameObject);
            //Debug.Log("num keys" + inventory.GetKeys());

            AudioSource.PlayClipAtPoint(keyCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("bomb"))
        {
            if (inventory != null)
                inventory.AddBombs(1);
            Destroy(other.gameObject);

            AudioSource.PlayClipAtPoint(bombCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("heart"))
        {
            if (hasHealth != null && hasHealth.GetNumLives() < hasHealth.maxHealth)
                hasHealth.GainLife(0.5f);
            Destroy(other.gameObject);

            AudioSource.PlayClipAtPoint(heartCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("boomerang"))
        {
            if (inventory != null)
            {
                inventory.weaponHotkeys[Weapon.WeaponType.Boomerang] = inventory.hotkeys[0];
                inventory.hotkeys.Remove(inventory.hotkeys[0]);
            }

            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(itemCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("bow"))
        {
            if (inventory != null)
            {
                inventory.weaponHotkeys[Weapon.WeaponType.Arrow] = inventory.hotkeys[0];
                inventory.hotkeys.Remove(inventory.hotkeys[0]);
            }

            //Destroy(other.gameObject);
            StartCoroutine(WaitForBow(other.gameObject));
            AudioSource.PlayClipAtPoint(itemCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("item"))
        {
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(itemCollectionSound, Camera.main.transform.position);
        }
        else if (other.gameObject.CompareTag("heart_container"))
        {
            GetComponent<HasHealth>().IncreaseMaxLife(1);
            StartCoroutine(WaitForBow(other.gameObject));
            AudioSource.PlayClipAtPoint(itemCollectionSound, Camera.main.transform.position);
        }
    }

    private IEnumerator WaitForBow(GameObject obj)
    {
        PlayerController player = GetComponent<PlayerController>();
        player.can_player_input = false;

        GetComponent<Animator>().SetTrigger("pickUpBow");

        StartCoroutine(
            CoroutineUtilities.MoveObjectOverTime(
                player.transform, player.transform.position,
                player.transform.position + Vector3.left * 0.5f, 0.1f)
        );

        //SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        //Sprite prevSprite = playerRenderer.sprite;
        //playerRenderer.sprite = getBowSprite;
        obj.transform.position += Vector3.up;

        yield return new WaitForSeconds(itemCollectionSound.length);

        //playerRenderer.sprite = prevSprite;
        player.can_player_input = true;
        Destroy(obj);
    }
}
