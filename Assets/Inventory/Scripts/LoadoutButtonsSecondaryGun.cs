using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class LoadoutButtonsSecondaryGun : MonoBehaviour {
	
	public int SecondaryGunType = 0;
	public int minGunType= 0;
	public int maxGunType = 0;
	public Text Gun2Text = null;
	public GameObject PickUpPistolPrefab;
	public GameObject PickUpKnifePrefab;
    public GameObject PickUpMacePrefab;
    private vp_PlayerInventory m_Inventory = null;
    private vp_FPPlayerEventHandler Ehandler = null;
    private vp_WeaponHandler Whandler = null;
    private int T_sweapons = 0;
    private List<vp_ItemType> SW_inventory = new List<vp_ItemType>(); // available secondary weapons in the inventory
    public void NextSecondaryGun() {

		if (SecondaryGunType <= maxGunType) {
			SecondaryGunType = SecondaryGunType +1;
            Debug.Log ("NextSecondaryGun=="+ SecondaryGunType);
            currentsecondary(SecondaryGunType);

        }
    }
	
	public void PreviousSecondaryGun() {
		if (SecondaryGunType > minGunType) {
			SecondaryGunType = SecondaryGunType - 1;
            Debug.Log("PreviousSecondaryGun== " + SecondaryGunType);
            currentsecondary(SecondaryGunType);

        }
    }

    //USED to CALCULATE  TOTAL AVAILABLE SECONDARY WEAPONS, HOW MANY SECONDARY WEAPONS IN INVENTORY
    //POPULATES THE AVAILABLE SECONDARY WEAPONS LIST
    public void searchforsecondary()
    {
        for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
        {
      //      Debug.Log("m_Inventory.m_ItemCapInstances[" + i +"].Type.name== " + m_Inventory.m_ItemCapInstances[i].Type.name);
            if (m_Inventory.m_ItemCapInstances[i].Type.name == "Pistol")
            {
                //Calculating total available secondary weapons and Then Calculate how many of each secondary
               // weapon there is in the inventory
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;
                //if the item exists then update available secondary weapons in the inventory
                Debug.Log("1 Does SW_inventory contain Pistol==" + SW_inventory.Contains(thisweapon));
                Debug.Log("How many  pistols in inventory==" + m_Inventory.GetItemCount(thisweapon));
                
                if (m_Inventory.GetItemCount(thisweapon) > 0 && SW_inventory.Contains(thisweapon)==false)
                {
                //    maxGunType++;
                    Gun2Text.text = m_Inventory.m_ItemCapInstances[i].Type.name;

                    //if item exists in the inventory but not yet in the SW_invenotry increment total available
                    //secondary weapon and add too list
                    T_sweapons++;
                    Debug.Log("Number of" + m_Inventory.m_ItemCapInstances[i].Type.name + "==" + m_Inventory.GetItemCount(thisweapon));
                    SW_inventory.Add(thisweapon);
                    Debug.Log(" 2 Does SW_inventory contain pistol==" + SW_inventory.Contains(thisweapon));
                    Debug.Log("SW_inventory ===" + SW_inventory.Capacity);
                 //   break;

                }
                else if (m_Inventory.GetItemCount(thisweapon) < 1 && SW_inventory.Contains(thisweapon) == true)
                {
                    m_Inventory.TryGiveItem(thisweapon, 0);

                }
          

            }
            if (m_Inventory.m_ItemCapInstances[i].Type.name == "Mace")
            {
                //Calculating total available secondary weapons and Then Calculate how many of each secondary
                // weapon there is in the inventory
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;
                //if the item exists then update available secondary weapons in the inventory
                Debug.Log("1 Does SW_inventory contain Mace==" + SW_inventory.Contains(thisweapon));

                if (m_Inventory.GetItemCount(thisweapon) > 0 && false == SW_inventory.Contains(thisweapon))
                {
                 //   maxGunType++;

                    //if item exists in the inventory but not yet in the SW_invenotry increment total available
                    //secondary weapon and add too list
                    T_sweapons++;
                    Debug.Log("Number of" + m_Inventory.m_ItemCapInstances[i].Type.name + "==" + m_Inventory.GetItemCount(thisweapon));
                    SW_inventory.Add(thisweapon);
                    Debug.Log(" 2 Does SW_inventory contain Mace==" + SW_inventory.Contains(thisweapon));
                    Debug.Log("SW_inventory ===" + SW_inventory.Capacity);

                }

            }
            if (m_Inventory.m_ItemCapInstances[i].Type.name == "Knife")
            {
                //Calculating total available secondary weapons and Then Calculate how many of each secondary
                // weapon there is in the inventory
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;
                //if the item exists then update available secondary weapons in the inventory
                Debug.Log("1 Does SW_inventory contain Knife==" + SW_inventory.Contains(thisweapon));

                if (m_Inventory.GetItemCount(thisweapon) > 0 && false == SW_inventory.Contains(thisweapon))
                {
                  //  maxGunType++;

                    //if item exists in the inventory but not yet in the SW_invenotry increment total available
                    //secondary weapon and add too list
                    T_sweapons++;
                    Debug.Log("Number of" + m_Inventory.m_ItemCapInstances[i].Type.name + "==" + m_Inventory.GetItemCount(thisweapon));
                    SW_inventory.Add(thisweapon);
                    Debug.Log(" 2 Does SW_inventory contain Knife==" + SW_inventory.Contains(thisweapon));
                    Debug.Log("SW_inventory ===" + SW_inventory.Capacity);

                }

            }
        }
        Debug.Log("T_sweapons===" + T_sweapons);

    }
    public vp_ItemType searchSWlist(string weaponame)
    {
        vp_ItemType weapon = null;
        for(int i=0; i<SW_inventory.Count; i++)
        {
            if (SW_inventory[i].name == weaponame) {
                weapon = SW_inventory[i];
          }

        }
        return weapon;
            }
    public bool SecondaryExist(string weaponame)
    {
        for (int i = 0; i < SW_inventory.Count; i++)
        {
            if (SW_inventory[i].name == weaponame)
            {
                return true;
            }

        }
        return false;
    }
 
    public int changeweapon() {
        if (SecondaryGunType == 0)
        {
  
                        Debug.Log("No Secondary Weapon Selected");

            //herrrrrrrrrrrrrrrrrrrrrrwweeeeeeeeeeeeeeeeee
            //Whandler.SetWeapon(1);
            Ehandler.SetWeapon.TryStart(0);


        }

        if (SecondaryGunType == 1 ) {
            string weaponName = "Knife";
            for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
            {
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;

                if (m_Inventory.m_ItemCapInstances[i].Type.name == weaponName)
                {
                    if (SW_inventory.Contains(thisweapon))
                    {
                        Debug.Log("Knife EXISTS IN LIST AND SETTING Knife");


                        //Whandler.SetWeapon(1);
                        //Ehandler.SetWeapon.TryStart(5);
                        return 5;
                    }
                }

            }

		}
        if (SecondaryGunType == 2)
        {
            string weaponName = "Pistol";
            for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
            {
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;

                if (m_Inventory.m_ItemCapInstances[i].Type.name == weaponName)
                {
                    if (SW_inventory.Contains(thisweapon))
                    {
                        Debug.Log("Pistol EXISTS IN LIST AND SETTING Pistol");


                        //       Whandler.SetWeapon(1);
                        // Ehandler.SetWeapon.TryStart(1);
                        return 1;
                    }
                }

            }

        }
     
        if (SecondaryGunType == 3)
        {
            string weaponName = "Mace";
            for (int i = 0; i < m_Inventory.m_ItemCapInstances.Count; i++)
            {
                vp_ItemType thisweapon = m_Inventory.m_ItemCapInstances[i].Type;

                if (m_Inventory.m_ItemCapInstances[i].Type.name == weaponName)
                {
                  if (SW_inventory.Contains(thisweapon))
                    {
                        Debug.Log("Mace EXISTS IN LIST AND SETTING Mace");

                        //herrrrrrrrrrrrrrrrrrrrrrwweeeeeeeeeeeeeeeeee
                        //Whandler.SetWeapon(1);
                        //Ehandler.SetWeapon.TryStart(6);
                        return 6;
                    }
                }
            }
        }
        return 0;
    }
    public void currentsecondary(int SecondaryGunType)
    {
        if (SecondaryGunType == 1)
        {
        if (searchSWlist("Knife") != null)
                Gun2Text.text = searchSWlist("Knife").name;
            //Debug.Log ("Changed Text to Knife");
            changeweapon();
        }
        if (SecondaryGunType == 0)
        {
            if (searchSWlist("None") == null)
                Gun2Text.text = "None";
            //Debug.Log ("Changed Text to Knife");
            changeweapon();
        }
        //Debug.Log ("Changed Text to None");

        if (SecondaryGunType == 2)
        {
       if (searchSWlist("Pistol") != null)
                Gun2Text.text = searchSWlist("Pistol").name;
            //Debug.Log ("Changed Text to Pistol");
            changeweapon();
        }
        if (SecondaryGunType == 3)
        {
             if (searchSWlist("Mace") != null)
                Gun2Text.text = searchSWlist("Mace").name;
            changeweapon();

        }
    }
    void Awake()
    {
        m_Inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_PlayerInventory>();
        Ehandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPPlayerEventHandler>();
        Whandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_WeaponHandler>();
        //How many secondary weapons exist?  maxGunType
  
        Ehandler.SetWeapon.TryStart(0);


    }
    void Start()
    {
//        searchforsecondary();
    }
    void Update (){ //Update is called once per frame
        searchforsecondary();

        maxGunType = SW_inventory.Count;

   //     Debug.Log("SW_inventory[2] " + SW_inventory[2]);


        Debug.Log("maxGunType==" + maxGunType);

        //     Debug.Log("SW_inventory[2] " + SW_inventory[2]);

       


    }
}
