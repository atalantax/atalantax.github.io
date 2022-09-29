using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour
{
    //public List<GameObject> enemies;
    public GameObject itemPrefab = null;
    public Vector3 spawnPos;
    public bool spawned;
    public AudioClip spawnSound;
    //public int roomNum;

    //private GameObject key;
    //private string roomString;

    // Start is called before the first frame update
    void Start()
    {
        //GetEnemies();
        spawned = false;
        //roomNum = PlayerController.currentRoom;
        //roomString = "Rm" + roomNum + "Enemies";
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (GameObject enemy in enemies)
        //{
        //    if (enemy == null)
        //    {
        //        enemies.Remove(enemy);
        //    }
        //}
    }

    public void Spawn()
    {
        //if (enemies.Count == 0 && !spawned)
        //{
        //    Debug.Log(gameObject);
        //    Debug.Log("all enemies killed, spawning key");
        //    spawned = true;
        //    Instantiate(
        //        itemPrefab, transform.position + spawnPos,
        //        Quaternion.identity, transform);

        //    AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position);
        //}

        //Instantiate(
        //        itemPrefab, transform.position + spawnPos,
        //        Quaternion.identity, transform);

        GameObject item = Instantiate(
                itemPrefab, transform.position, Quaternion.identity);
        item.transform.position = new Vector3(
            item.transform.position.x, item.transform.position.y, 0.0f);

        AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position);
    }

    //private void GetEnemies()
    //{
    //    enemies.Clear();
    //    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
    //    {
    //        if (enemy.transform.parent.name == roomString)
    //        {
    //            //Debug.Log(enemy.transform.parent.name);
    //            enemies.Add(enemy);
    //        }
    //    }
    //    //Debug.Log(enemies.Count);
    //}
}
