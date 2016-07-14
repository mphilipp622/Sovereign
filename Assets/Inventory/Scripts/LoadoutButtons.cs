using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class LoadoutButtons : MonoBehaviour {
	
	public int GunType = 0;
	public int minGunType= 0;
	public int maxGunType = 2;
	public Text Gun1Text = null;
	public GameObject M4A1 = null;
	public GameObject Shotgun = null;
	public Transform player = null;
    private bool hasprimary = false;
    private vp_PlayerInventory m_Inventory = null;
    private vp_FPPlayerEventHandler Ehandler = null;
    private vp_WeaponHandler Whandler = null;
    private int T_Pweapons = 0;
    private List<vp_ItemType> PW_inventory = new List<vp_ItemType>(); // available secondary weapons in the inventory

    public void NextGun() {
		if (GunType < maxGunType) {
			GunType = GunType +1;
            currentprimary(GunType);

        }
    }
	
	public void PreviousGun() {
		if (GunType > minGunType) {
			GunType = GunType - 1;
            currentprimary(GunType);

        }
    }
    public void searchforprimary()
    {
        for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
        {
            //      Debug.Log("m_Inventory.m_ItemCapInstances[" + i +"].Type.name== " + m_Inventory.m_ItemCapInstances[i].Type.name);
            if (m_Inventory.m_ItemCapInstances[i].Type.name == "AssaultRifle01")
            {
                //Calculating total available secondary weapons and Then Calculate how many of each secondary
                // weapon there is in the inventory
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;
                //if the item exists then update available secondary weapons in the inventory
                Debug.Log("1 Does SW_inventory contain AssaultRifle==" + PW_inventory.Contains(thisweapon));
                Debug.Log("How many  AssaultRifle in inventory==" + m_Inventory.GetItemCount(thisweapon));

                if (m_Inventory.GetItemCount(thisweapon) > 0 && PW_inventory.Contains(thisweapon) == false)
                {
                    //    maxGunType++;
                    Gun1Text.text = m_Inventory.m_ItemCapInstances[i].Type.name;

                    //if item exists in the inventory but not yet in the SW_invenotry increment total available
                    //secondary weapon and add too list
                    T_Pweapons++;
                    Debug.Log("Number of" + m_Inventory.m_ItemCapInstances[i].Type.name + "==" + m_Inventory.GetItemCount(thisweapon));
                    PW_inventory.Add(thisweapon);
                    Debug.Log(" 2 Does SW_inventory contain AssaultRifle==" + PW_inventory.Contains(thisweapon));
                    Debug.Log("PW_inventory ===" + PW_inventory.Capacity);
                    //   break;

                }
         


            }
            if (m_Inventory.m_ItemCapInstances[i].Type.name == "Shotgun01")
            {
                //Calculating total available secondary weapons and Then Calculate how many of each secondary
                // weapon there is in the inventory
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;
                //if the item exists then update available secondary weapons in the inventory
                Debug.Log("1 Does SW_inventory contain Shotgun01==" + PW_inventory.Contains(thisweapon));
                Debug.Log("How many  Shotgun01 in inventory==" + m_Inventory.GetItemCount(thisweapon));

                if (m_Inventory.GetItemCount(thisweapon) > 0 && PW_inventory.Contains(thisweapon) == false)
                {
                    //    maxGunType++;
                    Gun1Text.text = m_Inventory.m_ItemCapInstances[i].Type.name;

                    //if item exists in the inventory but not yet in the SW_invenotry increment total available
                    //secondary weapon and add too list
                    T_Pweapons++;
                    Debug.Log("Number of" + m_Inventory.m_ItemCapInstances[i].Type.name + "==" + m_Inventory.GetItemCount(thisweapon));
                    PW_inventory.Add(thisweapon);
                    Debug.Log(" 2 Does SW_inventory contain Shotgun01==" + PW_inventory.Contains(thisweapon));
                    Debug.Log("PW_inventory ===" + PW_inventory.Capacity);
                    //   break;

                }



            }

        }
        Debug.Log("Primary T_Pweapons===" + T_Pweapons);

    }

    public vp_ItemType searchSWlist(string weaponame)
    {
        vp_ItemType weapon = null;
        for (int i = 0; i < PW_inventory.Count; i++)
        {
            if (PW_inventory[i].name == weaponame)
            {
                weapon = PW_inventory[i];
            }

        }
        return weapon;
    }
    public bool PrimaryExist(string weaponame)
    {
        for (int i = 0; i < PW_inventory.Count; i++)
        {
            if (PW_inventory[i].name == weaponame)
            {
                return true;
            }

        }
        return false;
    }


    public int changeweapon(){
        Debug.Log("Current: GUN ID==" + GunType + "GunTYpe"+ GunType);

        if (GunType == 0)
        {

            Debug.Log("No primary Weapon Selected");

            //herrrrrrrrrrrrrrrrrrrrrrwweeeeeeeeeeeeeeeeee
            //Whandler.SetWeapon(1);
           
         //   Ehandler.SetWeapon.TryStart(0);

            return 0;

        }

        if (GunType == 1)
        {
            string weaponName = "AssaultRifle01";
            for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
            {
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;

                if (m_Inventory.m_ItemCapInstances[i].Type.name == weaponName)
                {
                    if (PW_inventory.Contains(thisweapon) && m_Inventory.GetItemCount(thisweapon)==1)
                    {
                        Debug.Log("AssaultRifle EXISTS IN LIST AND SETTING AssaultRifle");


                        //Whandler.SetWeapon(1);
                    //    Ehandler.SetWeapon.TryStart(0);
                     //   Ehandler.SetWeapon.TryStart(2);
                        return 2;

                    }
                }

            }

        }
        if (GunType == 2)
        {
            string weaponName = "Shotgun01";
            for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
            {
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;

                if (m_Inventory.m_ItemCapInstances[i].Type.name == weaponName)
                {
                    if (PW_inventory.Contains(thisweapon) && m_Inventory.GetItemCount(thisweapon) == 1)
                    {
                        Debug.Log("Shotgun EXISTS IN LIST AND SETTING Shotgun");

                        //Whandler.SetWeapon(1);
                       // Ehandler.SetWeapon.TryStart(0);

                    //    Ehandler.SetWeapon.TryStart(3);
                        return 3;
                    }
                }

            }

        }
        return 0;
    }
    public void currentprimary(int GunType)
    {
        if (GunType == 1)
        {
            if (searchSWlist("AssaultRifle01") != null)
                Gun1Text.text = searchSWlist("AssaultRifle01").name;
            //Debug.Log ("Changed Text to Knife");
            changeweapon();
        }
        if (GunType == 2)
        {
            if (searchSWlist("Shotgun01") != null)
                Gun1Text.text = searchSWlist("Shotgun01").name;
            //Debug.Log ("Changed Text to Knife");
            changeweapon();
        }
        if (GunType == 0)
        {
            if (searchSWlist("None") == null)
                Gun1Text.text = "None";
            //Debug.Log ("Changed Text to Knife");
            changeweapon();
        }
        //Debug.Log ("Changed Text to None");


    }

    void Awake()
    {
        m_Inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_PlayerInventory>();
        Ehandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPPlayerEventHandler>();
        Whandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_WeaponHandler>();
        //How many secondary weapons exist?  maxGunType

        Ehandler.SetWeapon.TryStart(0);
        searchforprimary();
        currentprimary(0);

    }
    void Update (){
        searchforprimary();

        maxGunType = PW_inventory.Count;

        //     Debug.Log("SW_inventory[2] " + SW_inventory[2]);


    }
}
