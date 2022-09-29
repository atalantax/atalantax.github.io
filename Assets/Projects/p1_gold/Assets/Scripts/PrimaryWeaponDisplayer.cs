using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimaryWeaponDisplayer : MonoBehaviour
{
    public Inventory inventory;

    [Tooltip("in order of: sword, arrow, boomerang, bomb")]
    public List<Sprite> sprites;

    private Weapon.WeaponType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        type = inventory.GetPrimaryWeapon();
        switch (type)
        {
            case Weapon.WeaponType.Sword:
                GetComponent<Image>().sprite = sprites[0];
                break;
            case Weapon.WeaponType.Arrow:
                GetComponent<Image>().sprite = sprites[1];
                break;
            case Weapon.WeaponType.Boomerang:
                GetComponent<Image>().sprite = sprites[2];
                break;
            case Weapon.WeaponType.Bomb:
                GetComponent<Image>().sprite = sprites[3];
                break;
            case Weapon.WeaponType.Null:
                GetComponent<Image>().sprite = null;
                break;
        }
    }
}
