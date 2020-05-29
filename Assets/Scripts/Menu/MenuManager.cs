using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunBattle
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        string startupMenuName = "Startup";


        List<MenuBase> mMenus = new List<MenuBase>();
        MenuBase mActiveMenu = null;

        // Start is called before the first frame update
        void Start()
        {
            var menus = GetComponentsInChildren<MenuBase>(true);
            mMenus.AddRange(menus);

            Debug.Log("Found menus : ", this);
            foreach (var menu in mMenus)
            {
                Debug.Log("    " + menu.name, this);
            }

            foreach (var menu in mMenus)
            {
                menu.menuManager = this;
                menu.DeactivateMenu();
            }

            ActivateMenu(startupMenuName);
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ActivateMenu(string menuName)
        {
            Debug.Log("Activating menu :" + menuName + ", prevMenu=" + (mActiveMenu ? mActiveMenu.name : "*"), this);

            var newMenu = mMenus.Find(m => m.name == menuName);
            if(mActiveMenu && newMenu != mActiveMenu)
            {
                mActiveMenu.DeactivateMenu();
                mActiveMenu = null;
            }

            if (newMenu)
            {
                mActiveMenu = newMenu;
                mActiveMenu.ActivateMenu();
            }
            else
            {
                Debug.LogError("Menu not found : " + menuName, this);
            }
        }
    }

}