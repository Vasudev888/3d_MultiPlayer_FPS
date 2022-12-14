using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManger : MonoBehaviour
{
    public static MenuManger Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].OpenMenuUI();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }

        menu.OpenMenuUI();


    }

    public void CloseMenu(Menu menu)
    {
        menu.CloseMenuUI();
    }

}
