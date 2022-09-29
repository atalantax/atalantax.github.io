using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    //public Sprite icon;
    public GameObject[] hearts;
    public HasHealth hasHealth;
    public GameObject[] heartMasks;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }

        foreach (GameObject mask in heartMasks)
        {
            mask.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float numLives = hasHealth.GetNumLives();
        for (int i = 0; i < (int)numLives; i++)
        {
            hearts[i].SetActive(true);
        }

        for (int i = 3; i > (int)numLives; i--)
        {
            hearts[i - 1].SetActive(false);
        }
        
        foreach (GameObject mask in heartMasks)
        {
            mask.SetActive(false);
        }

        if (numLives % 1 != 0)
        {
            hearts[(int)numLives].SetActive(true);
            heartMasks[(int)numLives].SetActive(true);
        }
    }
}
