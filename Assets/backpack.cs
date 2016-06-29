
//The assumption here is that this script
// is hanging on the player.
// I'm using GetKeyDown's for ease of testing,
// but naturally all of these things could be invoked
// in other ways.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class backpack : MonoBehaviour
{
    public GameObject FPS_Player;//UFPS Player
    private vp_PlayerInventory m_Inventory = null;
    public vp_ItemType Weapon = null;
    public vp_FPWeapon WeapScript;
    private int WeaponIndex;

    public static bool gamepaused;
    private GameObject itemmenu;
    private GameObject gamecontroller;
    private vp_FPPlayerEventHandler m_Player = null;//UFPS Event Handler for Player
    private vp_FPInput m_Input = null;//UFPS Input for Player
   // private GameObject[] pi_Objects;
    int i = 0;
    int pi_objectCount =0;
    private LoadoutButtons loadButtons;
    private LoadoutButtonsSecondaryGun lsecondarygun;


    void showmenu()
    {
        Time.timeScale.Equals(0);

        itemmenu.SetActive(true);
        i = 0;
        /*
        while (i < pi_objectCount && pi_objectCount > 0)
        {
            Debug.Log(i + " " + pi_Objects[i].name);
            pi_Objects[i].SetActive(true);
            i++;
        }
        */
        m_Player.Pause.Set(true);
        m_Input.MouseCursorForced = true;
        Cursor.visible = true;
    }
    void hidemenu()
    {
        Time.timeScale.Equals(1);
        loadButtons.StartGamePrimaryGun();
        lsecondarygun.StartGameSecondary();
            itemmenu.SetActive(false);
        
        i = 0;
        /*
      while (i < pi_objectCount && pi_objectCount > 0)
          {
              Debug.Log(i + " " + pi_Objects[i].name);
              pi_Objects[i].SetActive(false);
              i++;
          }
          */
      m_Player.Pause.Set(false);
      Cursor.lockState = CursorLockMode.Locked;//Unity 5.2 lock the cursor
      m_Input.MouseCursorForced = false;
      Cursor.visible = false;

  }

  void Awake()
  {
  //    WeaponIndex = WeapScript.GetInstanceID();
      m_Inventory = transform.GetComponent<vp_PlayerInventory>();
      FPS_Player = GameObject.FindGameObjectWithTag("Player");
      m_Player = FPS_Player.transform.GetComponent<vp_FPPlayerEventHandler>();
      m_Input = FPS_Player.transform.GetComponent<vp_FPInput>();
      gamepaused = false;
      gamecontroller = GameObject.FindGameObjectWithTag("GameController");
     itemmenu = GameObject.FindGameObjectWithTag("PlayerInventory");
        itemmenu.SetActive(false);
        loadButtons = GameObject.Find("Button Controller").GetComponent<LoadoutButtons>();
        lsecondarygun = GameObject.Find("Button Controller").GetComponent<LoadoutButtonsSecondaryGun>();
        //  pi_Objects= GameObject.FindGameObjectsWithTag("PlayerInventory");

        //  pi_objectCount = pi_Objects.Length;
        /*
     while(i < pi_objectCount && pi_objectCount >0)
     {
        // Debug.Log(i+" "+pi_Objects[i].name);
          pi_Objects[i].SetActive(false);
        i++;
      }
      */

    }

    void Update()
  {

      if (Input.GetKeyDown(KeyCode.G))
      {
          Debug.Log("crafting MENUEUUUE 2");
          //toggle pause
          if (gamepaused == false)
          {

              Debug.Log("Game is not paused 3");

              gamepaused = true;
              showmenu();
          }
          else
          {

              Debug.Log("Game is paused 4");
              gamepaused = false;
              hidemenu();

          }
      }


      /*


      // -------------------------------------------------------------------------------------
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




*/
    }
    void CraftMenu()
    {
      
        Debug.Log("crafting MENUEUUUE 1");
        

        //when game is paused
        if (gamepaused == true)
        {
            Debug.Log("Game is pause 5");
          
          
            showmenu();

        }
        else
        {
            Debug.Log("Game is pause 6");
            Time.timeScale.Equals(1);
            //itemmenu.SetActive(false);
            hidemenu();

     

        }

    }



}
