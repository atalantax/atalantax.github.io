using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyDisplayer : MonoBehaviour
{

    public Inventory inventory;
    TextMeshProUGUI text_component;

    // Start is called before the first frame update
    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory != null && Invincibility.godMode)
        {
            text_component.text = "\u221E";
        }
        else if (inventory != null && text_component != null)
        {
            //Debug.Log("in KeyDisplayer " + inventory.GetKeys());
            text_component.text = inventory.GetKeys().ToString();
        }
    }
}
