using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public Sword sword;
    //public Arrow arrow;

    public Dictionary<Weapon.WeaponType, string> weaponHotkeys = new();
    public List<string> hotkeys;

    //private List<Weapon> weapons;

    public int rupee_count;
    public int keyCount;
    public int bombs;


    private void Start()
    {
        hotkeys = new List<string> { "Z", "1", "2" };
        //hotkeys = new List<string> { "1", "2" };
        rupee_count = 0;
        keyCount = 0;
        bombs = 0;
        //sword = GetComponent<Sword>();
        weaponHotkeys[Weapon.WeaponType.Sword] = "X";
        //weaponHotkeys[Weapon.WeaponType.Boomerang] = "Z";
        //weaponHotkeys[Weapon.WeaponType.Arrow] = "Z";
        //weaponHotkeys[Weapon.WeaponType.Bomb] = "1";

        //AddWeapon(sword);
        //AddWeapon(Weapon.Sword);
    }

    private void Update()
    {

    }

    public void AddRupees(int num_rupees)
    {
        rupee_count += num_rupees;
    }

    public void AddKeys(int numKeys)
    {
        keyCount += numKeys;
    }

    public void AddBombs(int num)
    {
        bombs += num;

        if (bombs <= 0)
        {
            hotkeys.Add(weaponHotkeys[Weapon.WeaponType.Bomb]);
            weaponHotkeys.Remove(Weapon.WeaponType.Bomb);
            sortHotkeys();
        }
        else if (weaponHotkeys[Weapon.WeaponType.Bomb] == "")
        {
            weaponHotkeys[Weapon.WeaponType.Bomb] = hotkeys[0];
            hotkeys.Remove(hotkeys[0]);
        }
    }

    public int GetRupees()
    {
        return rupee_count;
    }

    public int GetKeys()
    {
        return keyCount;
    }

    public int GetBombs()
    {
        return bombs;
    }

    public void RemoveRupees(int num_rupees)
    {
        rupee_count -= num_rupees;
    }

    public void RemoveKeys(int numKeys)
    {
        keyCount -= numKeys;
    }

    public void RemoveBombs(int num)
    {
        bombs -= num;
    }

    public void RotateWeapons()
    {
        Weapon.WeaponType weapon1 = Weapon.WeaponType.Null;
        Weapon.WeaponType weapon2 = Weapon.WeaponType.Null;
        Weapon.WeaponType weapon3 = Weapon.WeaponType.Null;

        foreach (KeyValuePair<Weapon.WeaponType, string> weapon in weaponHotkeys)
        {
            if (weapon.Value == "Z")
                weapon1 = weapon.Key;
            else if (weapon.Value == "1")
                weapon2 = weapon.Key;
            else if (weapon.Value == "2")
                weapon3 = weapon.Key;
        }

        if (weaponHotkeys.Count == 4)
        {
            if (weapon1 != Weapon.WeaponType.Null)
                weaponHotkeys[weapon1] = "2";
            if (weapon2 != Weapon.WeaponType.Null)
                weaponHotkeys[weapon2] = "Z";
            if (weapon3 != Weapon.WeaponType.Null)
                weaponHotkeys[weapon3] = "1";
        }
        else if (weaponHotkeys.Count == 2) { }
        else
        {
            if (weapon1 != Weapon.WeaponType.Null)
                weaponHotkeys[weapon1] = "1";
            if (weapon2 != Weapon.WeaponType.Null)
                weaponHotkeys[weapon2] = "Z";
        }

    }

    // set weapon to primary or secondary
    //public void SetWeaponKey(Weapon.WeaponType typeIn, string key)
    //{
    //    // unequip current primary
    //    foreach (KeyValuePair<Weapon.WeaponType, string> weapon in weaponHotkeys)
    //    {
    //        if (weapon.Value == key)
    //            weaponHotkeys[weapon.Key] = "";
    //    }

    //    weaponHotkeys[typeIn] = key;
    //}

    public Weapon.WeaponType GetPrimaryWeapon()
    {
        foreach (KeyValuePair<Weapon.WeaponType, string> weapon in weaponHotkeys)
        {
            if (weapon.Value == "X")
                return weapon.Key;
        }

        return Weapon.WeaponType.Null;
    }

    public Weapon.WeaponType GetSecondaryWeapon()
    {
        foreach (KeyValuePair<Weapon.WeaponType, string> weapon in weaponHotkeys)
        {
            if (weapon.Value == "Z")
                return weapon.Key;
        }

        return Weapon.WeaponType.Null;
    }

    public void sortHotkeys()
    {
        List<string> sorted = new List<string> { };

        if (hotkeys.Contains("Z"))
            sorted.Add("Z");

        if (hotkeys.Contains("1"))
            sorted.Add("1");

        if (hotkeys.Contains("2"))
            sorted.Add("2");

        hotkeys.Clear();
        hotkeys = sorted;
    }
}
