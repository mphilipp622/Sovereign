﻿
//The assumption here is that this script
// is hanging on the player.
// I'm using GetKeyDown's for ease of testing,
// but naturally all of these things could be invoked
// in other ways.

using UnityEngine;
using System.Collections;

public class backpack : MonoBehaviour
{

    private vp_PlayerInventory m_Inventory = null;
    

    void Awake()
    {

        m_Inventory = transform.GetComponent<vp_PlayerInventory>();
    }




    void Update()
    {


        // remove the currently wielded weapon

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (m_Inventory.CurrentWeaponInstance != null)
            {
                string currentInstanceType = m_Inventory.CurrentWeaponInstance.ToString();

                // this will be a weapon that uses ammo

                if (currentInstanceType == "vp_UnitBankInstance")
                {
                    vp_UnitBankInstance currentObject = m_Inventory.CurrentWeaponInstance as vp_UnitBankInstance;
                    if (currentObject == null) return;
                    m_Inventory.TryRemoveUnitBank(currentObject);
                }

                // this will be a no-ammo weapon

                else if (currentInstanceType == "vp_ItemInstance")
                {
                    vp_ItemInstance currentObject = m_Inventory.CurrentWeaponInstance as vp_ItemInstance;
                    if (currentObject == null) return;
                    m_Inventory.TryRemoveItem(currentObject);
                }

            }
        }


        // remove a specific weapon by NAME

        if (Input.GetKeyDown(KeyCode.N))
        {

            // use this for weapons that don't use ammo

            string weaponName = "Mace";
            vp_ItemInstance currentObject = null;
            currentObject = m_Inventory.GetItem(weaponName) as vp_ItemInstance;
            //	if (currentObject != null) Debug.Log(currentObject.Type.name);
            if (currentObject != null) m_Inventory.TryRemoveItem(currentObject);

            // use this for weapons that do use ammo

            //string weaponName = "Machinegun"; 
            //	vp_UnitBankInstance currentObject =	null;
            //currentObject = m_Inventory.GetItem(weaponName) as vp_UnitBankInstance;
            //if (currentObject != null) Debug.Log(currentObject.Type.name);
            //if (currentObject != null) m_Inventory.TryRemoveUnitBank(currentObject);
        }


        // add a specific weapon by name
        // adding weapons is not as picky as removing weapons.
        // no specific Type is required using the TryGiveItem method, it just works.

        if (Input.GetKeyDown(KeyCode.I))
        {
            string weaponName = "Mace";
            for (int v = 0; v < m_Inventory.m_ItemCapInstances.Count; v++)
            {

                if (m_Inventory.m_ItemCapInstances[v].Type.name == weaponName) m_Inventory.TryGiveItem(m_Inventory.m_ItemCapInstances[v].Type, 0);
            }

        }


        // give all inventory items

        if (Input.GetKeyDown(KeyCode.M))
        {

            // Debug.Log(m_Inventory.m_ItemCapInstances.Count);

            for (int v = 0; v < m_Inventory.m_ItemCapInstances.Count; v++)
            {
                // handy debug for getting the names of everything
                //Debug.Log(m_Inventory.m_ItemCapInstances[v].Type.name);
                m_Inventory.TryGiveItem(m_Inventory.m_ItemCapInstances[v].Type, 0);
            }


        }

    }







}
